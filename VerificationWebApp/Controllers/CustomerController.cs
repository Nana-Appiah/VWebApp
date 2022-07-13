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

                var requestService = new ApiRequest() {
                    databaseURI = ConfigObject.API,
                    databasePayLoad = dbData
                };

                if (await requestService.hasUserVerified())
                {
                    return Json(new { status = true, data = string.Format("{0} with account Number {1} has already undergone successful verification",customer.actName, customer.actNo)});
                }

                var obj = await requestService.GetDatabaseRecordAsync(customer.TelNo);

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
                        flexcubeAPI = ConfigObject.API
                    };

                    var dt = await api.ApiRequestDataAsync(objPayLoad);

                    try
                    {
                        blnStatus = dt.verified == @"TRUE" ? true : false;

                        if (blnStatus)
                        {
                            var verifiedObj = new Verified()
                            {
                                AcctNo = obj.accountNumber,
                                AcctName = customer.actName,
                                NationalId = dt.person.nationalId,
                                ShortCode =  dt.shortGuid,
                                Telephone = customer.TelNo,
                                frontPicture = new ImageFormatter() { rawBase64String = customer.frontPicture }.trimBase64String(),
                                backPicture = new ImageFormatter() { rawBase64String = customer.backPicture }.trimBase64String(),
                                rawData = JsonSerializer.Serialize(dt)
                            };

                            requestService.oVerified = verifiedObj;
                            bool b = await requestService.SaveRecordAsync();

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

