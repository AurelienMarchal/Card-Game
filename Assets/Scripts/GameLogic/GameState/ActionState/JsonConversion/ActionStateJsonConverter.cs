

using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameLogic.GameState
{
    public class ActionStateConverter : JsonConverter<ActionState>
    {
        public override ActionState ReadJson(JsonReader reader, Type objectType, ActionState existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            string typeName = jo["type"]?.ToString()?.ToLower();

            if (!ActionStateRegistry.Types.TryGetValue(typeName, out Type targetType))
                throw new JsonSerializationException($"Unknown GameAction type: {typeName}");

            ActionState instance = (ActionState)Activator.CreateInstance(targetType);
            serializer.Populate(jo.CreateReader(), instance);
            return instance;
        }

        
        public override void WriteJson(JsonWriter writer, ActionState value, JsonSerializer serializer)
        {
            JObject jo = JObject.FromObject(value);
            
            string typeName = ActionStateRegistry.Types.FirstOrDefault(x => x.Value == value.GetType()).Key;

            if (typeName != null)
                jo.AddFirst(new JProperty("type", typeName));

            jo.WriteTo(writer);
        }
        
    }
}
