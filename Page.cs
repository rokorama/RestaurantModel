using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantModel
{
    static public class Page<T>
    {
        static public List<T> PageItems { get; set; }
        static public int PageNumber { get; private set; }
        static public int PageSize { get; set; } 

        static public List<T> GetPage(bool printItems, List<T> list, int pageNumber = 1, int pageSize = 9)
        {
            if (printItems)
                Console.Clear();
            PageItems = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageItems.ForEach(x => Console.WriteLine(x.ToString()));
            return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        //look into System.Text.StringBuilder as an alternative to simple WriteLines

        static public int CalculateTotalPages(int totalItems, int pageSize = 9)
        {
            return (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        }
    }
}
