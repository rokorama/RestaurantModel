using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class HouseReceipt : IEmailable
    {
        [JsonProperty("TableNumber")]
        public int TableNumber;
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotalPrice;
        public decimal RevenueWithoutTax { get; set; }
        public decimal TaxPaid { get; set; }
        public bool ClientReceiptCopySentByEmail { get; set; }
        public bool HouseReceiptCopySentByEmail { get; set; }
        #nullable enable
        public string? ClientReceiptEmailAddress { get; set; } = null;
        public string? HouseReceiptEmailAddress { get; set; } = null; 
        #nullable disable

        public HouseReceipt(Order orderInfo, bool emailReceiptToClient, string clientEmailAdress, bool emailReceiptToHouse, string houseEmailAdress)
        {
            TableNumber = orderInfo.OrderTable.Number;
            OrderedItems = orderInfo.OrderedItems;
            OrderStartDate = orderInfo.OrderStartDate;
            OrderFinishDate = orderInfo.OrderFinishDate;
            OrderTotalPrice = orderInfo.OrderTotalPrice;
            RevenueWithoutTax = (OrderTotalPrice * 88) / 100;
            TaxPaid = OrderTotalPrice - RevenueWithoutTax;
            ClientReceiptCopySentByEmail = emailReceiptToClient;
            ClientReceiptEmailAddress = clientEmailAdress;
            HouseReceiptCopySentByEmail = emailReceiptToHouse;
            HouseReceiptEmailAddress = houseEmailAdress;

            // TODO - add method to write data to receipt database
        }

        [JsonConstructor]
        private HouseReceipt()
        {
        }

        public void EmailReceipt()
        {
            
        }

        public override string ToString()
        {
            return base.ToString();
            // StringBuilder here?
        }
    }
}
