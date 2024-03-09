using HashBack.Models;

namespace HashBack.Services;

public interface ICryptoService
{
    public string GenerateRandomString();
    public string GetHash(Request request);
}