using System.Text.Json;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotExportTests
{
    [Fact]
    public void DisplayOnlyFrequencyCannotBeSilentlyDropped()
    {
        var compatible = SnapshotApi.Copy<FinancialSnapshotIncome>(new { Frequency = "Weekly" }, ("Frequency", "FrequencyValue"));
        Assert.Equal(Frequency.Weekly, compatible.Frequency);
        Assert.Throws<InvalidOperationException>(() => SnapshotApi.Copy<FinancialSnapshotIncome>(new { Frequency = "Every two weeks" }, ("Frequency", "FrequencyValue")));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ExportFollowsAllPagesResolvesRelationshipsAndPreservesSnapshotFields(bool unspecifiedOwnership)
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            var response = Respond(request);
            return unspecifiedOwnership
                ? response.Replace("\"ownershipPercentage\":60", "\"ownershipPercentage\":null")
                    .Replace("\"ownershipPercentage\":40", "\"ownershipPercentage\":null")
                : response;
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var book = await new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ExportAsync(5001);
        Assert.Equal(2, book.Contacts.Count);
        Assert.Equal(2, book.Attributes.Assets.Count);
        Assert.Equal([1001, 1002], book.Attributes.Assets[0].Ownership.Select(owner => owner.Id));
        Assert.Equal(FinancialSnapshotAsset_valuationBasis.ApplicantEstimate, book.Attributes.Assets[0].ValuationBasis);
        Assert.Equal("00123456", book.Attributes.Assets[1].AccountNumber);
        Assert.Equal("0123", book.Attributes.Addresses.Single(a => a.Lid == "financial-address-9001").PostCode);
        Assert.Equal("financial-address-9002", Assert.Single(book.Attributes.Employments).Address.Lid);
        Assert.Equal(1001, book.Attributes.Employments[0].ContactId);
        Assert.Equal(8201, Assert.Single(book.Attributes.Incomes).Employment.Id);
        Assert.Equal(Frequency.Fortnightly, book.Attributes.Incomes[0].Frequency);
        Assert.False(book.Attributes.Incomes[0].NzIsGross);
        var liability = Assert.Single(book.Attributes.Liabilities);
        Assert.Equal(8101, liability.LinkedAsset.Id);
        Assert.Equal(6.25, liability.InterestRate);
        Assert.Equal(306, liability.NzDocumentedLoanTermMonths);
        Assert.Equal(FinancialSnapshotLiability_repaymentFrequency.Monthly, liability.RepaymentFrequency);
        Assert.False(liability.InterestTaxDeductible);
        Assert.Equal(8501, Assert.Single(book.Attributes.Expenses).ExpenseId);
        Assert.Equal(2, book.Excluded.Count);
        Assert.DoesNotContain(book.Notes, note => note.Contains("ownership", StringComparison.OrdinalIgnoreCase));
        Assert.All(SnapshotWorkbook.Collections, collection => Assert.Equal("Merge", book.Mode(collection)));
        var searches = handler.Requests.Where(u => new[] { "/assets?", "/incomes?", "/expenses?", "/liabilities?" }.Any(u.Contains)).ToArray();
        Assert.Equal(5, searches.Length);
        Assert.All(searches, url => Assert.Contains("has(ownership,any(contact.id,'1001','1002'))", url));
    }

    [Theory]
    [InlineData("https://untrusted.test/steal")]
    [InlineData("http://example.test/jsonapi/contact-groups/5001/contacts")]
    public async Task RejectsPaginationOutsideApiOrigin(string next)
    {
        using var handler = new SnapshotTests.RecordingHandler(_ => JsonSerializer.Serialize(new { data = Array.Empty<object>(), links = new { next } }));
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        await Assert.ThrowsAsync<InvalidOperationException>(() => new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ContactsAsync(5001));
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task EmptyGroupNeverRunsAnUnfilteredFinancialSearch()
    {
        using var handler = new SnapshotTests.RecordingHandler(_ => "{\"data\":[]}");
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var book = await new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ExportAsync(5001);
        Assert.Empty(book.Attributes.Assets);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public async Task MissingIncludedOwnershipStopsExport()
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            var response = Respond(request);
            return request.RequestUri!.AbsolutePath == "/jsonapi/assets" ? response.Replace("\"owners\"", "\"unknown-owners\"") : response;
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        await Assert.ThrowsAsync<InvalidOperationException>(() => new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ExportAsync(5001));
    }

    [Theory]
    [InlineData("9002")]
    [InlineData("9001")]
    public async Task EmploymentAddressesPreserveFieldsAndStayDistinctFromFinancialAddressIds(string addressId)
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            var response = Respond(request);
            return request.RequestUri!.AbsolutePath == "/jsonapi/contacts/1001/employments"
                ? response.Replace("financial-address", "addresses").Replace("9002", addressId)
                : response;
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };

        var book = await new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ExportAsync(5001);

        Assert.Equal(2, book.Attributes.Addresses.Count);
        var employment = Assert.Single(book.Attributes.Employments);
        Assert.Equal("addresses-" + addressId, employment.Address.Lid);
        var address = book.Attributes.Addresses.Single(a => a.Lid == employment.Address.Lid);
        Assert.Equal("20 Example Road", address.StreetAddress);
        Assert.Equal("Australia", address.Country);

        var asset = book.Attributes.Assets.Single(a => a.AssetId == 8101);
        Assert.NotEqual(asset.Address.Lid, employment.Address.Lid);
        Assert.Equal("1 Example St", book.Attributes.Addresses.Single(a => a.Lid == asset.Address.Lid).StreetAddress);
    }

    [Fact]
    public async Task UnknownIncludedAddressTypeReportsDiscriminatorProblem()
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            var response = Respond(request);
            return request.RequestUri!.AbsolutePath == "/jsonapi/contacts/1001/employments"
                ? response.Replace("financial-address", "unknown-addresses")
                : response;
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).ExportAsync(5001));

        Assert.Contains("unknown-addresses/9002", error.Message);
        Assert.Contains("deserialized as IncludedResource", error.Message);
        Assert.Contains("Swagger discriminator", error.Message);
        Assert.DoesNotContain("Missing included", error.Message);
    }

    private const string Owners = """
        {"type":"owners","id":"1","attributes":{"ownershipPercentage":60},"relationships":{"contact":{"data":{"type":"contacts","id":"1001"}}}},
        {"type":"owners","id":"2","attributes":{"ownershipPercentage":40},"relationships":{"contact":{"data":{"type":"contacts","id":"1002"}}}}
        """;
    private static string Respond(HttpRequestMessage request)
    {
        var uri = request.RequestUri!;
        var second = Uri.UnescapeDataString(uri.Query).Contains("page[number]=2");
        switch (uri.AbsolutePath)
        {
            case "/jsonapi/contact-groups/5001/contacts":
                return second
                    ? """{"data":[{"type":"contacts","id":"1002","attributes":{"role":"Adult","firstName":"Sam"}}]}"""
                    : """{"data":[{"type":"contacts","id":"1001","attributes":{"role":"Adult","firstName":"Alex"}}],"links":{"next":"?page[number]=2"}}""";
            case "/jsonapi/assets":
                if (second) return $$$$$"""{"data":[{"type":"assets","id":"8102","attributes":{"assetTypeId":15,"value":20000,"accountNumber":"00123456","description":"Savings"},"relationships":{"ownership":{"data":[{"type":"owners","id":"1"}]},"addresses":{"data":[]}}}],"included":[{{{{{Owners}}}}}]}""";
                var next = "https://example.test/jsonapi/assets?page[number]=2&filter=" + Uri.EscapeDataString(SnapshotApi.OwnershipFilter([1001, 1002]));
                return $$$$$"""{"data":[{"type":"assets","id":"8101","attributes":{"assetTypeId":1,"value":780000,"propertyTypeId":1,"propertyPrimaryPurpose":"PurchaseInvestment","valueBasis":"ApplicantEstimate","valuationBasis":"Applicant estimate"},"relationships":{"ownership":{"data":[{"type":"owners","id":"1"},{"type":"owners","id":"2"}]},"addresses":{"data":[{"type":"financial-address","id":"9001"}]}}}],"included":[{{{{{Owners}}}}},{"type":"financial-address","id":"9001","attributes":{"streetAddress":"1 Example St","postCode":"0123","country":"Australia"}}],"links":{"next":{{{{{JsonSerializer.Serialize(next)}}}}}}}""";
            case "/jsonapi/liabilities":
                return $$$$$"""{"data":[{"type":"liabilities","id":"8401","attributes":{"liabilityTypeId":1,"value":490000,"creditorName":"Example","repayment":3200,"repaymentFrequencyValue":"Monthly","repaymentFrequency":"Monthly display","interestRate":6.25,"interestTaxDeductible":false,"nzDocumentedLoanTermMonths":306,"nzLoanStartDate":"2022-07-01","loanRepaymentType":"PrincipalInterest","mortgagePriority":"First"},"relationships":{"ownership":{"data":[{"type":"owners","id":"1"}]},"linkedAsset":{"data":{"type":"assets","id":"8101"}}}}],"included":[{{{{{Owners}}}}}]}""";
            case "/jsonapi/incomes":
                return $$$$$"""{"data":[{"type":"incomes","id":"8301","attributes":{"incomeTypeId":19,"description":"Salary","value":4500,"frequencyValue":"Fortnightly","frequency":"Every two weeks","nzIsGross":false},"relationships":{"ownership":{"data":[{"type":"owners","id":"1"}]},"employment":{"data":{"type":"employments","id":"8201"}},"linkedAsset":{"data":null}}},{"type":"incomes","id":"8302","attributes":{"incomeType":"Addbacks","incomeCategory":"Addback","value":200}}],"included":[{{{{{Owners}}}}}]}""";
            case "/jsonapi/expenses":
                return $$$$$"""{"data":[{"type":"expenses","id":"8501","attributes":{"expenseTypeId":17,"value":280,"description":"Groceries","frequencyValue":"Weekly"},"relationships":{"ownership":{"data":[{"type":"owners","id":"1"}]}}},{"type":"expenses","id":"8502","attributes":{"expenseTypeId":31,"expenseType":"Mortgage Repayments","value":3200,"frequency":"Monthly"}}],"included":[{{{{{Owners}}}}}]}""";
            case "/jsonapi/contacts/1001/employments":
                return """{"data":[{"type":"employments","id":"8201","attributes":{"employerName":"Example","employmentType":"PAYG","dateStarted":"2021-03-01","employmentBasis":"FullTime"},"relationships":{"contact":{"data":{"type":"contacts","id":"1001"}},"address":{"data":{"type":"financial-address","id":"9002"}}}}],"included":[{"type":"financial-address","id":"9002","attributes":{"streetAddress":"20 Example Road","country":"Australia"}}]}""";
            case "/jsonapi/contacts/1002/employments": return "{\"data\":[]}";
            default: throw new InvalidOperationException("Unexpected request " + uri);
        }
    }
}
