using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Repository<T>
    {
        public List<T> Items;

        public Repository(IFetchable objectClass)
        {
            Items = objectClass.FetchRecords<T>();
            if (Items == null)
            {
                Items = new List<T>();
            }
        }

        public void AddRecord(IFetchable newEntry)
        {
            Items.Add((T)newEntry);
            FileReaderService.WriteJsonData(newEntry.DatabaseLocation, Items);
        }

        public List<T> LoadRecords(IFetchable objectCategory)
        {
            Items = objectCategory.FetchRecords<T>();
            return Items;
        }
    }
}
