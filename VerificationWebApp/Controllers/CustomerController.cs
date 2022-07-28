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

                var verifyObj = await requestService.CallVerificationAPIAsync(customer.ghCardNo);


                if ((verifyObj.responseCode == @"00") && (verifyObj.additionalData == @"1"))
                {
                    return Json(new { status = false, data = string.Format("Data of {0} has already been submitted", customer.actName) });
                }

                if ((verifyObj.responseCode == @"00") && (verifyObj.additionalData == @"2"))
                {
                    return Json(new { status = false, data = string.Format("Ghana card of {0} has already been verified and linked to account",customer.actName) });
                }

                if ((verifyObj.responseCode == @"00") && (verifyObj.additionalData == @"0"))
                {
                    /* go ahead */
                    var objCustomer = await requestService.CallDatabaseRecordAPIAsync(customer.TelNo, customer.dateOfBirth.ToString("yyyy-MM-dd"));

                    if (objCustomer.responseCode == @"01")
                    {
                        //object is null. account number not found in database
                        return Json(new { status = false, data = "Account number not found" });
                    }

                    if (objCustomer.responseCode == @"06")
                    {
                        return Json(new {status = false, data = @"An error occured. Please contact Administrator"});
                    }

                    if (objCustomer.DoBverification == false)
                    {
                        //date of birth verification returned false
                        return Json(new { status = false, data = string.Format("Date of birth supplied by {0} does not match one in Database", customer.actName) });
                    }

                    if (objCustomer.verificationStatus == false)
                    {
                        //telephone number verification returned false
                        return Json(new { status = false, data = "Telephone number NOT found in Database" });
                    }

                    if (objCustomer.responseCode == @"00")
                    {
                        //bool blnStatus = false;
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

                        var dt = new data { };

                        if (ConfigObject.SELFIE == @"yes")
                        {
                            dt = await api.ApiRequestDataAsync(objPayLoad);
                        }

                        bool b = await requestService.SaveRecordAsync(objCustomer, customer, dt, objPayLoad.image);

                        if (b)
                        {
                            return Json(new { status = true, data = string.Format("Data of {0} has been submitted successfully!!!", customer.actName) });
                        }
                        else
                        {
                            return Json(new { status = false, data = string.Format("Sorry, your data could not be submitted.Please try again") });
                        }

                    }
                    else { return Json(new { status = false, data = @"Account number not found" }); }
                }
                else { return Json(new { status = false, data = @"Error!! Please contact administrator" }); }

            }
            catch(Exception x)
            {
                return Json(new { status = false, data = @"Please enter all mandatory fields to initiate verification" });
            }
        }
    }
}

