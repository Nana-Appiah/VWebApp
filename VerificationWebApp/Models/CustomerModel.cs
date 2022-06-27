using System;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace VerificationWebApp.Models
{
	
	public class CustomerModel
	{
		
		//[Required]
		//[StringLength(12)]
		public string actNo { get; set; }

		//[Required]
		//[StringLength(15)]
		public string ghCardNo { get; set; }

		//[Required]
		//[StringLength(10)]
		public string TelNo { get; set; }

		//[Required]
		public string motherName { get; set; }

		//[Required]
		//public IFormFile file { get; set; }

		public string imgData { get; set; }

	}
}

