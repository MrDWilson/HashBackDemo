using System.ComponentModel;
using System.Collections;

namespace HashBack.Services;

public class ConfigurationService : IConfigurationService
{
    public T? Get<T>(string key)
    {
        var value = Environment.GetEnvironmentVariable(key);
        if (value == null)
        {
            return default;
        }

        var type = typeof(T);
        if (IsList(type))
        {
            var listType = typeof(List<>).MakeGenericType(type.GenericTypeArguments[0]);
            var list = (IList)Activator.CreateInstance(listType)!;
            foreach (var item in value.Split(','))
            {
                list.Add(Convert.ChangeType(item, type.GenericTypeArguments[0]));
            }
            return (T)list;
        }
        else
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            return (T?)converter.ConvertFromString(value);
        }
    }

    private bool IsList(Type type)
    {
        return type.IsGenericType && 
                   type.GetGenericTypeDefinition() == typeof(List<>);
    }
}