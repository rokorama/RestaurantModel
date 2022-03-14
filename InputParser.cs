using System;
using System.Collections.Generic;
using System.Linq;

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
                bool validInput = false;
                int result = 0;
                while (!validInput)
                {
                    var selection = PromptCharFromUser();
                    result = Convert.ToInt32(Char.GetNumericValue(selection));
                    validInput = (result != -1);
                }
                return result;
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

        public static char PromptCharFromUser<T>(List<T> validIndexSelections, params char[] acceptableValues)
        {
            //rework all arrays here into list<char>
            bool validInput = false;
            char selection = ' ';
            var validSelectionRange = Enumerable.Range(1, validIndexSelections.Count).ToList().ConvertAll(x => Char.Parse(x.ToString()));
            validSelectionRange.AddRange(acceptableValues.ToList());

            while (!validInput)
            {
                Console.Write(">>> ");
                selection = Console.ReadKey().KeyChar;
                if (Char.IsLetter(selection))
                    selection = Char.ToUpper(selection);
                var selectionIndex = validSelectionRange.SingleOrDefault(x => x == selection);
                if (selectionIndex == default(char))
                    Console.WriteLine("\n\nSorry, invalid input. Please try again!");
                else
                    validInput = true;
            }
            return selection;
        }

        public static bool PromptForYesOrNo()
        {
            var result = PromptCharFromUser(new char[] {'Y', 'N'});
            if (result == 'Y')
                return true;
            else
                return false;
        }

        public static void PromptForAnyKey()
        {
            Console.Write(">>> ");
            Console.ReadKey();
        }

        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static int GetIntFromChar(char inputChar)
        {
            return Convert.ToInt32(Char.GetNumericValue(inputChar));
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
