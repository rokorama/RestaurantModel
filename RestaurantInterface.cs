using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class RestaurantInterface
    {
        private readonly string _foodDatabaseLocation = "/Users/crisc/csharp/RestaurantModel/csvData/food.csv";
        private readonly string _drinkDatabaseLocation = "/Users/crisc/csharp/RestaurantModel/csvData/drinks.csv";
        private readonly string _tableDatabaseLocation = "/Users/crisc/csharp/RestaurantModel/csvData/tables.csv";
        public List<Table> RestaurantTables;
        public List<MenuItem> FoodItems;
        public List<MenuItem> DrinkItems;

        public RestaurantInterface()
        {
            RestaurantTables = FileReaderService.GenerateTableList(_tableDatabaseLocation);
            FoodItems = FileReaderService.BuildMenuFromCSV(_foodDatabaseLocation);
            DrinkItems = FileReaderService.BuildMenuFromCSV(_drinkDatabaseLocation);
            HomeMenu();
        }

        public void HomeMenu()
        {
            //acceptable values go here
            Console.Clear();
            Console.WriteLine("\n\nEnter 1 to start a new order");
            Console.WriteLine("      2 to manage existing orders");
            Console.WriteLine("      3 for table overview");
            Console.WriteLine("      4 see order history");
            Console.WriteLine("\n\n Other options:\n");
            Console.WriteLine("Q - quit");
            char selection = InputParser.PromptCharFromUser(new char[] {'1','2','3','4','Q'});
            if (selection == '1')
                StartNewOrder(RestaurantTables);
            else if (selection == '2')
                PrintAllTables(RestaurantTables, 1);
            else if (selection == '3')
                PrintAllTables(RestaurantTables, 1);
            else if (selection == '4')
                ViewOrderHistory();
            else if (selection == 'Q')
            {
                Console.Clear();
                Environment.Exit(0);
            }
        }

        public void PrintAllTables(List<Table> tables, int pageNumber)
        {
            var page = Page<Table>.GetPage(true, tables, pageNumber);
            Console.Clear();
            Console.Write($"Table\tSeats\tStatus\n\n");
            page.ForEach(table =>
                Console.WriteLine($"{table.Number}\t{table.Seats}\t{(table.IsOccupied ? "occupied" : "available")}"));
        }

        public void StartNewOrder(List<Table> tables)
        {
            Table selectedTable = default;
            Order startedOrder = default;
            PrintAllTables(tables, 1);
            Console.WriteLine("\nSelect a vacant table to start a new order, or press B to go back.");
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                char selection = InputParser.PromptCharFromUser();
                if (selection == 'B')
                    HomeMenu();
                try
                {
                    selectedTable = tables[InputParser.GetIntFromChar(selection) - 1];
                    if (!selectedTable.IsOccupied)
                    {
                        startedOrder = new Order(selectedTable);
                        ValidSelectionMade = true;
                    }
                    else
                        Console.WriteLine("This table is occupied, please select another one.");
                        Console.ReadKey();
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid selection, please try again.\n");
                    InputParser.PromptForAnyKey();
                }
            }
            Console.WriteLine($"\nNew order started at table {selectedTable.Number}. Would you like to add items now?");
            var orderAdditionAnswer = InputParser.PromptCharFromUser(new char[] {'Y', 'N'});
            if (orderAdditionAnswer == 'Y')
            {
                OrderAdditionMenu(startedOrder);
            }
            else if (orderAdditionAnswer == 'N')
                HomeMenu();
        }

        public void ManageTables(List<Table> tables)
        {
            
        }

        public void OrderAdditionMenu(Order targetOrder)
        {
            Console.WriteLine("Select menu category:\n");
            Console.WriteLine("Press 1 for food");
            Console.WriteLine("      2 for drinks\n");
            Console.WriteLine("      B to go back");
            var menuCategoryAnswer = InputParser.PromptCharFromUser(new char[] {'1', '2', 'B'});
            if (menuCategoryAnswer == '1')
                {
                    var page = Page<MenuItem>.GetPage(true, FoodItems);
                    var selectionIndex = InputParser.PromptIntFromUser() - 1; //add check for acceptable values
                    var selectedItem = page[selectionIndex];
                    targetOrder.AddItemToOrder(selectedItem);
                    Console.WriteLine($"{selectedItem.Name} has been added to the order. Press any key to return to the item menu.");
                    InputParser.PromptForAnyKey();
                    OrderAdditionMenu(targetOrder);
                }
            else if (menuCategoryAnswer == '2')
                {
                    var page = Page<MenuItem>.GetPage(true, DrinkItems);
                    var selectionIndex = InputParser.PromptIntFromUser() - 1; //add check for acceptable values
                    var selectedItem = page[selectionIndex];
                    targetOrder.AddItemToOrder(selectedItem);
                    Console.WriteLine($"{selectedItem.Name} has been added to the order. Press any key to return to the item menu.");
                    InputParser.PromptForAnyKey();
                    OrderAdditionMenu(targetOrder);
                }
            else if (menuCategoryAnswer == 'B')
                HomeMenu();
        }

        public void ViewOrderHistory()
        {
            // read HouseReceipt database
        }
    }
}

// start new order
    // print tables & status and ask to select a free one
// manage orders
    // add items to order
    // remove item
    // finish order
// view tables
    // all table stats & order start date
// view order history
// quit

// housereceipt also has a order start date