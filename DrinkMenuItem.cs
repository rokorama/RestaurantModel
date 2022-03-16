using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class DrinkMenuItem : MenuItem, IFetchable, IPageDisplayable
    {
        [JsonIgnore]
        static public string DatabaseLocation
        {
            get { return SettingConstants.DrinkItemDatabaseLocation; }
            set { }   
        }
        [JsonIgnore]
        new static public string[] PageMenuHeaders
        {
            get { return new string[] {"Item name", "Price (Euros)"}; }
            set { }
        }
        [JsonIgnore]
        new static public string PageMenuSpacing
        {
            get { return "{0,-30} {1,10}"; }
            set { }
        }

        public DrinkMenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public DrinkMenuItem() : base()
        {
            
        }

        public List<DrinkMenuItem> FetchRecords<DrinkMenuItem>()
        {
            return FileManipulationService.LoadJsonDataToList<DrinkMenuItem>(DatabaseLocation); 
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