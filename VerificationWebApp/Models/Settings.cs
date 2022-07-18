using System;
namespace VerificationWebApp.Models
{
    public class Settings
    {
        public string connection { get; set; }
        public string apiUrl { get; set; }
        public string imsghAPI { get; set; }
        public string verifyGHCardAPI { get; set; }
        public string postDataAPI { get; set; }
        public string apiKey { get; set; }
    }

    public class ConfigObject
    {
        public static string KONNECT { get; set; }
        public static string Db_API { get; set; }
        public static string NIA_API { get; set; }

        public static string GhanaCardVerificationAPI { get; set; }
        public static string postCustomerDataAPI { get; set; }
        public static string API_KEY { get; set; }
    }

}

