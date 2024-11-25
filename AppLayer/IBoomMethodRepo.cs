using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Modal;

namespace AppLayer
{
    public interface IBoomMethodRepo
    {
        #region Boom_Instatnt_Activation
        Task<ValidationDbResult> ValidateBoomApiRequest(Boom_Instant_Activation_Req data, string ip_Address);
        Task<string> GetTokenFromDB(string BoomApiUrl, string BoomMobileAPIUserName, string BoomMobileAPIPassword);
        Task<string> checkUserExists(string EmailId, Int32 DistributorId);
        Task<string> CreateUserForBoomMobile(string boomAPIURL, string strRequest, string BoomAPIAuthToken);
        Task<int> StoredAPIRequestBeforeCall(StoredAPIRequestBeforeCall req);
        Task<ValidationDbResult> BoomMobileTransactionsBalanceDeduction(objBoomMobileTransactionDeduction obj);

        Task<string> BoomActivation_Endpoint_Request_get_Response(string BoomAPIAuthToken, string boomAPIURL, string jsonRequest_BoomMobileActivation, string userAgent);
        Task<int> StoredAPIResponseAfterCall(StoredAPIResponseAfterCall obj);
        Task<int> UpdateFailResponseAndReturnBoomMobileChargedAmount(objUpdateFailResponseAndReturnBoomMobileChargedAmount obj);
        Task<int> UpdateBoomMobileTransactionResponseDetails(objUpdateBoomMobileTransactionResponseDetails obj);
        #endregion

        #region Boom_Rcharge
        Task<ValidationDbResult> Validaterechargefromdb(Boom_Recharge_Req rechargeReq, string ip_Address);
        Task<ValidationDbResult> BoomMobileRechargeTransactionsBalanceDeduction(BoomRechargeTransactionDeduction parameters);
        Task<int> UpdateFailResponseAndReturnBoomMobileRechargeChargedAmount(BoomRechargeUpdateRefund parameters);

        Task<int> BoomMobileUpdateRechargeResponseDetails(BoomRechargeResponseUpdate parameters);
        #endregion
        Task<ValidationDbResult> ValidateBoomApiRequest(ValidatefromDbReq data, string ip_Address);

        #region BoomBUlk
        Task<ValidationDbResult> GetBoomBulkTransactions(string batchid);
        #endregion

    }
}
