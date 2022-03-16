using System;

namespace RestaurantModel
{
    abstract public class MenuItem : IPageDisplayable
    {
        public string Name;

        public decimal Price;
        static public string[] PageMenuHeaders
        {
            get { return new string[] {"Item name", "Price (Euros)"}; }
            set { }
        }
        static public string PageMenuSpacing
        {
            get { return "{0,-30} {1,10}"; }
            set { }
        }

        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        protected MenuItem()
        {
            
        }
    }
}
