using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;

namespace RestaurantModel
{
    static public class FileReaderService
    {
        static public void WriteJsonData<T>(string databaseLocation, List<T> updatedData)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

            using (StreamWriter sw = new StreamWriter(databaseLocation))
            using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                serializer.Serialize(writer, updatedData, typeof(MenuItem));
            }
        }

        public static List<T> LoadJsonDataToList<T>(string fileName)
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(fileName), new Newtonsoft.Json.JsonSerializerSettings 
            { 
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            });
            return result;
        }
    }
}
