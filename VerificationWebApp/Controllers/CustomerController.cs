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
using System.Text;
using System.Text.Json;
using VerificationWebApp.DbModels;

namespace VerificationWebApp.Controllers
{
    public class CustomerController : Controller
    {
        [HttpPost]
        public async  Task<IActionResult> VerifyID(CustomerModel customer)
        {
            try
            {
                var dbData = new DbPayload() {
                    accountNumber = customer.actNo.Trim(),
                    customerNumber = String.Empty
                };

                var requestService = new ApiRequest()
                {
                    databasePayLoad = dbData
                };
                
                if (await requestService.CallVerificationAPIAsync(customer.ghCardNo))
                {
                    return Json(new { status = false, data = string.Format("{0} with account Number {1} has already undergone successful verification", customer.actName, customer.actNo) });
                }

                var obj = await requestService.CallDatabaseRecordAPIAsync(customer.TelNo,customer.dateOfBirth);

                if (obj.DoBverification == false)
                {
                    return Json(new { status = false, data = string.Format("Date of birth supplied by {0} does not match one in Database",customer.actName) });
                }

                if (obj.verificationStatus == false)
                {
                    return Json(new { status = false, data = "Telephone number NOT found on file" });
                }

                if (obj != null)
                {
                    bool blnStatus = false;

                    var objPayLoad = new PayLoad()
                    {
                        pinNumber = customer.ghCardNo.Trim(),
                        image = new ImageFormatter() { rawBase64String = customer.imgData }.trimBase64String(),
                        dataType = @"PNG",
                        center = @"BRANCHLESS",
                        merchantKey = @"e4a8745a-131b-4c05-a350-17fd992eba35"
                    };

                    var api = new ApiServer() 
                    {
                        imsGhAPI = ConfigObject.NIA_API,
                        flexcubeAPI = ConfigObject.Db_API
                    };

                    var dt = await api.ApiRequestDataAsync(objPayLoad);

                    try
                    {
                        blnStatus = dt.verified == @"TRUE" ? true : false;

                        if (blnStatus)
                        {
                            //using both obj and dt data structures
                            bool b = await requestService.SaveRecordAsync(obj,customer, dt);

                            if (b)
                            {
                                return Json(new { status = true, data = string.Format("{0},{1} has been verified by Liveness test",dt.person.surname,dt.person.forenames) });
                            }
                            else
                            {
                                return Json(new { status = false, data = string.Format("Sorry, your account exists but you failed Liveness test") });
                            }
                        }
                        else { return Json(new { status = false, data = string.Format("Sorry, you failed Liveness test") }); }
                    }
                    catch (Exception xx)
                    {
                        Debug.Print($"error: {xx.Message}");
                        return Json(new { status = false, data = $"error: {xx.Message}" });
                    }
                }
                else { return Json(new { status = false, data = $"account Number does not exist in the banking database" }); }
            }
            catch(Exception x)
            {
                return Json(new { status = false, data = $"error: {x.Message}" });
            }
        }
    }
}

