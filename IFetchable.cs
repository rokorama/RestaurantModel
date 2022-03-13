using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public interface IFetchable
    {
        string DatabaseLocation { get; set; }
        List<T> FetchRecords<T>();
    }
}
