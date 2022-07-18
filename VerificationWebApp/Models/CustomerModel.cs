using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace VerificationWebApp.Models
{
	
	public class CustomerModel
	{
		public string actNo { get; set; }

		public string actName { get; set; }

		public string ghCardNo { get; set; }

		public string TelNo { get; set; }

		//public string motherName { get; set; }
		public string dateOfBirth { get; set; }

		public string imgData { get; set; }

		public string frontPicture { get; set; }
		public string backPicture { get; set; }

	}
}

