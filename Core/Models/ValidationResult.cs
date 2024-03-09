namespace HashBack.Models;

public record ValidationResult
{
    public required bool Success { get; init; }
    public string? Error { get; init; }
}