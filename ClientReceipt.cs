using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantModel
{
    public class ClientReceipt : IEmailable
    {
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotalPrice;
        public decimal TaxPaid;
        public bool SendClientReceiptByEmail;
        public string ClientReceiptEmailAddress;

        public ClientReceipt(Order orderInfo, bool emailReceiptToClient, string clientEmailAdress)
        {
            OrderedItems = orderInfo.OrderedItems;
            OrderStartDate = orderInfo.OrderStartDate;
            OrderFinishDate = orderInfo.OrderFinishDate;
            OrderTotalPrice = orderInfo.OrderTotalPrice;
            TaxPaid = (OrderTotalPrice * 22) / 100;
            SendClientReceiptByEmail = emailReceiptToClient;
            ClientReceiptEmailAddress = clientEmailAdress;

            if (SendClientReceiptByEmail)
                EmailReceipt();
        }

        public void EmailReceipt()
        {
            StringBuilder emailBody = new StringBuilder();
            emailBody.AppendLine($"<h1>Thanks for your order from {SettingConstants.RestaurantName}!</h1>");
            emailBody.AppendLine($"<p>Order started at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}</p>");
            emailBody.AppendLine($"<p>     finished at: {OrderStartDate.ToString("HH:mm dd/MM/yyyy")}</p>");
            emailBody.AppendLine($"<p>Items ordered:</p>");
            OrderedItems.ForEach(x => emailBody.AppendLine($"<ol>{x.ToString()}</ol"));
            emailBody.AppendLine($"<p>Order price: {OrderTotalPrice}</p>");
            emailBody.AppendLine($"<p> - Of which value added tax ({SettingConstants.ValueAddedTax}%): {TaxPaid}</p>");

            EmailSendingService.SendEmail(ClientReceiptEmailAddress, emailBody.ToString());
        }
    }
}
