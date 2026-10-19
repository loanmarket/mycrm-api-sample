using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotComparisonTests
{
    private static SnapshotWorkbook Example(string country = "AU") => SnapshotExamples.Create(5001,
        [new() { Id = "1001", Attributes = new() { Role = ContactAttributes_role.Adult } },
         new() { Id = "1002", Attributes = new() { Role = ContactAttributes_role.Adult } }],
        SnapshotExampleData.FullControls(), country, seed: 731);

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
        var comparison = SnapshotComparison.Compare(spreadsheet, api);

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
}
