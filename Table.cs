using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Table : IFetchable
    {
        public string Type { get; set; }
        public string _databaseLocation = @"jsonData/tables.json";
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
        public int Number;
        public int Seats;
        public bool IsOccupied;
        public Order ActiveOrder;

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
            return FileReaderService.LoadJsonDataToList<Table>(DatabaseLocation); 
        }

        public override string ToString()
        {
            return $"{Number}\t{Seats}\t{(IsOccupied ? "occupied" : "vacant")}";
        }
    }

}
