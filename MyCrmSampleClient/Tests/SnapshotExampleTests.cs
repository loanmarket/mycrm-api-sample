using ClosedXML.Excel;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

internal static class SnapshotExampleData
{
    public static List<SnapshotLookup> MinimalControls() =>
    [
        new("asset-types", 1, "Real Estate"), new("asset-types", 15, "Savings"),
        new("income-types", 19, "Salary / Wages"), new("income-types", 23, "Rental Income"),
        new("liability-types", 21, "Mortgage"), new("expense-types", 17, "Groceries"),
        new("property-types", 13, "Fully Detached House"),
        new("income-verification-types", 1, "Current payslips"), new("income-verification-types", 21, "Payslips"),
        new("rental-verification-types", 3, "Tenancy agreement")
    ];

    public static List<SnapshotLookup> FullControls()
    {
        var controls = MinimalControls();

        void Add(string resource, params (int Id, string Name)[] items) =>
            controls.AddRange(items.Select(item => new SnapshotLookup(resource, item.Id, item.Name)));

        Add("asset-types", (2, "Boat"), (3, "Bonds"), (4, "Cash"), (5, "Cash Management"),
            (7, "Cheque Account"), (14, "Home Contents"), (17, "Managed Funds"), (18, "Motor Vehicle"),
            (20, "Other Deposit"), (24, "Savings Account"), (25, "Shares"), (27, "Superannuation"),
            (28, "Term Deposit"), (32, "KiwiSaver"), (33, "Art"), (37, "Jewellery"));
        Add("asset-sub-types", (1, "4WD"), (2, "Bike"), (3, "Large"), (4, "Luxury Car"), (5, "Medium"), (6, "Small"), (7, "Small Medium"));
        Add("property-types", (42, "Town house"), (46, "Villa"), (68, "Std Residential"));
        Add("income-verification-types", (8, "Letter from employer"), (9, "Employment contract"),
            (22, "Employment letter"), (17, "Rental appraisal"));
        Add("income-types", (5, "Dividends"), (10, "Interest"), (11, "Other Gross Income"), (14, "Bonus"),
            (15, "Car Allowance"), (16, "Commission"), (17, "Company car"), (18, "Regular Overtime"),
            (20, "Work Allowance"), (26, "Foreign Income"), (27, "Other Net Income"), (30, "Non-Recurring Income"),
            (1, "Depreciation"), (34, "Addbacks"), (35, "Net Profit Before Tax"));
        Add("liability-types", (7, "Credit Card"), (8, "Student Loan"), (17, "Overdraft"),
            (18, "Personal Loan"), (19, "Car Loan"), (25, "Buy Now Pay Later"));
        controls.Add(new("liability-sub-types", 4, "MasterCard", "liability-types", 7));
        controls.Add(new("liability-sub-types", 6, "Visa", "liability-types", 7));
        Add("expense-types", (2, "Vehicle Maintenance"), (3, "Registration"), (7, "Clothing & Footwear"),
            (11, "Electricity & Gas"), (12, "Travel & Holidays"), (26, "Contents Insurance"), (30, "Personal Care"),
            (36, "Medical & Health"), (37, "Petrol"), (38, "Public Transport"), (43, "Internet"), (46, "Gym / Sports"),
            (47, "Home/Mobile Phone"), (55, "Vehicle Insurance"), (56, "Health Insurance"), (31, "Mortgage Repayments"));

        return controls;
    }
}

public sealed class SnapshotExampleTests
{
    private static Contact[] Contacts(int adultCount) =>
    [
        .. Enumerable.Range(1001, adultCount).Select(id => new Contact
        {
            Id = id.ToString(), Attributes = new() { Role = ContactAttributes_role.Adult, FirstName = "Adult " + id }
        }),
        new() { Id = "1003", Attributes = new() { Role = ContactAttributes_role.Child, FirstName = "Child" } }
    ];

    [Theory]
    [InlineData("AU", 1)]
    [InlineData("AU", 2)]
    [InlineData("NZ", 1)]
    [InlineData("NZ", 2)]
    public void RandomExamplesRespectTheApiRulesAcrossSeeds(string country, int adultCount)
    {
        var controls = SnapshotExampleData.FullControls();
        var assetTypes = new HashSet<int>();
        var incomeTypes = new HashSet<int>();
        var liabilityTypes = new HashSet<int>();

        for (var seed = 0; seed < 100; seed++)
        {
            var book = SnapshotExamples.Create(5001, Contacts(adultCount), controls, country, seed);
            CheckRules(book, country, adultCount);
            assetTypes.UnionWith(book.Attributes.Assets.Select(asset => asset.AssetTypeId!.Value));
            incomeTypes.UnionWith(book.Attributes.Incomes.Select(income => income.IncomeTypeId!.Value));
            liabilityTypes.UnionWith(book.Attributes.Liabilities.Select(liability => liability.LiabilityTypeId!.Value));
        }

        Assert.Contains(18, assetTypes);
        Assert.Contains(24, assetTypes);
        Assert.Contains(14, incomeTypes);
        Assert.Contains(7, liabilityTypes);
        Assert.Contains(8, liabilityTypes);
        Assert.Contains(25, liabilityTypes);
        if (country == "NZ")
        {
            Assert.Contains(32, assetTypes);
            Assert.Contains(27, incomeTypes);
        }
    }

    private static void CheckRules(SnapshotWorkbook book, string country, int adultCount, bool sized = false)
    {
        var attributes = book.Attributes;
        var properties = attributes.Assets.Where(asset => asset.AssetTypeId == 1).ToArray();
        if (!sized) Assert.Single(properties);

        foreach (var property in properties)
        {
            Assert.Null(property.Description);
            Assert.Equal(FinancialSnapshotAsset_valueBasis.ApplicantEstimate, property.ValueBasis);
            Assert.NotNull(property.PropertyTypeId);
            Assert.Equal(FinancialSnapshotAsset_propertyPrimaryPurpose.PurchaseInvestment, property.PropertyPrimaryPurpose);
            var address = Assert.Single(attributes.Addresses, address => address.Lid == property.Address?.Lid);
            Assert.Equal(country == "NZ" ? "New Zealand" : "Australia", address.Country);
            Assert.Matches("^[0-9]{4}$", address.PostCode);
        }

        foreach (var asset in attributes.Assets)
        {
            Assert.True(asset.Value >= 0);
            if (asset.AssetTypeId != 1)
            {
                Assert.False(string.IsNullOrWhiteSpace(asset.Description));
                Assert.Null(asset.Address);
                Assert.Null(asset.PropertyTypeId);
                Assert.Null(asset.PropertyPrimaryPurpose);
            }

            if (asset.AssetTypeId is 5 or 7 or 15 or 20 or 21 or 24 or 28 or 31)
            {
                Assert.Matches("^[0-9]+$", asset.AccountNumber);
                if (country == "AU") Assert.Matches("^[0-9]{3}-[0-9]{3}$", asset.Bsb);
                else Assert.Null(asset.Bsb);
            }
            else
            {
                Assert.Null(asset.AccountName);
                Assert.Null(asset.AccountNumber);
                Assert.Null(asset.Bsb);
            }

            if (asset.AssetTypeId == 18)
            {
                Assert.InRange(asset.AssetSubTypeId!.Value, 1, 7);
                Assert.False(string.IsNullOrWhiteSpace(asset.VehicleMake));
                Assert.InRange(asset.VehicleYear!.Value, 1886, DateTime.Today.Year);
            }
            else
            {
                Assert.Null(asset.AssetSubTypeId);
                Assert.Null(asset.VehicleMake);
                Assert.Null(asset.VehicleYear);
            }

            if (country == "AU") Assert.True(asset.AssetTypeId < 32);
        }

        Assert.Equal(attributes.Employments.Count + properties.Length, attributes.Addresses.Count);
        if (!sized) Assert.Equal(adultCount, attributes.Employments.Count);
        foreach (var job in attributes.Employments)
        {
            Assert.False(string.IsNullOrWhiteSpace(job.EmployerName));
            Assert.Equal(FinancialSnapshotEmployment_employerType.Private, job.EmployerType);
            Assert.Matches(country == "NZ" ? @"^\+64[34679][0-9]{7}$" : @"^\+61[2378][0-9]{8}$", job.EmployerPhone);

            var employerAddress = Assert.Single(attributes.Addresses, address => address.Lid == job.Address?.Lid);
            Assert.DoesNotContain(properties, property => property.Address.Lid == employerAddress.Lid);
            Assert.Single(attributes.Employments, employment => employment.Address?.Lid == employerAddress.Lid);
            Assert.Equal(country == "NZ" ? "New Zealand" : "Australia", employerAddress.Country);
            Assert.False(string.IsNullOrWhiteSpace(employerAddress.StreetAddress));
            Assert.False(string.IsNullOrWhiteSpace(employerAddress.Suburb));
            Assert.False(string.IsNullOrWhiteSpace(employerAddress.State));
            Assert.Matches("^[0-9]{4}$", employerAddress.PostCode);
            Assert.Contains(employerAddress.StreetAddress, employerAddress.FormattedAddress);

            Assert.Equal(FinancialSnapshotEmployment_employmentType.PAYG, job.EmploymentType);
            Assert.NotNull(job.DateStarted);
            Assert.Null(job.DateEnded);
        }

        foreach (var income in attributes.Incomes)
        {
            Assert.True(income.Value > 0);
            Assert.NotNull(income.Frequency);
            Assert.False(string.IsNullOrWhiteSpace(income.Description));
            Assert.DoesNotContain(income.IncomeTypeId!.Value, new[] { 1, 2, 3, 34, 35 });

            if (income.IncomeTypeId == 23)
            {
                Assert.Contains(properties, property => property.Lid == income.LinkedAsset?.Lid);
                Assert.Equal(country == "NZ" ? FinancialSnapshotIncome_nzRentalType.RentalIncome : null, income.NzRentalType);
                Assert.Null(income.Employment);
                if (country == "NZ")
                    Assert.Contains(book.Lookups, item => item.Resource == "rental-verification-types" && item.Id == income.NzRentalVerificationTypeId);
                else Assert.Null(income.NzRentalVerificationTypeId);
            }
            else
            {
                Assert.Null(income.LinkedAsset);
                Assert.Null(income.NzRentalType);
                Assert.Null(income.NzRentalVerificationTypeId);
            }

            if (income.IncomeTypeId is 19 or 14 or 16 or 22 or 26) Assert.NotNull(income.Employment);
            if (income.Employment != null)
            {
                var job = Assert.Single(attributes.Employments, job => job.Lid == income.Employment.Lid);
                Assert.Equal(job.ContactId, Assert.Single(income.Ownership).Id);
                Assert.NotNull(income.IncomeVerificationId);
                Assert.Contains(income.IncomeVerificationId.Value, country == "NZ" ? new[] { 21, 22 } : new[] { 1, 8, 9 });
            }
            else Assert.Null(income.IncomeVerificationId);

            if (income.NzIsGross.HasValue && income.IncomeTypeId != 19)
                Assert.Equal(income.IncomeTypeId != 27, income.NzIsGross);
            if (country == "AU")
            {
                Assert.DoesNotContain(income.IncomeTypeId.Value, new[] { 24, 25, 26, 27 });
                Assert.Null(income.NzIsGross);
                Assert.Null(income.NzRentalType);
                Assert.Null(income.NzRentalVerificationTypeId);
            }
        }

        foreach (var liability in attributes.Liabilities)
        {
            Assert.True(liability.Value >= 0);
            Assert.True(liability.Repayment > 0);
            Assert.NotNull(liability.RepaymentFrequency);
            Assert.False(string.IsNullOrWhiteSpace(liability.CreditorName));
            Assert.Null(liability.NzInterestOnlyStartDate);
            Assert.Null(liability.NzInterestOnlyTermMonths);

            if (liability.LiabilityTypeId == 21)
            {
                var property = Assert.Single(properties, property => property.Lid == liability.LinkedAsset?.Lid);
                Assert.True(liability.Value < property.Value);
                Assert.InRange(liability.InterestRate!.Value, 0, 100);
                Assert.Equal(FinancialSnapshotLiability_loanRepaymentType.PrincipalInterest, liability.LoanRepaymentType);
                if (country == "AU")
                {
                    Assert.InRange(liability.LoanTerm!.Value, 0, 100);
                    Assert.Null(liability.NzLoanStartDate);
                    Assert.Null(liability.NzDocumentedLoanTermMonths);
                }
                else
                {
                    Assert.Null(liability.LoanTerm);
                    Assert.NotNull(liability.NzLoanStartDate);
                    Assert.InRange(liability.NzDocumentedLoanTermMonths!.Value, 1, 1200);
                }
            }
            else Assert.Null(liability.LinkedAsset);

            if (liability.LiabilityTypeId == 7 && liability.LiabilitySubTypeId.HasValue)
                Assert.Contains(book.Lookups, item => item.Resource == "liability-sub-types"
                    && item.ParentId == 7 && item.Id == liability.LiabilitySubTypeId);
            else Assert.Null(liability.LiabilitySubTypeId);

            if (liability.LiabilityTypeId is 7 or 17) Assert.True(liability.Limit >= liability.Value);
            if (liability.LiabilityTypeId != 25) Assert.Null(liability.IsClearingFromThisLoan);
            if (country == "NZ" && liability.LiabilityTypeId == 8) Assert.Single(liability.Ownership);
        }

        Assert.All(attributes.Expenses, expense =>
        {
            Assert.True(expense.Value > 0 && expense.Value < 1e12);
            Assert.NotNull(expense.Frequency);
            Assert.DoesNotContain(expense.ExpenseTypeId!.Value, new[] { 1, 8, 31, 33, 34, 35, 42 });
        });

        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            var columns = SnapshotWorkbook.Columns(sheet);
            var lids = book.Rows(sheet).Cast<object>().Select(row => (string)columns.Single(c => c.Header == "lid").Get(row)).ToArray();
            Assert.Equal(lids.Length, lids.Distinct().Count());
            Assert.All(lids, lid => Assert.False(string.IsNullOrWhiteSpace(lid)));

            foreach (var row in book.Rows(sheet))
            {
                foreach (var column in columns)
                {
                    var value = column.Get(row);
                    if (value == null) continue;

                    if (SnapshotControlData.Columns.TryGetValue(column.Header, out var resource))
                        Assert.Contains(book.Lookups, item => item.Resource == resource && item.Id == (int)value);
                    if (SnapshotLinkData.IsContactColumn(column.Header))
                        Assert.InRange((int)value, 1001, 1000 + adultCount);
                    if (value is string text)
                        Assert.Matches(@"^([a-zA-Z0-9 ,!@#$%^*()\[\]{}:'"",.;?\\/_+-]|&(?=\s))*$", text);
                }
            }
        }
    }

    [Theory]
    [InlineData("AU", 1)]
    [InlineData("AU", 2)]
    [InlineData("NZ", 1)]
    [InlineData("NZ", 2)]
    public async Task ExplicitCountsKeepExactTotalsValidLinksAndRepeatableData(string country, int adultCount)
    {
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var controls = SnapshotExampleData.FullControls();
        var seen = new HashSet<string>();

        foreach (var count in new[] { 1, 2, 3, 10, 25, 100, SnapshotExamples.MaximumItemCount })
        {
            var book = SnapshotExamples.Create(5001, Contacts(adultCount), controls, country, seed: 731, itemCount: count);
            Assert.Equal(count, SnapshotWorkbook.Collections.Sum(sheet => book.Rows(sheet).Count));
            CheckRules(book, country, adultCount, sized: true);
            Assert.Equal(book.Attributes.Expenses.Count, book.Attributes.Expenses.Select(expense => expense.ExpenseTypeId).Distinct().Count());
            Assert.All(book.Attributes.Employments.GroupBy(job => job.ContactId), jobs =>
                Assert.Single(jobs, job => job.EmploymentStatus == FinancialSnapshotEmployment_employmentStatus.PrimaryEmployment));
            Assert.All(SnapshotWorkbook.Collections, sheet => Assert.Equal("Merge", book.Mode(sheet)));
            seen.UnionWith(SnapshotWorkbook.Collections.Where(sheet => book.Rows(sheet).Count > 0));

            var repeated = SnapshotExamples.Create(5001, Contacts(adultCount), controls, country, seed: 731, itemCount: count);
            Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(book.ToDocument()),
                await KiotaJsonSerializer.SerializeAsStringAsync(repeated.ToDocument()));
        }

        Assert.Equal(SnapshotWorkbook.Collections.Order(), seen.Order());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1001)]
    public void ExplicitCountMustBeInTheSupportedRange(int count) =>
        Assert.Throws<ArgumentOutOfRangeException>(() => SnapshotExamples.Create(5001, Contacts(2),
            SnapshotExampleData.FullControls(), "AU", itemCount: count));

    [Theory]
    [InlineData("AU")]
    [InlineData("NZ")]
    public async Task SeedsRepeatDataAndChangeTypesAcrossAllFinancialCollections(string country)
    {
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var controls = SnapshotExampleData.FullControls();
        var contacts = Contacts(2);
        var first = SnapshotExamples.Create(5001, contacts, controls, country, seed: 731);
        var repeated = SnapshotExamples.Create(5001, contacts, controls, country, seed: 731);
        Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(first.ToDocument()), await KiotaJsonSerializer.SerializeAsStringAsync(repeated.ToDocument()));

        var samples = Enumerable.Range(1, 30).Select(seed => SnapshotExamples.Create(5001, contacts, controls, country, seed)).ToArray();
        foreach (var (sheet, header) in new[] { ("Assets", "assetTypeId"), ("Incomes", "incomeTypeId"),
            ("Expenses", "expenseTypeId"), ("Liabilities", "liabilityTypeId") })
        {
            var column = SnapshotWorkbook.Columns(sheet).Single(column => column.Header == header);
            var combinations = samples.Select(book => string.Join(",", book.Rows(sheet).Cast<object>().Select(column.Get).OrderBy(value => value))).Distinct();
            Assert.True(combinations.Count() > 5, $"Expected varied types in {sheet}.");
        }
    }

    [Theory]
    [InlineData("AU", null)]
    [InlineData("NZ", null)]
    [InlineData("AU", 100)]
    [InlineData("NZ", 100)]
    public async Task RandomWorkbookRoundTripsWithLookupValidation(string country, int? count)
    {
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var source = SnapshotExamples.Create(5001, Contacts(2), SnapshotExampleData.FullControls(), country, seed: 99, itemCount: count);
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");

        try
        {
            source.Save(path);
            var loaded = SnapshotWorkbook.Load(path);
            Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(source.ToDocument()), await KiotaJsonSerializer.SerializeAsStringAsync(loaded.ToDocument()));

            using var excel = new XLWorkbook(path);
            Assert.Equal(3, excel.Worksheet("Contact Lookups").LastRowUsed()!.RowNumber());
            Assert.Contains("99", loaded.Source);
            Assert.All(SnapshotWorkbook.Collections, sheet => Assert.NotEmpty(excel.Worksheet(sheet).DataValidations));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Theory]
    [InlineData("asset-types", 1)]
    [InlineData("asset-types", 15)]
    [InlineData("income-types", 19)]
    [InlineData("income-types", 23)]
    [InlineData("liability-types", 21)]
    [InlineData("expense-types", 17)]
    [InlineData("property-types", 13)]
    [InlineData("income-verification-types", 1)]
    public void MissingCoreTypesDoNotFallBackToAnIncompatibleId(string resource, int id)
    {
        var controls = SnapshotExampleData.MinimalControls();
        var required = controls.Single(item => item.Resource == resource && item.Id == id);
        controls.Remove(required);
        controls.Add(required with { Id = 9999 });

        var error = Assert.Throws<InvalidOperationException>(() => SnapshotExamples.Create(5001, Contacts(2), controls, "AU", seed: 42));
        Assert.Contains(resource, error.Message);
    }

    [Theory]
    [InlineData("income-verification-types")]
    [InlineData("rental-verification-types")]
    public void NewZealandExamplesRequireAppropriateVerificationControls(string resource)
    {
        var controls = SnapshotExampleData.MinimalControls();
        controls.RemoveAll(item => item.Resource == resource && (resource == "rental-verification-types" || item.Id == 21));

        var error = Assert.Throws<InvalidOperationException>(() => SnapshotExamples.Create(5001, Contacts(2), controls, "NZ", seed: 42));
        Assert.Contains(resource, error.Message);
    }

    [Fact]
    public void VehiclesNeedCompatibleSubtypesButCardsCanOmitThem()
    {
        var controls = SnapshotExampleData.FullControls();
        controls.RemoveAll(item => item.Resource is "asset-sub-types" or "liability-sub-types");
        controls.Add(new("asset-sub-types", 9999, "Unknown subtype"));
        controls.Add(new("liability-sub-types", 4, "Wrong parent", "liability-types", 18));

        var cards = new List<FinancialSnapshotLiability>();
        for (var seed = 0; seed < 20; seed++)
        {
            var book = SnapshotExamples.Create(5001, Contacts(2), controls, "AU", seed);
            Assert.DoesNotContain(book.Attributes.Assets, asset => asset.AssetTypeId == 18);
            cards.AddRange(book.Attributes.Liabilities.Where(liability => liability.LiabilityTypeId == 7));
        }
        Assert.NotEmpty(cards);
        Assert.All(cards, card => Assert.Null(card.LiabilitySubTypeId));
    }
}
