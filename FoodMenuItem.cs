using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class FoodMenuItem : MenuItem, IFetchable, IPageDisplayable
    {
        public string Type { get; set; }

        [JsonIgnore] 
        static public string DatabaseLocation
        {
            get { return SettingConstants.FoodItemDatabaseLocation; }
            set { }   
        }

        [JsonIgnore]
        static public string[] PageMenuHeaders
        {
            get { return new string[] {"Item name", "Price (Euros)"}; }
            set { }
        }
        
        [JsonIgnore]
        static public string PageMenuSpacing
        {
            get { return "{0,-30} {1,10}"; }
            set { }
        }
        
        public FoodMenuItem(string name, decimal price) : base(name, price)
        {
            Name = name;
            Price = price;
        }

        public FoodMenuItem() : base()
        {
            
        }

        public List<FoodMenuItem> FetchRecords<FoodMenuItem>()
        {
            return FileReaderService.LoadJsonDataToList<FoodMenuItem>(DatabaseLocation); 
        }

        public override string ToString()
        {
            return String.Format(PageMenuSpacing, Name, Price.ToString("0.00"));
        }
        public Type GetType<T>()
        {
            return typeof(T);
        } 
    }
}
