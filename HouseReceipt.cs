using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class HouseReceipt : IEmailable, IFetchable, IPageDisplayable
    {
        [JsonIgnore]
        static public string DatabaseLocation
        {
            get { return SettingConstants.HouseReceiptDatabaseLocation; }
            set { }   
        }
        [JsonIgnore]
        static public string[] PageMenuHeaders
        {
            get { return new string[] {"Order ID", "Time & date", "Price"}; }
            set { }
        }
        [JsonIgnore]
        static public string PageMenuSpacing
        {
            get { return "{0,-43} {1,-16} {2,10}"; }
            set { }
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
        public bool SendClientReceiptByEmail;
        public bool SendHouseReceiptByEmail;
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
            
            SendClientReceiptByEmail = emailReceiptToClient;
            ClientReceiptEmailAddress = clientEmailAdress;
            SendHouseReceiptByEmail = emailReceiptToHouse;
            HouseReceiptEmailAddress = houseEmailAdress;

            if (SendHouseReceiptByEmail)
                EmailReceipt();
        }

        [JsonConstructor]
        public HouseReceipt()
        {
        }

        public void EmailReceipt()
        {
            StringBuilder emailBody = new StringBuilder();
            emailBody.AppendLine($"<h1>Thanks for your order from {SettingConstants.RestaurantName}!</h1>");
            emailBody.AppendLine($"<p>Order started at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}</p>");
            emailBody.AppendLine($"<p>     finished at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}</p>");
            emailBody.AppendLine($"<p>Table {OrderTable}</p>");
            emailBody.AppendLine($"<p>Items ordered:</p>");
            OrderedItems.ForEach(x => emailBody.AppendLine($"<ol>{x.ToString()}</ol"));
            emailBody.AppendLine($"<p>Order price: {OrderTotalPrice}</p>");
            emailBody.AppendLine($"<p> - Value added tax ({SettingConstants.ValueAddedTax}%): {TaxPaid}</p>");
            emailBody.AppendLine($"<p> - House revenue, minus tax: {RevenueWithoutTax}</p>");

            EmailSendingService.SendEmail(HouseReceiptEmailAddress, emailBody.ToString());
        }

        public string PrintFullDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Order {OrderId}");
            sb.AppendLine();
            sb.AppendLine($"Started at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}");
            sb.AppendLine($"Finished at: {OrderFinishDate.ToString("HH:mm dd/MM/yyyy")}");
            sb.AppendLine();
            sb.AppendLine($"Table {OrderTable}");
            OrderedItems.ForEach(x => sb.AppendLine(x.ToString()));
            sb.AppendLine();
            sb.AppendLine($"Order price: {OrderTotalPrice}");
            sb.AppendLine();
            sb.AppendLine($" - Value added tax ({SettingConstants.ValueAddedTax}%): {TaxPaid}");
            sb.AppendLine($" - House revenue, minus tax: {RevenueWithoutTax}");
            return sb.ToString();
        }

        public List<HouseReceipt> FetchRecords<HouseReceipt>()
        {
            return FileManipulationService.LoadJsonDataToList<HouseReceipt>(DatabaseLocation); 
        }     

        public override string ToString()
        {

            return String.Format(PageMenuSpacing,
                                 OrderId, 
                                 OrderStartDate.ToString("HH:mm dd/MM/yyyy"),
                                 OrderTotalPrice.ToString("0.00"));
        }
    }
}
