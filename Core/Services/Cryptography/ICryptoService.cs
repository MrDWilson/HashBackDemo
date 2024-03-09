using HashBack.Models;

namespace HashBack.Services;

public interface ICryptoService
{
    public string GenerateRandomString();
    public bool ValidateRandomString(string random);
    public string GetHash(Request request);
}