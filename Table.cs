using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class Table : IFetchable, IPageDisplayable
    {
        public int Number;
        public int Seats;
        public bool IsOccupied;
        public Order ActiveOrder;

        [JsonIgnore]
        static public string DatabaseLocation
        {
            get { return SettingConstants.TableDatabaseLocation; }
            set { }   
        }
        [JsonIgnore]
        static public string[] PageMenuHeaders
        {
            get { return new string[] {"Table", "Seats", "Status"}; }
            set { }
        }
        [JsonIgnore]
        static public string PageMenuSpacing
        {
            get { return "{0,-10} {1,10} {2,10}"; }
            set { }
        }

        public Table(int number, int seats)
        {
            Number = number;
            Seats = seats;
            ActiveOrder = null;
            IsOccupied = false;
        }

        public Table()
        {
            
        }

        public Order AddOrder()
        {
            var newOrder = new Order(this);
            ActiveOrder = newOrder;
            IsOccupied = true;
            return newOrder;
        }

        public List<Table> FetchRecords<Table>()
        {
            return FileManipulationService.LoadJsonDataToList<Table>(DatabaseLocation); 
        }

        public override string ToString()
        {
            return String.Format(PageMenuSpacing,
                                 $"Table {Number}", 
                                 Seats,
                                 (IsOccupied ? "occupied" : "vacant"));
        }
        public Type GetType<T>()
        {
            return typeof(T);
        }
    }

}
