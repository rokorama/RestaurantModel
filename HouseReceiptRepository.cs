using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class HouseReceiptRepository
    {
        private readonly string _HouseReceiptDatabase = @"csvData/orderHistory.json";
        public List<HouseReceipt> HouseReceiptRepo;

        public HouseReceiptRepository()
        {
            HouseReceiptRepo = FetchRecords();
        }

        public List<HouseReceipt> FetchRecords()
        {
            return FileReaderService.LoadJsonDataToList<HouseReceipt>(_HouseReceiptDatabase); 
        }

        public void AddRecord(HouseReceipt newEntry)
        {
            HouseReceiptRepo.Add(newEntry);
            FileReaderService.WriteJsonData<HouseReceipt>(_HouseReceiptDatabase, HouseReceiptRepo);
        }
    }
}
