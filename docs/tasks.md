# Creating, reading and updating tasks

The task JSON:API resource supports `POST /jsonapi/tasks`, `GET /jsonapi/tasks/{id}` and `PATCH /jsonapi/tasks/{id}`. The [interactive C# samples](../MyCrmSampleClient/TaskSamples.cs) demonstrate residential-deal tasks, contact-group tasks and the one-time promotion from a contact task to a deal task.

Use the usual [authentication and request headers](../README.md#required-request-headers). Every request needs `Authorization: Bearer <token>`, `UserId: <AdviserContactId>` and `Accept: application/vnd.api+json`. Writes also need `Content-Type: application/vnd.api+json`.

| Operation | Scope |
|---|---|
| Read a task | `api.tasks.read` |
| Create a task | `api.tasks.create` |
| Update a task | `api.tasks.update` |

Broader applicable scopes also work. An assignee uses an **adviser family ID** from `GET /jsonapi/advisers` (`data[].id`). This is different from the **adviser contact ID** configured as `MyCRM:AdviserContactId` and sent in `UserId`. The assignee and linked records must be accessible to your credentials.

## Run the C# samples

Run `dotnet run` from `MyCrmSampleClient` after configuring credentials as described in the [client README](../MyCrmSampleClient/README.md). These samples make live writes.

1. Run `advisers` and note the assignee's `Id`. This lookup requires `api.advisers.search`.
2. Run `create-lead` or `latest-deals` to set `LastDealId`, then `create-residential-task`. These setup samples require `api.leads.create` or `api.deals.search`.
3. Run `get-task`, `task-fields`, `patch-task` and `task-progress` to read and change the residential task.
4. Run `latest-contact-groups` (requires `api.contact-groups.search`) to set `LastContactGroupId`, then `create-contact-group-task`.
5. Repeat the read and update samples for the contact-group task.
6. Run `promote-contact-task` and enter the residential deal ID to associate with that task. Read it again with `get-task`.
7. To revisit the original residential task, run `get-task` with `LastResidentialTaskId` from **Show shared state**.

Creation prompts for the assignee ID. Due dates are calculated from the current date. `LastTaskId` selects the task used by field selection and attribute/progress edits; `get-task` can select any accessible task by ID. `LastContactGroupTaskId` retains the task used by promotion, even if you read another task. PATCH samples require read scope as well as update scope because they read back the result.

To use fresh records like the original HTTP walkthrough, run `create-lead` twice and note both deal IDs. For the second deal, `GET /jsonapi/deals/{id}?include=contacts.contactGroup` returns the generated contact group in `included` with `type: "contact-groups"` (requires `api.deals.read`). Use that group's ID in the contact-group POST below, then promote its task to the second deal. The console creation samples use the IDs currently shown in shared state.

## Create a residential-deal task

Replace all example IDs with accessible records. The assignee is required.

```http
POST /jsonapi/tasks
Authorization: Bearer <token>
UserId: <AdviserContactId>
Content-Type: application/vnd.api+json
Accept: application/vnd.api+json

{
  "data": {
    "type": "tasks",
    "attributes": {
      "title": "Follow up on residential deal",
      "detail": "Confirm the outstanding application details.",
      "progress": "NotStarted",
      "priority": "Normal",
      "dueDate": "2026-10-05T09:00:00+10:00"
    },
    "relationships": {
      "assignee": { "data": { "type": "advisers", "id": "1234567" } },
      "residentialDeal": { "data": { "type": "deals", "id": "4567890" } }
    }
  }
}
```

A successful POST returns `201 Created`. Save `data.id` for later reads and updates. Use an ISO 8601 due date with an explicit offset.

## Create a contact-group task

Use the same POST endpoint and headers with this body. One contact group and no engagement prepares the task for promotion.

```json
{
  "data": {
    "type": "tasks",
    "attributes": {
      "title": "Follow up with contact group",
      "detail": "Contact the group regarding the outstanding requirements.",
      "progress": "NotStarted",
      "priority": "Normal",
      "dueDate": "2026-10-06T09:00:00+10:00"
    },
    "relationships": {
      "assignee": { "data": { "type": "advisers", "id": "1234567" } },
      "contactGroups": {
        "data": [{ "type": "contact-groups", "id": "3456789" }]
      }
    }
  }
}
```

## Read a task and select fields

```http
GET /jsonapi/tasks/5678901
Authorization: Bearer <token>
UserId: <AdviserContactId>
Accept: application/vnd.api+json
```

The response contains the task's attributes and relationship identifiers. Restrict the returned fields with:

```http
GET /jsonapi/tasks/5678901?fields[tasks]=title,progress,dueDate
Authorization: Bearer <token>
UserId: <AdviserContactId>
Accept: application/vnd.api+json
```

Sparse fieldsets also work on POST and PATCH responses. They restrict the response, not the attributes written.

The current task API **rejects `include` with `400 Bad Request`**. To fetch related resources, use the IDs in `data.relationships` to call their own endpoints, for example `GET /jsonapi/deals/{residentialDealId}` or `GET /jsonapi/contact-groups/{contactGroupId}`, with the corresponding read scopes.

## Update task attributes and progress

PATCH must include `type: "tasks"` and an `id` matching the URL. Send only the fields to change; omitted fields retain their values.

```http
PATCH /jsonapi/tasks/5678901
Authorization: Bearer <token>
UserId: <AdviserContactId>
Content-Type: application/vnd.api+json
Accept: application/vnd.api+json

{
  "data": {
    "type": "tasks",
    "id": "5678901",
    "attributes": {
      "title": "Follow up on residential deal - updated",
      "detail": "Confirm the outstanding application and lender details."
    }
  }
}
```

For a progress-only update, use the same endpoint and headers with:

```json
{
  "data": {
    "type": "tasks",
    "id": "5678901",
    "attributes": { "progress": "InProgress" }
  }
}
```

Progress values are `NotStarted`, `InProgress`, `OnHold`, `Completed` and `NotRequired`. Priority values are `Normal`, `Medium`, `High` and `Critical`. Title is limited to 500 characters; detail supports safe HTML up to 10 KiB of UTF-8 text. The same attribute updates work for both residential and contact-group tasks.

A PATCH can return `200 OK` with a resource or `204 No Content`. The console samples read the task again to display the persisted values.

## Promote a contact task to a residential-deal task

Promotion requires **exactly one existing contact link, no existing engagement links and one new engagement**. Send the new relationship through the main PATCH endpoint:

```http
PATCH /jsonapi/tasks/6789012
Authorization: Bearer <token>
UserId: <AdviserContactId>
Content-Type: application/vnd.api+json
Accept: application/vnd.api+json

{
  "data": {
    "type": "tasks",
    "id": "6789012",
    "relationships": {
      "residentialDeal": { "data": { "type": "deals", "id": "4567891" } }
    }
  }
}
```

Omit `contactGroups` from this request. The service removes the existing contact link and adds the engagement link. Read the task again to verify `relationships.residentialDeal.data.id` and the empty `relationships.contactGroups.data` array. This is a one-time transition, not a general way to reassign tasks between deals. The console sample checks the initial links and the result.

Collection reads (`GET /jsonapi/tasks`), deletion and relationship-only mutation endpoints are not supported. Apply relationship changes with `PATCH /jsonapi/tasks/{id}`.
