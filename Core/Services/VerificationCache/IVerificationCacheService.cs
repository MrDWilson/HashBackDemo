namespace HashBack.Services;

public interface IVerificationCacheService
{
    public void Add(string key, string value);
    public bool Exists(string key);
    public string? GetAndRemove(string key);
}