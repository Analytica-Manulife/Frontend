using System.Text.Json;
using System.Text.Json.Serialization;

namespace Frontend.Model;

public class NullableIntConverter : JsonConverter<int?>
{
    public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string value = reader.GetString();
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return null; // or return a default value if needed
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.GetValueOrDefault());
    }
}
