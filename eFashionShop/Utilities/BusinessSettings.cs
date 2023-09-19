using System.Configuration;

namespace eFashionShop.Utilities
{
    public class BusinessSettings
    {
        private static string GetConfigValue(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            return value;
        }
        public static bool ImageSaveInFolder
        {
            get
            {
                if (string.IsNullOrEmpty(GetConfigValue("ImageSaveInFolder")))
                {
                    return false;
                }
                return bool.Parse(GetConfigValue("ImageSaveInFolder"));
            }
        }
        public static bool IsProduction
        {
            get
            {
                if (string.IsNullOrEmpty(GetConfigValue("IsProduction")))
                {
                    return false;
                }
                return bool.Parse(GetConfigValue("IsProduction"));
            }
        }
        public static string DomainUrl
        {
            get
            {
                return GetConfigValue("DomainUrl");
            }
        }
        public static string USER_CONTENT_FOLDER_NAME
        {
            get
            {
                return GetConfigValue("USER_CONTENT_FOLDER_NAME");
            }
        }
    }
}
