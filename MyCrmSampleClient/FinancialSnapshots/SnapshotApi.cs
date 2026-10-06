using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using ApiAddress = MyCrmSampleClient.Kiota.Models.Address;

namespace MyCrmSampleClient.FinancialSnapshots;

/// <summary>Reads the complete group profile through generated JSON:API request builders.</summary>
public sealed class SnapshotApi(MyCrmApiClient client, string baseUrl)
{
    private readonly Uri _origin = new(baseUrl.TrimEnd('/') + "/");
    private readonly Dictionary<(string Type, string Id), IncludedResource> _included = new();

    public static string OwnershipFilter(IEnumerable<int> contactIds)
    {
        var ids = contactIds.Distinct().OrderBy(x => x).ToArray();
        if (ids.Length == 0 || ids.Any(id => id <= 0)) throw new ArgumentException("At least one positive contact ID is required.");

        return "has(ownership,any(contact.id," + string.Join(",", ids.Select(id => "'" + id.ToString(CultureInfo.InvariantCulture) + "'")) + "))";
    }

    private string Url(string path, string include = null, string filter = null) =>
        new Uri(_origin, "jsonapi/" + path).AbsoluteUri + "?page[size]=100&sort=id" +
        (include == null ? "" : "&include=" + Uri.EscapeDataString(include)) +
        (filter == null ? "" : "&filter=" + Uri.EscapeDataString(filter));

    public Task<List<Contact>> ContactsAsync(int groupId) => ReadPages<Contact>(Url($"contact-groups/{groupId}/contacts"),
        async url => await client.Jsonapi.ContactGroups[groupId].Contacts.WithUrl(url).GetAsync());

    // Follow the server's links.next even when a page is shorter than the requested size.
    // Only the original API origin may receive the authenticated request.
    private async Task<List<T>> ReadPages<T>(string url, Func<string, Task<object>> fetch) where T : IncludedResource
    {
        var records = new Dictionary<(string Type, string Id), T>();
        var visited = new HashSet<string>(StringComparer.Ordinal);

        while (url != null)
        {
            var uri = new Uri(_origin, url);
            if (uri.Scheme != _origin.Scheme || uri.Authority != _origin.Authority || !uri.AbsolutePath.StartsWith(_origin.AbsolutePath, StringComparison.Ordinal))
                throw new InvalidOperationException("The API returned a pagination link outside the configured API host.");
            if (!visited.Add(uri.AbsoluteUri)) throw new InvalidOperationException("The API returned a repeated pagination link.");

            var doc = await fetch(uri.AbsoluteUri) ?? throw new InvalidOperationException("The API returned an empty document.");

            // Collection documents have the same shape, but Kiota does not give them a common interface.
            var docType = doc.GetType();
            var data = (IEnumerable<T>)docType.GetProperty("Data")!.GetValue(doc) ?? throw new InvalidOperationException("The API response has no data array.");

            foreach (var item in data)
            {
                if (string.IsNullOrEmpty(item.Type) || string.IsNullOrEmpty(item.Id)) throw new InvalidOperationException("A read resource has no type or ID.");
                records[(item.Type, item.Id)] = item;
                _included[(item.Type, item.Id)] = item;
            }

            foreach (var item in (IEnumerable<IncludedResource>)docType.GetProperty("Included")!.GetValue(doc) ?? []) _included[(item.Type, item.Id)] = item;

            var next = ((TopLevelLinks)docType.GetProperty("Links")!.GetValue(doc))?.Next;
            url = string.IsNullOrWhiteSpace(next) ? null : new Uri(uri, next).AbsoluteUri;
        }

        return records.Values.ToList();
    }

    public async Task<SnapshotWorkbook> ExportAsync(int groupId)
    {
        var contacts = await ContactsAsync(groupId);
        var book = new SnapshotWorkbook { ContactGroupId = groupId, Source = "Export" };
        AddContacts(book, contacts);
        if (contacts.Count == 0) return book; // Never send an unfiltered financial search.

        var filter = OwnershipFilter(contacts.Select(c => SnapshotWorkbook.PositiveId(c.Id)));
        var assets = await ReadPages<Asset>(Url("assets", "ownership.contact,addresses", filter), async u => await client.Jsonapi.Assets.WithUrl(u).GetAsync());
        var liabilities = await ReadPages<Liability>(Url("liabilities", "ownership.contact,linkedAsset", filter), async u => await client.Jsonapi.Liabilities.WithUrl(u).GetAsync());
        var incomes = await ReadPages<Income>(Url("incomes", "ownership.contact,linkedAsset,employment", filter), async u => await client.Jsonapi.Incomes.WithUrl(u).GetAsync());
        var expenses = await ReadPages<Expense>(Url("expenses", "ownership.contact", filter), async u => await client.Jsonapi.Expenses.WithUrl(u).GetAsync());

        var jobs = new Dictionary<string, Employment>(StringComparer.Ordinal);
        foreach (var contact in contacts)
        {
            var id = SnapshotWorkbook.PositiveId(contact.Id);
            foreach (var job in await ReadPages<Employment>(Url($"contacts/{id}/employments", "address,contact"), async u => await client.Jsonapi.Contacts[id].Employments.WithUrl(u).GetAsync()))
                jobs[job.Id] = job;
        }

        foreach (var asset in assets)
        {
            var row = Copy<FinancialSnapshotAsset>(asset.Attributes, ("ValuationBasis", "ValueBasis"));
            row.AssetId = SnapshotWorkbook.PositiveId(asset.Id);
            row.Lid = "asset-" + asset.Id;
            row.Ownership = Owners(asset.Relationships?.Ownership, "asset " + asset.Id);

            var addresses = asset.Relationships?.Addresses?.Data ?? throw new InvalidOperationException($"Asset {asset.Id}: missing addresses relationship.");
            if (addresses.Count > 1) throw new InvalidOperationException($"Asset {asset.Id} has several addresses; the snapshot supports one. Resolve this before exporting an importable workbook.");
            row.Address = Address(addresses.SingleOrDefault(), book);
            book.Attributes.Assets.Add(row);
        }

        foreach (var job in jobs.Values)
        {
            if (job.Relationships?.Address == null) throw new InvalidOperationException($"Employment {job.Id}: missing address relationship; export cannot safely preserve its address.");

            var row = Copy<FinancialSnapshotEmployment>(job.Attributes);
            row.EmploymentId = SnapshotWorkbook.PositiveId(job.Id);
            row.Lid = "employment-" + job.Id;
            row.ContactId = LinkedId(job.Relationships?.Contact, "contacts");
            row.Address = Address(job.Relationships?.Address?.Data, book);
            book.Attributes.Employments.Add(row);
        }

        foreach (var liability in liabilities)
        {
            var row = Copy<FinancialSnapshotLiability>(liability.Attributes, ("RepaymentFrequency", "RepaymentFrequencyValue"));
            row.LiabilityId = SnapshotWorkbook.PositiveId(liability.Id);
            row.Lid = "liability-" + liability.Id;
            row.Ownership = Owners(liability.Relationships?.Ownership, "liability " + liability.Id);

            var assetId = LinkedId(liability.Relationships?.LinkedAsset, "assets");
            row.LinkedAsset = assetId.HasValue ? new FinancialSnapshotAssetReference { Id = assetId } : null;
            book.Attributes.Liabilities.Add(row);
        }

        foreach (var income in incomes)
        {
            if (IsUnsupportedIncome(income.Attributes))
            {
                book.Notes.Add($"Income {income.Id} ({income.Attributes?.IncomeType}) is outside the snapshot contract and was excluded. It is preserved by collection replacement, except for linked asset deletion.");
                book.Excluded.Add(["incomes", income.Id, ExportText(income.Attributes?.IncomeType) ?? "", income.Attributes?.Value?.ToString(CultureInfo.InvariantCulture) ?? "", ExportText(income.Attributes?.Frequency) ?? "", ExportText(income.Attributes?.Description) ?? ""]);
                continue;
            }

            var row = Copy<FinancialSnapshotIncome>(income.Attributes, ("Frequency", "FrequencyValue"));
            row.IncomeId = SnapshotWorkbook.PositiveId(income.Id);
            row.Lid = "income-" + income.Id;
            row.Ownership = Owners(income.Relationships?.Ownership, "income " + income.Id);

            var assetId = LinkedId(income.Relationships?.LinkedAsset, "assets");
            var jobId = LinkedId(income.Relationships?.Employment, "employments");
            row.LinkedAsset = assetId.HasValue ? new FinancialSnapshotAssetReference { Id = assetId } : null;
            row.Employment = jobId.HasValue ? new FinancialSnapshotEmploymentReference { Id = jobId } : null;
            book.Attributes.Incomes.Add(row);
        }

        foreach (var expense in expenses)
        {
            if (expense.Attributes?.ExpenseTypeId is int typeId && SnapshotControlData.RepaymentExpenseTypes.Contains(typeId))
            {
                book.Notes.Add($"Expense {expense.Id} ({expense.Attributes.ExpenseType}) is represented by liability repayment fields and excluded from direct expense input.");
                book.Excluded.Add(["expenses", expense.Id, ExportText(expense.Attributes.ExpenseType) ?? "", expense.Attributes.Value?.ToString(CultureInfo.InvariantCulture) ?? "", ExportText(expense.Attributes.Frequency) ?? "", ExportText(expense.Attributes.Description) ?? ""]);
                continue;
            }

            var row = Copy<FinancialSnapshotExpense>(expense.Attributes, ("Frequency", "FrequencyValue"));
            row.ExpenseId = SnapshotWorkbook.PositiveId(expense.Id);
            row.Lid = "expense-" + expense.Id;
            row.Ownership = Owners(expense.Relationships?.Ownership, "expense " + expense.Id);
            book.Attributes.Expenses.Add(row);
        }

        book.Notes.Add("Review expense types against current control data before importing. The snapshot endpoint validates type availability.");

        return book;
    }

    private static bool IsUnsupportedIncome(IncomeAttributes a) =>
        (a?.IncomeCategory?.Contains("Addback", StringComparison.OrdinalIgnoreCase) ?? false) ||
        a?.IncomeType is "Addbacks" or "Net Profit Before Tax" or "Depreciation" or "Non-Cash Benefits" or "Non Cash Benefits" or "Non-Recurring Expenses" or "Non Recurring Expenses";

    // Some existing records use the literal NULL string for an absent value.
    private static string ExportText(string value) =>
        string.Equals(value?.Trim(), "NULL", StringComparison.OrdinalIgnoreCase) ? null : value;

    public static T Copy<T>(object source, params (string Target, string Source)[] aliases) where T : new()
    {
        if (source == null) throw new InvalidOperationException("A resource is missing its attributes; export cannot safely preserve its fields.");

        var target = new T();
        foreach (var property in typeof(T).GetProperties().Where(p => p.Name != "AdditionalData"))
        {
            var alias = aliases.FirstOrDefault(a => a.Target == property.Name).Source;
            var value = source.GetType().GetProperty(alias ?? property.Name)?.GetValue(source);

            // Older reads may expose only the display field. Accept it only if it is a
            // valid input enum below; never turn a populated frequency/valuation into null.
            if (value == null && alias != null) value = source.GetType().GetProperty(property.Name)?.GetValue(source);
            if (value is string text) value = ExportText(text);
            if (value == null) continue;

            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum)
            {
                if (!Enum.GetNames(type).Contains(value.ToString())) throw new InvalidOperationException($"Cannot export {property.Name} '{value}' as a snapshot enum.");
                value = Enum.Parse(type, value.ToString()!);
            }

            property.SetValue(target, value);
        }

        return target;
    }

    private List<FinancialSnapshotContactReference> Owners(RelationshipsMultipleDocument relationship, string label)
    {
        if (relationship?.Data == null || relationship.Data.Count == 0) throw new InvalidOperationException($"{label}: no ownership was returned.");

        var owners = relationship.Data.Select(Resolve<Owner>).ToArray();
        var ids = owners.Select(o => LinkedId(o.Relationships?.Contact, "contacts") ?? throw new InvalidOperationException($"{label}: an owner has no contact; cannot represent business ownership in a snapshot.")).Distinct().ToArray();

        // Ownership is shared equally between the referenced contacts; ignore reported percentages.
        return ids.Select(id => new FinancialSnapshotContactReference { Id = id }).ToList();
    }

    private T Resolve<T>(ResourceIdentifier reference) where T : IncludedResource
    {
        if (!_included.TryGetValue((reference.Type, reference.Id), out var resource))
            throw new InvalidOperationException($"Missing included {reference.Type}/{reference.Id}; export stopped to avoid dropping a relationship.");

        if (resource is not T typed)
            throw new InvalidOperationException($"Included {reference.Type}/{reference.Id} was deserialized as {resource.GetType().Name}, expected {typeof(T).Name}. Check the Swagger discriminator mapping before exporting.");

        return typed;
    }

    private FinancialSnapshotAddressReference Address(ResourceIdentifier reference, SnapshotWorkbook book)
    {
        if (reference == null) return null;

        var address = Resolve<IncludedResource>(reference);

        // Employment addresses and financial-address links have separate ID namespaces.
        var lid = address.Type + "-" + address.Id;
        if (book.Attributes.Addresses.All(a => a.Lid != lid))
        {
            var row = address switch
            {
                ApiAddress employmentAddress => Copy<FinancialSnapshotAddress>(employmentAddress.Attributes),
                FinancialAddress financialAddress => Copy<FinancialSnapshotAddress>(financialAddress.Attributes),
                _ => throw new InvalidOperationException($"Included address {reference.Type}/{reference.Id} was deserialized as {address.GetType().Name}, expected Address or FinancialAddress. Check the Swagger discriminator mapping before exporting.")
            };

            row.Lid = lid;
            book.Attributes.Addresses.Add(row);
        }

        return new FinancialSnapshotAddressReference { Lid = lid };
    }

    private static int? LinkedId(RelationshipsSingleDocument relationship, string type)
    {
        if (relationship == null) throw new InvalidOperationException($"Missing {type} relationship; cannot safely export.");
        if (relationship.Data == null) return null;
        if (relationship.Data.Type != type) throw new InvalidOperationException($"Expected {type} relationship, got {relationship.Data.Type}.");

        return SnapshotWorkbook.PositiveId(relationship.Data.Id);
    }

    public static void AddContacts(SnapshotWorkbook book, IEnumerable<Contact> contacts)
    {
        book.Contacts.AddRange(contacts);
    }

    public async Task<List<SnapshotLookup>> LookupsAsync()
    {
        var result = new List<SnapshotLookup>();

        void Add<T>(string resource, IEnumerable<T> rows, Func<T, ResourceIdentifier> parent = null) where T : IncludedResource
        {
            foreach (var row in rows)
            {
                var attributes = row.GetType().GetProperty("Attributes")!.GetValue(row);
                var label = (attributes?.GetType().GetProperty("Name") ?? attributes?.GetType().GetProperty("Description"))?.GetValue(attributes)?.ToString();
                var reference = parent?.Invoke(row);

                result.Add(new SnapshotLookup(resource, SnapshotWorkbook.PositiveId(row.Id), label ?? "", reference?.Type,
                    reference == null ? null : SnapshotWorkbook.PositiveId(reference.Id)));
            }
        }

        Add("asset-categories", await ReadPages<AssetCategory>(Url("asset-categories"), async u => await client.Jsonapi.AssetCategories.WithUrl(u).GetAsync()));
        Add("liability-categories", await ReadPages<LiabilityCategory>(Url("liability-categories"), async u => await client.Jsonapi.LiabilityCategories.WithUrl(u).GetAsync()));
        Add("income-categories", await ReadPages<IncomeCategory>(Url("income-categories"), async u => await client.Jsonapi.IncomeCategories.WithUrl(u).GetAsync()));
        Add("expense-categories", await ReadPages<ExpenseCategory>(Url("expense-categories"), async u => await client.Jsonapi.ExpenseCategories.WithUrl(u).GetAsync()));

        Add("asset-types", await ReadPages<AssetType>(Url("asset-types"), async u => await client.Jsonapi.AssetTypes.WithUrl(u).GetAsync()));
        Add("asset-sub-types", await ReadPages<AssetSubType>(Url("asset-sub-types"), async u => await client.Jsonapi.AssetSubTypes.WithUrl(u).GetAsync()));
        Add("expense-types", await ReadPages<ExpenseType>(Url("expense-types", "expenseCategory"), async u => await client.Jsonapi.ExpenseTypes.WithUrl(u).GetAsync()), t => t.Relationships?.ExpenseCategory?.Data);
        Add("income-types", await ReadPages<IncomeType>(Url("income-types", "incomeCategory"), async u => await client.Jsonapi.IncomeTypes.WithUrl(u).GetAsync()), t => t.Relationships?.IncomeCategory?.Data);

        var liabilities = await ReadPages<LiabilityType>(Url("liability-types", "liabilityCategory,liabilitySubTypes"), async u => await client.Jsonapi.LiabilityTypes.WithUrl(u).GetAsync());
        Add("liability-types", liabilities, t => t.Relationships?.LiabilityCategory?.Data);

        // Liability subtypes have no standalone collection route. Their parent type is
        // retained for the dependent Excel dropdown and for import validation.
        foreach (var type in liabilities)
            foreach (var reference in type.Relationships?.LiabilitySubTypes?.Data ?? [])
                Add("liability-sub-types", new[] { Resolve<LiabilitySubType>(reference) }, _ => new ResourceIdentifier { Type = "liability-types", Id = type.Id });

        Add("property-types", await ReadPages<PropertyType>(Url("property-types"), async u => await client.Jsonapi.PropertyTypes.WithUrl(u).GetAsync()));
        Add("income-verification-types", await ReadPages<IncomeVerificationType>(Url("income-verification-types"), async u => await client.Jsonapi.IncomeVerificationTypes.WithUrl(u).GetAsync()));
        Add("rental-verification-types", await ReadPages<RentalVerificationType>(Url("rental-verification-types"), async u => await client.Jsonapi.RentalVerificationTypes.WithUrl(u).GetAsync()));

        if (result.Count == 0) throw new InvalidOperationException("No control data was returned; cannot validate financial reference IDs.");

        return result;
    }
}
