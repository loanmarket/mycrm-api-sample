using System.Reflection;
using System.Net;
using System.Text.Json;
using ClosedXML.Excel;
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
    [InlineData(3)]
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
    [InlineData("create-residential-task", "Assignee adviser ID:", false)]
    [InlineData("create-contact-group-task", "Assignee adviser ID:", false)]
    [InlineData("get-task", "Task ID:", false)]
    [InlineData("patch-task", "New title for task 7001:", false)]
    [InlineData("patch-task", "New task detail:", true)]
    [InlineData("task-progress", "New progress for task 7001:", false)]
    public async Task EscapeFromTaskPromptsReturnsToMainWithoutAnApiRequest(string sample, string prompt, bool enterTitle)
    {
        SelectSample(sample);
        if (enterTitle) _terminal.Input.PushTextWithEnter("Unsubmitted title");
        _terminal.Input.PushKey(ConsoleKey.Escape);
        QueueExit();

        await RunMenu(new SampleState { LastDealId = 6001, LastContactGroupId = 5001, LastTaskId = 7001 });

        Assert.Contains(prompt, _terminal.Output);
        Assert.Equal(1, CountExitPrompts());
        Assert.DoesNotContain($"Completed {sample}", _terminal.Output);
        Assert.DoesNotContain("Press any key", _terminal.Output);
    }

    [Fact]
    public async Task EscapeFromTaskPromotionReadsTheExistingTaskButDoesNotWrite()
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("https://example.test/jsonapi/tasks/7001", request.RequestUri!.AbsoluteUri);

            return """{"data":{"type":"tasks","id":"7001","relationships":{"contactGroups":{"data":[{"type":"contact-groups","id":"5001"}]}}}}""";
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var console = new SampleConsole(new MyCrmApiClient(adapter), new MyCrmConfig { AdviserContactId = 123 },
            Path.Combine(_directory, "state.json"), "api");
        console.UpdateState(new SampleState { LastContactGroupTaskId = 7001 });

        SelectSample("promote-contact-task");
        _terminal.Input.PushKey(ConsoleKey.Escape);
        QueueExit();

        await console.Run().WaitAsync(TimeSpan.FromSeconds(10));

        Assert.Single(handler.Requests);
        Assert.False(_terminal.Input.IsKeyAvailable());
        Assert.Contains("Residential deal ID to link:", _terminal.Output);
        Assert.Equal(1, CountExitPrompts());
        Assert.DoesNotContain("Completed", _terminal.Output);
        Assert.DoesNotContain("Press any key", _terminal.Output);
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

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task ComparisonReadsEveryApiPageUsesSelectedGroupAndLeavesTheFileUnchanged(bool changed, bool overrideGroup)
    {
        Directory.CreateDirectory(_directory);
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);

            if (request.RequestUri.AbsolutePath == "/jsonapi/contact-groups/5001/adviser")
                return """{"data":{"type":"advisers","id":"42","attributes":{"countryCode":"AU"}}}""";

            return SnapshotExportTests.Respond(request);
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var client = new MyCrmApiClient(adapter);
        var book = await new SnapshotApi(client, adapter.BaseUrl).ExportAsync(5001);
        if (overrideGroup)
        {
            book.ContactGroupId = 5002;
            book.Country = "NZ"; // Comparison must use the selected group's country, not workbook metadata.
        }
        book.Attributes.Liabilities[0].NzDocumentedLoanTermMonths = null;
        book.Attributes.Liabilities[0].NzLoanStartDate = null;
        book.Attributes.Incomes[0].NzIsGross = null;
        if (changed) book.Attributes.Expenses[0].Value = 999;
        var path = Path.Combine(_directory, "comparison.xlsx");
        book.Save(path);
        using (var excel = new XLWorkbook(path))
        {
            excel.DefinedNames.DeleteAll();
            excel.Worksheet("Expenses").Row(1).CellsUsed().Single(cell => cell.GetString() == "description").WorksheetColumn().Delete();
            excel.Save();
        }

        var original = File.ReadAllBytes(path);
        handler.Requests.Clear();

        // No financial-snapshot write or control-data scopes are needed.
        var console = new SampleConsole(client, new MyCrmConfig(), Path.Combine(_directory, "state.json"),
            "api.contact-groups.read api.contacts.read api.assets.search api.incomes.search api.expenses.search api.liabilities.search");
        console.UpdateState(new SampleState { LastContactGroupId = 5999 });

        for (var i = 0; i < 3; i++) _terminal.Input.PushKey(ConsoleKey.DownArrow);
        _terminal.Input.PushKey(ConsoleKey.Enter);
        _terminal.Input.PushTextWithEnter(path);
        if (overrideGroup) _terminal.Input.PushTextWithEnter("5001");
        else _terminal.Input.PushKey(ConsoleKey.Enter); // Default to workbook's group, not saved state.
        _terminal.Input.PushKey(ConsoleKey.Escape);

        await Assert.ThrowsAsync<MenuBackException>(() => new Samples(console).RunFinancialSnapshotsSample().WaitAsync(TimeSpan.FromSeconds(10)));

        Assert.Equal(10, handler.Requests.Count);
        Assert.Contains("https://example.test/jsonapi/contact-groups/5001/adviser", handler.Requests);
        var searches = handler.Requests.Where(url => new[] { "/assets?", "/liabilities?", "/incomes?", "/expenses?" }.Any(url.Contains)).ToArray();
        Assert.Equal(5, searches.Length);
        Assert.All(searches, url => Assert.Contains("has(ownership,any(contact.id,'1001','1002'))", url));
        Assert.Contains(handler.Requests, url => url.Contains("page[number]=2"));
        Assert.Equal(original, File.ReadAllBytes(path));
        Assert.False(_terminal.Input.IsKeyAvailable());
        Assert.DoesNotContain("Request failed", _terminal.Output);
        Assert.DoesNotContain("Apply this financial snapshot", _terminal.Output);
        Assert.Contains("Only columns present in the spreadsheet are compared", _terminal.Output);
        Assert.Contains("NZ-only fields", _terminal.Output);

        if (changed)
        {
            Assert.Contains("Expenses: Changed", _terminal.Output);
            Assert.Contains("999", _terminal.Output);
            Assert.Contains("280", _terminal.Output);
            Assert.DoesNotContain("No differences", _terminal.Output);
        }
        else Assert.Contains("No differences in the compared snapshot fields.", _terminal.Output);
    }

    [Theory]
    [InlineData("contact-group-contacts")]
    [InlineData("contact-group-emails")]
    [InlineData("custom-statuses")]
    [InlineData("organisation-custom-statuses")]
    public async Task CompleteListSamplesDisplayRecordsFromLaterPages(string sample)
    {
        Directory.CreateDirectory(_directory);
        var contactSample = sample.StartsWith("contact-group", StringComparison.Ordinal);
        var path = contactSample ? "/jsonapi/contact-groups/5001/contacts"
            : sample == "custom-statuses" ? "/jsonapi/custom-statuses" : "/jsonapi/organisations/123/customStatuses";
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal(path, request.RequestUri!.AbsolutePath);
            var second = Uri.UnescapeDataString(request.RequestUri.Query).Contains("page[number]=2");
            var type = contactSample ? "contacts" : "custom-statuses";
            var id = second ? "2" : "1";
            var label = second ? "LaterPage" : "FirstPage";

            if (!second)
            {
                var query = Uri.UnescapeDataString(request.RequestUri.Query);
                Assert.Contains(contactSample ? "sort=id" : "sort=sortOrder,id", query);
                if (sample == "contact-group-emails") Assert.Contains("fields[contacts]=email", query);
            }

            return JsonSerializer.Serialize(new
            {
                data = new[] { new { type, id, attributes = new { firstName = label, email = label + "@example.test", name = label, sortOrder = second ? 2 : 1 } } },
                links = new { next = second ? null : "?page[number]=2" }
            });
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var console = new SampleConsole(new MyCrmApiClient(adapter), new MyCrmConfig(), Path.Combine(_directory, "state.json"), "api");
        console.UpdateState(new SampleState { LastContactGroupId = 5001 });
        var samples = new Samples(console);

        if (sample == "contact-group-contacts") await samples.RunContactGroupContactsSample();
        else if (sample == "contact-group-emails") await samples.RunContactGroupEmailsSample();
        else if (sample == "custom-statuses") await samples.RunCustomStatusesSample();
        else
        {
            _terminal.Input.PushTextWithEnter("123");
            await samples.RunOrganisationCustomStatusesSample();
        }

        Assert.Equal(2, handler.Requests.Count);
        Assert.Contains("FirstPage", _terminal.Output);
        Assert.Contains("LaterPage", _terminal.Output);
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

    [Theory]
    [InlineData(429)]
    [InlineData(401)]
    [InlineData(422)]
    [InlineData(503)]
    [InlineData(504)]
    public async Task ImportRetriesOnly429AndKeepsTheSubmittedBody(int firstStatus)
    {
        Directory.CreateDirectory(_directory);
        var book = new SnapshotWorkbook { ContactGroupId = 5001 };
        book.Attributes.Assets.Add(new() { Lid = "asset", AssetTypeId = 99999, Value = -7, Ownership = [new() { Id = 1001 }] });
        var path = Path.Combine(_directory, "import.xlsx");
        book.Save(path);

        var bodies = new List<string>();
        using var handler = new ResponseHandler(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://example.test/jsonapi/contact-groups/5001/financial-snapshot", request.RequestUri!.AbsoluteUri);
            bodies.Add(request.Content!.ReadAsStringAsync().GetAwaiter().GetResult());
            if (bodies.Count == 1) return new HttpResponseMessage((HttpStatusCode)firstStatus);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"data":{"type":"financial-snapshots","attributes":{"operations":{"created":[],"updated":[],"deleted":[]}}}}""",
                    System.Text.Encoding.UTF8, "application/vnd.api+json")
            };
        });
        using var http = KiotaClientFactory.Create(KiotaClientFactory.CreateDefaultHandlers(), handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var console = new SampleConsole(new MyCrmApiClient(adapter), new MyCrmConfig { AdviserContactId = 123 },
            Path.Combine(_directory, "state.json"), "api.contactgroups.financialsnapshot");
        var expected = await KiotaJsonSerializer.SerializeAsStringAsync(SnapshotWorkbook.Load(path).ToDocument());

        _terminal.Input.PushKey(ConsoleKey.Enter);
        _terminal.Input.PushTextWithEnter(path);
        _terminal.Input.PushKey(ConsoleKey.Enter); // Workbook group.
        _terminal.Input.PushKey(ConsoleKey.Enter); // Workbook modes.
        _terminal.Input.PushTextWithEnter("n"); // Request preview.
        _terminal.Input.PushTextWithEnter("y"); // Submit.
        _terminal.Input.PushKey(ConsoleKey.Escape);

        await Assert.ThrowsAsync<MenuBackException>(() => new Samples(console).RunFinancialSnapshotsSample().WaitAsync(TimeSpan.FromSeconds(15)));

        Assert.Equal(firstStatus == 429 ? 2 : 1, bodies.Count);
        Assert.All(bodies, body => Assert.Equal(expected, body));
        if (firstStatus == 429)
        {
            // Spectre wraps long request URLs to the terminal width.
            var output = string.Concat(_terminal.Output.Where(character => !char.IsWhiteSpace(character)));
            Assert.Contains("HTTP429:POSThttps://example.test/jsonapi/contact-groups/5001/financial-snapshot", output);
            Assert.Contains("retry1/8", output);
            Assert.Contains("Export the current financials", _terminal.Output);
        }
    }

    [Theory]
    [InlineData(null)]
    [InlineData(50)]
    public async Task GenerateMenuSupportsTheDefaultAndAnExplicitItemCount(int? count)
    {
        Directory.CreateDirectory(_directory);
        var controls = SnapshotExampleData.FullControls();
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            if (request.RequestUri!.AbsolutePath.StartsWith("/jsonapi/contact-groups/", StringComparison.Ordinal))
                return SnapshotExportTests.Respond(request);

            var resource = request.RequestUri.AbsolutePath.Split('/').Last();
            return JsonSerializer.Serialize(new
            {
                data = controls.Where(item => item.Resource == resource).Select(item => new
                {
                    type = resource, id = item.Id.ToString(), attributes = new { name = item.Name, description = item.Name }
                })
            });
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var client = new MyCrmApiClient(adapter);
        var console = new SampleConsole(client, new MyCrmConfig { AdviserContactId = 123 },
            Path.Combine(_directory, "state.json"), "api");
        var path = Path.Combine(_directory, "generated.xlsx");
        var api = new SnapshotApi(client, adapter.BaseUrl);
        var expected = SnapshotExamples.Create(5001, await api.ContactsAsync(5001), await api.LookupsAsync(), "AU", seed: 42, itemCount: count);

        _terminal.Input.PushKey(ConsoleKey.DownArrow);
        _terminal.Input.PushKey(ConsoleKey.DownArrow);
        _terminal.Input.PushKey(ConsoleKey.Enter);
        _terminal.Input.PushTextWithEnter("5001");
        _terminal.Input.PushTextWithEnter("42");
        if (count.HasValue) _terminal.Input.PushTextWithEnter(count.Value.ToString());
        else _terminal.Input.PushKey(ConsoleKey.Enter);
        _terminal.Input.PushTextWithEnter(path);
        _terminal.Input.PushKey(ConsoleKey.Escape);

        await Assert.ThrowsAsync<MenuBackException>(() => new Samples(console).RunFinancialSnapshotsSample().WaitAsync(TimeSpan.FromSeconds(15)));

        var loaded = SnapshotWorkbook.Load(path);
        var actual = SnapshotWorkbook.Collections.Sum(sheet => loaded.Rows(sheet).Count);
        Assert.Equal(count ?? SnapshotWorkbook.Collections.Sum(sheet => expected.Rows(sheet).Count), actual);
        Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(expected.ToDocument()),
            await KiotaJsonSerializer.SerializeAsStringAsync(loaded.ToDocument()));
        Assert.Contains($"Generated {actual} financial items", _terminal.Output);
    }

    private sealed class ResponseHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => Task.FromResult(respond(request));
    }

    private void QueueExit()
    {
        _terminal.Input.PushKey(ConsoleKey.Escape);
        _terminal.Input.PushTextWithEnter("y");
    }

    private int CountExitPrompts() => _terminal.Output.Split("Exit the sample application?").Length - 1;

    private async Task RunMenu(SampleState state = null)
    {
        using var handler = new SnapshotTests.RecordingHandler(_ => throw new InvalidOperationException("Navigation must not make an API request."));
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var console = new SampleConsole(new MyCrmApiClient(adapter), new MyCrmConfig { AdviserContactId = 123 }, Path.Combine(_directory, "state.json"), "api");
        if (state != null) console.UpdateState(state);

        await console.Run().WaitAsync(TimeSpan.FromSeconds(10));

        Assert.Empty(handler.Requests);
        Assert.False(_terminal.Input.IsKeyAvailable());
    }
}
