using System.Text.Json;
using System.Text.Json.Serialization;

public class NullableDoubleConverter : JsonConverter<double?>
{
    public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string value = reader.GetString();
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            return null; // or you can return a default value like 0 depending on your requirements
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetDouble();
        }

        return null; // or throw exception depending on your needs
    }

    public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.GetValueOrDefault());
    }
}