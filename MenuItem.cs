using System;

namespace RestaurantModel
{
    public class MenuItem
    {
        public string Name;
        public decimal Price;

        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }
}
