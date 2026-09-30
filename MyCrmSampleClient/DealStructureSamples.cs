using System.Threading.Tasks;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.Kiota.Models;
using Spectre.Console;

namespace MyCrmSampleClient;

public partial class Samples
{
    [Sample("get-deal-structure", "GET /jsonapi/deal-structures/{id}")]
    public async Task RunGetDealStructureSample()
    {
        if (!_console.RequireAdviserContactId("get-deal-structure")) return;
        if (!_console.RequireScope("api.deal-structures.read", "get-deal-structure")) return;

        var structureId = AnsiConsole.Ask<int>("Deal structure ID:");
        var response = await _console.Client.Jsonapi.DealStructures[structureId].GetAsync();
        if (response?.Data is not { } structure)
        {
            AnsiConsole.MarkupLine("[Yellow]No deal structure was returned for the requested ID.[/]");
            return;
        }

        WriteDealStructure(structure);
    }

    [Sample("create-deal-structure", "POST /jsonapi/deal-structures, then GET the created structure")]
    public async Task RunCreateDealStructureSample()
    {
        if (!_console.RequireAdviserContactId("create-deal-structure")) return;
        if (!_console.RequireScope("api.deal-structures.create", "create-deal-structure")) return;
        if (!_console.RequireScope("api.deal-structures.read", "create-deal-structure")) return;
        if (!_console.State.TryGetDealId("create-deal-structure", out var dealId)) return;

        AnsiConsole.WriteLine(
            "The deal must be in pre-submission. This creates a 30-year, variable principal-and-interest structure.");
        var amount = AnsiConsole.Ask<double>("Loan amount:");
        var interestRate = AnsiConsole.Ask<double>("Interest rate as a decimal (e.g. 0.055 for 5.5%):");
        var paymentAmount = AnsiConsole.Ask<double>("Monthly repayment amount:");
        var created = await _console.Client.Jsonapi.DealStructures.PostAsync(new DealStructureCreateDocument
        {
            Data = new DealStructureCreate
            {
                Type = "deal-structures",
                Attributes = new DealStructureAttributes
                {
                    Amount = amount,
                    LoanStructureType = "PrincipalInterest",
                    LoanTermYears = 30,
                    InterestOnlyTermYears = 0,
                    RateType = "Variable",
                    InterestRate = interestRate,
                    PaymentAmount = paymentAmount,
                    RepaymentFrequency = "Monthly"
                },
                Relationships = new DealStructureCreateRelationships
                {
                    Deal = new RelationshipsSingleDocument
                    {
                        Data = new ResourceIdentifier { Type = "deals", Id = dealId.ToString() }
                    }
                }
            }
        });

        var structureId = int.Parse(created.Data.Id);
        var response = await _console.Client.Jsonapi.DealStructures[structureId].GetAsync();
        WriteDealStructure(response.Data);
    }

    [Sample("patch-deal-structure", "PATCH /jsonapi/deal-structures/{id}, then GET to read back")]
    public async Task RunPatchDealStructureSample()
    {
        if (!_console.RequireAdviserContactId("patch-deal-structure")) return;
        if (!_console.RequireScope("api.deal-structures.update", "patch-deal-structure")) return;
        if (!_console.RequireScope("api.deal-structures.read", "patch-deal-structure")) return;

        AnsiConsole.WriteLine("The structure's deal must be in pre-submission. Omitted attributes remain unchanged.");
        var structureId = AnsiConsole.Ask<int>("Deal structure ID:");
        var paymentAmount = AnsiConsole.Ask<double>("New repayment amount:");
        await _console.Client.Jsonapi.DealStructures[structureId].PatchAsync(new DealStructurePatchDocument
        {
            Data = new DealStructurePatch
            {
                Type = "deal-structures",
                Id = structureId.ToString(),
                Attributes = new DealStructureAttributes { PaymentAmount = paymentAmount }
            }
        });

        var response = await _console.Client.Jsonapi.DealStructures[structureId].GetAsync();
        WriteDealStructure(response.Data);
    }

    private void WriteDealStructure(DealStructure structure)
    {
        var table = _console.CreateTable("Structure Id", "Amount", "Interest Rate", "Repayment", "Frequency");
        table.AddRow(
            _console.Cell(structure.Id),
            _console.Cell(structure.Attributes?.Amount?.ToString()),
            _console.Cell(structure.Attributes?.InterestRate?.ToString()),
            _console.Cell(structure.Attributes?.PaymentAmount?.ToString()),
            _console.Cell(structure.Attributes?.RepaymentFrequency));
        _console.WriteTable(table);
    }
}
