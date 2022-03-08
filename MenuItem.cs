using System;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class MenuItem
    {
        [JsonProperty("Name")]
        public string Name;

        [JsonProperty("Price")]
        public decimal Price;

        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public override string ToString()
        {
            return $"{Name}\t\t\t{Price}";
        }
    }
}
