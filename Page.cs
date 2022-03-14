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

        static public T SelectFromPage(List<T> list, int pageNumber = 1, int pageSize = 9)
        {
            var totalPages = CalculateTotalPages(list.Count);
            Console.Clear();
            PageItems = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            PageNumber = pageNumber;
            PageSize = pageSize;
            Console.WriteLine($"Page {pageNumber} of {totalPages}\n");
            PageItems.ForEach(x => Console.WriteLine(x.ToString()));
            Console.WriteLine();

            Console.WriteLine("Press the number of the item you wish to select");
            if (pageNumber != 1 && totalPages > 1)
                Console.WriteLine("Press , to see previous page");
            if (pageNumber < totalPages)
                Console.WriteLine("Press . to see next page");
            Console.WriteLine("Press B to go back");
            Console.WriteLine();

            var inputPromptResult = InputParser.PromptCharFromUser(PageItems, ',', '.', 'B');
            if (Char.IsNumber(inputPromptResult))
                return PageItems[InputParser.GetIntFromChar(inputPromptResult) - 1];
            else if (inputPromptResult == ',')
            {
                SelectFromPage(list, pageNumber > 1 ? pageNumber-1: 1);
                return default(T);
            }
            else if (inputPromptResult == '.')
            {
                SelectFromPage(list, pageNumber < totalPages ? pageNumber+1 : totalPages);
                return default(T);
            }
            else if (inputPromptResult == 'B')
                return default(T);
            else throw new ArgumentException("Error parsing input");
        }

        //look into System.Text.StringBuilder as an alternative to simple WriteLines

        static public int CalculateTotalPages(int totalItems, int pageSize = 9)
        {
            return (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        }
    }
}
