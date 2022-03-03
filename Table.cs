using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Table
    {
        public int Number;
        public int Seats;
        public bool IsOccupied;
        public List<MenuItem> Orders;

        public Table(int number, int seats)
        {
            Number = number;
            Seats = seats;
            IsOccupied = false;
            Orders = new List<MenuItem>();
        }
}

}
