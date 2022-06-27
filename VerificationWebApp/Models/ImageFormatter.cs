using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;

namespace VerificationWebApp.Models
{
	public class ImageFormatter
	{
		
		public IFormFile uploadedFile { get; set; }
        public string rawBase64String { get; set; }


		public async Task<byte[]> ConvertToBytes()
        {
            byte[] bArray; 
            try
            {
                if (uploadedFile.Length == 0)
                {
                    return new byte[0];
                }
                else
                {
                    using (var ms = new MemoryStream())
                    {
                        await uploadedFile.CopyToAsync(ms);
                        bArray = ms.ToArray();

                        return bArray;
                    }
                }  
            }
            catch (Exception x)
            {
                Debug.Print($"error: {x.Message}");
                return new byte[0];
            }
        }

        public string convertToBase64String()
        {
            //converts bytes to base 64 string
            byte[] fileBytes;
            string base64String = string.Empty;

            if (uploadedFile.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        uploadedFile.CopyTo(ms);
                        fileBytes = ms.ToArray();
                        base64String = Convert.ToBase64String(fileBytes);
                        
                    }

                }
                catch(Exception x)
                {
                    Debug.Print($"error: {x.Message}");
                    return base64String;
                }
            }


            return base64String;
        }

        public string trimBase64String()
        {
            string regx = @"data:image/png;base64,";

            return this.rawBase64String.Replace(regx, "").Trim();
        }

	}
}

