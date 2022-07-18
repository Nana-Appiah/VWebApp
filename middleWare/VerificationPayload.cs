using System;
namespace middleWare
{
    public class VerificationPayload
    {
        public string shortGuid { get; set; }
        public string responseCode { get; set; }
        public string description { get; set; }
        public string additionalData { get; set; }

    }

    public class GHVerificationRequestBody
    {
        public string nationalId { get; set; }
    }

}

