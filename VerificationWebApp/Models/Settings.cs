using System;
namespace VerificationWebApp.Models
{
    public class Settings
    {
        public string connection { get; set; }
        public string apiUrl { get; set; }
        public string imsghAPI { get; set; }
    }

    public class ConfigObject
    {
        public static string KONNECT { get; set; }
        public static string API { get; set; }
        public static string NIA_API { get; set; }
    }

}

