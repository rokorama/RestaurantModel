using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantModel
{
    public class RestaurantInterface
    {
        public Repository<HouseReceipt> HouseReceiptRepo;
        public Repository<Table> TableRepo;
        public Repository<FoodMenuItem> FoodRepo;
        public Repository<DrinkMenuItem> DrinksRepo;

        public RestaurantInterface()
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
            Console.WriteLine("\n\nEnter 1 to start a new order");
            Console.WriteLine("      2 to manage existing orders");
            Console.WriteLine("      3 for table overview");
            Console.WriteLine("      4 see order history");
            Console.WriteLine("\n\n Other options:\n");
            Console.WriteLine("Q - quit");
            char selection = InputParser.PromptCharFromUser(new char[] {'1','2','3','4','Q'});
            if (selection == '1')
                StartNewOrder(TableRepo);
            else if (selection == '2')
                TableManagementMenu(TableRepo);
            else if (selection == '3')
                Page<Table>.SelectFromPage(TableRepo.Items, 1);
            else if (selection == '4')
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
            Console.WriteLine("\nSelect a vacant table to start a new order, or press B to go back.");
            bool ValidSelectionMade = false;
            while (!ValidSelectionMade)
            {
                selectedTable = Page<Table>.SelectFromPage(tables.Items, 1);
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
            Console.WriteLine($"\nNew order started at table {selectedTable.Number}. Would you like to add items now?");
            if (InputParser.PromptForYesOrNo())
                OrderAdditionMenu(startedOrder);
            else 
                HomeMenu();
        }

        public void TableManagementMenu(Repository<Table> tables)
        {
            Console.Clear();
            Console.WriteLine("Please choose a table:");
            var selectedTable = Page<Table>.SelectFromPage(tables.Items, 1);
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
                Page<MenuItem>.SelectFromPage(selectedTable.ActiveOrder.OrderedItems);
                var itemToRemove = selectedTable.ActiveOrder.SelectItem();
                selectedTable.ActiveOrder.OrderedItems.Remove(itemToRemove);
            }
            else if (selection == 'F')
            {
                FinaliseOrder(selectedTable.ActiveOrder);
            }
        }

        public void OrderAdditionMenu(Order targetOrder)
        {
            var page = new List<MenuItem>();

            Console.Clear();
            Console.WriteLine("Select menu category:\n");
            Console.WriteLine("Press 1 for food");
            Console.WriteLine("      2 for drinks\n");
            Console.WriteLine("      B to go back");

            List<MenuItem> listOfMenuItems = new List<FoodMenuItem>().Cast<MenuItem>().ToList();

            var menuCategoryAnswer = InputParser.PromptCharFromUser(new char[] {'1', '2', 'B'});
            var selectedItem = default(MenuItem);
            if (menuCategoryAnswer == 'B')
                HomeMenu();
            else if (menuCategoryAnswer == '1')
                selectedItem = Page<FoodMenuItem>.SelectFromPage(FoodRepo.Items, 1);
            else if (menuCategoryAnswer == '2')
                selectedItem = Page<DrinkMenuItem>.SelectFromPage(DrinksRepo.Items, 1);
            targetOrder.AddItemToOrder(selectedItem);
            Console.WriteLine($"{selectedItem.Name} has been added to the table's order. Press any key to return to the item menu.");
            InputParser.PromptForAnyKey();
            OrderAdditionMenu(targetOrder);
        }

        public void FinaliseOrder(Order orderToFinalise) // TODO - move into Order class
        {
            Console.WriteLine("\n\nFinalise this order? Press Y for yes, N for no:");
            if (!InputParser.PromptForYesOrNo())
                TableManagementMenu(TableRepo);

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
            Console.WriteLine(selectedOrder.GetFullDetails());
            Console.WriteLine("Press any key to go back.");
            InputParser.PromptForAnyKey();
        }
    }
}

// start new order
    // print tables & status and ask to select a free one -- DONE
// manage orders
    // add items to order -- DONE
    // remove item -- DONE (kinda)
    // finish order -- DONE
// view tables
    // all table stats & order start date
// view order history
// quit -- DONE