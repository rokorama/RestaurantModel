using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantModel
{
    public class Order
    {
        public Guid OrderId;
        public string Type;
        public Table OrderTable;
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotalPrice;

        public Order(Table table)
        {
            OrderId = Guid.NewGuid();
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

        public void FinaliseOrder(bool sendEmailReceiptToClient, string clientEmailAdress,
                                                         bool sendEmailReceiptToHouse, string houseEmailAdress)
        {
            var generatedClientReceipt = new ClientReceipt(this, sendEmailReceiptToClient, clientEmailAdress);
            var generatedHouseReceipt = new HouseReceipt(this, sendEmailReceiptToClient, clientEmailAdress,
                                                                            sendEmailReceiptToHouse, houseEmailAdress);
            OrderTable.IsOccupied = false;       
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Order {OrderId}");
            sb.AppendLine();
            sb.AppendLine($"Started at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine($"Table {OrderTable.Number}");
            sb.AppendLine();
            OrderedItems.ForEach(x => sb.AppendLine(x.ToString()));
            sb.AppendLine();
            sb.AppendLine($"Order price: {OrderTotalPrice}");
            return sb.ToString();
        }
    }
}
