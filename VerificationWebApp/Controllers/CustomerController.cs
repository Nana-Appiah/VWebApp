﻿using System;
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
                //check for database record existing before doing liveness test
                var dbData = new DbPayload() {
                    accountNumber = customer.actNo.Trim(),
                    customerNumber = String.Empty
                };

                var requestService = new ApiRequest() { databasePayLoad = dbData };
                var obj = await requestService.GetDatabaseRecordAsync();

                if (obj != null)
                {
                    bool blnStatus = false;

                    //convert file
                    var objPayLoad = new PayLoad()
                    {
                        pinNumber = customer.ghCardNo.Trim(),
                        image = new ImageFormatter() { rawBase64String = customer.imgData }.trimBase64String(),
                        dataType = @"PNG",
                        center = @"BRANCHLESS",
                        merchantKey = @"e4a8745a-131b-4c05-a350-17fd992eba35"
                    };

                    var api = new ApiServer();
                    var dt = await api.ApiRequestDataAsync(objPayLoad);

                    try
                    {
                        blnStatus = dt.verified == @"TRUE" ? true : false;
                        if (blnStatus)
                        {
                            //save record in the database
                            var verifiedObj = new Verified() 
                            {
                                AcctNo = obj.accountNumber,
                                NationalId = dt.person.nationalId,
                                ShortCode = dt.shortGuid,
                                Telephone = String.Empty,
                                frontPicture = customer.frontPicture,
                                backPicture = customer.backPicture
                            };

                            requestService.oVerified = verifiedObj;
                            bool b = await requestService.SaveRecordAsync();

                            if (await requestService.SaveRecordAsync())
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
                        return Json(new { status = false, error = $"error: {xx.Message}" });
                    }
                }
                else { return Json(new { status = false, data = $"account Number does not exist in the banking database" }); }
            }
            catch(Exception x)
            {
                return Json(new { status = false, error = $"error: {x.Message}" });
            }
        }
    }
}

