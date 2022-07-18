using VerificationWebApp.DbData;

namespace VerificationWebApp.Models
{
    public class DbVerification
    {
        
        #region Properties

        public string customerNumber { get; set; }
        public string accountNumber { get; set; }
        public string fullName { get; set; }
        public string dateOfBirth { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string gender { get; set; }
        public string motherMaidenName { get; set; }
        public string mobileNumber { get; set; }
        public string responseCode { get; set; }
        public string description { get; set; }
        public string additionalData { get; set; }

        public bool verificationStatus { get; set; }
        public bool DoBverification { get; set; }
        #endregion


    }
}
