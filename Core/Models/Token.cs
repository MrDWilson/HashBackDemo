namespace HashBack.Models
{
    public record Token
    {
        public required string BearerToken { get; init; }
        public required long IssuedAt { get; init; }
        public required long ExpiresAt { get; init; }
    }
}