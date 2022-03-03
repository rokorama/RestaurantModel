using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Order
    {
        public Table Table;
        public List<MenuItem> OrderedItems;
        public DateTime TimeOfOrder;
        public decimal OrderTotal;
    }
}
