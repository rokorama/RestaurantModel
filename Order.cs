using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Order
    {
        public Table Table;
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotal;

        public Order()
        {
            OrderedItems = new List<MenuItem>();
            OrderStartDate = DateTime.Now;
        }
    }
}
