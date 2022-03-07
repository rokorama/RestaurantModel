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

        public Order(Table table)
        {
            Table = table;
            table.IsOccupied = true;

            OrderedItems = new List<MenuItem>();
            OrderStartDate = DateTime.Now;
        }

        public void AddItemToOrder(MenuItem entry)
        {
            OrderedItems.Add(entry);
        }

        public MenuItem SelectItem()
        {
            var selection = InputParser.PromptIntFromUser() - 1;
            return OrderedItems[selection];
        }
    }
}
