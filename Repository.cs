using System;
using System.Collections.Generic;
using System.Linq;

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
            Items.Add(newEntry);
            var dbLocation = (string)newEntry.GetType()
                                             .GetProperties()
                                             .Single(x => x.Name == "DatabaseLocation")
                                             .GetValue("DatabaseLocation");
            FileManipulationService.WriteJsonData(dbLocation, Items);
        }

        public List<T> LoadRecords(IFetchable objectCategory)
        {
            Items = objectCategory.FetchRecords<T>();
            return Items;
        }
    }
}
