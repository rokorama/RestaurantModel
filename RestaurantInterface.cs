using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RestaurantModel
{
    public class RestaurantInterface
    {
        private readonly string _foodDatabaseLocation = @"csvData/food.csv";
        private readonly string _drinkDatabaseLocation = @"csvData/drinks.csv";
        private readonly string _tableDatabaseLocation = @"csvData/tables.csv";
        public HouseReceiptRepository HouseReceiptRepo;
        public List<Table> RestaurantTables;
        public List<MenuItem> FoodItems;
        public List<MenuItem> DrinkItems;

        public RestaurantInterface()
        {
            HouseReceiptRepo = new HouseReceiptRepository();
            RestaurantTables = FileReaderService.GenerateTableList(_tableDatabaseLocation);
            FoodItems = FileReaderService.BuildMenuFromCSV(_foodDatabaseLocation);
            DrinkItems = FileReaderService.BuildMenuFromCSV(_drinkDatabaseLocation);
            HomeMenu();
        }

        public void HomeMenu()
        {
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
                TableManagementMenu(RestaurantTables);
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

        public void PrintAllTables(List<Table> tables, int pageNumber = 1)
        {
            var page = Page<Table>.GetPage(true, tables, pageNumber);
            Console.Clear();
            Console.Write($"Table\tSeats\tActive order\n\n");
            page.ForEach(table =>
                Console.WriteLine($"{table.Number}\t{table.Seats}\t{(table.IsOccupied ? "yes" : "no")}"));
        }

        public void StartNewOrder(List<Table> tables)
        {
            Table selectedTable = default;
            Order startedOrder = default;
            Console.WriteLine("\nSelect a vacant table to start a new order, or press B to go back.");
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                selectedTable = TableSelectionMenu(RestaurantTables);
                if (!selectedTable.IsOccupied)
                {
                    startedOrder = selectedTable.AddOrder();
                    ValidSelectionMade = true;
                }
                else
                {  
                    Console.WriteLine("\n\nError - this table is occupied, please select another one.");
                    Console.ReadKey();
                }
            }
            Console.WriteLine($"\nNew order started at table {selectedTable.Number}. Would you like to add items now?");
            if (InputParser.PromptForYesOrNo())
                OrderAdditionMenu(startedOrder);
            else 
                HomeMenu();
        }

        public void TableManagementMenu(List<Table> tables)
        {
            Console.Clear();
            Console.WriteLine("Please choose a table:");
            var selectedTable = TableSelectionMenu(RestaurantTables);
            Console.WriteLine("\nChoose an action from the options below:\n");
            Console.WriteLine("Press A to add an item to the order");
            Console.WriteLine("      R to remove an item to the order");
            Console.WriteLine("      F to finish the order and generate receipts");
            var selection = InputParser.PromptCharFromUser(new char[] {'A', 'R', 'F'});
            if (selection == 'A') 
            {
                OrderAdditionMenu(selectedTable.ActiveOrder);
            }
            else if (selection == 'R') // refactor into ItemRemovalMenu()?
            {
                Page<MenuItem>.GetPage(true, selectedTable.ActiveOrder.OrderedItems);
                var itemToRemove = selectedTable.ActiveOrder.SelectItem();
                selectedTable.ActiveOrder.OrderedItems.Remove(itemToRemove);
            }
            else if (selection == 'F')
            {
                FinaliseOrder(selectedTable.ActiveOrder);
            }
        }

        public Table TableSelectionMenu(List<Table> tables)
        {
            PrintAllTables(RestaurantTables);
            Table resultTable = default;
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                char selection = InputParser.PromptCharFromUser();
                if (selection == 'B')
                    HomeMenu();
                try
                {
                    resultTable = tables[InputParser.GetIntFromChar(selection) - 1];
                    ValidSelectionMade = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Invalid selection, please try again.\n");
                    InputParser.PromptForAnyKey();
                }
            }
            return resultTable;
        }

        public void OrderAdditionMenu(Order targetOrder)
        {
            List<MenuItem> page = null;

            Console.Clear();
            Console.WriteLine("Select menu category:\n");
            Console.WriteLine("Press 1 for food");
            Console.WriteLine("      2 for drinks\n");
            Console.WriteLine("      B to go back");

            var menuCategoryAnswer = InputParser.PromptCharFromUser(new char[] {'1', '2', 'B'});
            if (menuCategoryAnswer == 'B')
                HomeMenu();
            else if (menuCategoryAnswer == '1')
                page = Page<MenuItem>.GetPage(true, FoodItems);
            else if (menuCategoryAnswer == '2')
                page = Page<MenuItem>.GetPage(true, DrinkItems);
            var selectionIndex = InputParser.PromptIntFromUser() - 1; //add check for acceptable values
            var selectedItem = page[selectionIndex];
            targetOrder.AddItemToOrder(selectedItem);
            Console.WriteLine($"{selectedItem.Name} has been added to the table's order. Press any key to return to the item menu.");
            InputParser.PromptForAnyKey();
            OrderAdditionMenu(targetOrder);
        }

        public void FinaliseOrder(Order orderToFinalise)
        {
            Console.WriteLine("\n\nFinalise this order? Press Y for yes, N for no:");
            if (!InputParser.PromptForYesOrNo())
                TableManagementMenu(RestaurantTables);
            
            Console.WriteLine("\n\nPrint client receipt?");
            if (InputParser.PromptForYesOrNo())
            {
                Console.WriteLine("\n\nReceipt sent to the printer.");
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("\nDoes the client wish to get a receipt via email?");
            bool sendEmailReceiptToClient = InputParser.PromptForYesOrNo();
            string clientEmailAdress = null;
            if (sendEmailReceiptToClient)
            {
                Console.WriteLine("\n\nEnter destination address:\n>>>");
                clientEmailAdress = Console.ReadLine();
            }

            Console.WriteLine("\n\nSend a copy of the house receipt via email?");
            bool sendEmailReceiptToHouse = InputParser.PromptForYesOrNo();
            string houseEmailAdress = null;
            if (sendEmailReceiptToHouse)
            {
                Console.WriteLine("\n\nEnter destination address:\n>>>");
                houseEmailAdress = Console.ReadLine();
            }

            var generatedClientReceipt = new ClientReceipt(orderToFinalise, sendEmailReceiptToClient, clientEmailAdress);
            var generatedHouseReceipt = new HouseReceipt(orderToFinalise, sendEmailReceiptToClient, clientEmailAdress,
                                                                            sendEmailReceiptToHouse, houseEmailAdress);
            
            HouseReceiptRepo.AddRecord(generatedHouseReceipt);
            
            orderToFinalise.OrderTable.IsOccupied = false;

            Console.WriteLine($"Order finalised. You may now start another order at table {orderToFinalise.OrderTable.Number}.");
            InputParser.PromptForAnyKey();
            HomeMenu();
        }

        public void ViewOrderHistory()
        {
        }
    }
}

// start new order
    // print tables & status and ask to select a free one -- DONE
// manage orders
    // add items to order -- DONE
    // remove item -- DONE (kinda)
    // finish order
// view tables
    // all table stats & order start date
// view order history
// quit

// housereceipt also has a order start date