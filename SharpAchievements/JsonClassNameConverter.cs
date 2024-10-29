namespace SharpAchievements.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsonClassNameConverter : JsonConverter
    {
        private static readonly Dictionary<string, Type> ClassNameToTypeMap = new Dictionary<string, Type>
        {
            { nameof(Badge), typeof(Badge) },
            { nameof(Rank), typeof(Rank) },
            { nameof(Achievement), typeof(Achievement) },
            { nameof(ScoreData), typeof(ScoreData) },
            { nameof(AchievementData), typeof(AchievementData) }
        };

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.GetType().Name);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var typeName = (string)reader.Value;

            var type = GetTypeFromName(typeName);
            if (type != null)
            {
                return Activator.CreateInstance(type);
            }

            throw new JsonSerializationException($"Unknown class name: {typeName}");
        }

        private Type GetTypeFromName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }   

            Type type = Type.GetType(typeName);
            if (type == null)
            {
                ClassNameToTypeMap.TryGetValue(typeName, out type);
            }

            return type;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
