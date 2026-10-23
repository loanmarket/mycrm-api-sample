using ClosedXML.Excel;
using System.Text.Json;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotComparisonTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-comparison-" + Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(_directory)) Directory.Delete(_directory, true);
    }

    private SnapshotWorkbook LoadWithoutColumns(SnapshotWorkbook book, params (string Sheet, string[] Headers)[] omitted)
    {
        Directory.CreateDirectory(_directory);
        var path = Path.Combine(_directory, Guid.NewGuid().ToString("N") + ".xlsx");
        book.Save(path);
        using (var excel = new XLWorkbook(path))
        {
            // Removing columns in ClosedXML attempts to rewrite structured defined names;
            // these read-only comparison fixtures do not need dropdown definitions.
            excel.DefinedNames.DeleteAll();
            foreach (var worksheet in excel.Worksheets) worksheet.Clear(XLClearOptions.DataValidation);

            foreach (var (sheet, headers) in omitted)
            {
                var ws = excel.Worksheet(sheet);
                foreach (var number in ws.Row(1).CellsUsed().Where(cell => headers.Contains(cell.GetString()))
                    .Select(cell => cell.Address.ColumnNumber).OrderByDescending(number => number).ToArray())
                    ws.Column(number).Delete();
            }

            excel.Save();
        }

        var original = File.ReadAllBytes(path);
        var loaded = SnapshotWorkbook.Load(path, allowMissingColumns: true);

        Assert.Equal(original, File.ReadAllBytes(path));
        Assert.Throws<FormatException>(() => SnapshotWorkbook.Load(path)); // Import still requires the full template.

        return loaded;
    }

    private static SnapshotWorkbook Example(string country = "AU") => SnapshotExamples.Create(5001,
        [new() { Id = "1001", Attributes = new() { Role = ContactAttributes_role.Adult } },
         new() { Id = "1002", Attributes = new() { Role = ContactAttributes_role.Adult } }],
        SnapshotExampleData.FullControls(), country, seed: 731);

    private static void AssertEquivalent(SnapshotWorkbook left, SnapshotWorkbook right, string country = "AU")
    {
        var comparison = SnapshotComparison.Compare(left, right, country);

        Assert.False(comparison.HasDifferences, JsonSerializer.Serialize(comparison.Rows.Where(row => row.Status != SnapshotComparisonStatus.Equal)));
    }

    private static void AssignApiIds(SnapshotWorkbook book)
    {
        var assets = book.Attributes.Assets.Select((asset, index) => (asset.Lid, Id: 8001 + index)).ToDictionary(item => item.Lid, item => item.Id);
        var jobs = book.Attributes.Employments.Select((job, index) => (job.Lid, Id: 9001 + index)).ToDictionary(item => item.Lid, item => item.Id);
        var addresses = book.Attributes.Addresses.Select((address, index) => (address.Lid, NewLid: "api-address-" + index)).ToDictionary(item => item.Lid, item => item.NewLid);

        foreach (var asset in book.Attributes.Assets)
        {
            asset.AssetId = assets[asset.Lid];
            asset.Lid = "api-asset-" + asset.AssetId;
            if (asset.Address != null) asset.Address.Lid = addresses[asset.Address.Lid];
        }

        foreach (var job in book.Attributes.Employments)
        {
            job.EmploymentId = jobs[job.Lid];
            job.Lid = "api-job-" + job.EmploymentId;
            if (job.Address != null) job.Address.Lid = addresses[job.Address.Lid];
        }

        foreach (var address in book.Attributes.Addresses) address.Lid = addresses[address.Lid];
        foreach (var (income, index) in book.Attributes.Incomes.Select((row, index) => (row, index)))
        {
            income.IncomeId = 10001 + index;
            income.Lid = "api-income-" + index;
            if (income.Employment != null) income.Employment = new() { Id = jobs[income.Employment.Lid] };
            if (income.LinkedAsset != null) income.LinkedAsset = new() { Id = assets[income.LinkedAsset.Lid] };
        }

        foreach (var (liability, index) in book.Attributes.Liabilities.Select((row, index) => (row, index)))
        {
            liability.LiabilityId = 11001 + index;
            liability.Lid = "api-liability-" + index;
            if (liability.LinkedAsset != null) liability.LinkedAsset = new() { Id = assets[liability.LinkedAsset.Lid] };
        }

        foreach (var (expense, index) in book.Attributes.Expenses.Select((row, index) => (row, index)))
        {
            expense.ExpenseId = 12001 + index;
            expense.Lid = "api-expense-" + index;
        }

        foreach (var sheet in SnapshotWorkbook.Collections)
        {
            foreach (var row in book.Rows(sheet))
                (row.GetType().GetProperty("Ownership")?.GetValue(row) as List<FinancialSnapshotContactReference>)?.Reverse();
        }
    }

    [Theory]
    [InlineData("AU")]
    [InlineData("NZ")]
    public async Task GeneratedLocalIdsMatchAssignedApiIdsWithoutMutatingInputs(string country)
    {
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var spreadsheet = Example(country);
        var api = Example(country);
        AssignApiIds(api);
        api.Source = "Export";
        foreach (var sheet in SnapshotWorkbook.Collections) api.SetMode(sheet, "Replace");
        api.Attributes.Assets.Reverse();
        api.Attributes.Employments.Reverse();
        api.Attributes.Incomes.Reverse();

        var beforeSpreadsheet = await KiotaJsonSerializer.SerializeAsStringAsync(spreadsheet.ToDocument());
        var beforeApi = await KiotaJsonSerializer.SerializeAsStringAsync(api.ToDocument());
        var comparison = SnapshotComparison.Compare(spreadsheet, api, country);

        Assert.False(comparison.HasDifferences, string.Join("\n", comparison.Rows.Where(row => row.Status != SnapshotComparisonStatus.Equal)));
        Assert.Equal(SnapshotWorkbook.Collections.Sum(sheet => spreadsheet.Rows(sheet).Count), comparison.Rows.Count);
        Assert.Equal(beforeSpreadsheet, await KiotaJsonSerializer.SerializeAsStringAsync(spreadsheet.ToDocument()));
        Assert.Equal(beforeApi, await KiotaJsonSerializer.SerializeAsStringAsync(api.ToDocument()));
    }

    [Fact]
    public void ExplicitIdsExposeFieldDifferencesAcrossEveryCollectionAndLinkedAddresses()
    {
        var spreadsheet = Example();
        var api = Example();
        AssignApiIds(spreadsheet);
        AssignApiIds(api);
        api.Attributes.Assets[0].Value += 1;
        api.Attributes.Assets[0].Ownership.RemoveAt(0);
        api.Attributes.Addresses[0].PostCode = "0001";
        api.Attributes.Employments[0].EmployerName = "Different employer";
        api.Attributes.Employments[0].EmployerPhone = null;
        api.Attributes.Incomes[0].IncomeVerificationId = null;
        api.Attributes.Incomes[0].Employment.Id = 9999;
        api.Attributes.Liabilities[0].Repayment += 0.01;
        api.Attributes.Expenses[0].Description = "Changed description";

        var comparison = SnapshotComparison.Compare(spreadsheet, api);
        var differences = comparison.Rows.SelectMany(row => row.Differences).ToArray();

        Assert.All(SnapshotWorkbook.Collections, sheet => Assert.Contains(comparison.Rows, row => row.Collection == sheet && row.Status == SnapshotComparisonStatus.Changed));
        foreach (var field in new[] { "value", "ownership", "address.postCode", "employerName", "employerPhone", "incomeVerificationId", "employment", "repayment", "description" })
            Assert.Contains(differences, difference => difference.Field == field);
        Assert.Equal("9999", Assert.Single(differences, difference => difference.Field == "employment").Api[3..]);
        Assert.DoesNotContain(comparison.Rows, row => row.Status is SnapshotComparisonStatus.SpreadsheetOnly or SnapshotComparisonStatus.ApiOnly);
    }

    [Fact]
    public void MissingExplicitIdDoesNotFallBackToIdenticalContent()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetId = 10, AssetTypeId = 4, Description = "Cash", Value = 20 });
        right.Attributes.Assets.Add(new() { AssetId = 11, AssetTypeId = 4, Description = "Cash", Value = 20 });

        var comparison = SnapshotComparison.Compare(left, right);

        Assert.Contains(comparison.Rows, row => row.Status == SnapshotComparisonStatus.SpreadsheetOnly);
        Assert.Contains(comparison.Rows, row => row.Status == SnapshotComparisonStatus.ApiOnly);
        Assert.Equal(2, comparison.Rows.Count);
    }

    [Fact]
    public void ExplicitIdsAreReservedBeforeAutomaticMatches()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetTypeId = 4, Description = "Cash", Value = 20 });
        left.Attributes.Assets.Add(new() { AssetId = 10, AssetTypeId = 4, Description = "Cash", Value = 20 });
        right.Attributes.Assets.Add(new() { AssetId = 10, AssetTypeId = 4, Description = "Cash", Value = 20 });
        right.Attributes.Assets.Add(new() { AssetId = 11, AssetTypeId = 4, Description = "Cash", Value = 20 });

        var comparison = SnapshotComparison.Compare(left, right);

        Assert.False(comparison.HasDifferences);
        Assert.Contains("assetId=10", comparison.Rows.Single(row => row.SpreadsheetRecord.Contains("assetId=10")).ApiRecord);
        Assert.Contains("assetId=11", comparison.Rows.Single(row => !row.SpreadsheetRecord.Contains("assetId=10")).ApiRecord);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MultipleCandidatesAreReportedWithoutChoosingByAmountOrOrder(bool reverse)
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { Lid = "cash", AssetTypeId = 4, Description = "Cash", Value = 20 });
        right.Attributes.Assets.Add(new() { AssetId = 10, AssetTypeId = 4, Description = "Cash", Value = 20 });
        right.Attributes.Assets.Add(new() { AssetId = 11, AssetTypeId = 4, Description = "Cash", Value = 30 });
        if (reverse) right.Attributes.Assets.Reverse();

        var row = Assert.Single(SnapshotComparison.Compare(left, right).Rows);

        Assert.Equal(SnapshotComparisonStatus.Ambiguous, row.Status);
        Assert.Contains("assetId=10", row.ApiRecord);
        Assert.Contains("assetId=11", row.ApiRecord);
    }

    [Fact]
    public void DuplicateExplicitIdsAreReportedInsteadOfMatchingOneTwice()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetId = 10 });
        left.Attributes.Assets.Add(new() { AssetId = 10 });
        right.Attributes.Assets.Add(new() { AssetId = 10 });

        var comparison = SnapshotComparison.Compare(left, right);

        Assert.Equal(2, comparison.Rows.Count);
        Assert.All(comparison.Rows, row => Assert.Equal(SnapshotComparisonStatus.Ambiguous, row.Status));
    }

    [Fact]
    public void OwnerContextDisambiguatesButOwnerOrderDoesNotChangeValues()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetTypeId = 4, Description = "Cash", Value = 20, Ownership = [new() { Id = 1 }, new() { Id = 2 }] });
        left.Attributes.Assets.Add(new() { AssetTypeId = 4, Description = "Cash", Value = 30, Ownership = [new() { Id = 3 }] });
        right.Attributes.Assets.Add(new() { AssetId = 10, AssetTypeId = 4, Description = "Cash", Value = 30, Ownership = [new() { Id = 3 }] });
        right.Attributes.Assets.Add(new() { AssetId = 11, AssetTypeId = 4, Description = "Cash", Value = 20, Ownership = [new() { Id = 2 }, new() { Id = 1 }] });

        Assert.False(SnapshotComparison.Compare(left, right).HasDifferences);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void UnresolvedLinksAndAddressesCannotProduceAFalseMatch(bool duplicateLid)
    {
        var left = Example();
        var right = Example();
        AssignApiIds(left);
        AssignApiIds(right);
        if (duplicateLid)
            left.Attributes.Addresses.Add(new() { Lid = left.Attributes.Addresses[0].Lid });
        else left.Attributes.Assets[0].Address.Lid = "not-in-workbook";
        left.Attributes.Incomes[0].Employment = new() { Lid = "missing-job" };

        var comparison = SnapshotComparison.Compare(left, right);
        var differences = comparison.Rows.SelectMany(row => row.Differences).ToArray();

        Assert.Contains(differences, row => row.Field == "address" && row.Spreadsheet.Contains("Unresolved"));
        Assert.Contains(differences, row => row.Field == "employment" && row.Spreadsheet.Contains("Unresolved"));
        Assert.True(comparison.HasDifferences);
    }

    [Fact]
    public void ComparisonReportsUnreferencedAddressesAndPreservesValueDistinctions()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetId = 10, Description = "", Value = null, AccountNumber = "00123" });
        right.Attributes.Assets.Add(new() { AssetId = 10, Description = null, Value = 0, AccountNumber = "123" });
        left.Attributes.Addresses.Add(new() { Lid = "unused" });

        var comparison = SnapshotComparison.Compare(left, right);
        var asset = Assert.Single(comparison.Rows, row => row.Collection == "Assets");

        Assert.DoesNotContain(asset.Differences, difference => difference.Field == "description");
        Assert.Contains(asset.Differences, difference => difference.Field == "value");
        Assert.Contains(asset.Differences, difference => difference.Field == "accountNumber");
        Assert.Contains(comparison.Rows, row => row.Collection == "Addresses" && row.Status == SnapshotComparisonStatus.Ambiguous);
    }

    [Fact]
    public void EmptyProfilesCompareEqual()
    {
        var comparison = SnapshotComparison.Compare(new(), new());

        Assert.False(comparison.HasDifferences);
        Assert.Empty(comparison.Rows);
    }

    [Theory]
    [InlineData("AU", 0)]
    [InlineData("NZ", 9)]
    [InlineData(null, 9)]
    public void NzOnlyValuesAreIgnoredOnlyForAustralianGroups(string country, int expectedDifferences)
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Liabilities.Add(new() { LiabilityId = 10 });
        right.Attributes.Liabilities.Add(new()
        {
            LiabilityId = 10, NzDocumentedLoanTermMonths = 0, NzInterestOnlyTermMonths = 0,
            NzIsInArrears = false, NzRevolvingCreditRedraw = false,
            NzLoanStartDate = new(2020, 1, 1), NzInterestOnlyStartDate = new(2021, 1, 1)
        });
        left.Attributes.Incomes.Add(new() { IncomeId = 20 });
        right.Attributes.Incomes.Add(new()
        {
            IncomeId = 20, NzIsGross = true, NzRentalType = FinancialSnapshotIncome_nzRentalType.RentalIncome,
            NzRentalVerificationTypeId = 1
        });

        var comparison = SnapshotComparison.Compare(left, right, country);

        Assert.Equal(expectedDifferences, comparison.Rows.Sum(row => row.Differences.Count));
        Assert.Equal(expectedDifferences > 0, comparison.HasDifferences);
    }

    [Theory]
    [InlineData("AU", true)]
    [InlineData("NZ", false)]
    [InlineData(null, true)]
    public void AustralianLoanTermIsIgnoredOnlyForNewZealandGroups(string country, bool differs)
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Liabilities.Add(new() { LiabilityId = 10 });
        right.Attributes.Liabilities.Add(new() { LiabilityId = 10, LoanTerm = 0 });

        Assert.Equal(differs, SnapshotComparison.Compare(left, right, country).HasDifferences);
    }

    [Fact]
    public void ApplicableEmptyValuesStillDifferFromZeroAndFalse()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Liabilities.Add(new() { LiabilityId = 10 });
        right.Attributes.Liabilities.Add(new() { LiabilityId = 10, Value = 0, InterestTaxDeductible = false });

        var differences = Assert.Single(SnapshotComparison.Compare(left, right, "AU").Rows).Differences;

        Assert.Equal(new[] { "interestTaxDeductible", "value" }, differences.Select(difference => difference.Field));
    }

    [Theory]
    [InlineData(10, false)]
    [InlineData(11, true)]
    public void DateOnlyValuesIgnoreHiddenTimeButStillCompareCalendarDates(int day, bool differs)
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Employments.Add(new() { EmploymentId = 10, DateStarted = new(2024, 2, 10) });
        right.Attributes.Employments.Add(new() { EmploymentId = 10, DateStarted = new(new DateTime(2024, 2, day, 14, 30, 0)) });

        Assert.Equal(differs, SnapshotComparison.Compare(left, right).HasDifferences);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingColumnsAreIgnoredAcrossCollectionsAndAddresses(bool explicitIds)
    {
        var left = Example();
        var right = Example();
        if (explicitIds) AssignApiIds(left);
        AssignApiIds(right);
        left = LoadWithoutColumns(left,
            ("Assets", ["description", "value"]), ("Expenses", ["value"]),
            ("Incomes", ["incomeVerificationId"]), ("Liabilities", ["repayment"]),
            ("Employments", ["employerPhone"]), ("Addresses", ["postCode"]));

        AssertEquivalent(left, right);

        right.Attributes.Assets[0].AccountName = "Changed account name";
        var differences = SnapshotComparison.Compare(left, right, "AU").Rows.SelectMany(row => row.Differences);

        Assert.Equal("accountName", Assert.Single(differences).Field);
    }

    [Fact]
    public void MissingRelationshipAndOwnerColumnsDoNotCompareApiLinksOrReportUnusedAddresses()
    {
        var left = Example();
        var right = Example();
        AssignApiIds(left);
        AssignApiIds(right);
        left = LoadWithoutColumns(left,
            ("Assets", ["owner1ContactId", "owner2ContactId", "address.lid"]),
            ("Employments", ["address.lid"]),
            ("Incomes", ["linkedAsset.id", "linkedAsset.lid", "employment.id", "employment.lid"]),
            ("Liabilities", ["linkedAsset.id", "linkedAsset.lid"]));

        AssertEquivalent(left, right);
    }

    [Theory]
    [InlineData("owner1ContactId", 1, 2, false)]
    [InlineData("owner1ContactId", 3, 2, false)]
    [InlineData("owner1ContactId", 3, 1, true)]
    [InlineData("owner2ContactId", 2, 1, false)]
    [InlineData("owner2ContactId", 3, 2, true)]
    public void RemainingOwnerColumnIsComparedWithoutDependingOnItsPosition(string omitted, int first, int second, bool differs)
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetId = 10, Ownership = [new() { Id = 1 }, new() { Id = 2 }] });
        right.Attributes.Assets.Add(new() { AssetId = 10, Ownership = [new() { Id = first }, new() { Id = second }] });
        left = LoadWithoutColumns(left, ("Assets", [omitted]));

        Assert.Equal(differs, SnapshotComparison.Compare(left, right).HasDifferences);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RemainingLinkAlternativeResolvesToApiId(bool explicitIds)
    {
        var left = Example();
        var right = Example();
        if (explicitIds) AssignApiIds(left);
        AssignApiIds(right);
        var omitted = explicitIds ? "lid" : "id";
        left = LoadWithoutColumns(left,
            ("Incomes", ["linkedAsset." + omitted, "employment." + omitted]),
            ("Liabilities", ["linkedAsset." + omitted]));

        AssertEquivalent(left, right);

        right.Attributes.Incomes[0].Employment.Id = 9999;
        var differences = SnapshotComparison.Compare(left, right).Rows.SelectMany(row => row.Differences);
        Assert.Equal("employment", Assert.Single(differences).Field);
    }

    [Fact]
    public void OmittedOwnerColumnsCannotBeUsedToGuessBetweenCandidates()
    {
        var left = new SnapshotWorkbook();
        var right = new SnapshotWorkbook();
        left.Attributes.Assets.Add(new() { AssetTypeId = 4, Description = "Cash", Ownership = [new() { Id = 1 }] });
        right.Attributes.Assets.Add(new() { AssetId = 10, AssetTypeId = 4, Description = "Cash", Ownership = [new() { Id = 1 }] });
        right.Attributes.Assets.Add(new() { AssetId = 11, AssetTypeId = 4, Description = "Cash", Ownership = [new() { Id = 2 }] });
        left = LoadWithoutColumns(left, ("Assets", ["owner1ContactId", "owner2ContactId"]));

        Assert.Equal(SnapshotComparisonStatus.Ambiguous, Assert.Single(SnapshotComparison.Compare(left, right).Rows).Status);
    }
}
