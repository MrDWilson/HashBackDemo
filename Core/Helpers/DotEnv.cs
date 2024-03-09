using System;
using System.IO;

namespace HashBack.Helpers;

public static class DotEnv
{
    public static void Load(string filePath)
    {
        if (!File.Exists(filePath))
            return;

        string ToString(IEnumerable<char> x) => new(x.ToArray());

        foreach (var line in File.ReadAllLines(filePath))
        {
            Environment.SetEnvironmentVariable(
                ToString(line.TakeWhile(x => x != '=')).Trim('"'), 
                ToString(line.SkipWhile(x => x != '=').Skip(1)).Trim('"')
            );
        }
    }
}