using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Order
    {
        public Table OrderTable;
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotalPrice;

        public Order(Table table)
        {
            OrderTable = table;
            table.IsOccupied = true;

            OrderedItems = new List<MenuItem>();
            OrderStartDate = DateTime.Now;
        }

        public void AddItemToOrder(MenuItem entry)
        {
            OrderedItems.Add(entry);
            OrderTotalPrice += entry.Price;
        }

        public MenuItem SelectItem()
        {
            var selection = InputParser.PromptIntFromUser() - 1;
            return OrderedItems[selection];
        }

        public void FinaliseOrder()
        {
            OrderTable.IsOccupied = true;
            
        }
    }
}
