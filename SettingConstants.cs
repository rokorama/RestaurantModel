using System;

namespace RestaurantModel
{
    public class SettingConstants
    {
        // Interface menu formatting
        internal const string RestaurantName = "Restaurant 'Chez Test'";

        // Day-to-day operation settings
        internal const int ValueAddedTax = 22; // in percent

        // Email service settings
        internal const string MailServer = "mail.rokorama.com";
        internal const int MailServerPort = 587;
        internal const string MailClientUsername = "csharp@rokorama.com";
        internal const string MailClientPassword = "43NeHj;2Hc";

        // File location settings
        internal const string TableDatabaseLocation = @"jsonData/tables.json";
        internal const string FoodItemDatabaseLocation = @"jsonData/food.json";
        internal const string DrinkItemDatabaseLocation = @"jsonData/drinks.json";
        internal const string HouseReceiptDatabaseLocation = @"jsonData/orderHistory.json";
    }
}
