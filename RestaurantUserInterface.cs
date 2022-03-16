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
                QuitProgramScreen();
        }

        public void StartNewOrder(Repository<Table> tables)
        {
            Table selectedTable = default;
            Order startedOrder = default;
            var pageDisplayMessage = "Select a vacant table to start a new order";
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                var pageToDisplay = new Page<Table>(tables.Items, pageDisplayMessage, Table.PageMenuHeaders, 1);
                selectedTable = pageToDisplay.GetUserSelectionFromPage();
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
            var pageToDisplay = new Page<Table>(tables.Items, displayMessage, Table.PageMenuHeaders, 1);
            var selectedTable = pageToDisplay.GetUserSelectionFromPage();

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
                var pageDisplayMessage = "Select the item you wish to remove";
                var removalMenuPage = new Page<MenuItem>(selectedTable.ActiveOrder.OrderedItems, pageDisplayMessage);
                var itemToRemove = removalMenuPage.GetUserSelectionFromPage();
                if (itemToRemove == null)
                    OrderManagementMenu(tables);
                selectedTable.ActiveOrder.OrderedItems.Remove(itemToRemove);
            }
            else if (selection == 'F')
            {
                FinaliseOrderMenu(selectedTable.ActiveOrder);
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
            // var pageToDisplay = default(Page<MenuItem>);
            var pageDisplayMessage = "Please select an item from the list:";

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
            {
                var pageToDisplay = new Page<FoodMenuItem>(FoodRepo.Items, pageDisplayMessage, FoodMenuItem.PageMenuHeaders, 1);
                selectedItem = pageToDisplay.GetUserSelectionFromPage();
            }

            else if (menuCategoryAnswer == '2')
            {
                var pageToDisplay = new Page<DrinkMenuItem>(DrinksRepo.Items, pageDisplayMessage, DrinkMenuItem.PageMenuHeaders, 1);
                selectedItem = pageToDisplay.GetUserSelectionFromPage();
            } 
            if (selectedItem == null)
                OrderAdditionMenu(targetOrder);
            targetOrder.AddItemToOrder(selectedItem);
            Console.WriteLine();
            Console.WriteLine($"{selectedItem.Name} has been added to the table's order. Press any key to return to the item menu.");
            InputParser.PromptForAnyKey();
            OrderAdditionMenu(targetOrder);
        }

        public void FinaliseOrderMenu(Order orderToFinalise) // TODO - move into Order class
        {
            Console.WriteLine("\n\nFinalise this order? Press Y for yes, N for no:");
            if (!InputParser.PromptForYesOrNo())
                OrderManagementMenu(TableRepo);

            Console.WriteLine("\n\nPrint client receipt?");
            if (InputParser.PromptForYesOrNo())
            {
                Console.WriteLine("\nReceipt sent to the printer.");
                System.Threading.Thread.Sleep(1000);
            }

            Console.WriteLine("\nDoes the client wish to get a receipt via email?");
            bool sendEmailReceiptToClient = InputParser.PromptForYesOrNo();
            string clientEmailAdress = null;
            if (sendEmailReceiptToClient)
                clientEmailAdress = InputParser.PromptForEmailAddress();

            Console.WriteLine("\n\nSend a copy of the house receipt via email?");
            bool sendEmailReceiptToHouse = InputParser.PromptForYesOrNo();
            string houseEmailAdress = null;
            if (sendEmailReceiptToHouse)
                houseEmailAdress = InputParser.PromptForEmailAddress();
            
            orderToFinalise.FinaliseOrder(sendEmailReceiptToClient,
                                          clientEmailAdress, 
                                          sendEmailReceiptToHouse, 
                                          houseEmailAdress,
                                          HouseReceiptRepo);

            Console.WriteLine($"Order finalised. You may now start another order at table {orderToFinalise.OrderTable.Number}.");
            InputParser.PromptForAnyKey();
            HomeMenu();
        }

        public void ViewOrderHistory()
        {
            var orderHistoryPage = new Page<HouseReceipt>(HouseReceiptRepo.Items);
            var selectedOrder = orderHistoryPage.GetUserSelectionFromPage();
            if (selectedOrder == null)
                HomeMenu();
            Console.Clear();
            Console.WriteLine(selectedOrder.PrintFullDetails());
            Console.WriteLine("Press any key to go back.");
            InputParser.PromptForAnyKey();
            ViewOrderHistory();
        }

        public void QuitProgramScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Do you wish to exit the program? (Y/N)");
            if (InputParser.PromptForYesOrNo())
            {
                Console.Clear();
                Environment.Exit(0);
            }
            else
                HomeMenu();
        }
    }
}