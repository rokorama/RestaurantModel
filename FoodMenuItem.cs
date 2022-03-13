using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class FoodMenuItem : MenuItem, IFetchable
    {
        public string Type { get; set; }
        public string _databaseLocation = @"jsonData/food.json";
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
            return $"{Name}\t\t\t{Price}";
        }
    }
}
