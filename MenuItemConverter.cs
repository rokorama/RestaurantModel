using System;
using Newtonsoft.Json;

namespace RestaurantModel
{
    class MenuItemConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return (objectType == typeof(MenuItem));
    }
 
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return serializer.Deserialize(reader, typeof(MenuItem));
    }
 
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value, typeof(MenuItem));
    }
}
}
