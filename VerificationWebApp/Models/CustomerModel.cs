using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace VerificationWebApp.Models
{
	
	public class CustomerModel
	{
		[Required]
		public string actNo { get; set; }

		[Required]
		public string actName { get; set; }

		[Required]
		public string ghCardNo { get; set; }

		[Required]
		public string TelNo { get; set; }

		//public string motherName { get; set; }
		[Required]
		public DateTime dateOfBirth { get; set; }

		[Required]
		public string imgData { get; set; }

		[Required]
		public string frontPicture { get; set; }

		[Required]
		public string backPicture { get; set; }

	}
}

