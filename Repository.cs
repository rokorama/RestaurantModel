using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class Repository<T> where T : IFetchable
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

        public void AddRecord(T newEntry)
        {
            Items.Add((T)newEntry);
            FileReaderService.WriteJsonData(IFetchable.DatabaseLocation, Items);
        }

        public List<T> LoadRecords(IFetchable objectCategory)
        {
            Items = objectCategory.FetchRecords<T>();
            return Items;
        }
    }
}
