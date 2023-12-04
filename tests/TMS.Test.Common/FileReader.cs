using System.Reflection;

namespace TMS.Test.Common;

public static class FileReader 
{
    public static string ReadAsString(params string[] path) 
    {
        var filePath = BuildPath(path);

        return File.ReadAllText(filePath);
    }

    private static string BuildPath(params string[] path) 
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var result = assemblyPath!;

        foreach (var item in path)
        {
            result = Path.Combine(result, item);
        }

        return result;
    }
}