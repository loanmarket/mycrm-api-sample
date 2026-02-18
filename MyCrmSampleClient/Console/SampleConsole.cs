using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.Kiota;
using Serilog;
using Spectre.Console;

namespace MyCrmSampleClient.Console;

public class SampleConsole
{
    private static readonly JsonSerializerOptions StateSerializerOptions = new()
    {
        WriteIndented = true
    };

    public MyCrmApiClient Client { get; }
    private readonly MyCrmConfig _myCrmConfig;
    private readonly SampleScopes _scopes;
    private readonly string _stateFilePath;
    private readonly IReadOnlyList<SampleDefinition> _sampleDefinitions;
    private SampleState _state;
    private readonly Samples _samples;
    
    public SampleState State => _state;
    
    public void UpdateState(SampleState value)
    {
        _state = value ?? throw new ArgumentNullException(nameof(value));
    }
    
    private sealed record SampleDefinition(
        string Name,
        string ApiMethod,
        Func<Task> RunAsync);

    private enum MenuAction
    {
        RunSample,
        RunAll,
        ShowState,
        ClearState,
        Exit
    }

    private sealed record MenuOption(
        string Label,
        MenuAction Action,
        SampleDefinition Sample = null);
    
    public SampleConsole(MyCrmApiClient client, MyCrmConfig myCrmConfig, string stateFilePath, string scopes)
    {
        Client = client ?? throw new ArgumentNullException(nameof(client));
        _myCrmConfig = myCrmConfig ?? throw new ArgumentNullException(nameof(myCrmConfig));
        _stateFilePath = string.IsNullOrWhiteSpace(stateFilePath)
            ? throw new ArgumentException("State file path cannot be empty", nameof(stateFilePath))
            : stateFilePath;
        _state = LoadSampleState(_stateFilePath);
        _samples = new Samples(this);
        _sampleDefinitions = DiscoverSampleDefinitions();
        _scopes = new SampleScopes(scopes);
    }

    public bool RequireScope(string requiredScope, string sampleName)
    {
        if (_scopes.HasScope(requiredScope))
        {
            return true;
        }

        Log.Warning(
            "Skipping sample {SampleName}. Missing required scope {RequiredScope}. Available scopes: {Scopes}",
            sampleName,
            requiredScope,
            _scopes.RawScopes);

        return false;
    }

    public bool RequireAdviserContactId(string sampleName)
    {
        if (_myCrmConfig.AdviserContactId > 0)
        {
            return true;
        }

        Log.Warning(
            "Unable to run sample {SampleName} as AdviserContactId must be > 0. Use the adviser-details sample to find an id and then set AdviserContactId.",
            sampleName);

        return false;
    }

    public void WarnAdviserContactIdForbidden(string sampleName)
    {
        Log.Warning(
            "Sample {SampleName} failed with 403 Forbidden. This usually means AdviserContactId is wrong. Use the adviser-details sample to find the correct id and then update AdviserContactId.",
            sampleName);
    }

    public static bool IsForbidden(ApiException ex)
    {
        return ex is { ResponseStatusCode: 403 };
    }

    private IReadOnlyList<SampleDefinition> DiscoverSampleDefinitions()
    {
        var discovered = typeof(Samples)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Select(method => new { Method = method, Attribute = method.GetCustomAttribute<SampleAttribute>() })
            .Where(x => x.Attribute != null)
            .Select(x => new { x.Method, Attribute = x.Attribute! })
            .ToList();

        if (discovered.Count == 0)
        {
            throw new InvalidOperationException("No sample methods were discovered. Add [Sample] attributes to sample methods.");
        }

        var invalidSignatureMethods = discovered
            .Where(x => !HasValidSampleMethodSignature(x.Method))
            .Select(x => x.Method.Name)
            .OrderBy(x => x, StringComparer.Ordinal)
            .ToArray();
        if (invalidSignatureMethods.Length > 0)
        {
            throw new InvalidOperationException(
                $"Sample methods with invalid signature: {string.Join(", ", invalidSignatureMethods)}. Expected: Task Method().");
        }

        var duplicateNames = discovered
            .GroupBy(x => x.Attribute.Name, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        if (duplicateNames.Length > 0)
        {
            throw new InvalidOperationException($"Duplicate [Sample] names found: {string.Join(", ", duplicateNames)}.");
        }

        return discovered
            .OrderBy(x => x.Method.MetadataToken)
            .Select(x => new SampleDefinition(
                x.Attribute.Name,
                x.Attribute.ApiMethod,
                (Func<Task>)x.Method.CreateDelegate(typeof(Func<Task>), _samples)))
            .ToList();
    }

    private static bool HasValidSampleMethodSignature(MethodInfo method)
    {
        if (method.IsStatic || method.ReturnType != typeof(Task))
        {
            return false;
        }

        return method.GetParameters().Length == 0;
    }

    public async Task Run()
    {
        Log.Information(
            "Using sample state file {StateFilePath}. LastContactId={LastContactId}, LastContactGroupId={LastContactGroupId}",
            _stateFilePath,
            _state.LastContactId,
            _state.LastContactGroupId);

        while (true)
        {
            AnsiConsole.Clear();

            var selectedOption = PromptMenuOption();
            
            switch (selectedOption.Action)
            {
                case MenuAction.RunSample:
                    if (selectedOption.Sample != null)
                    {
                        await RunSample(selectedOption.Sample);
                    }
                    break;
                case MenuAction.RunAll:
                    await RunSampleGroup(_sampleDefinitions);
                    break;
                case MenuAction.ShowState:
                    ShowCurrentState();
                    break;
                case MenuAction.ClearState:
                    ClearState();
                    break;
                case MenuAction.Exit:
                    AnsiConsole.MarkupLine("[grey]Exiting sample menu[/]");
                    return;
            }

            PauseConsole();
        }
    }

    private MenuOption PromptMenuOption()
    {
        var options = BuildMenuOptions();
        var prompt = new SelectionPrompt<MenuOption>()
            .Title($"Choose an action ([grey]state: contact={FormatNullableInt(_state.LastContactId)}, group={FormatNullableInt(_state.LastContactGroupId)}, deal={FormatNullableInt(_state.LastDealId)}[/])")
            .UseConverter(x => x.Label)
            .PageSize(options.Count)
            .AddChoices(options);

        try
        {
            return AnsiConsole.Prompt(prompt);
        }
        catch (Exception ex) when (IsPromptAbort(ex))
        {
            return new MenuOption("[red]Exit[/]", MenuAction.Exit);
        }
    }

    private static bool IsPromptAbort(Exception ex)
    {
        if (ex is OperationCanceledException)
        {
            return true;
        }

        var exceptionTypeName = ex.GetType().Name;
        return exceptionTypeName.Contains("Abort", StringComparison.OrdinalIgnoreCase) ||
               exceptionTypeName.Contains("Canceled", StringComparison.OrdinalIgnoreCase) ||
               exceptionTypeName.Contains("Cancelled", StringComparison.OrdinalIgnoreCase);
    }

    private IReadOnlyList<MenuOption> BuildMenuOptions()
    {
        var options = _sampleDefinitions
            .Select(sample => new MenuOption(
                $"[Aqua]{Markup.Escape(sample.Name)}[/]",
                MenuAction.RunSample,
                sample))
            .ToList();

        options.Add(new MenuOption("[Yellow]Run all samples[/]", MenuAction.RunAll));
        options.Add(new MenuOption("[CornflowerBlue]Show shared state[/]", MenuAction.ShowState));
        options.Add(new MenuOption("[CornflowerBlue]Clear shared state[/]", MenuAction.ClearState));
        options.Add(new MenuOption("[Red]Exit[/]", MenuAction.Exit));

        return options;
    }

    private async Task RunSample(SampleDefinition sample)
    {
        var escapedSampleName = Markup.Escape(sample.Name);

        var start = DateTime.Now;
        
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .StartAsync($"Executing [aqua]{escapedSampleName}[/]...", async _ =>
            {
                Log.Information("Calling {ApiMethod} ...", sample.ApiMethod);
                await sample.RunAsync();
            });

        AnsiConsole.MarkupLine($"[Grey]Completed[/] [Aqua]{escapedSampleName}[/] in [Grey]{(DateTime.Now - start).TotalSeconds:0.00}s[/]");
        
        SaveSampleState(_stateFilePath, _state);
    }

    private async Task RunSampleGroup(IEnumerable<SampleDefinition> sampleNames)
    {
        foreach (var sample in sampleNames)
        {
            await RunSample(sample);
        }
    }

    private void ShowCurrentState()
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("Key")
            .AddColumn("Value");

        table.AddRow("State file", _stateFilePath);
        table.AddRow("LastContactId", FormatNullableInt(_state.LastContactId));
        table.AddRow("LastContactGroupId", FormatNullableInt(_state.LastContactGroupId));
        table.AddRow("LastDealId", FormatNullableInt(_state.LastDealId));

        AnsiConsole.Write(table);
    }

    private void ClearState()
    {
        if (!AnsiConsole.Confirm("Clear shared sample state?"))
        {
            AnsiConsole.MarkupLine("[Grey]State unchanged[/]");
            return;
        }

        _state = new SampleState();
        SaveSampleState(_stateFilePath, _state);

        AnsiConsole.MarkupLine("[Yellow]Cleared shared sample state[/]");
    }

    private static void PauseConsole()
    {
        AnsiConsole.MarkupLine("[Grey]Press any key to continue...[/]");
        if (!System.Console.IsInputRedirected)
        {
            System.Console.ReadKey(intercept: true);
        }
        else
        {
            System.Console.ReadLine();
        }
    }

    private static string FormatNullableInt(int? value) => value.HasValue ? value.Value.ToString() : "unset";

    private static SampleState LoadSampleState(string stateFilePath)
    {
        if (string.IsNullOrWhiteSpace(stateFilePath) || !File.Exists(stateFilePath))
        {
            return new SampleState();
        }

        try
        {
            var json = File.ReadAllText(stateFilePath);
            return JsonSerializer.Deserialize<SampleState>(json) ?? new SampleState();
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to read state file {StateFilePath}. Using empty state", stateFilePath);
            return new SampleState();
        }
    }

    private static void SaveSampleState(string stateFilePath, SampleState state)
    {
        if (string.IsNullOrWhiteSpace(stateFilePath))
        {
            return;
        }

        try
        {
            var directory = Path.GetDirectoryName(stateFilePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(state, StateSerializerOptions);
            File.WriteAllText(stateFilePath, json);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to write state file {StateFilePath}", stateFilePath);
        }
    }

    public Table CreateTable(params string[] columns)
    {
        var table = new Table().Border(TableBorder.Rounded);
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }

        return table;
    }

    public Text Cell(string value)
    {
        return new Text(string.IsNullOrWhiteSpace(value) ? "-" : value);
    }

    public Text Cell(bool? value)
    {
        return new Text(value.HasValue ? value.Value.ToString().ToLowerInvariant() : "-");
    }

    public void WriteTable(Table table)
    {
        if (table == null)
        {
            return;
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }
}
