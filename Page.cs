using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantModel
{
     public class Page<T> where T : IPageDisplayable
    {
        static public List<T> PageItems { get; set; }
        static public int PageNumber { get; private set; }
        static public int PageSize { get; set; } 

        static public T PageMenu(List<T> list, 
                                        string displayMessage = "Press the number of the item you wish to select",
                                        string[] displayHeaders = null,
                                        int pageNumber = 1,
                                        int pageSize = 9)
        {
            // Generate headers above list items spacing for the entire list
            var objectProperties = list.GetType().GetGenericArguments()[0].GetProperties();
            var objectHeaders = (string[])objectProperties.Single(x => x.Name == "PageMenuHeaders")
                                                .GetValue("PageMenuHeaders");
            var objectSpacing = (string)objectProperties.Single(x => x.Name == "PageMenuSpacing")
                                                .GetValue("PageMenuSpacing"); // feel like these could be simplified

            // Print page to console
            var totalPages = CalculateTotalPages(list.Count);
            Console.Clear();
            PageItems = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            PageNumber = pageNumber;
            PageSize = pageSize;
            if (displayMessage != null)
            {
                Console.WriteLine(displayMessage);
                Console.WriteLine();
            }
            Console.WriteLine($"Page {pageNumber} of {totalPages}\n");
            Console.WriteLine("   " + String.Format(objectSpacing, objectHeaders)); //spaces to account for item enumeration below
            Console.WriteLine();
            PageItems.ForEach(x => Console.WriteLine($"{PageItems.IndexOf(x) + 1}. {x.ToString()}"));
            Console.WriteLine();
            if (pageNumber != 1 && totalPages > 1)
                Console.WriteLine("Press , to see previous page");
            if (pageNumber < totalPages)
                Console.WriteLine("Press . to see next page");
            Console.WriteLine("Press B to go back");
            Console.WriteLine();

            // Parse and react to user input - move into separate method?
            var inputPromptResult = InputParser.PromptCharFromUser(PageItems, ',', '.', 'B');
            if (Char.IsNumber(inputPromptResult))
                return PageItems[InputParser.GetIntFromChar(inputPromptResult) - 1];
            else if (inputPromptResult == ',')
            {
                PageMenu(list, displayMessage, IPageDisplayable.PageMenuHeaders, pageNumber > 1 ? pageNumber - 1 : 1);
                return default(T);
            }
            else if (inputPromptResult == '.')
            {
                PageMenu(list, displayMessage, IPageDisplayable.PageMenuHeaders, pageNumber < totalPages ? pageNumber + 1 : totalPages);
                return default(T);
            }
            else if (inputPromptResult == 'B')
                return default(T);
            else throw new ArgumentException("Error parsing input");
        }

        static private int CalculateTotalPages(int totalItems, int pageSize = 9)
        {
            return (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
        }
    }
}
