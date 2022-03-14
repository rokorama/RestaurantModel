using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class DrinkMenuItem : MenuItem, IFetchable
    {
        public string Type { get; set; }
        public string _databaseLocation = @"jsonData/drinks.json";
        public string DatabaseLocation
        {
            get
            {
                return _databaseLocation;
            }
            set
            {
                _databaseLocation = value;
            }   
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
            return FileReaderService.LoadJsonDataToList<DrinkMenuItem>(DatabaseLocation); 
        }     

        public override string ToString()
        {
            return String.Format(SettingConstants.MenuItemSpacing, Name, Price.ToString("0.00"));
        }
    }
}