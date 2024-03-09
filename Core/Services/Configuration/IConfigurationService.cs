namespace HashBack.Services;

public interface IConfigurationService
{
    public T? Get<T>(string key);
}