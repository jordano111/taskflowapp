namespace TaskFlow.Api.Models;

public class Project
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }

}