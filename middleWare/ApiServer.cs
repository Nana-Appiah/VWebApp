using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;

using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json.Net;

using System.Diagnostics;

namespace middleWare
{
	public class ApiServer
	{
        public string imsGhAPI { get; set; }
        public string flexcubeAPI { get; set; }

        public string verificationAPI { get; set; }
        public string apiHeader { get; set; }

        public string pushAPI { get; set; }

		public ApiServer()
		{
		}

        #region API consumption

		public async Task<data> ApiRequestDataAsync(PayLoad payLoad)
        {
			data o = null;

            try
            {
                Response statusObj = null;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(imsGhAPI);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var stringPayLoad = JsonConvert.SerializeObject(payLoad);
                var content = new StringContent(stringPayLoad, Encoding.UTF8, "application/json");


                using(HttpResponseMessage response = await client.PostAsync(imsGhAPI, content))
                {
                    response.EnsureSuccessStatusCode();
                    var ct = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var x = JObject.Parse(ct);
                        
                        o = new data();

                        o.transactionGuid = x["data"]["transactionGuid"].ToString();
                        o.shortGuid = x["data"]["shortGuid"].ToString();
                        o.requestTimestamp = x["data"]["requestTimestamp"].ToString();
                        o.responseTimestamp = x["data"]["responseTimestamp"].ToString();
                        o.verified = x["data"]["verified"].ToString();
                        o.center = x["data"]["center"].ToString();
                        o.isException = x["data"]["isException"].ToString();
                        o.person = x["data"]["person"].ToObject<Person>();
                        o.success = String.Format("{0},{1} validated using Liveness test", o.person.surname, o.person.forenames);
                        o.code = x["code"].ToString();
                        o.msg = x["msg"].ToString();

                        o.error = statusObj;
                    }
                }

                return o;
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                o = new data()
                {
                    error = new Response()
                    {
                        success = @"false",
                        code = "04 - Intenal Server error",
                        data = x.Message
                    }
                };

                return o;
            }
        }

        public async Task<DbResponsePayload> GetDatabaseRecordAsync(DbPayload dbPayload)
        {
            DbResponsePayload db = null;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(flexcubeAPI);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", apiHeader);

                var stringPayLoad = JsonConvert.SerializeObject(dbPayload);
                var content = new StringContent(stringPayLoad, Encoding.UTF8, "application/json");

                using (HttpResponseMessage response = await client.PostAsync(flexcubeAPI, content))
                {
                    response.EnsureSuccessStatusCode();
                    var strResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var x = JObject.Parse(strResponse);

                        //create object and send back to request
                        db = new DbResponsePayload();

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
                    else { return db; }
                }
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return db;
            }
        }

        public async Task<VerificationPayload> GCVerifyAsync(string strPayload)
        {
            //method is responsible for verifying the national id of a customer
            VerificationPayload obj = null;

            try
            {
                var payLoadObj = new GHVerificationRequestBody() { nationalId = strPayload };
                var client = new HttpClient()
                {
                    BaseAddress = new Uri(verificationAPI)
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", apiHeader);

                var stringPayLoad = JsonConvert.SerializeObject(payLoadObj);
                var content = new StringContent(stringPayLoad, Encoding.UTF8, "application/json");

                //make the call
                using (var response = await client.PostAsync(verificationAPI,content))
                {
                    response.EnsureSuccessStatusCode();
                    var dta = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var _parsed = JObject.Parse(dta);
                        //var obj = JsonConvert.DeserializeObject<VerificationPayload>(dta);  //unneccessary?

                        obj = new VerificationPayload()
                        {
                            shortGuid = _parsed["shortGuid"].ToString(),
                            responseCode = _parsed["responseCode"].ToString(),
                            description = _parsed["description"].ToString(),
                            additionalData = _parsed["additionalData"].ToString()
                        };
                    }
                }

                return obj;
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return obj;
            }
        }

        public async Task<PUSHResponse> PushCustomerDataAsync(PUSHPayload payload)
        {
            //method is responsible for pushing customer data to the database
            PUSHResponse obj = null;

            try
            {
                var client = new HttpClient()
                {
                    BaseAddress = new Uri(pushAPI)
                };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("x-api-key", apiHeader);

                var strPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(strPayload, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(pushAPI, content))
                {
                    response.EnsureSuccessStatusCode();
                    var dt = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var parsed = JObject.Parse(dt);

                        obj = new PUSHResponse() {
                            responseCode = parsed["responseCode"].ToString(),
                            description = parsed["description"].ToString(),
                            additionalData = parsed["additionalData"].ToString()
                        };
                    }
                }

                return obj;
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return obj;
            }
        }

        #endregion
    }
}

