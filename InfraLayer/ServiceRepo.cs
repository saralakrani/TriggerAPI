using AppLayer;
using InfraStructureLayer;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using static Domain.Modal;

namespace InfraLayer
{
    public class ServiceRepo: IService
    {
        public readonly DataBase _data = new DataBase();

        #region Boom_Instant_Activation
        public async Task<DataTable> GetBoomTokenFromDB()
        {
            DataTable dtGetBoomTokenFromDB = new DataTable();
            try
            {
                using (SqlCommand objCmd = new SqlCommand("spBoomGetTokenForPOS"))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    dtGetBoomTokenFromDB = await Task.Run(() => _data.ReturnDataSet(objCmd).Tables[0]);
                }
                return dtGetBoomTokenFromDB;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataTable> checkBoomMobileUserExists(string EmailId, Int32 DistributorId)
        {
            try
            {
                using (SqlCommand objCmd = new SqlCommand("spcheckBoomMobileUserExists"))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.AddWithValue("@EmailId", SqlDbType.VarChar).Value = EmailId;
                    objCmd.Parameters.AddWithValue("@DistributorId", SqlDbType.BigInt).Value =DistributorId;
                    return await Task.Run(() => _data.ReturnDataSet(objCmd).Tables[0]); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DataSet> ValidateBoomApiRequest(Boom_Instant_Activation_Req data, string ip_Address)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("API_ValidateBoomActivationInstantTransaction"))
                {
                    command.Parameters.AddWithValue("@ClientCode", data.ClientCode);
                    command.Parameters.AddWithValue("@SerialNumber", data.SerialNumber);
                    command.Parameters.AddWithValue("@IMEI", data.IMEI);
                    command.Parameters.AddWithValue("@PlanId", data.TariffId);
                    command.Parameters.AddWithValue("@Month", data.Month);
                    command.Parameters.AddWithValue("@TransactionId", data.TransactionId);
                    command.Parameters.AddWithValue("@Ip_Address", ip_Address);
                    return await Task.Run(() => _data.ReturnDataSet(command)); 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> StoredAPIRequestBeforeCall(StoredAPIRequestBeforeCall req)
        {
            int a = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "PStoredAPIRequestBeforeCall";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = req.Title;
                cmd.Parameters.Add("@Request", SqlDbType.VarChar).Value = req.Request;
                cmd.Parameters.Add("@DistributorId", SqlDbType.BigInt).Value = req.DistributorId;
                cmd.Parameters.Add("@TransactionID", SqlDbType.VarChar).Value = req.TransactionID;
                cmd.Parameters.Add("@Msisdn", SqlDbType.VarChar).Value = req.Msisdn;
                cmd.Parameters.Add("@SIMNumber", SqlDbType.VarChar).Value = req.SIMNumber;
                a = await Task.Run(() => _data.RunExecuteNoneQuery(cmd));
                return a;
            }
            catch (Exception ex)
            {
                return a;
            }
        }
        public async Task<DataTable> BoomMobileTransactionsBalanceDeduction(objBoomMobileTransactionDeduction obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "spBoomActivationTransactionsBalanceDeduction";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ChargedAmount", SqlDbType.Decimal).Value = obj.ChargedAmount;
                cmd.Parameters.Add("@distributorID", SqlDbType.BigInt).Value = obj.DistributorID;
                cmd.Parameters.Add("@LoginID", SqlDbType.BigInt).Value = obj.LoginID;
                cmd.Parameters.Add("@PaymentFrom", SqlDbType.Int).Value = obj.PaymentFrom;
                cmd.Parameters.Add("@PayeeID", SqlDbType.Int).Value = obj.PayeeID;
                cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = obj.PaymentType;
                cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = obj.PaymentMode;
                cmd.Parameters.Add("@TransactionId", SqlDbType.VarChar).Value = obj.TransactionId;
                cmd.Parameters.Add("@TransactionStatus", SqlDbType.VarChar).Value = obj.TransactionStatus;
                cmd.Parameters.Add("@TransactionStatusId", SqlDbType.Int).Value = obj.TransactionStatusId;
                cmd.Parameters.Add("@ActivationType", SqlDbType.Int).Value = obj.ActivationType;
                cmd.Parameters.Add("@ActivationStatus", SqlDbType.BigInt).Value = obj.ActivationStatus;
                cmd.Parameters.Add("@ActivationVia", SqlDbType.Int).Value = obj.ActivationVia;
                cmd.Parameters.Add("@ActivationRequest", SqlDbType.VarChar).Value = obj.ActivationRequest;
                cmd.Parameters.Add("@TariffID", SqlDbType.Int).Value = obj.TariffID;
                cmd.Parameters.Add("@SerialNumber", SqlDbType.VarChar).Value = obj.SerialNumber;
                cmd.Parameters.Add("@Regulatery", SqlDbType.Decimal).Value = obj.Regulatery;
                cmd.Parameters.Add("@Month", SqlDbType.Int).Value = obj.Month;
                cmd.Parameters.Add("@ZipCode", SqlDbType.VarChar).Value = obj.ZipCode;
                cmd.Parameters.Add("@AreaCode", SqlDbType.VarChar).Value = obj.AreaCode;
                cmd.Parameters.Add("@AccountNumber", SqlDbType.VarChar).Value = obj.AccountNumber;
                cmd.Parameters.Add("@PinNumber", SqlDbType.VarChar).Value = obj.PinNumber;
                cmd.Parameters.Add("@MSISDN", SqlDbType.VarChar).Value = obj.MSISDN;
                cmd.Parameters.Add("@IMEI", SqlDbType.VarChar).Value = obj.IMEI;
                cmd.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = obj.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.VarChar).Value = obj.LastName;
                cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = obj.Address;
                cmd.Parameters.Add("@City", SqlDbType.VarChar).Value = obj.City;
                cmd.Parameters.Add("@State", SqlDbType.VarChar).Value = obj.State;
                cmd.Parameters.Add("@CustomerEmailId", SqlDbType.VarChar).Value = obj.CustomerEmailId;
                cmd.Parameters.Add("@PortFromFirstName", SqlDbType.VarChar).Value = obj.PortFromFirstName;
                cmd.Parameters.Add("@PortFromLastName", SqlDbType.VarChar).Value = obj.PortFromLastName;
                cmd.Parameters.Add("@PortFromZipCode", SqlDbType.VarChar).Value = obj.PortFromZipCode;
                cmd.Parameters.Add("@PortFromAddress", SqlDbType.VarChar).Value = obj.PortFromAddress;
                cmd.Parameters.Add("@PortFromCity", SqlDbType.VarChar).Value = obj.PortFromCity;
                cmd.Parameters.Add("@PortFromState", SqlDbType.VarChar).Value = obj.PortFromState;
                cmd.Parameters.Add("@PortFromEmailId", SqlDbType.VarChar).Value = obj.PortFromEmailId;
                cmd.Parameters.Add("@PortFromContact", SqlDbType.VarChar).Value = obj.PortFromContact;
                cmd.Parameters.Add("@PortFromProvider", SqlDbType.VarChar).Value = obj.PortFromProvider;
                cmd.Parameters.Add("@BoomCustomerId", SqlDbType.VarChar).Value = obj.BoomCustomerId;
                cmd.Parameters.Add("@SimType", SqlDbType.Int).Value = obj.SimType;
                return await Task.Run(() => _data.ReturnDataSet(cmd).Tables[0]) ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> StoredAPIResponseAfterCall(StoredAPIResponseAfterCall obj)
        {
            int a = 0;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "PStoredAPIResponseAfterCall";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Title", SqlDbType.VarChar).Value = obj.Title;
                cmd.Parameters.Add("@Response", SqlDbType.VarChar).Value = obj.Response;
                cmd.Parameters.Add("@DistributorId", SqlDbType.BigInt).Value = obj.DistributorId;
                cmd.Parameters.Add("@TransactionID", SqlDbType.VarChar).Value = obj.TransactionID;
                cmd.Parameters.Add("@Msisdn", SqlDbType.VarChar).Value = obj.Msisdn;
                cmd.Parameters.Add("@SIMNumber", SqlDbType.VarChar).Value = obj.SIMNumber;
                a = await Task.Run(() => _data.RunExecuteNoneQuery(cmd));
                return a;
            }
            catch (Exception ex)
            {
                return a;
            }
        }
        public async Task<int> UpdateFailResponseAndReturnBoomMobileChargedAmount(objUpdateFailResponseAndReturnBoomMobileChargedAmount obj)
        {
            try
            {
                using (SqlCommand objCmd = new SqlCommand("pUpdateFailBoomActivationResponseAndReturnChargedAmount"))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("@Activationid", SqlDbType.BigInt).Value = obj.Activationid;
                    objCmd.Parameters.Add("@PaymentID", SqlDbType.BigInt).Value = obj.PaymentID;
                    objCmd.Parameters.Add("@LoginID", SqlDbType.BigInt).Value = obj.LoginID;
                    objCmd.Parameters.Add("@RESP", SqlDbType.VarChar).Value = obj.RESP;
                    objCmd.Parameters.Add("@ALLOCATED_MSISDN", SqlDbType.VarChar).Value = obj.ALLOCATED_MSISDN;
                    objCmd.Parameters.Add("@ChargedAmount", SqlDbType.Decimal).Value = obj.ChargedAmount;
                    objCmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = obj.PaymentMode;
                    objCmd.Parameters.Add("@ActivationType", SqlDbType.Int).Value = obj.ActivationType;
                    objCmd.Parameters.Add("@ActivationStatus", SqlDbType.Int).Value = obj.ActivationStatus;
                    return await Task.Run(() => _data.RunExecuteNoneQuery(objCmd));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdateBoomMobileTransactionResponseDetails(objUpdateBoomMobileTransactionResponseDetails obj)
        {
            try
            {
                using (SqlCommand objCmd = new SqlCommand("pUpdateBoomMobileTransactionResponseDetails"))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("@Activationid", SqlDbType.BigInt).Value = obj.Activationid;
                    objCmd.Parameters.Add("@PaymentID", SqlDbType.BigInt).Value = obj.PaymentID;
                    objCmd.Parameters.Add("@RESP", SqlDbType.VarChar).Value = obj.RESP;
                    objCmd.Parameters.Add("@ALLOCATED_MSISDN", SqlDbType.VarChar).Value = obj.ALLOCATED_MSISDN;
                    objCmd.Parameters.Add("@MNPNO", SqlDbType.VarChar).Value = obj.MNPNO;
                    return await Task.Run(() => _data.RunExecuteNoneQuery(objCmd));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Boom_Recharge
        public async Task<DataSet> Validaterechargefromdb(Boom_Recharge_Req rechargeReq, string ClientIPAddress)
        {
            try
            {
                using (SqlCommand objcmd = new SqlCommand("API_ValidateBoomRechargeRequest"))
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@ClientCode", SqlDbType.VarChar).Value = rechargeReq.ClientCode;
                    objcmd.Parameters.Add("@ClientIPAddress", SqlDbType.VarChar).Value = ClientIPAddress;
                    objcmd.Parameters.Add("@TXNID", SqlDbType.VarChar).Value = rechargeReq.TransactionId;
                    objcmd.Parameters.Add("@MSISDN", SqlDbType.VarChar).Value = rechargeReq.MSISDN;
                    objcmd.Parameters.Add("@PlanID", SqlDbType.VarChar).Value = rechargeReq.PlanID;
                    objcmd.Parameters.Add("@Rental", SqlDbType.Decimal).Value = rechargeReq.Rental;
                    objcmd.Parameters.Add("@Carrier_Type", SqlDbType.Int).Value = rechargeReq.Carrier_Type;
                    objcmd.Parameters.Add("@RechargeType", SqlDbType.Int).Value = rechargeReq.RechargeType;

                    return await Task.Run(() => _data.ReturnDataSet(objcmd));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> BoomMobileRechargeTransactionsBalanceDeduction(BoomRechargeTransactionDeduction parameters)
        {
            DataTable dtBalanceDeduction = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "spBoomRechargeBalanceDeduction";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ChargedAmount", SqlDbType.Decimal).Value = parameters.ChargedAmount;
                cmd.Parameters.Add("@DistributorID", SqlDbType.BigInt).Value = parameters.DistributorID;
                cmd.Parameters.Add("@LoginID", SqlDbType.BigInt).Value = parameters.LoginID;
                cmd.Parameters.Add("@PaymentFrom", SqlDbType.Int).Value = parameters.PaymentFrom;
                cmd.Parameters.Add("@PaymentType", SqlDbType.Int).Value = parameters.PaymentType;
                cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = parameters.PaymentMode;
                cmd.Parameters.Add("@TransactionId", SqlDbType.VarChar).Value = parameters.TransactionId;
                cmd.Parameters.Add("@TransactionStatus", SqlDbType.VarChar).Value = parameters.TransactionStatus;
                cmd.Parameters.Add("@TransactionStatusId", SqlDbType.BigInt).Value = parameters.TransactionStatusId;
                cmd.Parameters.Add("@RechargeType", SqlDbType.BigInt).Value = parameters.RechargeType;
                cmd.Parameters.Add("@ActivationStatus", SqlDbType.BigInt).Value = parameters.ActivationStatus;
                cmd.Parameters.Add("@ActivationVia", SqlDbType.BigInt).Value = parameters.ActivationVia;
                cmd.Parameters.Add("@ActivationRequest", SqlDbType.VarChar).Value = parameters.ActivationRequest;
                cmd.Parameters.Add("@TariffID", SqlDbType.VarChar).Value = parameters.TariffID;
                cmd.Parameters.Add("@Regulatery", SqlDbType.Decimal).Value = parameters.Regulatery;
                cmd.Parameters.Add("@MSISDN", SqlDbType.VarChar).Value = parameters.MSISDN;
                cmd.Parameters.Add("@PlanAmount", SqlDbType.Decimal).Value = parameters.PlanAmount;
                return await Task.Run(() => _data.ReturnDataSet(cmd).Tables[0]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateFailResponseAndReturnBoomMobileRechargeChargedAmount(BoomRechargeUpdateRefund parameters)
        {
            try
            {
                using (SqlCommand objCmd = new SqlCommand("pUpdateFailBoomRechargeResponseAndReturnChargedAmount"))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("@Rechargeid", SqlDbType.BigInt).Value = parameters.RechargeId;
                    objCmd.Parameters.Add("@LoginID", SqlDbType.BigInt).Value = parameters.LoginID;
                    objCmd.Parameters.Add("@RESP", SqlDbType.VarChar).Value = parameters.RESP;
                    objCmd.Parameters.Add("@ChargedAmount", SqlDbType.Decimal).Value = parameters.ChargedAmount;
                    objCmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = parameters.PaymentMode;
                    objCmd.Parameters.Add("@RechargeType", SqlDbType.Int).Value = parameters.RechargeType;
                    objCmd.Parameters.Add("@RechargeStatus", SqlDbType.Int).Value = parameters.RechargeStatus;
                    return await Task.Run(() => _data.RunExecuteNoneQuery(objCmd));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> BoomMobileUpdateRechargeResponseDetails(BoomRechargeResponseUpdate parameters)
        {
            try
            {
                using (SqlCommand objCmd = new SqlCommand("pUpdateBoomRechargeResponseDetails"))
                {
                    objCmd.CommandType = CommandType.StoredProcedure;
                    objCmd.Parameters.Add("@RechargeId", SqlDbType.BigInt).Value = parameters.RechargeId;
                    objCmd.Parameters.Add("@RechargeResponse", SqlDbType.VarChar).Value = parameters.RechargeResponse;
                    objCmd.Parameters.Add("@SaveResponse", SqlDbType.Int);
                    objCmd.Parameters["@SaveResponse"].Direction = ParameterDirection.Output;
                    await Task.Run(() => _data.RunExecuteNoneQuery(objCmd));
                    return Convert.ToInt32(objCmd.Parameters["@SaveResponse"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public async Task<DataTable> ValidateBoomApiRequest(ValidatefromDbReq data, string ip_Address)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("API_ValidateGenricBoomAPI"))
                {
                    command.Parameters.AddWithValue("@ClientCode", data.ClientCode);
                    command.Parameters.AddWithValue("@SimNo",data.SerialNumber);
                    command.Parameters.AddWithValue("@MSISDN", data.Msisdn);
                    command.Parameters.AddWithValue("@IMEI", data.IMEI);
                    command.Parameters.AddWithValue("@Api_EndPoint", data.ApiEndPoint);
                    command.Parameters.AddWithValue("@ClientIPAddress", ip_Address);
                    return await Task.Run(() => _data.ReturnDataSet(command).Tables[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Boom_Bulk
        public async Task<DataTable> GetBoomBulkTransactions(string BatchId)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("SPGETBOOMBULKBATCHREQUEST"))
                {
                    command.Parameters.AddWithValue("@BATCHID", BatchId);
                    return await Task.Run(() => _data.ReturnDataSet(command).Tables[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
