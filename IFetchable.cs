using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public interface IFetchable
    {
        static string DatabaseLocation { get; set; }
        List<T> FetchRecords<T>();
    }
}
