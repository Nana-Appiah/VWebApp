using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using middleWare;
using VerificationWebApp.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VerificationWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public async  Task<IActionResult> VerifyID(CustomerModel customer)
        {
            
            try
            {
                //convert the image to png
                bool blnStatus = false;
                var extension = Path.GetExtension(customer.file.FileName);

                if (extension == @".png")
                {
                    //convert file
                    var objPayLoad = new PayLoad() {
                        pinNumber = customer.ghCardNo.Trim(),
                        image = new ImageFormatter() { uploadedFile = customer.file }.convertToBase64String(),
                        dataType = extension.Replace(".", "").Trim().ToUpper(),
                        center = @"BRANCHLESS",
                        merchantKey = @"e4a8745a-131b-4c05-a350-17fd992eba35"
                    };

                    var api = new ApiServer();
                    var dt = await api.ApiRequestDataAsync(objPayLoad);

                    try
                    {
                        blnStatus = dt.verified == @"TRUE" ? true : false;
                    }
                    catch (Exception xx)
                    {

                    }

                    return Json(new { status = blnStatus.ToString(), data = dt });
                }
                else
                {
                    var imgByte = await new ImageFormatter() { uploadedFile = customer.file }.ConvertToBytes();
                    return Json(true);
                }
            }
            catch(Exception x)
            {
                return Json(new { status = false, error = $"error: {x.Message}" });
            }
        }
    }
}

