using System.Text.Json;
using System.Text.Json.Serialization;

namespace Frontend.Model;

public class NullableLongConverter : JsonConverter<long?>
{
    public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string value = reader.GetString();
            if (long.TryParse(value, out long result))
            {
                return result;
            }
            return null; // or return 0 or a default value depending on your preference
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt64();
        }

        return null; // or throw exception based on requirements
    }

    public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.GetValueOrDefault());
    }
}
