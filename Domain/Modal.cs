using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Domain.Modal.MdnStatusDto;

namespace Domain
{
    public class Modal
    {
        public class ApiResponse<T>
        {
            public int Errorcode { get; set; } //100 SUCC, 101 NO DATA FOUND, 102 ERROR
            public T? Data { get; set; }
            public string? Details { get; set; }
            public bool Status { get; set; }
        }


        #region Boom_Instant_GENERIC
        public class StoredAPIRequestBeforeCall
        {
            public string Title { get; set; }
            public string Request { get; set; }
            public Int32 DistributorId { get; set; }
            public string TransactionID { get; set; }
            public string Msisdn { get; set; }
            public string SIMNumber { get; set; }
        }
        public class StoredAPIResponseAfterCall
        {
            public string Title { get; set; }
            public string Response { get; set; }
            public int DistributorId { get; set; }
            public string TransactionID { get; set; }
            public string Msisdn { get; set; }
            public string SIMNumber { get; set; }
        }
        #endregion Boom_Instant_GENERIC

        #region Boom_Instant_Activation
        public class Boom_Instant_Activation_Req
        {
            public string ClientCode { get; set; }
            public string IMEI { get; set; }
            public string SerialNumber { get; set; }
            public string PortMobileNo { get; set; }
            public int Carrrier_Type { get; set; } //97 FOR PURPLE2 (TMOBILE)  AND 90 FOR RED (VERIZON)
            public string TariffId { get; set; }
            public int Month { get; set; }
            public decimal Rental { get; set; }
            public bool isWifi { get; set; }
            public bool isEsim { get; set; }
            public string TransactionId { get; set; }
            public int ActivationOrPortin { get; set; }  //0 FOR ACTIVATION 1 FOR PORTIN
            public objBoomMobileActivationCustomerDetails Customer_Details { get; set; }
            public List<string> GetNullProperties()
            {
                List<string> nullProperties = new List<string>();

                // Check common properties of Request
                if (string.IsNullOrEmpty(ClientCode)) nullProperties.Add("Required -" + nameof(ClientCode));
                if (string.IsNullOrEmpty(IMEI)) nullProperties.Add("Required -" + nameof(IMEI));
                if (Convert.ToString(IMEI).Length < 14 || Convert.ToString(IMEI).Length > 16) nullProperties.Add("IMEI length should between 14 to 16 Digits");
                if (string.IsNullOrEmpty(SerialNumber) && !isEsim) nullProperties.Add("Required -" + nameof(SerialNumber));
                if (string.IsNullOrEmpty(TariffId)) nullProperties.Add("Required -" + nameof(TariffId));
                if (Month <= 0) nullProperties.Add("Required -" + nameof(Month));
                if (Rental <= 0) nullProperties.Add("Required -" + nameof(Rental));
                if (string.IsNullOrEmpty(TransactionId)) nullProperties.Add("Required -" + nameof(TransactionId));
                if (string.IsNullOrEmpty(Convert.ToString(isWifi))) nullProperties.Add("Required -" + nameof(isWifi));
                if (string.IsNullOrEmpty(Convert.ToString(isEsim))) nullProperties.Add("Required -" + nameof(isEsim));
                if (string.IsNullOrEmpty(Convert.ToString(Carrrier_Type))) nullProperties.Add("Required -" + nameof(Carrrier_Type));
                if (Carrrier_Type != 97 && Carrrier_Type != 90) nullProperties.Add("Carrrier_Type must be 97 for Tmobile and 90 for verizon");
                if (string.IsNullOrEmpty(Convert.ToString(ActivationOrPortin))) nullProperties.Add("Required -" + nameof(ActivationOrPortin));
                if (ActivationOrPortin != 0 && ActivationOrPortin != 1) nullProperties.Add("ActivationOrPortin must be 0 Activation OR 1 For Port In");
                if (string.IsNullOrEmpty(Customer_Details.firstName)) nullProperties.Add("Required -" + nameof(Customer_Details.firstName));
                if (string.IsNullOrEmpty(Customer_Details.lastName)) nullProperties.Add("Required -" + nameof(Customer_Details.lastName));
                if (string.IsNullOrEmpty(Customer_Details.state)) nullProperties.Add("Required -" + nameof(Customer_Details.state));
                if (string.IsNullOrEmpty(Customer_Details.city)) nullProperties.Add("Required -" + nameof(Customer_Details.city));
                if (string.IsNullOrEmpty(Customer_Details.address)) nullProperties.Add("Required -" + nameof(Customer_Details.address));
                if (string.IsNullOrEmpty(Customer_Details.zip)) nullProperties.Add("Required -" + nameof(Customer_Details.zip));
                
                if (ActivationOrPortin == 1)
                {
                    if (string.IsNullOrEmpty(PortMobileNo)) nullProperties.Add("Required -" + nameof(PortMobileNo));
                    if (PortMobileNo.Length < 10 || PortMobileNo.Length > 11) nullProperties.Add("Port Mobile no Can't be grater than 11 or less than  10.");
                    if (string.IsNullOrEmpty(Customer_Details.contactNumber)) nullProperties.Add("Required -" + nameof(Customer_Details.contactNumber));
                    if (string.IsNullOrEmpty(Customer_Details.portableFromProvider)) nullProperties.Add("Required -" + nameof(Customer_Details.portableFromProvider));
                    if (string.IsNullOrEmpty(Customer_Details.portableFromAccountNumber)) nullProperties.Add("Required -" + nameof(Customer_Details.portableFromAccountNumber));
                    if (string.IsNullOrEmpty(Customer_Details.portableFromAccountPassCode)) nullProperties.Add("Required -" + nameof(Customer_Details.portableFromAccountPassCode));
                }
                return nullProperties;
            }
        }
        public class objBoomMobileTransactionDeduction
        {
            public decimal ChargedAmount { get; set; }
            public Int32 DistributorID { get; set; }
            public Int32 LoginID { get; set; }
            public int PaymentFrom { get; set; }
            public Int32 PayeeID { get; set; }
            public int PaymentType { get; set; }
            public string PaymentMode { get; set; }
            public string TransactionId { get; set; }
            public string TransactionStatus { get; set; }
            public int TransactionStatusId { get; set; }
            public int ActivationType { get; set; }
            public int ActivationStatus { get; set; }
            public int ActivationVia { get; set; }
            public string ActivationRequest { get; set; }
            public int TariffID { get; set; }
            public string SerialNumber { get; set; }
            public decimal Regulatery { get; set; }
            public int Month { get; set; }
            public string ZipCode { get; set; }
            public string AreaCode { get; set; }
            public string AccountNumber { get; set; }
            public string PinNumber { get; set; }
            public string MSISDN { get; set; }
            public string IMEI { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string CustomerEmailId { get; set; }
            public string PortFromFirstName { get; set; }
            public string PortFromLastName { get; set; }
            public string PortFromZipCode { get; set; }
            public string PortFromAddress { get; set; }
            public string PortFromCity { get; set; }
            public string PortFromState { get; set; }
            public string PortFromEmailId { get; set; }
            public string PortFromContact { get; set; }
            public string PortFromProvider { get; set; }
            public string BoomCustomerId { get; set; }
            public int SimType { get; set; }
        }
        public class objUpdateBoomMobileTransactionResponseDetails
        {
            public Int32 Activationid { get; set; }
            public Int32 PaymentID { get; set; }
            public string RESP { get; set; }
            public string ALLOCATED_MSISDN { get; set; }
            public string MNPNO { get; set; }
        }
        public class objUpdateFailResponseAndReturnBoomMobileChargedAmount
        {
            public Int32 Activationid { get; set; }
            public Int32 PaymentID { get; set; }
            public Int32 LoginID { get; set; }
            public string RESP { get; set; }
            public string ALLOCATED_MSISDN { get; set; }
            public decimal ChargedAmount { get; set; }
            public string PaymentMode { get; set; }
            public int ActivationType { get; set; }
            public int ActivationStatus { get; set; }
        }
        #endregion Boom_Instant_Activation
        public class objBoomMobileActivationApiRequest
        {
            public string type { get; set; }
            public string sim { get; set; }
            public string imei { get; set; }
            public string shareType { get; set; }
            public string planId { get; set; }
            public string dealerCustomerId { get; set; }
            public string sharedDealerCustomerId { get; set; }
            public bool delayActivation { get; set; }
            public string additionalSubscriptions { get; set; }
            public objBoomMobileActivationCustomerDetails customer { get; set; }
            public List<TransactionCode> transactionCodes { get; set; }
            public Boolean isWifi { get; set; }
        }
        public class objBoomMobilePorttinActivationApiRequest
        {
            public string type { get; set; }
            public string sim { get; set; }
            public string imei { get; set; }
            public string shareType { get; set; }
            public string planId { get; set; }
            public string dealerCustomerId { get; set; }
            public string sharedDealerCustomerId { get; set; }
            public bool delayActivation { get; set; }
            public string portableNumber { get; set; }
            public string contactNumber { get; set; }
            public string authorizedSignature { get; set; }
            public string portableFromFirstName { get; set; }
            public string portableFromMiddleInitial { get; set; }
            public string portableFromLastName { get; set; }
            public string portableFromEmailAddress { get; set; }
            public string portableFromAddress { get; set; }
            public string portableFromCity { get; set; }
            public string portableFromState { get; set; }
            public string portableFromZip { get; set; }
            public string portableFromProvider { get; set; }
            public string portableFromAccountNumber { get; set; }
            public string portableFromAccountPassCode { get; set; }
            public string additionalSubscriptions { get; set; }
            public List<TransactionCode> transactionCodes { get; set; }
            public Boolean isWifi { get; set; }

        }
        public class objBoomMobileActivationCustomerDetails
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string email { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }

            public string? contactNumber { get; set; } //portin detail
                         
            public string? portableFromProvider { get; set; } //portin detail
            public string? portableFromAccountNumber { get; set; } //portin detail
            public string? portableFromAccountPassCode { get; set; } //portin detail
        }
        public class TransactionCode
        {
            public string code { get; set; }
        }
        public class ValidationDbResult
        {
            public int ErrorCode { get; set; }
            public string ErrorMessage { get; set; }
            public DataSet DbTables { get; set; }
            public DataTable Table { get; set; }
        }
        public class Boom_Recharge_Req
        {
            public string ClientCode { get; set; }
            public string TransactionId { get; set; }
            public string MSISDN { get; set; }
            public string PlanID { get; set; }
            public decimal Rental { get; set; }
            public int Carrier_Type { get; set; } //90 for Red,97 for Purple
            public int RechargeType { get; set; } //0 means On plan expiry , 1 means starting from now
            public List<string> GetNullProperties_Boom_Recharge()
            {
                List<string> nullProperties = new List<string>();
                // Check common properties
                if (string.IsNullOrEmpty(ClientCode)) nullProperties.Add("Required -" + nameof(ClientCode));
                if (string.IsNullOrEmpty(TransactionId)) nullProperties.Add("Required -" + nameof(TransactionId));
                if (string.IsNullOrEmpty(MSISDN)) nullProperties.Add("Required -" + nameof(MSISDN));
                if (MSISDN.Length > 11 || MSISDN.Length < 10) nullProperties.Add("Msisdn No Must Be Between 10 to 11.");
                if (string.IsNullOrEmpty(PlanID)) nullProperties.Add("Required -" + nameof(PlanID));
                if (Rental == 0) nullProperties.Add("Required -" + nameof(Rental));
                if (Carrier_Type != 97 && Carrier_Type != 90) nullProperties.Add("Carrrier_Type must be 97 for T-mobile and 90 for Verizon");
                if (RechargeType != 0 && RechargeType != 1) nullProperties.Add("RechargeType Must Be Between 0 OR 1.");
                return nullProperties;
            }
        }
        public class BoomMobileRecharge
        {
            public string newPlanId { get; set; }
            public string updateOption { get; set; }
            public string type { get; set; }
            public string transactionCode { get; set; }
        }
        public class BoomRechargeTransactionDeduction
        {
            public decimal ChargedAmount { get; set; }
            public long DistributorID { get; set; }
            public long LoginID { get; set; }
            public int PaymentFrom { get; set; }
            public int PaymentType { get; set; }
            public string PaymentMode { get; set; }
            public string TransactionId { get; set; }
            public string TransactionStatus { get; set; }
            public long TransactionStatusId { get; set; }
            public long RechargeType { get; set; }
            public long ActivationStatus { get; set; }
            public long ActivationVia { get; set; }
            public string ActivationRequest { get; set; }
            public int TariffID { get; set; }
            public decimal Regulatery { get; set; }
            public string MSISDN { get; set; }
            public decimal PlanAmount { get; set; }
        }
        public class BoomRechargeUpdateRefund
        {
            public long RechargeId { get; set; }
            public long LoginID { get; set; }
            public string RESP { get; set; }
            public decimal ChargedAmount { get; set; }
            public string PaymentMode { get; set; }
            public int RechargeType { get; set; }
            public int RechargeStatus { get; set; }
        }
        public class BoomRechargeResponseUpdate
        {
            public long RechargeId { get; set; }
            public string RechargeResponse { get; set; }
        }
        public class ValidatefromDbReq
        {
            public string ClientCode { get; set; }
            public string Msisdn { get; set; }
            public string? SerialNumber { get; set; }
            public string? IMEI { get; set; }
            public string? ApiEndPoint { get; set; }
            public int Carrier_Type { get; set; } //97 for purple2 90 for red
        }
        public class CheckPortinStatus
        {
            public string ClientCode { get; set;}
            public string Msisdn { get; set; }
            public int Carrier_Type { get; set; } //97 FOR PURPLE2 90 FOR RED
            public List<string> GetNullProperties_ObjCheckPortinStatus()
            {
                List<string> nullProperties = new List<string>();
                // Check common properties
                if (string.IsNullOrEmpty(ClientCode)) nullProperties.Add("Required -" + nameof(ClientCode));
                if (string.IsNullOrEmpty(Msisdn)) nullProperties.Add("Required -" + nameof(Msisdn));
                if (string.IsNullOrEmpty(Convert.ToString(Carrier_Type))) nullProperties.Add("Required -" + nameof(Carrier_Type));
                if (Carrier_Type != 97 && Carrier_Type != 90) nullProperties.Add("Carrrier_Type must be 97 for T-mobile and 90 for Verizon");
                if (Msisdn.Length < 10 || Msisdn.Length > 11) nullProperties.Add("Msisdn No. Must be contain 10 or 11 Digits.");
                return nullProperties;
            }
        }
        public class ClientInfoService
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public ClientInfoService(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public string GetIpAddress()
            {
                try
                {
                    return "";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
               
            }

            public string GetUserAgent()
            {
                try
                {
                    return "";
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                
            }
        }
        public class ErrorFormat { 
            public int Error_code { get; set; }
            public string? Error_msg { get; set; }
        }
        public class MdnStatusDto 
        {
            public class Purchase
            {
                public string id { get; set; }
                public string accountNumber { get; set; }
                public string pin { get; set; }
            }

            public class Mdn
            {
                public string id { get; set; }
                public string number { get; set; }
                public string status { get; set; }
                public string imei { get; set; }
                public string sim { get; set; }
            }

            public class Plan
            {
                public string startDate { get; set; }
                public string endDate { get; set; }
                public string status { get; set; }
                public string network { get; set; }
                public string agentCode { get; set; }
                public string data { get; set; }
                public string id { get; set; }
                public string name { get; set; }
                public string sku { get; set; }
            }

            public class Subscription
            {
                public string startDate { get; set; }
                public string endDate { get; set; }
                public bool isAutoPay { get; set; }
                public string status { get; set; }
                public string network { get; set; }
                public object agentCode { get; set; }
                public string spiffMonth { get; set; }
                public string transactionDate { get; set; }
                public SummaryUsage summaryUsage { get; set; }
                public SummaryUsage shortSummaryUsage { get; set; }
                public Transaction transaction { get; set; }
                public string id { get; set; }
                public string name { get; set; }
                public string sku { get; set; }
            }

            public class SummaryUsage
            {
                public int minutesIn { get; set; }
                public int textingIn { get; set; }
                public int minutesOut { get; set; }
                public int textingOut { get; set; }
                public double data { get; set; }
            }

            public class Transaction
            {
                public string transactionId { get; set; }
                public object transactionCode { get; set; }
            }

            public class Customer
            {
                public string id { get; set; }
                public string firstName { get; set; }
                public string lastName { get; set; }
                public string address { get; set; }
                public string city { get; set; }
                public string state { get; set; }
                public string zip { get; set; }
                public string email { get; set; }
            }

            public class FullSummaryUsage
            {
                public Monthly monthly { get; set; }
                public object annually { get; set; }
            }

            public class Monthly
            {
                public Usage text { get; set; }
                public Usage talk { get; set; }
                public Usage data { get; set; }
            }

            public class Usage
            {
                public string usage { get; set; }
                public string plan { get; set; }
                public object additional { get; set; }
                public object isThrottled { get; set; }
                public object totalAdditionalData { get; set; }
            }

           
            public class MdnStatusDtoData
            {
                public Purchase purchase { get; set; }
                public Mdn mdn { get; set; }
                public Plan plan { get; set; }
                public List<Subscription> subscriptions { get; set; }
                public Customer customer { get; set; }
                public SummaryUsage summaryUsage { get; set; }
                public FullSummaryUsage fullSummaryUsage { get; set; }
                public string creationTime { get; set; }
                public bool delayActivation { get; set; }
            }
        }
        public class CheckSerialNumber
        {
            public string ClientCode { get; set; }
            public string SerialNumber { get; set; }
            public int Carrier_Type { get; set; } //97 for purple2 90 for red
            public List<string> GetNullProperties_objCheckSerialNumber()
            {
                List<string> nullProperties = new List<string>();
                // Check common properties
                if (string.IsNullOrEmpty(ClientCode)) nullProperties.Add("Required -" + nameof(ClientCode));
                if (string.IsNullOrEmpty(SerialNumber)) nullProperties.Add("Required -" + nameof(SerialNumber));
                if (string.IsNullOrEmpty(Convert.ToString(Carrier_Type))) nullProperties.Add("Required -" + nameof(Carrier_Type));
                if (Carrier_Type != 97 && Carrier_Type != 90) nullProperties.Add("Carrrier_Type must be 97 for T-mobile and 90 for Verizon");
                if (SerialNumber.Length < 18 || SerialNumber.Length > 20) nullProperties.Add("SerialNumber No. Must be between 19 to 20");
                return nullProperties;
            }
        }
        public class CheckImei
        {
            public string ClientCode { get; set; }
            public string Imei { get; set; }
            public int Carrier_Type { get; set; } //97 FOR PURPLE2 90 FOR RED
            public bool is4g { get; set; }
            public List<string> GetNullProperties_objCheckImei()
            {
                List<string> nullProperties = new List<string>();
                // CHECK COMMON PROPERTIES
                if (string.IsNullOrEmpty(ClientCode)) nullProperties.Add("Required -" + nameof(ClientCode));
                if (string.IsNullOrEmpty(Imei)) nullProperties.Add("Required -" + nameof(Imei));
                if (string.IsNullOrEmpty(Convert.ToString(Carrier_Type))) nullProperties.Add("Required -" + nameof(Carrier_Type));
                if (Carrier_Type != 97 && Carrier_Type != 90) nullProperties.Add("Carrrier_Type must be 97 for T-mobile and 90 for Verizon");
                if (Imei.Length < 14 || Imei.Length > 16)nullProperties.Add("IMEI No. Can't be grater than 16 or less than  14.");
                return nullProperties;
            }
        }
        public class CheckPortinMdn
        {
            public string ClientCode { get; set; }
            public string Imei { get; set; }
            public string SerialNumber { get; set; }
            public string Msisdn { get; set; }
            public int Carrier_Type { get; set; } //97 for purple2 90 for red
            public bool is4g { get; set; }
            public List<string> GetNullProperties_CheckPortinMdn()
            {
                List<string> nullProperties = new List<string>();

                //CHECK PARAMTERS ARE NULL OR EMPTY
                if (string.IsNullOrEmpty(ClientCode)) nullProperties.Add("Required -" + nameof(ClientCode));
                if (string.IsNullOrEmpty(Imei)) nullProperties.Add("Required -" + nameof(Imei));
                if (string.IsNullOrEmpty(Msisdn)) nullProperties.Add("Required -" + nameof(Msisdn));
                if (string.IsNullOrEmpty(SerialNumber)) nullProperties.Add("Required -" + nameof(SerialNumber));
                if (string.IsNullOrEmpty(Convert.ToString(Carrier_Type))) nullProperties.Add("Required -" + nameof(Carrier_Type));

                //VALIDATE VALUE OF PARAMTERS ARE NULL OR EMPTY
                if (Carrier_Type != 97 && Carrier_Type != 90) nullProperties.Add("Carrrier_Type must be either 97 for T-mobile or 90 for Verizon");
                if (Imei.Length < 14 || Imei.Length > 16) nullProperties.Add("Imei No. Can't be grater than 16 or less than  14.");
                if (SerialNumber.Length < 18 || SerialNumber.Length > 23) nullProperties.Add("SerialNumber No. Must be between 18 to 23.");
                if (Msisdn.Length < 10 || Msisdn.Length > 11) nullProperties.Add("Msisdn No. Must be between 10 to 11.");
                return nullProperties;
            }
        }
        public class Boom_BulkSingle_Activation_Req
        {
            public string ClientCode { get ; set; }
            public objBoomMobileActivationApiRequest Request { get; set; }
            public string token { get; set; }
            public string boomAPIURL { get; set; }
            public Int32 Distributorid { get; set; }
            public Int32 LoginId { get; set; }
            public string SerialNumber { get; set; }
            public Int32 Activationid { get ; set; }
            public decimal ChargedAmount { get; set; }
            public Int32 PaymentID { get; set; }
            public string TransactionId { get; set; }
            public int ActivationOrPortin { get; set; }
            public string PortMobileNo { get; set; }
        }
        public class BoomBulkBatchId
        {
            public string BatchId { get; set; }

            public List<string> GetNullPropertiesBoomBulkBatchId()
            {
                List<string> nullProperties = new List<string>();
                if (string.IsNullOrEmpty(BatchId)) 
                    nullProperties.Add("Required -" + nameof(BatchId));

                return nullProperties;
            }
        }

    }
}
