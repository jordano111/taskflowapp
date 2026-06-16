namespace TaskFlow.Api.Models;

public class TaskItem
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }

}