using Microsoft.AspNetCore.Http.Features;
using TaskFlow.Api.Requests;
using TaskFlow.Api.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var projects = new List<Project>();
var tasks = new List<TaskItem>();

// Project ID helper check
static IResult? ValidateProjectExists(List<Project> projects, Guid projectId)
{
    if (!projects.Any(p => p.Id == projectId))
        return Results.BadRequest(new { message = "Project ID not found." });
    return null;
}

// project ID is required
static IResult? ValidateProjectIdIsRequired(CreateTaskRequest request)
{
    if (request.ProjectId == Guid.Empty)
        return Results.BadRequest(new { message = "Project ID is required." });
    return null;
}

// Task Title null or whitespace checked
static IResult? ValidateStringIsRequired(string? title)
{
    if (string.IsNullOrWhiteSpace(title))
        return Results.BadRequest(new { message = "Task title is required." });
    return null;
}

// Task Id is required
static IResult? ValidateTaskIdExists(List<TaskItem> tasks, Guid id)
{
    var index = tasks.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound(new { message = "Task ID not found." });
    return null;
}


app.MapGet("/api/tasks", () => Results.Ok(tasks));

app.MapGet("/api/tasks/{id:guid}", (Guid id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    return task is null
    ? Results.NotFound(new { message = "Task not found." })
    : Results.Ok(task);
});

app.MapPost("/api/tasks", (CreateTaskRequest request) =>
{
    //task and both project validations
    IResult? error;
    error = ValidateProjectExists(projects, request.ProjectId);
    if (error is not null)
        return error;
    error = ValidateProjectIdIsRequired(request);
    if (error is not null)
        return error;
    error = ValidateStringIsRequired(request.Title);
    if (error is not null)
        return error;


    // public Guid Id { get; init; }
    // public Guid ProjectId { get; init; }
    // public string Title { get; set; } = string.Empty;
    // public string Status { get; set; } = string.Empty;
    // public DateTime CreatedAtUtc { get; init; }
    var task = new TaskItem
    {
        Id = Guid.NewGuid(),
        ProjectId = request.ProjectId,
        Title = request.Title.Trim(),
        Status = "Backlog",
        CreatedAtUtc = DateTime.UtcNow,
    };

    tasks.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

app.MapPatch("/api/tasks/{id:guid}/status", (Guid id, UpdateTaskStatusRequest request) =>
{
    var normalizedStatus = request.Status?.Trim();
    IResult? error;
    error = ValidateStringIsRequired(normalizedStatus);
    if (error is not null)
        return error;

    error = ValidateTaskIdExists(tasks, id);
    if (error is not null)
        return error;

    var allowedStatuses = new[] { "Backlog", "In Progress", "Done" };
    // we could just do tasks[index] = tasks[index] with { Status = normalizedStatus }; but the current version is more readable.
    // honestly I feel like tasks[index] = tasks[index] with { Status = normalizedStatus }; is more readable though, I will leave it for now
    var index = tasks.FindIndex(t => t.Id == id);
    tasks[index].Status = normalizedStatus!;

    if (!allowedStatuses.Contains(normalizedStatus, StringComparer.OrdinalIgnoreCase))
        return Results.BadRequest(new { message = $"Invalid status. Allowed values are: {string.Join(", ", allowedStatuses)}." });

    return Results.Ok(tasks[index]);
});

app.MapDelete("/api/tasks/{id:guid}", (Guid id) =>
{
    IResult? error;
    error = ValidateTaskIdExists(tasks, id);
    if (error is not null)
        return error;

    // delete
    var index = tasks.FindIndex(t => t.Id == id);
    tasks.RemoveAt(index);
    return Results.NoContent();
});

app.MapGet("/api/projects/{id:guid}/tasks", (Guid id) =>
{
    var project = projects.FirstOrDefault(p => p.Id == id);
    if (project is null)
        return Results.NotFound(new { message = "Project not found." });

    var projectTasks = tasks.Where(t => t.ProjectId == id).ToList();
    return Results.Ok(new
    {
        project,
        tasks = projectTasks
    });
});

app.MapGet("/api/projects/{id:guid}", (Guid id) =>
{
    var project = projects.FirstOrDefault(p => p.Id == id);
    return project is null
    ? Results.NotFound(new { message = "Project not found." })
    : Results.Ok(project);
});

app.MapGet("/api/projects", () => Results.Ok(projects));

app.MapPost("/api/projects", (CreateProjectRequest request) =>
{
    // if null empty or whitespace (tabs/newlines etc.) return bad request
    if (string.IsNullOrWhiteSpace(request.Name))
        return Results.BadRequest(new { message = "Project name is required." });

    // else create new project with NewGuid, the name from the request, and the date it was created (UTC)
    var project = new Project
    {
        Id = Guid.NewGuid(),
        Name = request.Name.Trim(),
        CreatedAtUtc = DateTime.UtcNow,
    };

    projects.Add(project);

    return Results.Created($"/api/projects/{project.Id}", project);
});

app.MapPatch("/api/projects/{id:guid}/name", (Guid id, UpdateProjectNameRequest request) =>
{

    var normalizedName = request.Name?.Trim();

    if (string.IsNullOrWhiteSpace(normalizedName))
        return Results.BadRequest(new { message = "Project name is required." });

    var index = projects.FindIndex(p => p.Id == id);
    if (index == -1)
        return Results.NotFound(new { message = "Project not found. (ID missing)" });

    projects[index].Name = normalizedName!;

    return Results.Ok(projects[index]);
});

app.MapDelete("/api/projects/{id:guid}", (Guid id) =>
{
    var index = projects.FindIndex(p => p.Id == id);
    if (index == -1)
        return Results.NotFound(new { message = "Project not found." });

    projects.RemoveAt(index);
    tasks.RemoveAll(t => t.ProjectId == id);
    return Results.NoContent();
});

app.Run();




