using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public static class InputParser
    {
        public static char PromptCharFromUser()
        {
                Console.Write(">>> ");
                var selection = Console.ReadKey().KeyChar;
                return selection;
        }

        public static int PromptIntFromUser()
        {
                Console.Write(">>> ");
                var selection = Console.ReadKey().KeyChar;
                return Convert.ToInt32(Char.GetNumericValue(selection));
        }

        public static char PromptCharFromUser(params char[] acceptableValues)
        {
            bool validInput = false;
            char selection = ' ';

            while (!validInput)
            {
                Console.Write(">>> ");
                selection = Console.ReadKey().KeyChar;
                if (Char.IsLetter(selection))
                    selection = Char.ToUpper(selection);
                var selectionIndex = Array.FindIndex(acceptableValues, x => x == selection);
                if (selectionIndex == -1)
                    Console.WriteLine("\n\nSorry, invalid input. Please try again!");
                else
                    validInput = true;
            }
            return selection;
        }

        public static void PromptForAnyKey()
        {
            Console.Write(">>> ");
            Console.ReadKey();
        }

        public static int GetIntFromChar(char inputChar)
        {
            return Convert.ToInt32(Char.GetNumericValue(inputChar));
        }

        public static decimal PromptDecimalFromUser()
        {
            decimal result = 0;
            bool valueIsValid = false;
            while (!valueIsValid)
            {
            Console.Write(">>> ");
            if (Decimal.TryParse(Console.ReadLine(), out result))
                break;    
            Console.Write("\nInvalid input, please try again! Hit any key to retry.");
            Console.ReadKey();
            }
            return result;
        }

        public static List<char> AddRangeOfAcceptableValues(int list)
        {
            var result = new List<char>();
            for (int indexCounter = 1; indexCounter <= list; indexCounter++)
            result.Add(Char.Parse(indexCounter.ToString()));
            return result;
        }
    }
}
