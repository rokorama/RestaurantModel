using System;

namespace RestaurantModel
{
    public interface IPageDisplayable
    {
        static string[] PageMenuHeaders { get; set; }
        static string PageMenuSpacing { get; set; }
        string ToString();
    }
}
