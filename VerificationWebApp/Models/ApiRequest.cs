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
		private string baseURI = @"https://selfie.imsgh.org:9020/api/v1/third-party/verification";
		private string databaseURI =  @"http://localhost:8000/api/customer";

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

		public async Task<DbVerification> GetDatabaseRecordAsync()
        {
			var api = new ApiServer();
			var returnString = await api.GetDatabaseRecordAsync(databasePayLoad);

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
				db.dateOfBirth = x["dateOfBirth"].ToString();
				db.documentType = x["documentType"].ToString();
				db.documentNumber = x["documentNumber"].ToString();
				db.gender = x["gender"].ToString();
				db.motherMaidenName = x["motherMaidenName"].ToString();
				db.mobileNumber = x["mobileNumber"].ToString();
				db.responseCode = x["responseCode"].ToString();
				db.description = x["description"].ToString();
				db.additionalData = x["additionalData"].ToString();

				return db;
			}
            else
            {
				return null;
            }
        }

		public async Task<bool> SaveRecordAsync()
        {
            //method is used to save record asynchronously
            try
            {
				using (var config = new IDVerificationTestContext())
                {
					config.Verifieds.Add(oVerified);
					config.SaveChangesAsync();

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

