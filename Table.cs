using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Table
    {
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

        public Order AddOrder()
        {
            var newOrder = new Order(this);
            ActiveOrder = newOrder;
            IsOccupied = true;
            return newOrder;
        }
}

}
