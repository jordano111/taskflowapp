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

app.MapGet("/api/tasks", () => Results.Ok(tasks));

app.MapPost("/api/tasks", (CreateTaskRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest(new { message = "Task title is required." });
    if (request.ProjectId == Guid.Empty)
        return Results.BadRequest(new { message = "Project ID is required." });
    if (!projects.Any(p => p.Id == request.ProjectId))
        return Results.BadRequest(new { message = "Project ID not found." });


    var task = new TaskItem(
        Guid.NewGuid(),
        request.ProjectId,
        request.Title.Trim(),
        "Backlog",
        DateTime.UtcNow
    );

    tasks.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
});

app.MapPatch("/api/tasks/{id:guid}/status", (Guid id, UpdateTaskStatusRequest request) =>
{
    var normalizedStatus = request.Status?.Trim();
    if (string.IsNullOrWhiteSpace(normalizedStatus))
        return Results.BadRequest(new { message = "Task status failed." });

    var index = tasks.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound(new { message = "Task not found." });

    var allowedStatuses = new[] { "Backlog", "In Progress", "Done" };
    // we could just do tasks[index] = tasks[index] with { Status = normalizedStatus }; but the current version is more readable.
    // honestly I feel like tasks[index] = tasks[index] with { Status = normalizedStatus }; is more readable though, I will leave it for now
    var existing = tasks[index];
    var updated = existing with { Status = normalizedStatus };
    tasks[index] = updated;

    if (!allowedStatuses.Contains(normalizedStatus, StringComparer.OrdinalIgnoreCase))
        return Results.BadRequest(new { message = $"Invalid status. Allowed values are: {string.Join(", ", allowedStatuses)}." });

    return Results.Ok(updated);
});

app.MapDelete("/api/tasks/{id:guid}", (Guid id) =>
{
    var index = tasks.FindIndex(t => t.Id == id);
    if (index == -1)
        return Results.NotFound(new { message = "Task not found." });

    tasks.RemoveAt(index);
    return Results.NoContent();
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
    var project = new Project(
        Guid.NewGuid(),
        request.Name.Trim(),
        DateTime.UtcNow);

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
        return Results.NotFound(new { message = "Project not found." });

    var existing = projects[index];
    var updated = existing with { Name = normalizedName };
    projects[index] = updated;

    return Results.Ok(updated);
});

app.MapDelete("/api/projects/{id:guid}", (Guid id) =>
{
    var index = projects.FindIndex(p => p.Id == id);
    if (index == -1)
        return Results.NotFound(new { message = "Project not found." });

    projects.RemoveAt(index);
    return Results.NoContent();
});

app.Run();

// record declarations define the shape of the object being creating
record TaskItem(Guid Id, Guid ProjectId, string Title, string Status, DateTime CreatedAtUtc);
record CreateTaskRequest(Guid ProjectId, string Title);
record UpdateTaskStatusRequest(string Status);
record Project(Guid Id, string Name, DateTime CreatedAtUtc);
record CreateProjectRequest(string Name);
record UpdateProjectNameRequest(string Name);
