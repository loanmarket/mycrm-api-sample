using System.Reflection;
using System.Net;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Spectre.Console;
using Spectre.Console.Testing;
using Xunit;

namespace MyCrmSampleClient.Tests;

[CollectionDefinition("Menu console", DisableParallelization = true)]
public sealed class MenuConsoleCollection;

[Collection("Menu console")]
public sealed class MenuNavigationTests : IDisposable
{
    private readonly TestConsole _terminal = new();
    private readonly IAnsiConsole _originalConsole = AnsiConsole.Console;
    private readonly string _originalBaseUrl = JsonApiFluentContext.BaseUrl;
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-menu-" + Guid.NewGuid().ToString("N"));

    public MenuNavigationTests()
    {
        _terminal.Interactive().SupportsAnsi(true);
        AnsiConsole.Console = _terminal;
        JsonApiFluentContext.BaseUrl = "https://example.test";
    }

    public void Dispose()
    {
        AnsiConsole.Console = _originalConsole;
        JsonApiFluentContext.BaseUrl = _originalBaseUrl;
        _terminal.Dispose();

        if (Directory.Exists(_directory)) Directory.Delete(_directory, true);
    }

    [Theory]
    [InlineData("selection")]
    [InlineData("text")]
    [InlineData("confirmation")]
    public void EscapeCancelsAnyPromptAndTheNextPromptStillWorks(string kind)
    {
        _terminal.Input.PushKey(ConsoleKey.Escape);

        Assert.Throws<MenuBackException>(() =>
        {
            if (kind == "selection") MenuPrompt.Show(new SelectionPrompt<string>().AddChoices("One", "Two", "Back"));
            else if (kind == "text") MenuPrompt.Ask<string>("Name:");
            else MenuPrompt.Confirm("Continue?");
        });

        _terminal.Input.PushTextWithEnter("42");
        Assert.Equal(42, MenuPrompt.Ask<int>("Next value:"));
        Assert.False(_terminal.Input.IsKeyAvailable());
    }

    [Fact]
    public void EscapeDiscardsPartialTextInsteadOfSubmittingIt()
    {
        _terminal.Input.PushText("unfinished-file.xlsx");
        _terminal.Input.PushKey(ConsoleKey.Escape);

        Assert.Throws<MenuBackException>(() => MenuPrompt.Ask<string>("Spreadsheet path:"));

        _terminal.Input.PushTextWithEnter("next-file.xlsx");
        Assert.Equal("next-file.xlsx", MenuPrompt.Ask<string>("Spreadsheet path:"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConfirmationKeepsItsConfiguredEnterDefault(bool defaultValue)
    {
        _terminal.Input.PushKey(ConsoleKey.Enter);

        Assert.Equal(defaultValue, MenuPrompt.Confirm("Overwrite?", defaultValue));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task TopLevelEscapeAsksBeforeExitingAndEnterConfirms(bool pressEnter)
    {
        _terminal.Input.PushKey(ConsoleKey.Escape);
        if (pressEnter) _terminal.Input.PushKey(ConsoleKey.Enter);
        else _terminal.Input.PushTextWithEnter("y");

        await RunMenu();

        Assert.Contains("Exit the sample application?", _terminal.Output);
        Assert.Contains("Exiting sample menu", _terminal.Output);
    }

    [Theory]
    [InlineData("no")]
    [InlineData("escape")]
    public async Task DecliningExitReturnsToTheMainMenu(string response)
    {
        _terminal.Input.PushKey(ConsoleKey.Escape);
        if (response == "no") _terminal.Input.PushTextWithEnter("n");
        else _terminal.Input.PushKey(ConsoleKey.Escape);

        SelectSample("financial-snapshots");
        _terminal.Input.PushKey(ConsoleKey.Escape);
        QueueExit();

        await RunMenu();

        Assert.Contains("Financial snapshots", _terminal.Output);
        Assert.Equal(2, CountExitPrompts());
    }

    [Fact]
    public async Task EscapeFromFinancialMenuReturnsToMainWithoutAnExtraPause()
    {
        SelectSample("financial-snapshots");
        _terminal.Input.PushKey(ConsoleKey.Escape);
        QueueExit();

        await RunMenu();

        Assert.Equal(1, CountExitPrompts());
        Assert.DoesNotContain("Press any key", _terminal.Output);
        Assert.DoesNotContain("Completed", _terminal.Output);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task EscapeFromFinancialActionReturnsToItsMenuBeforeMain(int action)
    {
        SelectSample("financial-snapshots");
        for (var i = 0; i < action; i++) _terminal.Input.PushKey(ConsoleKey.DownArrow);
        _terminal.Input.PushKey(ConsoleKey.Enter);

        _terminal.Input.PushKey(ConsoleKey.Escape); // Cancel the action's first input.
        _terminal.Input.PushKey(ConsoleKey.Escape); // Leave the financial menu.
        QueueExit();

        await RunMenu();

        Assert.Equal(1, CountExitPrompts());
        Assert.DoesNotContain("Request failed", _terminal.Output);
        Assert.DoesNotContain("Press any key", _terminal.Output);
    }

    [Fact]
    public async Task EscapeFromAnotherSampleReturnsToMainWithoutAnApiRequest()
    {
        SelectSample("get-deal-structure");
        _terminal.Input.PushKey(ConsoleKey.Escape);
        QueueExit();

        await RunMenu();

        Assert.Contains("Deal structure ID:", _terminal.Output);
        Assert.DoesNotContain("Completed", _terminal.Output);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task ImportOnlyPostsTheEnteredValuesToTheNominatedGroup(bool rejectedByServer, bool acceptWorkbookGroup)
    {
        Directory.CreateDirectory(_directory);
        var book = new SnapshotWorkbook { ContactGroupId = 5001 };
        book.SetMode("Assets", "Replace");
        for (var i = 0; i < 2; i++)
            book.Attributes.Assets.Add(new FinancialSnapshotAsset
            {
                Lid = "  repeated lid  ", AssetId = -1, AssetTypeId = 99999, Value = -7,
                Description = "  NULL  ", Address = new() { Lid = " missing-address " },
                Ownership = [new() { Id = 9999 }, new() { Id = 9999 }]
            });

        book.Attributes.Incomes.Add(new FinancialSnapshotIncome
        {
            Lid = "income", LinkedAsset = new() { Id = 0, Lid = "unknown-asset" },
            Employment = new() { Lid = "unknown-employment" }
        });
        book.Attributes.Addresses.Add(new FinancialSnapshotAddress { Lid = "unreferenced" });
        var path = Path.Combine(_directory, "import.xlsx");
        book.Save(path);

        string expectedRequest = null;
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method); // No contact or control-data GETs.
            var expectedGroup = acceptWorkbookGroup ? 5001 : 5002;
            Assert.Equal($"https://example.test/jsonapi/contact-groups/{expectedGroup}/financial-snapshot", request.RequestUri!.AbsoluteUri);
            Assert.Equal(expectedRequest, request.Content!.ReadAsStringAsync().GetAwaiter().GetResult());

            return rejectedByServer
                ? """{"errors":[{"status":"422","detail":"Snapshot rejected by the service."}]}"""
                : """{"data":{"type":"financial-snapshots","attributes":{"operations":{"created":[],"updated":[],"deleted":[]}}}}""";
        }, rejectedByServer ? HttpStatusCode.UnprocessableEntity : HttpStatusCode.OK);
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var client = new MyCrmApiClient(adapter);
        expectedRequest = await KiotaJsonSerializer.SerializeAsStringAsync(book.ToDocument());
        var console = new SampleConsole(client, new MyCrmConfig { AdviserContactId = 123 },
            Path.Combine(_directory, "state.json"), "api.contactgroups.financialsnapshot");
        console.UpdateState(new SampleState { LastContactGroupId = 5999 });

        _terminal.Input.PushKey(ConsoleKey.Enter); // Import spreadsheet.
        _terminal.Input.PushTextWithEnter(path);
        if (acceptWorkbookGroup) _terminal.Input.PushKey(ConsoleKey.Enter); // Workbook takes priority over saved state.
        else _terminal.Input.PushTextWithEnter("5002"); // Deliberately differs from workbook metadata.
        _terminal.Input.PushKey(ConsoleKey.Enter); // Use workbook modes.
        _terminal.Input.PushTextWithEnter("n"); // No full request display.
        _terminal.Input.PushTextWithEnter("y"); // Confirm submission to the nominated group.
        _terminal.Input.PushKey(ConsoleKey.Escape);

        await Assert.ThrowsAsync<MenuBackException>(() => new Samples(console).RunFinancialSnapshotsSample().WaitAsync(TimeSpan.FromSeconds(10)));

        Assert.Single(handler.Requests);
        Assert.False(_terminal.Input.IsKeyAvailable());
        Assert.Contains(rejectedByServer ? "Correct the reported fields" : "Export the current financials", _terminal.Output);
    }

    private void SelectSample(string name)
    {
        var names = typeof(Samples).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .OrderBy(method => method.MetadataToken)
            .Select(method => method.GetCustomAttribute<SampleAttribute>()?.Name)
            .Where(sample => sample != null)
            .ToArray();
        var index = Array.IndexOf(names, name);
        Assert.True(index >= 0);

        for (var i = 0; i < index; i++) _terminal.Input.PushKey(ConsoleKey.DownArrow);
        _terminal.Input.PushKey(ConsoleKey.Enter);
    }

    private void QueueExit()
    {
        _terminal.Input.PushKey(ConsoleKey.Escape);
        _terminal.Input.PushTextWithEnter("y");
    }

    private int CountExitPrompts() => _terminal.Output.Split("Exit the sample application?").Length - 1;

    private async Task RunMenu()
    {
        using var handler = new SnapshotTests.RecordingHandler(_ => throw new InvalidOperationException("Navigation must not make an API request."));
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var console = new SampleConsole(new MyCrmApiClient(adapter), new MyCrmConfig { AdviserContactId = 123 }, Path.Combine(_directory, "state.json"), "api");

        await console.Run().WaitAsync(TimeSpan.FromSeconds(10));

        Assert.Empty(handler.Requests);
        Assert.False(_terminal.Input.IsKeyAvailable());
    }
}
