using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class HouseReceipt : IEmailable, IFetchable
    {
        public string Type { get; set; }
        private string _databaseLocation = @"jsonData/orderHistory.json";
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

        public Guid OrderId;
        public int OrderTable;
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotalPrice;
        public decimal RevenueWithoutTax;
        public decimal ValueAddedTax;
        public decimal TaxPaid;
        public bool ClientReceiptCopySentByEmail;
        public bool HouseReceiptCopySentByEmail;
        #nullable enable
        public string? ClientReceiptEmailAddress = null;
        public string? HouseReceiptEmailAddress = null;
        #nullable disable

        public HouseReceipt(Order orderInfo, bool emailReceiptToClient, string clientEmailAdress, bool emailReceiptToHouse, string houseEmailAdress)
        {
            OrderId = orderInfo.OrderId;
            OrderTable = orderInfo.OrderTable.Number;
            OrderedItems = orderInfo.OrderedItems;
            OrderStartDate = orderInfo.OrderStartDate;
            OrderFinishDate = orderInfo.OrderFinishDate;
            OrderTotalPrice = orderInfo.OrderTotalPrice;
            ValueAddedTax = SettingConstants.ValueAddedTax;
            TaxPaid = (OrderTotalPrice * ValueAddedTax) / 100;
            RevenueWithoutTax = OrderTotalPrice - TaxPaid;
            ClientReceiptCopySentByEmail = emailReceiptToClient;
            ClientReceiptEmailAddress = clientEmailAdress;
            HouseReceiptCopySentByEmail = emailReceiptToHouse;
            HouseReceiptEmailAddress = houseEmailAdress;
        }

        [JsonConstructor]
        public HouseReceipt()
        {
        }

        public void EmailReceipt()
        {

        }

        public override string ToString()
        {
            return $"ID: {OrderId} || Time: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")} || Price: {OrderTotalPrice}";
        }

        public string GetFullDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Order {OrderId}");
            sb.AppendLine();
            sb.AppendLine($"Started at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}");
            sb.AppendLine($"Finished at: {OrderFinishDate.ToString("HH:mm dd/MM/yyyy")}");
            sb.AppendLine($"Table {OrderTable}");
            OrderedItems.ForEach(x => sb.AppendLine(x.ToString()));
            sb.AppendLine();
            sb.AppendLine($"Order price: {OrderTotalPrice}");
            sb.AppendLine();
            sb.AppendLine($" - Value added tax ({SettingConstants.ValueAddedTax}%): {TaxPaid}");
            sb.AppendLine($" - House evenue, minus tax: {RevenueWithoutTax}");
            return sb.ToString();
        }

        public List<HouseReceipt> FetchRecords<HouseReceipt>()
        {
            return FileReaderService.LoadJsonDataToList<HouseReceipt>(DatabaseLocation); 
        }
    }
}
