using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.FinancialSnapshots;

public static class SnapshotExamples
{
    public static SnapshotWorkbook Create(int groupId, IEnumerable<Contact> contacts,
        IReadOnlyCollection<SnapshotLookup> lookups, string country, int? seed = null)
    {
        if (country is not ("AU" or "NZ")) throw new ArgumentException("Examples support AU and NZ groups.");

        return new Generator(groupId, contacts, lookups, country, seed ?? Random.Shared.Next(1, int.MaxValue)).Create();
    }

    private sealed class Generator
    {
        private readonly SnapshotWorkbook _book;
        private readonly IReadOnlyCollection<SnapshotLookup> _lookups;
        private readonly string _country;
        private readonly int _seed;
        private readonly int[] _adults;
        private readonly Faker _fake;

        public Generator(int groupId, IEnumerable<Contact> contacts, IReadOnlyCollection<SnapshotLookup> lookups, string country, int seed)
        {
            _lookups = lookups;
            _country = country;
            _seed = seed;
            _fake = new Faker("en_AU") { Random = new Randomizer(seed) };
            _book = new SnapshotWorkbook { ContactGroupId = groupId, Source = $"Fabricated example (seed {seed})" };
            _book.Lookups.AddRange(lookups);
            SnapshotApi.AddContacts(_book, contacts);

            _adults = _book.Contacts.Where(c => c.Attributes?.Role == ContactAttributes_role.Adult)
                .Select(c => SnapshotWorkbook.PositiveId(c.Id)).Distinct().Take(2).ToArray();
            if (_adults.Length == 0) throw new InvalidOperationException("The group needs at least one active Adult contact.");
        }

        public SnapshotWorkbook Create()
        {
            // Keep a linked property/rent/mortgage and employment/income example in every
            // workbook, then vary the additional types using their own field rules.
            AddProperty();
            AddDeposit(ChooseRequired("asset-types", "Savings / Savings Account", 15, 24), "savings");
            AddEmployments();
            AddRent();
            AddMortgage();
            AddExpense(ChooseRequired("expense-types", "Groceries", 17), "groceries");

            AddOtherAssets();
            AddOtherIncomes();
            AddOtherLiabilities();
            AddOtherExpenses();

            _book.Notes.Add($"Generated with Bogus using seed {_seed}. Reuse this seed with the same contacts, controls and generator version to repeat the example.");
            _book.Notes.Add("All balances, employers, accounts and addresses are fabricated. Real contact IDs and lookup IDs are used. Review before importing into a test group.");

            return _book;
        }

        private void AddProperty()
        {
            var location = _country == "NZ"
                ? _fake.PickRandom(new[] { ("Hamilton East", "Waikato", "3216"), ("Mount Eden", "Auckland", "1024"), ("Riccarton", "Canterbury", "8041") })
                : _fake.PickRandom(new[] { ("Carindale", "QLD", "4152"), ("Paddington", "NSW", "2021"), ("Richmond", "VIC", "3121") });

            _book.Attributes.Addresses.Add(new FinancialSnapshotAddress
            {
                Lid = "investment-address",
                StreetAddress = _fake.Address.StreetAddress(),
                Suburb = location.Item1,
                State = location.Item2,
                PostCode = location.Item3,
                Country = _country == "NZ" ? "New Zealand" : "Australia"
            });

            _book.Attributes.Assets.Add(new FinancialSnapshotAsset
            {
                Lid = "investment-property",
                AssetTypeId = ChooseRequired("asset-types", "Real Estate", 1),
                PropertyTypeId = ChooseRequired("property-types", "residential property", 13, 42, 46, 68),
                // The API forbids a description for real estate; the address describes it.
                Value = _fake.Random.Int(500, 1400) * 1000,
                PropertyPrimaryPurpose = FinancialSnapshotAsset_propertyPrimaryPurpose.PurchaseInvestment,
                ValuationBasis = FinancialSnapshotAsset_valuationBasis.ApplicantEstimate,
                Address = new FinancialSnapshotAddressReference { Lid = "investment-address" },
                Ownership = Owners()
            });
        }

        private void AddDeposit(int type, string lid)
        {
            _book.Attributes.Assets.Add(new FinancialSnapshotAsset
            {
                Lid = lid,
                AssetTypeId = type,
                Description = $"Household {Name("asset-types", type).ToLowerInvariant()}",
                Value = _fake.Random.Int(25, 800) * 100,
                Institution = Bank(),
                AccountName = $"{_fake.Name.LastName()} savings",
                Bsb = _country == "AU" ? _fake.Random.Replace("###-###") : null,
                AccountNumber = _fake.Random.Replace("00########"),
                Ownership = Owners()
            });
        }

        private void AddEmployments()
        {
            var salaryType = ChooseRequired("income-types", "Salary / Wages", 19);

            for (var i = 0; i < _adults.Length; i++)
            {
                var jobLid = "job-" + (i + 1);

                _book.Attributes.Employments.Add(new FinancialSnapshotEmployment
                {
                    Lid = jobLid,
                    ContactId = _adults[i],
                    EmployerName = _fake.Company.CompanyName(),
                    EmploymentRoleName = _fake.Name.JobTitle(),
                    DateStarted = new Date(_fake.Date.Between(new DateTime(2018, 1, 1), new DateTime(2024, 12, 31))),
                    EmploymentType = FinancialSnapshotEmployment_employmentType.PAYG,
                    EmploymentStatus = FinancialSnapshotEmployment_employmentStatus.PrimaryEmployment,
                    EmploymentBasis = FinancialSnapshotEmployment_employmentBasis.FullTime,
                    IsProbation = false
                });

                _book.Attributes.Incomes.Add(new FinancialSnapshotIncome
                {
                    Lid = "salary-" + (i + 1),
                    IncomeTypeId = salaryType,
                    Description = "Main employment salary",
                    Value = _fake.Random.Int(55, 140) * 100,
                    Frequency = Frequency.Monthly,
                    NzIsGross = true,
                    Employment = new FinancialSnapshotEmploymentReference { Lid = jobLid },
                    Ownership = [new FinancialSnapshotContactReference { Id = _adults[i] }]
                });
            }
        }

        private void AddRent()
        {
            _book.Attributes.Incomes.Add(new FinancialSnapshotIncome
            {
                Lid = "rent",
                IncomeTypeId = ChooseRequired("income-types", "Rental Income", 23),
                Description = "Investment property rent",
                Value = _fake.Random.Int(45, 100) * 10,
                Frequency = Frequency.Weekly,
                NzRentalType = FinancialSnapshotIncome_nzRentalType.RentalIncome,
                IsEvidenceOfTenancy = true,
                LinkedAsset = new FinancialSnapshotAssetReference { Lid = "investment-property" },
                Ownership = Owners()
            });
        }

        private void AddMortgage()
        {
            var value = Math.Round(_book.Attributes.Assets[0].Value!.Value * _fake.Random.Double(0.45, 0.8) / 1000) * 1000;
            var rate = Money(5, 7);
            var years = _fake.Random.Int(20, 30);
            var monthlyRate = rate / 1200;
            var repayment = Math.Round(value * monthlyRate / (1 - Math.Pow(1 + monthlyRate, -years * 12)), 2);

            _book.Attributes.Liabilities.Add(new FinancialSnapshotLiability
            {
                Lid = "investment-mortgage",
                LiabilityTypeId = ChooseRequired("liability-types", "Mortgage", 21),
                CreditorName = Bank(),
                AccountNumber = _fake.Random.Replace("00########"),
                Value = value,
                Limit = value + _fake.Random.Int(5, 25) * 1000,
                Repayment = repayment,
                RepaymentFrequency = FinancialSnapshotLiability_repaymentFrequency.Monthly,
                InterestRate = rate,
                InterestTaxDeductible = true,
                LoanRepaymentType = FinancialSnapshotLiability_loanRepaymentType.PrincipalInterest,
                MortgagePriority = FinancialSnapshotLiability_mortgagePriority.First,
                LoanTerm = _country == "AU" ? years : null,
                NzLoanStartDate = _country == "NZ" ? new Date(_fake.Date.Between(new DateTime(2020, 1, 1), new DateTime(2024, 12, 31))) : null,
                NzDocumentedLoanTermMonths = _country == "NZ" ? years * 12 : null,
                LinkedAsset = new FinancialSnapshotAssetReference { Lid = "investment-property" },
                Ownership = Owners()
            });
        }

        private void AddOtherAssets()
        {
            // Motor vehicle subtypes 1-7 belong to type 18 in the API reference data.
            // The current Swagger does not expose that parent link, so unknown subtypes
            // cannot safely be used by this generator.
            var vehicleSubtypes = Available("asset-sub-types", 1, 2, 3, 4, 5, 6, 7);
            var types = new List<int> { 2, 3, 4, 5, 7, 14, 17, 20, 25, 27, 28 };
            if (vehicleSubtypes.Length > 0) types.Add(18);
            if (_country == "NZ") types.AddRange([32, 33, 37]);

            foreach (var type in PickTypes("asset-types", types, 2, 4))
            {
                if (type is 5 or 7 or 20 or 28)
                {
                    AddDeposit(type, "asset-" + type);
                    continue;
                }

                var asset = new FinancialSnapshotAsset
                {
                    Lid = "asset-" + type,
                    AssetTypeId = type,
                    Description = $"Household {Name("asset-types", type).ToLowerInvariant()}",
                    Value = _fake.Random.Int(10, 500) * 100,
                    Ownership = Owners(random: true)
                };

                if (type == 18)
                {
                    asset.AssetSubTypeId = _fake.PickRandom(vehicleSubtypes);
                    asset.VehicleMake = asset.AssetSubTypeId == 2 ? "Honda" : _fake.PickRandom(new[] { "Toyota", "Mazda", "Hyundai" });
                    asset.VehicleYear = _fake.Random.Int(2015, 2024);
                    asset.Description = $"{asset.VehicleYear} {asset.VehicleMake} {Name("asset-sub-types", asset.AssetSubTypeId.Value)}";
                }

                _book.Attributes.Assets.Add(asset);
            }
        }

        private void AddOtherIncomes()
        {
            var types = new List<int> { 5, 10, 11, 14, 15, 16, 17, 18, 20, 30 };
            if (_country == "NZ") types.AddRange([26, 27]);

            foreach (var type in PickTypes("income-types", types, 1, 3))
            {
                var income = new FinancialSnapshotIncome
                {
                    Lid = "income-" + type,
                    IncomeTypeId = type,
                    Description = Name("income-types", type),
                    Value = Money(100, 1500),
                    Frequency = Frequency.Monthly,
                    NzIsGross = type != 27,
                    Ownership = Owners(random: true)
                };

                if (type is 14 or 15 or 16 or 17 or 18 or 20 or 26)
                {
                    var employment = _fake.PickRandom(_book.Attributes.Employments);
                    income.Employment = new FinancialSnapshotEmploymentReference { Lid = employment.Lid };
                    income.Ownership = [new FinancialSnapshotContactReference { Id = employment.ContactId }];
                }

                if (type is 5 or 14 or 30)
                {
                    income.Frequency = Frequency.Yearly;
                    income.Value = Money(1000, 10000);
                }

                _book.Attributes.Incomes.Add(income);
            }
        }

        private void AddOtherLiabilities()
        {
            var cardSubtypes = _lookups.Where(item => item.Resource == "liability-sub-types"
                && item.ParentResource == "liability-types" && item.ParentId == 7 && item.Id is >= 1 and <= 6)
                .Select(item => item.Id).Distinct().OrderBy(id => id).ToArray();
            var types = new List<int> { 8, 17, 18, 19, 25 };
            if (cardSubtypes.Length > 0) types.Add(7);

            foreach (var type in PickTypes("liability-types", types, 1, 3))
            {
                var value = Money(1000, type == 25 ? 3000 : 25000);
                var liability = new FinancialSnapshotLiability
                {
                    Lid = "liability-" + type,
                    LiabilityTypeId = type,
                    LiabilitySubTypeId = type == 7 ? _fake.PickRandom(cardSubtypes) : null,
                    CreditorName = type == 8 ? (_country == "NZ" ? "Inland Revenue" : "Australian Taxation Office") : Bank(),
                    AccountNumber = _fake.Random.Replace("00########"),
                    Value = value,
                    Repayment = Math.Round(value / _fake.Random.Int(24, 60), 2),
                    RepaymentFrequency = FinancialSnapshotLiability_repaymentFrequency.Monthly,
                    Ownership = type == 8 && _country == "NZ"
                        ? [new FinancialSnapshotContactReference { Id = _fake.PickRandom(_adults) }]
                        : Owners(random: true)
                };

                if (type is 7 or 17) liability.Limit = Math.Ceiling(value / 1000) * 1000 + 1000;
                if (type == 25) liability.IsClearingFromThisLoan = _fake.Random.Bool();

                _book.Attributes.Liabilities.Add(liability);
            }
        }

        private void AddOtherExpenses()
        {
            // Ordinary living costs only; liability-managed repayments are excluded.
            foreach (var type in PickTypes("expense-types", [2, 3, 7, 11, 12, 26, 30, 36, 37, 38, 43, 46, 47, 55, 56], 3, 6))
                AddExpense(type, "expense-" + type);
        }

        private void AddExpense(int type, string lid)
        {
            var yearly = type is 3 or 12 or 26 or 55 or 56;

            _book.Attributes.Expenses.Add(new FinancialSnapshotExpense
            {
                Lid = lid,
                ExpenseTypeId = type,
                Description = Name("expense-types", type),
                Value = type == 17 ? Money(120, 200) * _adults.Length : yearly ? Money(400, 3000) : Money(30, 250),
                Frequency = type == 17 ? Frequency.Weekly : yearly ? Frequency.Yearly : Frequency.Monthly,
                Ownership = Owners()
            });
        }

        private int[] Available(string resource, params int[] ids) => ids.Distinct().Where(id =>
            _lookups.Any(item => item.Resource == resource && item.Id == id && SnapshotControlData.IsSelectable(item, _lookups))).ToArray();

        private int ChooseRequired(string resource, string name, params int[] ids)
        {
            var choices = Available(resource, ids);
            if (choices.Length == 0)
                throw new InvalidOperationException($"Cannot generate the {name} example: {resource} does not contain a supported type (IDs: {string.Join(", ", ids)}).");

            return _fake.PickRandom(choices);
        }

        private IEnumerable<int> PickTypes(string resource, IEnumerable<int> ids, int min, int max)
        {
            var choices = Available(resource, ids.ToArray());
            if (choices.Length == 0) return [];

            return _fake.PickRandom(choices, Math.Min(choices.Length, _fake.Random.Int(min, max))).ToArray();
        }

        private List<FinancialSnapshotContactReference> Owners(bool random = false)
        {
            var ids = random && _fake.Random.Bool() ? [_fake.PickRandom(_adults)] : _adults;

            return ids.Select(id => new FinancialSnapshotContactReference { Id = id }).ToList();
        }

        private string Name(string resource, int id) => _lookups.First(item => item.Resource == resource && item.Id == id).Name;

        private string Bank() => _fake.PickRandom(new[] { "Example Mutual", "Sample Community Bank", "Fictional Savings Bank" });

        private double Money(double min, double max) => Math.Round(_fake.Random.Double(min, max), 2);
    }
}
