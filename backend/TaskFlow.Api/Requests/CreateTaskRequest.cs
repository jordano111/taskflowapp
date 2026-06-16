namespace TaskFlow.Api.Requests;

record CreateTaskRequest(Guid ProjectId, string Title);