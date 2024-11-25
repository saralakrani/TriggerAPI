using AppLayer;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using static Domain.Modal;

namespace Genric_Boom_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenricController : ControllerBase
    {
        private readonly IRepository _Repo;
        public List<ApiResponse<dynamic>> response;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public string? IpAddress { get; private set; }
        public string? UserAgent { get; private set; }


        public GenricController(IRepository Service, IBackgroundJobClient backgroundJobClient, IHttpContextAccessor httpContextAccessor)
        {
            IpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            UserAgent = httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

            _backgroundJobClient = backgroundJobClient;
            _Repo = Service;
            response = new List<ApiResponse<dynamic>>()
            {
                new ApiResponse < dynamic > {Errorcode= 101, Data = null, Details = "Error - ", Status = false },
            };
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Get()
        {

            response[0].Data = "Successfully Hit the API.";
            response[0].Data = "Success";
            response[0].Status = true;
            return Ok((response[0]));
        }


        [HttpPost]
        [Route("BoomBulkBatchRunningTask")]
        public async Task<IActionResult> BoomBulkBatchRunningTask(BoomBulkBatchId req)
        {
            var nullProperties = req.GetNullPropertiesBoomBulkBatchId();
            if (nullProperties.Count > 0)
            {
                string nullPropertiesString = string.Join(", ", nullProperties);
                response[0].Details = response[0].Details + nullPropertiesString;
                return Ok((response[0]));
            }
            
            _backgroundJobClient.Enqueue(() =>  _Repo.MultiGetBoomBatchTransactionIds(req.BatchId, IpAddress, UserAgent));
            response[0].Details = "Work in Progress";
            response[0].Status=true; 
            return Ok((response[0]));
         

        }
    }
}
