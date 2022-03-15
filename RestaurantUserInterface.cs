using System;
using System.Collections.Generic;

namespace RestaurantModel
{
    public class RestaurantUserInterface
    {
        public Repository<HouseReceipt> HouseReceiptRepo;
        public Repository<Table> TableRepo;
        public Repository<FoodMenuItem> FoodRepo;
        public Repository<DrinkMenuItem> DrinksRepo;

        public RestaurantUserInterface()
        {
            HouseReceiptRepo = new Repository<HouseReceipt>(new HouseReceipt());
            TableRepo = new Repository<Table>(new Table());
            FoodRepo = new Repository<FoodMenuItem>(new FoodMenuItem());
            DrinksRepo = new Repository<DrinkMenuItem>(new DrinkMenuItem());
            HomeMenu();
        }

        public void HomeMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"{SettingConstants.RestaurantName} - order interface");
            Console.WriteLine();
            Console.WriteLine("Enter 1 to start a new order");
            Console.WriteLine("      2 to manage existing orders");
            Console.WriteLine("      3 see order history");
            Console.WriteLine();
            Console.WriteLine("Q - quit");
            char selection = InputParser.PromptCharFromUser(new char[] {'1','2','3','Q'});
            if (selection == '1')
                StartNewOrder(TableRepo);
            else if (selection == '2')
                OrderManagementMenu(TableRepo);
            else if (selection == '3')
                ViewOrderHistory();
            else if (selection == 'Q')
            {
                Console.Clear();
                Environment.Exit(0);
            }
        }

        public void StartNewOrder(Repository<Table> tables)
        {
            Table selectedTable = default;
            Order startedOrder = default;
            var displayMessage = "Select a vacant table to start a new order";
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                selectedTable = Page<Table>.SelectFromPage(tables.Items, displayMessage, Table.PageMenuHeaders, 1);
                if (selectedTable == null)
                    HomeMenu();
                else if (!selectedTable.IsOccupied)
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
            Console.WriteLine($"\nNew order started at table {selectedTable.Number}. Would you like to add items now? (Y/N)");
            if (InputParser.PromptForYesOrNo())
                OrderAdditionMenu(startedOrder);
            else 
                HomeMenu();
        }

        public void OrderManagementMenu(Repository<Table> tables)
        {
            var displayMessage = "Please choose a table:";
            var selectedTable = Page<Table>.SelectFromPage(tables.Items, displayMessage, Table.PageMenuHeaders, 1);
            if (selectedTable == null)
                HomeMenu();
            if (!selectedTable.IsOccupied)
            {
                    Console.WriteLine();
                    Console.WriteLine("No active order at this table. Press any key to retry");
                    InputParser.PromptForAnyKey();
                    OrderManagementMenu(tables);
            }
            Console.Clear();
            Console.WriteLine($"TABLE {selectedTable.Number} - MENU");
            Console.WriteLine();
            Console.WriteLine("Choose an action from the options below:");
            Console.WriteLine();
            Console.WriteLine("Press A to add an item to the order");
            Console.WriteLine("      R to remove an item to the order");
            Console.WriteLine("      F to finish the order and generate receipts");
            Console.WriteLine("      V to view items ordered so far");
            Console.WriteLine("      B to go back");
            var selection = InputParser.PromptCharFromUser(new char[] {'A', 'R', 'F', 'V', 'B'});
            if (selection == 'A') 
            {
                OrderAdditionMenu(selectedTable.ActiveOrder);
            }
            else if (selection == 'R') // refactor into ItemRemovalMenu()?
            {
                var itemToRemove = Page<MenuItem>.SelectFromPage(selectedTable.ActiveOrder.OrderedItems);
                if (itemToRemove == null)
                    OrderManagementMenu(tables);
                selectedTable.ActiveOrder.OrderedItems.Remove(itemToRemove);
            }
            else if (selection == 'F')
            {
                FinaliseOrder(selectedTable.ActiveOrder);
            }
            else if (selection == 'V')
            {
                Console.Clear();
                Console.WriteLine(selectedTable.ActiveOrder.ToString());
                Console.WriteLine();
                Console.WriteLine("Press any key to go back");
                InputParser.PromptForAnyKey();
                OrderManagementMenu(tables);
            }
            else if (selection == 'B')
            {
                OrderManagementMenu(tables);
            }
        }

        public void OrderAdditionMenu(Order targetOrder)
        {
            var page = new List<MenuItem>();
            var displayMessage = "Please select an item from the list:";

            Console.Clear();
            Console.WriteLine("Select menu category:");
            Console.WriteLine();
            Console.WriteLine("Press 1 for food");
            Console.WriteLine("      2 for drinks");
            Console.WriteLine();
            Console.WriteLine("      B to go back");

            var menuCategoryAnswer = InputParser.PromptCharFromUser(new char[] {'1', '2', 'B'});
            var selectedItem = default(MenuItem);
            if (menuCategoryAnswer == 'B')
                HomeMenu();
            else if (menuCategoryAnswer == '1')
                selectedItem = Page<FoodMenuItem>.SelectFromPage(FoodRepo.Items, displayMessage, FoodMenuItem.PageMenuHeaders);
            else if (menuCategoryAnswer == '2')
                selectedItem = Page<DrinkMenuItem>.SelectFromPage(DrinksRepo.Items, displayMessage, DrinkMenuItem.PageMenuHeaders);
            if (selectedItem == null)
                OrderAdditionMenu(targetOrder);
            targetOrder.AddItemToOrder(selectedItem);
            Console.WriteLine();
            Console.WriteLine($"{selectedItem.Name} has been added to the table's order. Press any key to return to the item menu.");
            InputParser.PromptForAnyKey();
            OrderAdditionMenu(targetOrder);
        }

        public void FinaliseOrder(Order orderToFinalise) // TODO - move into Order class
        {
            Console.WriteLine("\n\nFinalise this order? Press Y for yes, N for no:");
            if (!InputParser.PromptForYesOrNo())
                OrderManagementMenu(TableRepo);

            orderToFinalise.OrderFinishDate = DateTime.Now;
            
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
            var selectedOrder = Page<HouseReceipt>.SelectFromPage(HouseReceiptRepo.Items);
            if (selectedOrder == null)
                HomeMenu();
            Console.Clear();
            Console.WriteLine(selectedOrder.PrintFullDetails());
            Console.WriteLine("Press any key to go back.");
            InputParser.PromptForAnyKey();
            ViewOrderHistory();
        }
    }
}