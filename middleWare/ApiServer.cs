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
		private string baseURI = @"https://selfie.imsgh.org:9020/api/v1/third-party/verification";

		public ApiServer()
		{
		}

        #region API consumption

		public async Task<data> ApiRequestDataAsync(PayLoad payLoad)
        {
			data o = null;

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(baseURI);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var stringPayLoad = JsonConvert.SerializeObject(payLoad);
                var content = new StringContent(stringPayLoad, Encoding.UTF8, "application/json");

                HttpResponseMessage response =  client.PostAsync(baseURI, content).Result;

                var ct = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var x = JObject.Parse(ct);
                    var dt = JsonConvert.DeserializeObject<data>(ct);

                    /* format data and send response of request to client */
                    o = new data();
                    o.transactionGuid = x["data"]["transactionGuid"].ToString();
                    o.shortGuid = x["data"]["shortGuid"].ToString();
                    o.requestTimestamp = x["data"]["requestTimestamp"].ToString();
                    o.responseTimestamp = x["data"]["responseTimestamp"].ToString();
                    o.verified = x["data"]["verified"].ToString();
                    o.center = x["data"]["center"].ToString();
                    o.isException = x["data"]["isException"].ToString();
                    o.person = x["data"]["person"].ToObject<Person>();
                    o.success = x["success"].ToString();
                    o.code = x["code"].ToString();
                    o.msg = x["msg"].ToString();
                }

                return o;
            }
            catch(Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return o;
            }
        }

        #endregion
    }
}

