using Azure.Core;
using Azure;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using System.Xml;
using AppLayer;
using static Domain.Modal;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace InfraLayer
{
    public class BoomMethodRepo : IBoomMethodRepo
    {
        public readonly IService _service;
        private string? BoomAPIAuthToken;
        public BoomMethodRepo(IService service)
        {
            _service = service;
        }

        #region Boom_Instant_Activation 
        public async Task<ValidationDbResult> ValidateBoomApiRequest(Boom_Instant_Activation_Req data, string ip_Address)
        {
            var result = new ValidationDbResult
            {
                ErrorCode = 0,
                ErrorMessage = "Something Went Wrong on Validate Boom Activation Api, Plaese Contact to Api Provider."
            };
            try
            {
                DataSet dsValidate = new DataSet();
                dsValidate = await Task.Run(() => _service.ValidateBoomApiRequest(data, ip_Address));
                if (dsValidate != null && dsValidate.Tables.Count > 0)
                {
                    if (dsValidate.Tables[0].Rows.Count > 0 && Convert.ToInt32(dsValidate.Tables[0].Rows[0]["ERROCODE"]) == 1)
                    {
                        result = new ValidationDbResult
                        {
                            ErrorCode = Convert.ToInt32(dsValidate.Tables[0].Rows[0]["ERROCODE"]),
                            ErrorMessage = Convert.ToString(dsValidate.Tables[0].Rows[0]["ERRORMSG"]),
                            DbTables = dsValidate
                        };
                    }
                    else
                    {
                        result = new ValidationDbResult
                        {
                            ErrorCode = Convert.ToInt32(dsValidate.Tables[0].Rows[0]["ERROCODE"]),
                            ErrorMessage = Convert.ToString(dsValidate.Tables[0].Rows[0]["ERRORMSG"])
                        };
                    }
                }
                else
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 0,
                        ErrorMessage = "Unable To Validate Request ,Plaese Contact to Api Provider."
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ValidationDbResult
                {
                    ErrorCode = 0,
                    ErrorMessage = "Catch Error : Validate Request, Plaese Contact to Api Provider-" + ex.Message
                };
            }
            return result;
        }
        public async Task<string> GetTokenFromDB(string BoomApiUrl, string BoomMobileAPIUserName, string BoomMobileAPIPassword)
        {
            try
            {
                DataTable dttoken = await Task.Run(() => _service.GetBoomTokenFromDB());
                if (dttoken.Rows.Count > 0)
                {
                    BoomAPIAuthToken = Convert.ToString(dttoken.Rows[0]["accessToken"]);
                    if (BoomAPIAuthToken == null || BoomAPIAuthToken == "")
                    {
                        BoomAPIAuthToken = await Task.Run(() => Boom_Apis_Endpoint.GetToken(BoomApiUrl, BoomMobileAPIUserName, BoomMobileAPIPassword));
                        if (string.IsNullOrEmpty(BoomAPIAuthToken))
                            return BoomAPIAuthToken = "*2 Token Api Doesn't give any response";
                    }
                }
                else
                {
                    BoomAPIAuthToken = await Task.Run(() => Boom_Apis_Endpoint.GetToken(BoomApiUrl, BoomMobileAPIUserName, BoomMobileAPIPassword));
                    if (string.IsNullOrEmpty(BoomAPIAuthToken))
                        return BoomAPIAuthToken = "*2 Token Api Doesn't give any response";
                }
            }
            catch (Exception ex)
            {
                BoomAPIAuthToken = "*2 TokenFromDTO Error-" + ex.Message;
            }
            return BoomAPIAuthToken;
        }
        public async Task<string> checkUserExists(string EmailId, Int32 DistributorId)
        {
            string dealerCustometId = "";
            try
            {
                DataTable dt = new DataTable();
                dt = await Task.Run(() => _service.checkBoomMobileUserExists(EmailId, DistributorId));

                if (dt != null)
                    if (dt.Rows.Count > 0)
                        if (Convert.ToString(dt.Rows[0]["Validated"]) == "0")
                            dealerCustometId = Convert.ToString(dt.Rows[0]["dealerCustomerId"]);
            }
            catch (Exception ex)
            {
                dealerCustometId = "*2 Check User Error-" + ex.Message;
            }
            return dealerCustometId;
        }
        public async Task<string> CreateUserForBoomMobile(string boomAPIURL, string strRequest, string BoomAPIAuthToken)
        {
            string strResponse = "";
            try
            {
                strResponse = await Task.Run(() => Boom_Apis_Endpoint.createBoomUser(boomAPIURL, strRequest, BoomAPIAuthToken));
                if (strResponse.Contains("*2"))
                    return strResponse;
                if (string.IsNullOrEmpty(strResponse))
                    return strResponse = "*2 Create User Api Doesn't give any response";

                var jsonResponse = JObject.Parse(strResponse);
                if (Convert.ToString(jsonResponse["status"]).ToUpper() == "TRUE")
                    strResponse = Convert.ToString(jsonResponse["data"].First.ToString().Split(':')[1].Replace("\"", "")).Trim();
                else
                    strResponse = "*2 Error in User creation," + Convert.ToString(jsonResponse["details"][0].ToString());

            }

            catch (Exception ex)
            {
                strResponse = "*2 Catch Error in User creation-" + ex.Message;
            }
            return strResponse;
        }
        public async Task<int> StoredAPIRequestBeforeCall(StoredAPIRequestBeforeCall req)
        {
            int a = 0;
            try
            {
                a = await Task.Run(() => _service.StoredAPIRequestBeforeCall(req));
            }
            catch (Exception ex)
            {
            }
            return a;
        }
        public async Task<ValidationDbResult> BoomMobileTransactionsBalanceDeduction(objBoomMobileTransactionDeduction obj)
        {
            var result = new ValidationDbResult
            {
                ErrorCode = 0,
                ErrorMessage = "Intial-Something Went Wrong on Validate Boom Activation Api Plaese Contact to Api Provider."
            };
            try
            {
                DataTable dtResponse = new DataTable();
                dtResponse = await Task.Run(() => _service.BoomMobileTransactionsBalanceDeduction(obj));
                if (dtResponse.Rows.Count > 0)
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 1,
                        ErrorMessage = "Success",
                        Table = dtResponse
                    };
                }
                else
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 0,
                        ErrorMessage = "No Data Found. issue on Balance Deduction ,PLease Contact to Api Provider"
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ValidationDbResult
                {
                    ErrorCode = 0,
                    ErrorMessage = "Catch Error on  BoomMobile_Transactions_BalanceDeduction Plaese Contact to Api Provider-" + ex.Message
                };
            }
            return result;
        }
        public async Task<string> BoomActivation_Endpoint_Request_get_Response(string BoomAPIAuthToken, string boomAPIURL, string jsonRequest_BoomMobileActivation, string userAgent)
        {
            string strResponse = "";
            try
            {
                strResponse = await Task.Run(() => Boom_Apis_Endpoint.BoomApi_Put_Method(BoomAPIAuthToken, boomAPIURL, jsonRequest_BoomMobileActivation, userAgent));
                if (strResponse.Contains("*2"))
                    return strResponse;
                if (string.IsNullOrEmpty(strResponse))
                    return strResponse = "*2 Boom Activation Api didn't give any response";
            }
            catch (Exception ex)
            {
                strResponse = "*2 Boom Activation Api  error-" + ex.Message;
            }
            return strResponse;
        }
        public async Task<int> StoredAPIResponseAfterCall(StoredAPIResponseAfterCall req)
        {
            int a = 0;
            try
            {
                a = await Task.Run(() => _service.StoredAPIResponseAfterCall(req));
            }
            catch (Exception ex)
            { 
            }
            return a;
        }
        public async Task<int> UpdateFailResponseAndReturnBoomMobileChargedAmount(objUpdateFailResponseAndReturnBoomMobileChargedAmount obj)
        {
            int a = 0;
            try
            {
                a = await Task.Run(() => _service.UpdateFailResponseAndReturnBoomMobileChargedAmount(obj));

            }
            catch (Exception ex)
            {
            }
            return a;
        }
        public async Task<int> UpdateBoomMobileTransactionResponseDetails(objUpdateBoomMobileTransactionResponseDetails obj)
        {
            int a = 0;
            try
            {
                a = await Task.Run(() => _service.UpdateBoomMobileTransactionResponseDetails(obj));
            }
            catch (Exception ex)
            {
            }
            return a;
        }
        #endregion
        public async Task<ValidationDbResult> Validaterechargefromdb(Boom_Recharge_Req rechargeReq, string ip_Address)
        {
            var result = new ValidationDbResult
            {
                ErrorCode = 0,
                ErrorMessage = "Something Went Wrong on Validate Boom Activation Api, Plaese Contact to Api Provider."
            };

            try
            {
                DataSet dsValidate = new DataSet();
                dsValidate = await Task.Run(() => _service.Validaterechargefromdb(rechargeReq, ip_Address));
                if (dsValidate != null && dsValidate.Tables.Count > 0)
                {
                    if (dsValidate.Tables[0].Rows.Count > 0 && Convert.ToInt32(dsValidate.Tables[0].Rows[0]["ERROCODE"]) == 1)
                    {
                        result = new ValidationDbResult
                        {
                            ErrorCode = 1,
                            ErrorMessage = Convert.ToString(dsValidate.Tables[0].Rows[0]["ERRORMSG"]),
                            DbTables = dsValidate
                        };
                    }
                    else
                    {
                        result = new ValidationDbResult
                        {
                            ErrorCode = 0,
                            ErrorMessage = Convert.ToString(dsValidate.Tables[0].Rows[0]["ERRORMSG"])
                        };
                    }
                }
                else
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 0,
                        ErrorMessage = "Something Went Wrong in Validation Process, Plaese Contact to Api Provider."
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ValidationDbResult
                {
                    ErrorCode = 0,
                    ErrorMessage = "Catch Error on Validate Plaese Contact to Api Provider-" + ex.Message
                };
            }
            return result;
        }
        public async Task<ValidationDbResult> BoomMobileRechargeTransactionsBalanceDeduction(BoomRechargeTransactionDeduction parameters)
        {
            var result = new ValidationDbResult
            {
                ErrorCode = 0,
                ErrorMessage = "Intial-Something Went Wrong on Validate Boom Recharge Api Plaese Contact to Api Provider."
            };
            try
            {
                DataTable dtResponse = new DataTable();
                dtResponse = await Task.Run(() => _service.BoomMobileRechargeTransactionsBalanceDeduction(parameters));
                if (dtResponse.Rows.Count > 0)
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 1,
                        ErrorMessage = "Success",
                        Table = dtResponse
                    };
                }
                else
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 0,
                        ErrorMessage = "No Data Found. issue on Balance Deduction ,PLease Contact to Api Provider"
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ValidationDbResult
                {
                    ErrorCode = 0,
                    ErrorMessage = "Catch Error on  BoomMobile_Transactions_BalanceDeduction Plaese Contact to Api Provider-" + ex.Message
                };
            }
            return result;
        }
        public async Task<int> UpdateFailResponseAndReturnBoomMobileRechargeChargedAmount(BoomRechargeUpdateRefund parameters)
        {
            int a = 0;
            try
            {
                a = await Task.Run(() => _service.UpdateFailResponseAndReturnBoomMobileRechargeChargedAmount(parameters));

            }
            catch (Exception ex)
            {
                a = 0;
            }
            return a;
        }
        public async Task<int> BoomMobileUpdateRechargeResponseDetails(BoomRechargeResponseUpdate parameters)
        {
            int a = 0;
            try
            {
                a = await Task.Run(() => _service.BoomMobileUpdateRechargeResponseDetails(parameters));

            }
            catch (Exception ex)
            {
                a = 0;
            }
            return a;
        }
        public async Task<ValidationDbResult> ValidateBoomApiRequest(ValidatefromDbReq data, string ip_Address)
        {
            var result = new ValidationDbResult
            {
                ErrorCode = 101,
                ErrorMessage = "Something Went Wrong on Validate, " + data.ApiEndPoint + " - Plaese Contact to Api Provider."
            };

            try
            {
                DataTable dtValidate = new DataTable();
                dtValidate = await Task.Run(() => _service.ValidateBoomApiRequest(data, ip_Address));
                if (dtValidate.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dtValidate.Rows[0]["ERROCODE"]) == 1)
                    {
                        result = new ValidationDbResult
                        {
                            ErrorCode = 100,
                            ErrorMessage = Convert.ToString(dtValidate.Rows[0]["ERRORMSG"]),
                        };
                    }
                    else
                    {
                        result = new ValidationDbResult
                        {
                            ErrorCode = 101,
                            ErrorMessage = Convert.ToString(dtValidate.Rows[0]["ERRORMSG"])
                        };
                    }
                }
                else
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 101,
                        ErrorMessage = "Something Went Wrong on Validate ,Plaese Contact to Api Provider."
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ValidationDbResult
                {
                    ErrorCode = 101,
                    ErrorMessage = "Error to Validate, Plaese Contact to Api Provider - " + ex.Message
                };
            }
            return result;
        }

        public async Task<ValidationDbResult> GetBoomBulkTransactions(string batchid)
        {
            var result = new ValidationDbResult
            {
                ErrorCode = 101,
                ErrorMessage = "Something Went Wrong on Getting BatchResponse."
            };

            try
            {
                DataTable dtValidate = new DataTable();
                dtValidate = await Task.Run(() => _service.GetBoomBulkTransactions(batchid));
                if (dtValidate.Rows.Count > 0)
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 100,
                        ErrorMessage = "Done.",
                        Table=dtValidate
                    };
                }
                else
                {
                    result = new ValidationDbResult
                    {
                        ErrorCode = 101,
                        ErrorMessage = "Something Went Wrong on Getting Batch Transaction-IDS ,Plaese Contact to Api Provider."
                    };
                }
            }
            catch (Exception ex)
            {
                result = new ValidationDbResult
                {
                    ErrorCode = 101,
                    ErrorMessage = "Error: Plaese Contact to Api Provider - " + ex.Message
                };
            }
            return result;
        }
    }
}
