using Microsoft.Extensions.Configuration;
using System.IO;

namespace eFashionShop.Utilities
{
    static class BusinessSettings
    {
        public static IConfiguration AppSetting { get; }
        static BusinessSettings()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        public static bool ImageSaveInFolder
        {
            get
            {
                if (string.IsNullOrEmpty(AppSetting["ImageSaveInFolder"]))
                {
                    return false;
                }
                return bool.Parse(AppSetting["ImageSaveInFolder"]);
            }
        }
        public static bool IsProduction
        {
            get
            {
                if (string.IsNullOrEmpty(AppSetting["IsProduction"]))
                {
                    return false;
                }
                return bool.Parse(AppSetting["IsProduction"]);
            }
        }
        public static string DomainUrl
        {
            get
            {
                return AppSetting["DomainUrl"];
            }
        }
        public static string USER_CONTENT_FOLDER_NAME
        {
            get
            {
                return AppSetting["USER_CONTENT_FOLDER_NAME"];
            }
        }

    }
}
