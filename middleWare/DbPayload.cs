using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace middleWare
{
    public class DbPayload
    {
        public string accountNumber { get; set; }
        public string customerNumber { get; set; }
    }

    public class DbResponsePayload
    {
        
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


    }

}
