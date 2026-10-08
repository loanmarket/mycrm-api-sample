using System;
using System.Threading.Tasks;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Serilog;
using Spectre.Console;

namespace MyCrmSampleClient;

public partial class Samples
{
    [Sample("create-residential-task", "POST /jsonapi/tasks with a residentialDeal relationship")]
    public async Task RunCreateResidentialTaskSample()
    {
        if (!_console.RequireAdviserContactId("create-residential-task")) return;
        if (!_console.RequireScope("api.tasks.create", "create-residential-task")) return;
        if (!_console.State.TryGetDealId("create-residential-task", out var dealId)) return;

        var assigneeId = PromptTaskAssigneeId();
        var created = await _console.Client.Jsonapi.Tasks.PostAsync(new TaskDocument
        {
            Data = new TaskObject
            {
                Type = "tasks",
                Attributes = new TaskAttributes
                {
                    Title = "Follow up on residential deal",
                    Detail = "Confirm the outstanding application details.",
                    Progress = "NotStarted",
                    Priority = "Normal",
                    DueDate = DateTimeOffset.Now.AddDays(1)
                },
                Relationships = new TaskRelationships
                {
                    Assignee = new RelationshipsSingleDocument
                    {
                        Data = new ResourceIdentifier { Type = "advisers", Id = assigneeId.ToString() }
                    },
                    ResidentialDeal = new RelationshipsSingleDocument
                    {
                        Data = new ResourceIdentifier { Type = "deals", Id = dealId.ToString() }
                    }
                }
            }
        });

        if (!TryRememberTask(created, out var taskId)) return;
        _console.UpdateState(_console.State with { LastResidentialTaskId = taskId });
        WriteTask(created.Data);
    }

    [Sample("create-contact-group-task", "POST /jsonapi/tasks with one contactGroups relationship")]
    public async Task RunCreateContactGroupTaskSample()
    {
        if (!_console.RequireAdviserContactId("create-contact-group-task")) return;
        if (!_console.RequireScope("api.tasks.create", "create-contact-group-task")) return;
        if (!_console.State.TryGetContactGroupId("create-contact-group-task", out var contactGroupId)) return;

        var assigneeId = PromptTaskAssigneeId();
        var created = await _console.Client.Jsonapi.Tasks.PostAsync(new TaskDocument
        {
            Data = new TaskObject
            {
                Type = "tasks",
                Attributes = new TaskAttributes
                {
                    Title = "Follow up with contact group",
                    Detail = "Contact the group regarding the outstanding requirements.",
                    Progress = "NotStarted",
                    Priority = "Normal",
                    DueDate = DateTimeOffset.Now.AddDays(2)
                },
                Relationships = new TaskRelationships
                {
                    Assignee = new RelationshipsSingleDocument
                    {
                        Data = new ResourceIdentifier { Type = "advisers", Id = assigneeId.ToString() }
                    },
                    // One contact group and no engagement allows the later promotion sample.
                    ContactGroups = new RelationshipsMultipleDocument
                    {
                        Data = [new ResourceIdentifier { Type = "contact-groups", Id = contactGroupId.ToString() }]
                    }
                }
            }
        });

        if (!TryRememberTask(created, out var taskId)) return;
        _console.UpdateState(_console.State with { LastContactGroupTaskId = taskId });
        WriteTask(created.Data);
    }

    [Sample("get-task", "GET /jsonapi/tasks/{id}")]
    public async Task RunGetTaskSample()
    {
        if (!_console.RequireAdviserContactId("get-task")) return;
        if (!_console.RequireScope("api.tasks.read", "get-task")) return;

        var prompt = new TextPrompt<int>("Task ID:").Validate(id => id > 0
            ? ValidationResult.Success()
            : ValidationResult.Error("Enter a positive task ID."));
        if (_console.State.LastTaskId is > 0)
        {
            prompt.DefaultValue(_console.State.LastTaskId.Value);
        }

        var taskId = AnsiConsole.Prompt(prompt);
        var response = await _console.Client.Jsonapi.Tasks[taskId].GetAsync();
        if (!TryRememberTask(response, out _)) return;
        WriteTask(response.Data);
    }

    [Sample("task-fields", "GET /jsonapi/tasks/{id}?fields[tasks]=title,progress,dueDate")]
    public async Task RunTaskFieldsSample()
    {
        if (!_console.RequireAdviserContactId("task-fields")) return;
        if (!_console.RequireScope("api.tasks.read", "task-fields")) return;
        if (!_console.State.TryGetTaskId("task-fields", out var taskId)) return;

        // The generated builder has no typed sparse-field parameter. WithUrl preserves
        // its authentication, JSON:API headers and response handling.
        var response = await _console.Client.Jsonapi.Tasks[taskId]
            .WithUrl($"{JsonApiFluentContext.BaseUrl}/jsonapi/tasks/{taskId}?fields[tasks]=title,progress,dueDate")
            .GetAsync();

        if (response?.Data is not { } task)
        {
            Log.Warning("No task returned for task {TaskId}", taskId);
            return;
        }

        var table = _console.CreateTable("Task Id", "Title", "Progress", "Due Date");
        table.AddRow(
            _console.Cell(task.Id),
            _console.Cell(task.Attributes?.Title),
            _console.Cell(task.Attributes?.Progress),
            _console.Cell(task.Attributes?.DueDate?.ToString("O")));
        _console.WriteTable(table);
    }

    [Sample("patch-task", "PATCH /jsonapi/tasks/{id} title and detail, then GET to read back")]
    public async Task RunPatchTaskSample()
    {
        if (!_console.RequireAdviserContactId("patch-task")) return;
        if (!_console.RequireScope("api.tasks.update", "patch-task")) return;
        if (!_console.RequireScope("api.tasks.read", "patch-task")) return;
        if (!_console.State.TryGetTaskId("patch-task", out var taskId)) return;

        var title = AnsiConsole.Ask<string>($"New title for task {taskId}:");
        var detail = AnsiConsole.Ask<string>("New task detail:");
        await _console.Client.Jsonapi.Tasks[taskId].PatchAsync(new TaskDocument
        {
            Data = new TaskObject
            {
                Type = "tasks",
                Id = taskId.ToString(),
                Attributes = new TaskAttributes { Title = title, Detail = detail }
            }
        });

        var response = await _console.Client.Jsonapi.Tasks[taskId].GetAsync();
        WriteTask(response?.Data);
    }

    [Sample("task-progress", "PATCH /jsonapi/tasks/{id} progress, then GET to read back")]
    public async Task RunTaskProgressSample()
    {
        if (!_console.RequireAdviserContactId("task-progress")) return;
        if (!_console.RequireScope("api.tasks.update", "task-progress")) return;
        if (!_console.RequireScope("api.tasks.read", "task-progress")) return;
        if (!_console.State.TryGetTaskId("task-progress", out var taskId)) return;

        var progress = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title($"New progress for task {taskId}:")
            .AddChoices("InProgress", "NotStarted", "OnHold", "Completed", "NotRequired"));
        await _console.Client.Jsonapi.Tasks[taskId].PatchAsync(new TaskDocument
        {
            Data = new TaskObject
            {
                Type = "tasks",
                Id = taskId.ToString(),
                // Send only the attribute being changed.
                Attributes = new TaskAttributes { Progress = progress }
            }
        });

        var response = await _console.Client.Jsonapi.Tasks[taskId].GetAsync();
        WriteTask(response?.Data);
    }

    [Sample("promote-contact-task", "PATCH /jsonapi/tasks/{id} to link a residential deal, then GET to read back")]
    public async Task RunPromoteContactTaskSample()
    {
        if (!_console.RequireAdviserContactId("promote-contact-task")) return;
        if (!_console.RequireScope("api.tasks.update", "promote-contact-task")) return;
        if (!_console.RequireScope("api.tasks.read", "promote-contact-task")) return;
        if (_console.State.LastContactGroupTaskId is not > 0)
        {
            Log.Warning("Run create-contact-group-task first to set LastContactGroupTaskId");
            return;
        }

        var taskId = _console.State.LastContactGroupTaskId.Value;
        var existing = await _console.Client.Jsonapi.Tasks[taskId].GetAsync();
        var relationships = existing?.Data?.Relationships;
        if (relationships?.ContactGroups?.Data?.Count != 1 ||
            relationships.ResidentialDeal?.Data != null ||
            relationships.AssetFinanceDiversifiedDeal?.Data != null ||
            relationships.CommercialFinanceDiversifiedDeal?.Data != null)
        {
            Log.Warning(
                "Task {TaskId} must have exactly one contact group and no engagement to be promoted. Create a new contact-group task to try again",
                taskId);
            return;
        }

        AnsiConsole.WriteLine(
            $"Task {taskId} is linked to contact group {relationships.ContactGroups.Data[0].Id}. " +
            "Promotion replaces that contact link with one residential deal and can only happen once.");
        var dealId = AnsiConsole.Prompt(new TextPrompt<int>("Residential deal ID to link:")
            .Validate(id => id > 0
                ? ValidationResult.Success()
                : ValidationResult.Error("Enter a positive deal ID.")));

        await _console.Client.Jsonapi.Tasks[taskId].PatchAsync(new TaskDocument
        {
            Data = new TaskObject
            {
                Type = "tasks",
                Id = taskId.ToString(),
                Relationships = new TaskRelationships
                {
                    ResidentialDeal = new RelationshipsSingleDocument
                    {
                        Data = new ResourceIdentifier { Type = "deals", Id = dealId.ToString() }
                    }
                    // Omit contactGroups; the service removes the existing contact link.
                }
            }
        });

        var response = await _console.Client.Jsonapi.Tasks[taskId].GetAsync();
        if (!TryRememberTask(response, out _)) return;
        WriteTask(response.Data);
        if (response.Data.Relationships?.ResidentialDeal?.Data?.Id != dealId.ToString() ||
            response.Data.Relationships?.ContactGroups?.Data?.Count != 0)
        {
            Log.Warning("Task {TaskId} did not return the expected promoted links. Check the relationships above", taskId);
        }
    }

    private static int PromptTaskAssigneeId()
    {
        AnsiConsole.WriteLine(
            "Use the Id from the advisers sample (adviser family ID). " +
            "This is separate from AdviserContactId, which is sent in the UserId header.");
        return AnsiConsole.Prompt(new TextPrompt<int>("Assignee adviser ID:")
            .Validate(id => id > 0
                ? ValidationResult.Success()
                : ValidationResult.Error("Enter a positive adviser ID.")));
    }

    private bool TryRememberTask(TaskDocument response, out int taskId)
    {
        if (!int.TryParse(response?.Data?.Id, out taskId) || taskId <= 0)
        {
            Log.Warning("No task with a valid ID was returned");
            return false;
        }

        _console.UpdateState(_console.State with { LastTaskId = taskId });
        return true;
    }

    private void WriteTask(TaskObject task)
    {
        if (task == null)
        {
            Log.Warning("No task was returned");
            return;
        }

        var table = _console.CreateTable("Task Id", "Title", "Detail", "Progress", "Priority", "Due Date");
        table.AddRow(
            _console.Cell(task.Id),
            _console.Cell(task.Attributes?.Title),
            _console.Cell(task.Attributes?.Detail),
            _console.Cell(task.Attributes?.Progress),
            _console.Cell(task.Attributes?.Priority),
            _console.Cell(task.Attributes?.DueDate?.ToString("O")));
        _console.WriteTable(table);

        var relationships = _console.CreateTable("Relationship", "Resource Type", "Id");
        void AddRelationship(string name, ResourceIdentifier resource)
        {
            if (resource != null)
            {
                relationships.AddRow(_console.Cell(name), _console.Cell(resource.Type), _console.Cell(resource.Id));
            }
        }

        AddRelationship("assignee", task.Relationships?.Assignee?.Data);
        AddRelationship("residentialDeal", task.Relationships?.ResidentialDeal?.Data);
        AddRelationship("assetFinanceDiversifiedDeal", task.Relationships?.AssetFinanceDiversifiedDeal?.Data);
        AddRelationship("commercialFinanceDiversifiedDeal", task.Relationships?.CommercialFinanceDiversifiedDeal?.Data);
        foreach (var contactGroup in task.Relationships?.ContactGroups?.Data ?? [])
        {
            AddRelationship("contactGroups", contactGroup);
        }

        _console.WriteTable(relationships);
    }
}
