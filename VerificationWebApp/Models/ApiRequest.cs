using System;
using middleWare;
using System.Diagnostics;

namespace VerificationWebApp.Models
{
	public class ApiRequest
	{
		public PayLoad payLoad { get; set; }

		
		private const string merchant_key = @"e4a8745a-131b-4c05-a350-17fd992eba35";

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

	}
}

