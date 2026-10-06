using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotContactTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-contacts-" + Guid.NewGuid().ToString("N"));

    public SnapshotContactTests() => Directory.CreateDirectory(_directory);

    public void Dispose() => Directory.Delete(_directory, true);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ExportAndExamplesIncludeOnlyAdultsInContactLookups(bool example)
    {
        var exported = await ExportProfile();
        var snapshot = example
            ? SnapshotExamples.Create(5001, exported.Contacts, SnapshotExampleData.MinimalControls(), "AU", seed: 12345)
            : exported;
        var path = Path.Combine(_directory, "contact-lookups.xlsx");
        snapshot.Save(path);

        using (var excel = new XLWorkbook(path))
        {
            var ws = excel.Worksheet("Contact Lookups");
            Assert.Equal(["contactId", "Name", "Gender", "isPrimary"], ws.Row(1).CellsUsed().Select(cell => cell.GetString()));

            Assert.Equal(1001, Cell(ws, "contactId").GetDouble());
            Assert.Equal("Alex Robin Taylor", Cell(ws, "Name").GetString());
            Assert.Equal("Undisclosed", Cell(ws, "Gender").GetString());
            Assert.True(Cell(ws, "isPrimary").GetBoolean());
            Assert.Equal(2, ws.LastRowUsed()!.RowNumber());

            var validation = excel.Worksheet("Employments").Row(1).CellsUsed()
                .Single(cell => cell.GetString() == "contactId").CellBelow().GetDataValidation();
            Assert.Equal(["1001 — Alex Robin Taylor"], excel.DefinedName(validation.Value).Ranges.Single().Cells().Select(cell => cell.GetString()));
        }

        using var document = SpreadsheetDocument.Open(path, false);
        Assert.Empty(new OpenXmlValidator().Validate(document));
    }

    [Fact]
    public async Task ContactReferenceEditsCannotChangeTheImportedSnapshot()
    {
        var exported = await ExportProfile();
        var snapshot = SnapshotExamples.Create(5001, exported.Contacts, SnapshotExampleData.MinimalControls(), "AU", seed: 12345);
        var path = Path.Combine(_directory, "reference-only.xlsx");
        snapshot.Save(path);

        using (var excel = new XLWorkbook(path))
        {
            var ws = excel.Worksheet("Contact Lookups");
            Cell(ws, "Name").Value = "Edited in Excel";
            Cell(ws, "contactId").Value = 9999;
            Cell(ws, "Gender").Value = "Female";
            Cell(ws, "isPrimary", 3).Value = true;
            excel.Save();
        }

        var imported = SnapshotWorkbook.Load(path);
        Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(snapshot.ToDocument()),
            await KiotaJsonSerializer.SerializeAsStringAsync(imported.ToDocument()));
    }

    private static IXLCell Cell(IXLWorksheet sheet, string header, int row = 2) =>
        sheet.Cell(row, sheet.Row(1).CellsUsed().Single(cell => cell.GetString() == header).Address.ColumnNumber);

    private static async Task<SnapshotWorkbook> ExportProfile()
    {
        using var handler = new SnapshotTests.RecordingHandler(request => request.RequestUri!.AbsolutePath switch
        {
            "/jsonapi/contact-groups/5001/contacts" => """
                {"data":[
                  {"type":"contacts","id":"1001","attributes":{
                    "businessPhone":"0733334444","created":"2023-01-02T09:30:00+10:00","dateOfBirth":"1985-07-14",
                    "email":"alex@example.test","firstName":"Alex","gender":"Undisclosed","hasMarketingConsent":false,
                    "homePhone":"0711112222","isPrimary":true,"lastName":"Taylor","maritalStatus":"Married",
                    "middleName":"Robin","mobile":"0400123456","preferredName":"Al","role":"Adult",
                    "secondaryEmail":"al@example.test","title":"Dr","updated":"2026-10-06T12:45:00+10:00"}},
                  {"type":"contacts","id":"1002","attributes":{"firstName":"Sam","role":"Child","isPrimary":false}},
                  {"type":"contacts","id":"1003"}
                ]}
                """,
            "/jsonapi/assets" or "/jsonapi/liabilities" or "/jsonapi/incomes" or "/jsonapi/expenses" or
            "/jsonapi/contacts/1001/employments" or "/jsonapi/contacts/1002/employments" or "/jsonapi/contacts/1003/employments" => "{\"data\":[]}",
            _ => throw new InvalidOperationException("Unexpected request: " + request.RequestUri)
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };

        return await new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ExportAsync(5001);
    }
}
