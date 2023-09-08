using System;
using System.Collections.Generic;
using System.Text;

namespace eFashionShop.Constants
{
    public class SystemConstants
    {
        public const string MainConnectionString = "eShopSolutionDb";
        public const string CartSession = "CartSession";

        public class AppSettings
        {
            public const string Token = "Token";
            public const string BaseAddress = "BaseAddress";
        }

        public class ProductSettings
        {
            public const int NumberOfFeaturedProducts = 8;
            public const int NumberOfFeaturedImages = 5;
        }

        public class ProductConstants
        {
            public const string NA = "N/A";
        }
    }
}