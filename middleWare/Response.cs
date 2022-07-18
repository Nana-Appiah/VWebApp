using System;
namespace middleWare
{
	public class Response
	{
		public string success { get; set; }
		public string data { get; set; }
		public string code { get; set; }
	}

	public class PUSHResponse
    {
		public string responseCode { get; set; }
		public string description { get; set; }
		public string additionalData { get; set; }
    }

}

