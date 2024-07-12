using System.Text.Json;
using System.Text.Json.Serialization;

namespace PoeWikiApi.Utils
{
    internal static class JsonParser
    {
        public static T? ParseJson<T>(string pJsonString)
        {
            T? jsonDto = JsonSerializer.Deserialize<T>(pJsonString, GetJsonOptions());
            if (jsonDto == null)
            {
                return default;
            }

            return jsonDto;
        }

        public static IEnumerable<T> ParseJsonList<T>(string pJsonString)
        {
            IEnumerable<T>? jsonDtoList = ParseJson<IEnumerable<T>>(pJsonString);
            if (jsonDtoList == null)
            {
                return [];
            }

            return jsonDtoList;
        }

        public static Dictionary<K, T> ParseJsonDict<K, T>(string pJsonString) where K : notnull
        {
            Dictionary<K, T>? jsonDtoDict = ParseJson<Dictionary<K, T>>(pJsonString);
            if (jsonDtoDict == null)
            {
                return [];
            }

            return jsonDtoDict;
        }

        public static Dictionary<K, IEnumerable<T>> ParseJsonListDict<K, T>(string pJsonString) where K : notnull
        {
            Dictionary<K, IEnumerable<T>>? jsonDtoDict = ParseJson<Dictionary<K, IEnumerable<T>>>(pJsonString);
            if (jsonDtoDict == null)
            {
                return [];
            }

            return jsonDtoDict;
        }

        public static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new BoolJsonConverter(),
                    new UintJsonConverter(),
                    new StringJsonConverter(),
                    new StringListJsonConverter()
                }
            };
        }
    }

    public class BoolJsonConverter : JsonConverter<bool>
    {
        public override bool HandleNull => true;

        public override bool Read(ref Utf8JsonReader pReader, Type pTypeToConvert, JsonSerializerOptions pOptions)
        {
            if (pReader.TokenType == JsonTokenType.Null)
            {
                return false;
            }

            if (pReader.TokenType == JsonTokenType.False)
            {
                return false;
            }
            else if (pReader.TokenType == JsonTokenType.True)
            {
                return true;
            }

            return pReader.GetInt16() == 1;
        }

        public override void Write(Utf8JsonWriter pWriter, bool pValue, JsonSerializerOptions pOptions)
        {
            throw new NotImplementedException();
        }
    }

    public class UintJsonConverter : JsonConverter<uint>
    {
        public override bool HandleNull => true;

        public override uint Read(ref Utf8JsonReader pReader, Type pTypeToConvert, JsonSerializerOptions pOptions)
        {
            if (pReader.TokenType == JsonTokenType.Null)
            {
                return 0;
            }
            return pReader.GetUInt16();
        }

        public override void Write(Utf8JsonWriter pWriter, uint pValue, JsonSerializerOptions pOptions)
        {
            throw new NotImplementedException();
        }
    }

    public class StringJsonConverter : JsonConverter<string>
    {
        public override bool HandleNull => true;

        public override string? Read(ref Utf8JsonReader pReader, Type pTypeToConvert, JsonSerializerOptions pOptions)
        {
            if (pReader.TokenType == JsonTokenType.Null)
            {
                return string.Empty;
            }

            return pReader.GetString();
        }

        public override void Write(Utf8JsonWriter pWriter, string pValue, JsonSerializerOptions pOptions)
        {
            throw new NotImplementedException();
        }
    }

    public class StringListJsonConverter : JsonConverter<List<string>>
    {
        public override bool HandleNull => true;

        public override List<string> Read(ref Utf8JsonReader pReader, Type pTypeToConvert, JsonSerializerOptions pOptions)
        {
            // Remove all blank strings in a string list.
            if (pReader.TokenType != JsonTokenType.StartArray)
            {
                return [];
            }

            List<string> flattenedList = [];

            while (pReader.Read())
            {
                if (pReader.TokenType == JsonTokenType.EndArray)
                {
                    return flattenedList;
                }

                string? item = pReader.GetString();
                if (!string.IsNullOrWhiteSpace(item))
                {
                    flattenedList.Add(item);
                }
            }

            return flattenedList;
        }

        public override void Write(Utf8JsonWriter pWriter, List<string> pValue, JsonSerializerOptions pOptions)
        {
            throw new NotImplementedException();
        }
    }
}
