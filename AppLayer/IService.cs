using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Modal;

namespace AppLayer
{
    public interface IService
    {
        #region Boom_Instant_Activation
        Task<DataTable> GetBoomTokenFromDB();
        Task<DataTable> checkBoomMobileUserExists(string EmailId, Int32 DistributorId);
        Task<DataSet> ValidateBoomApiRequest(Boom_Instant_Activation_Req req, string ip_Address);
        Task<int> StoredAPIRequestBeforeCall(StoredAPIRequestBeforeCall req);
        Task<DataTable> BoomMobileTransactionsBalanceDeduction(objBoomMobileTransactionDeduction obj);
        Task<int> StoredAPIResponseAfterCall(StoredAPIResponseAfterCall obj);
        Task<int> UpdateFailResponseAndReturnBoomMobileChargedAmount(objUpdateFailResponseAndReturnBoomMobileChargedAmount obj);
        Task<int> UpdateBoomMobileTransactionResponseDetails(objUpdateBoomMobileTransactionResponseDetails obj);
        #endregion

        #region BoomRecharge
        Task<DataSet> Validaterechargefromdb(Boom_Recharge_Req rechargeReq, string ClientIPAddress);
        Task<DataTable> BoomMobileRechargeTransactionsBalanceDeduction(BoomRechargeTransactionDeduction parameters);
        Task<int> UpdateFailResponseAndReturnBoomMobileRechargeChargedAmount(BoomRechargeUpdateRefund parameters);
        Task<int> BoomMobileUpdateRechargeResponseDetails(BoomRechargeResponseUpdate parameters);
        #endregion

        Task<DataTable> ValidateBoomApiRequest(ValidatefromDbReq data, string ip_Address);

        #region BoomBulk
        Task<DataTable> GetBoomBulkTransactions(string BatchId);
        #endregion
    }
}
