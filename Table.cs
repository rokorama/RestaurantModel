using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Table
    {
        public int Number;
        public int Seats;
        public bool IsOccupied;
        public List<Order> Orders;

        public Table(int number, int seats)
        {
            Number = number;
            Seats = seats;  
            IsOccupied = false;
            // Orders = new List<Order>(); ???? do i need this
        }

        public void AddOrder()
        {
            // Orders.Add(new Order()); ??????
            IsOccupied = true;
        }
}

}
