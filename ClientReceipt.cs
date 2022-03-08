using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class ClientReceipt : IEmailable
    {
        public List<MenuItem> OrderedItems;
        public DateTime OrderStartDate;
        public DateTime OrderFinishDate;
        public decimal OrderTotalPrice;
        public decimal TaxPaid;
        public bool ClientReceiptCopySentByEmail;
        public string ClientReceiptEmailAddress;

        public ClientReceipt(Order orderInfo, bool emailReceiptToClient, string clientEmailAdress)
        {
            OrderedItems = orderInfo.OrderedItems;
            OrderStartDate = orderInfo.OrderStartDate;
            OrderFinishDate = orderInfo.OrderFinishDate;
            OrderTotalPrice = orderInfo.OrderTotalPrice;
            TaxPaid = (OrderTotalPrice * 22) / 100;
            ClientReceiptCopySentByEmail = emailReceiptToClient;
            ClientReceiptEmailAddress = clientEmailAdress;
        }

        public void EmailReceipt()
        {

        }
    }
}
