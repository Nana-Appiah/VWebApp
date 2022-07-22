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

using middleWare;

namespace VerificationWebApp.Models
{
	public class ApiRequest
	{
		public PayLoad payLoad { get; set; }

		public DbPayload databasePayLoad { get; set; } 
		
		public Verified oVerified { get; set; }


		private const string merchant_key = @"e4a8745a-131b-4c05-a350-17fd992eba35";

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

		public async Task<DbVerification> CallDatabaseRecordAPIAsync(string mobileNo, string DoB)
        {
			DbVerification db = null;

			var api = new ApiServer() {
				flexcubeAPI = ConfigObject.Db_API,
				apiHeader = ConfigObject.API_KEY
			};

			var returnObj = await api.GetDatabaseRecordAsync(databasePayLoad);

			/* format and send response to client */
			if (returnObj != null)
            {
				db = new DbVerification() {
					customerNumber = returnObj.customerNumber,
					accountNumber  = returnObj.accountNumber,
					fullName = returnObj.fullName,
					dateOfBirth = formatDoB(returnObj.dateOfBirth.Split('/')),
					documentType = returnObj.documentType,
					documentNumber = returnObj.documentNumber,
					gender = returnObj.gender,
					motherMaidenName = returnObj.motherMaidenName,
					mobileNumber = returnObj.mobileNumber,
					responseCode = returnObj.responseCode,
					description = returnObj.description,
					additionalData = returnObj.additionalData
				};

				db.verificationStatus = doesMobileExist(mobileNo, db.mobileNumber.Split('|'));
				db.DoBverification = isValidDob(DoB, db.dateOfBirth);
            }
            
			return db;
        }

		private bool isValidDob(string suppliedDoB, string dbaseDoB)
        {
			return string.CompareOrdinal(suppliedDoB, dbaseDoB) == 0 ? true : false;
        }

		private bool doesMobileExist(string telFromPage, string[] telsOnFile)
        {
			//telFromPage: the telephone number provided by the customer using the web page
			//telsOnFile: the list of telephone numbers on file in the core-banking system
			//string s = @"Telephone number NOT found on file";

			bool bln = false;

			foreach(var t in telsOnFile)
            {
				if (telFromPage.Contains(t))
                {
					//return s = @"Telephone number found on file";
					return bln = true;
				}
            }

			return bln;
        }

		private string formatDoB(string[] parts)
        {
			//pad date of births if they are single months or single days
			if (parts[0].Length == 1) { parts[0] = String.Format("{0}{1}", @"0", parts[0]); }
			if (parts[1].Length == 1) { parts[1] = String.Format("{0}{1}", @"0", parts[1]); }


			return string.Format("{0}-{1}-{2}", parts[2], parts[0], parts[1]);
			
        }
		
		public async Task<bool> hasUserVerified()
        {
            try
            {
				using (var config = new IDVerificationTestContext())
                {
					var obj = config.Verifieds.Where(x => x.AcctNo == this.databasePayLoad.accountNumber).FirstOrDefault();

					return obj == null ? false : true;
                }
            }
			catch(Exception x)
            {
				Debug.Print(x.Message);
				return false;
            }
        }

		public async Task<bool> CallVerificationAPIAsync(string GHCardNo)
        {
			//method is responsible for calling the verification API in the middleware
			
			var serviceRequest = new ApiServer()
			{
				verificationAPI = ConfigObject.GhanaCardVerificationAPI,
				apiHeader = ConfigObject.API_KEY
			};

			var response = await serviceRequest.GCVerifyAsync(GHCardNo);

			return response.responseCode == @"00" ? true : false;
        }

		public async Task<bool> SaveRecordAsync(DbVerification dbo, CustomerModel obj, data ghCardVerificationResponse, string LIVENESS_STRING)
        {
            try
            {
				
				var serviceRequest = new ApiServer() {
					pushAPI = ConfigObject.postCustomerDataAPI,
					apiHeader = ConfigObject.API_KEY
				};

				PUSHPayload push = new PUSHPayload()
				{
					customerNumber = dbo.customerNumber,
					accountNumber = obj.actNo,
					accountName = obj.actName,
					dateOfBirth = obj.dateOfBirth.ToString("yyyy-mm-dd"),
					mobileNumber = obj.TelNo,
					idPhotoFront = new ImageFormatter() { rawBase64String = obj.frontPicture }.trimBase64String(),
					idPhotoBack = new ImageFormatter() { rawBase64String = obj.backPicture }.trimBase64String(),
					callbackData = new CallBack()
                    {
						code = @"00",
						data = ghCardVerificationResponse,
						success = true
                    },
					photo = LIVENESS_STRING
				};

				var resp = await serviceRequest.PushCustomerDataAsync(push);

				return resp.responseCode == @"00" ? true : false;

            }
			catch(Exception x)
            {
				Debug.Print(x.Message);
				return false;
            }
        }

	}
}

