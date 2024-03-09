namespace HashBack.Models;

public record Request
{
    public required string HashBack { get; init; }
    public required ResponseType TypeOfResponse { get; init; }
    public required string IssuerUrl { get; init; }
    public required long Now { get; init; }
    public required string Unus { get; init; }
    public required int Rounds { get; init; } = 1;
    public required string VerifyUrl { get; init; }
}