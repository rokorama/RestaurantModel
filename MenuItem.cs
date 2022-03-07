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

        public override string ToString()
        {
            return $"{Name}\t\t\t{Price}";
        }
    }
}
