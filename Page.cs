using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantModel
{
    public class Page<T> where T : IPageDisplayable
    {
        static public List<T> AllItemsInRepo;
        static public List<T> PageItems
        {
            get { return AllItemsInRepo.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList(); }
            set { }
        }
        static public int PageNumber { get; private set; }
        static public int PageSize { get; set; } 
        static public int TotalPages { get; private set; }
        static public string DisplayMessage;
        static public string ObjectSpacing;
        static public string[] ObjectHeaders;

        public Page(List<T> sourceList, 
                    string displayMessage = "Press the number of the item you wish to select",
                    string[] displayHeaders = null,
                    int pageNumber = 1,
                    int pageSize = 9)
        {
            AllItemsInRepo = sourceList;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((decimal)AllItemsInRepo.Count / (decimal)PageSize);
            DisplayMessage = displayMessage;

            // Generate headers above list items and spacing between columns
            var objectProperties = sourceList.GetType().GetGenericArguments()[0].GetProperties();
            ObjectHeaders = (string[])objectProperties.Single(x => x.Name == "PageMenuHeaders")
                                                .GetValue("PageMenuHeaders");
            ObjectSpacing = (string)objectProperties.Single(x => x.Name == "PageMenuSpacing")
                                                .GetValue("PageMenuSpacing"); // 
                                                
            PrintPage(PageNumber);
        }
        static internal void PrintPage(int pageNumber)
        {
            PageNumber = pageNumber;
            PageItems = AllItemsInRepo.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();

            // Print page items to console
            Console.Clear();
            if (DisplayMessage != null)
            {
                Console.WriteLine(DisplayMessage);
                Console.WriteLine();
            }
            if (TotalPages != 0)
                Console.WriteLine($"Page {PageNumber} of {TotalPages}\n");
            Console.WriteLine("   " + String.Format(ObjectSpacing, ObjectHeaders)); //Spaces in beginning of string to account for item enumeration below
            Console.WriteLine();
            if (PageItems.Count == 0)
                Console.WriteLine("\n\t** No items to display! **\n");
            else
                PageItems.ForEach(x => Console.WriteLine($"{PageItems.IndexOf(x) + 1}. {x.ToString()}"));

            // List out available navigation options
            Console.WriteLine();
            if (PageNumber != 1 && TotalPages > 1)
                Console.WriteLine("Press , to see previous page");
            if (PageNumber < TotalPages)
                Console.WriteLine("Press . to see next page");
            Console.WriteLine("Press B to go back");
            Console.WriteLine();
        }

        public T GetUserSelectionFromPage()
        {
            var result = default(T);
            bool validSelectionMade = false;
            while (!validSelectionMade)
            {
                var inputPromptResult = InputParser.PromptCharFromUser(PageItems, ',', '.', 'B');
                if (Char.IsNumber(inputPromptResult))
                {
                    result = PageItems[InputParser.GetIntFromChar(inputPromptResult) - 1];
                    validSelectionMade = true;
                }
                else if (inputPromptResult == ',')
                    PrintPage(PageNumber > 1 ? PageNumber - 1 : 1);
                else if (inputPromptResult == '.')
                    PrintPage(PageNumber < TotalPages ? PageNumber + 1 : TotalPages);
                else if (inputPromptResult == 'B')
                    validSelectionMade = true;
                else throw new ArgumentException("Error parsing input");
            }
            return result;
        }
    }
}
