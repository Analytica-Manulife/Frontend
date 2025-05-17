using System.Text;
using System.Text.Json;

namespace Frontend.Filter;

// Add this class to handle snake_case naming policy
public class JsonSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        // Convert property names from PascalCase to snake_case
        if (string.IsNullOrEmpty(name))
            return name;

        var result = new StringBuilder();
        for (int i = 0; i < name.Length; i++)
        {
            if (i > 0 && char.IsUpper(name[i]))
                result.Append('_');
            result.Append(char.ToLower(name[i]));
        }
        return result.ToString();
    }
}

// Add this to your startup or program configuration
public static class JsonExtensions
{
    public static void AddSnakeCaseNamingPolicy(this JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
    }
}