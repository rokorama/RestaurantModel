using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class RestaurantInterface
    {
        private readonly string _foodDatabaseLocation = "/Users/crisc/csharp/RestaurantModel/csvData/food.csv";
        private readonly string _drinkDatabaseLocation = "/Users/crisc/csharp/RestaurantModel/csvData/drinks.csv";
        private readonly string __tableDatabaseLocation = "/Users/crisc/csharp/RestaurantModel/csvData/tables.csv";
        public List<Table> RestaurantTables;
        public List<MenuItem> FoodItems;
        public List<MenuItem> DrinkItems;

        public RestaurantInterface()
        {
            RestaurantTables = FileReaderService.GenerateTableList(__tableDatabaseLocation);
            FoodItems = FileReaderService.BuildMenuFromCSV(_foodDatabaseLocation);
            DrinkItems = FileReaderService.BuildMenuFromCSV(_drinkDatabaseLocation);
            HomeMenu();
        }

        public void HomeMenu()
        {
            //acceptable values go here
            List<char> inputOptions = new List<char>() {'1','2','3','4','q','Q'};
            Console.Clear();
            Console.WriteLine("\n\nEnter 1 to start a new order");
            Console.WriteLine("      2 to manage existing orders");
            Console.WriteLine("      3 for table overview");
            Console.WriteLine("      4 see order history");
            Console.WriteLine("\n\n Other options:\n");
            Console.WriteLine("Q - quit");
            char selection = InputParser.PromptCharFromUser(inputOptions);
            if (selection == '1')
                StartNewOrder(RestaurantTables);
            if (selection == '2')
                ViewAllTables(RestaurantTables);
            if (selection == '3')
                ViewAllTables(RestaurantTables);
            if (selection == '4')
                ViewOrderHistory();
            if (selection == 'b' || selection == 'B')
            if (selection == 'q' || selection == 'Q')
            {
                Console.Clear();
                Environment.Exit(0);
            }
        }

        public void ViewAllTables(List<Table> tables)
        {
            Console.Clear();
            Console.Write($"Table\tSeats\tStatus\n");
            RestaurantTables.ForEach(table =>
                Console.WriteLine($"{table.Number}\t{table.Seats}\t{(table.IsOccupied ? "occupied" : "vacant")}"));
        }

        public void StartNewOrder(List<Table> tables)
        {
            ViewAllTables(RestaurantTables);
            Console.WriteLine("\nSelect a vacant table to start a new order.");
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                var selectedTable = RestaurantTables[InputParser.PromptIntFromUser() - 1];
                if (!selectedTable.IsOccupied)
                    {
                        selectedTable.AddOrder();
                        break;
                    }
                Console.WriteLine("Invalid selection, please try again.");
            }
        }

        public void ManageTables(List<Table> tables)
        {
            
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