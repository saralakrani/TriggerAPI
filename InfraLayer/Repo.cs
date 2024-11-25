
using AppLayer;
using Azure;
using InfraLayer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using static Domain.Modal;

namespace InfraStructureLayer
{
    public class Repo : IRepository
    {
        public List<ApiResponse<dynamic>> response;
        private readonly Igenric _IG;
        private readonly IBoomMethodRepo _BoomService;
        private readonly IConfiguration _configuration;
        public readonly string? _BoomPortinMultiMonth;
        public readonly string? _CHANNEL;
        public readonly string? ip_Address;
        public readonly string? useragent;
        private readonly ClientInfoService _clientInfoService;
        public bool IsBoomSuccess = false;
        public Repo(Igenric IG, IBoomMethodRepo Activationservice, IConfiguration configuration, ClientInfoService clientInfoService)
        {
            _IG = IG;
            _BoomService = Activationservice;
            _configuration = configuration;
           
            _BoomPortinMultiMonth = _configuration["BoomCredentials:BoomPortinMultiMonth"];
            _CHANNEL = _configuration["BoomCredentials:BoomPortinMultiMonth"];
            _clientInfoService = clientInfoService;
            ip_Address = _clientInfoService.GetIpAddress();
            useragent = _clientInfoService.GetUserAgent();
            response = new List<ApiResponse<dynamic>>()
            {
                new ApiResponse<dynamic> {Errorcode= 102, Data = null, Details = "Error", Status = false }, //our side error
                new ApiResponse<dynamic> {Errorcode= 101, Data = null, Details = "", Status = false },      //user error
                new ApiResponse<dynamic> {Errorcode= 100, Data = null, Details = "Success", Status = true },
            };
        }

        
        #region MULTI THREAD METHOD
        public async Task MultiGetBoomBatchTransactionIds(string BoomBulkBatchId, string ipAddress, string userAgent)
        {
            string ApiEndPoint = "BoomBulkBatchTransaction";
            try
            {
                ValidationDbResult objvalidate = await _BoomService.GetBoomBulkTransactions(BoomBulkBatchId);
                 if (objvalidate.ErrorCode == 100 && objvalidate.Table.Rows.Count > 0)
                 {
                    //PROCESS EACH ROW IN PARALLEL
                    Parallel.ForEach(objvalidate.Table.Rows.Cast<DataRow>(), async row =>
                    {
                        //Desrialize and Serialize For Maintaining the formate of Request
                        Boom_BulkSingle_Activation_Req updateReq = JsonConvert.DeserializeObject<Boom_BulkSingle_Activation_Req>(row["JSONREQ"].ToString());
                        string activationRequest = JsonConvert.SerializeObject(updateReq, Formatting.None);
                        //Desrialize and Serialize For Maintaining the formate of Request

                        await Boom_Apis_Endpoint.Post_Method("http://199.34.20.114/BoomAPI/api/Genric/BoomBulkSingleActivation", activationRequest);
                    });
                 }
            }
            catch (Exception ex)
            {
                await Task.Run(() => _IG.Log($"{ApiEndPoint} - Details: Client - GlobalLink, Reason - {ex.Message}"));
            }
        }
        #endregion 
    }
}
