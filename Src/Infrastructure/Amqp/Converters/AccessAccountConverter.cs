using Domain.AggregateModels.AccessAccountAggregate;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Infrastructure.Amqp.Converters;

internal sealed class AccessAccountConverter : JsonConverter<AccessAccount>
{
    public override void WriteJson(JsonWriter writer, AccessAccount? value, JsonSerializer serializer)
        => writer.WriteValue(value?.ToString());

    public override AccessAccount? ReadJson(
        JsonReader reader, 
        Type objectType, 
        AccessAccount? existingValue, 
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        while (reader.TokenType != JsonToken.String)
        {
            reader.Read();
        }

        var email = (string)reader.Value!;

        while (reader.TokenType != JsonToken.EndObject)
        {
            reader.Read();
        }

        return AccessAccount.Create(email);
    }
}