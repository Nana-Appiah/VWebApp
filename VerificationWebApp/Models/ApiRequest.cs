using System;
using middleWare;
using System.Diagnostics;


using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json.Net;

using VerificationWebApp.DbData;
using VerificationWebApp.DbModels;

namespace VerificationWebApp.Models
{
	public class ApiRequest
	{
		public PayLoad payLoad { get; set; }

		public DbPayload databasePayLoad { get; set; } 
		
		public Verified oVerified { get; set; }


		private const string merchant_key = @"e4a8745a-131b-4c05-a350-17fd992eba35";

		//private string baseURI = @"https://selfie.imsgh.org:9020/api/v1/third-party/verification";
		//private string databaseURI =  @"http://localhost:8000/api/customer";

		public string databaseURI { get; set; }

		public async Task<data> GetBiometricData()
        {
			data _data = null;

            try
            {
				var api = new ApiServer();

			    _data = await api.ApiRequestDataAsync(payLoad);
				return _data;
            }
			catch(Exception x)
            {
				Debug.Print($"error: {x.Message}");
				return _data;
            }
        }

		public async Task<DbVerification> GetDatabaseRecordAsync(string mobileNo)
        {
			var api = new ApiServer();
			var returnString = await api.GetDatabaseRecordAsync(databasePayLoad,databaseURI);

			if (returnString.Length > 0)
            {
				//deserialize the object
				var x = JObject.Parse(returnString);
				var dt = JsonConvert.DeserializeObject<DbVerification>(returnString);

				/* format and send response to client */
				var db = new DbVerification();

				db.customerNumber = x["customerNumber"].ToString();
				db.accountNumber = x["accountNumber"].ToString();
				db.fullName = x["fullName"].ToString();
				db.dateOfBirth = formatDoB(x["dateOfBirth"].ToString().Split('/'));
				db.documentType = x["documentType"].ToString();
				db.documentNumber = x["documentNumber"].ToString();
				db.gender = x["gender"].ToString();
				db.motherMaidenName = x["motherMaidenName"].ToString();
				db.mobileNumber = x["mobileNumber"].ToString();
				db.responseCode = x["responseCode"].ToString();
				db.description = x["description"].ToString();
				db.additionalData = x["additionalData"].ToString();

				
				db.verificationStatus = doesMobileExist(mobileNo, db.mobileNumber.Split('|'));

				return db;
			}
            else
            {
				return null;
            }
        }

		private string doesMobileExist(string telFromPage, string[] telsOnFile)
        {
			//telFromPage: the telephone number provided by the customer using the web page
			//telsOnFile: the list of telephone numbers on file in the core-banking system
			string s = @"Telephone number NOT found on file";

			foreach(var t in telsOnFile)
            {
				if (telFromPage.Contains(t))
                {
					return s = @"Telephone number found on file";
				}
            }

			return s;
        }

		private string formatDoB(string[] parts)
        {
			return string.Format("{0}-{1}-{2}", parts[2], parts[0], parts[1]);
        }
		

		public async Task<bool> SaveRecordAsync()
        {
            //method is used to save record asynchronously
            try
            {
				using (var config = new IDVerificationTestContext())
                {
					config.Verifieds.Add(oVerified);
					await config.SaveChangesAsync();

					return true;
                }
            }
			catch(Exception x)
            {
				Debug.Print(x.Message);
				return false;
            }
        }

	}
}

