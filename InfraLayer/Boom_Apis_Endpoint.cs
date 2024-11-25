using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Domain.Modal;

namespace InfraLayer
{
    public class Boom_Apis_Endpoint
    {
        public static string GetToken(string BoomApiUrl, string username, string password, string tenancyName = null)
        {
            string BoomAPIAuthToken = "";
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(BoomApiUrl + "authorization/token");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var input = "{\"email\":\"" + username + "\",\"password\":\"" + password + "\"}";
                    if (tenancyName != null)
                        input = input.TrimEnd('}') + ",\"tenancyName\":\"" + tenancyName + "\"}";
                    streamWriter.Write(input);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
                var entries = response.TrimStart('{').TrimEnd('}').Replace("\"", String.Empty).Split(',');
                foreach (var entry in entries)
                {
                    if (entry.Split(':')[1].Contains("accessToken") == true || entry.Split(':')[0].Contains("accessToken") == true)
                    {
                        if (entry.Length > 2)
                             BoomAPIAuthToken = entry.Split(':')[2];
                        else
                             BoomAPIAuthToken = entry.Split(':')[1];
                    }
                }
            }
            catch (Exception ex) {
                 BoomAPIAuthToken="*2 Token Api Catch Error-"+ex.Message;
            }
            return BoomAPIAuthToken;
        }
        public static string createBoomUser(string boomAPIURL,string strRequest,string BoomAPIAuthToken)
        {
            string strResponse="";
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(boomAPIURL);
                httpRequest.Method = "PUT";
                byte[] byteArray = Encoding.UTF8.GetBytes(strRequest);
                httpRequest.Accept = "*/*";
                httpRequest.Headers["Authorization"] = "Bearer " + BoomAPIAuthToken + "";
                httpRequest.KeepAlive = false;

                httpRequest.ContentType = "application/json";
                httpRequest.ContentLength = byteArray.Length;

                System.IO.StreamWriter stOut = new System.IO.StreamWriter(httpRequest.GetRequestStream(), System.Text.Encoding.ASCII);
                stOut.Write(strRequest);

                stOut.Flush();
                stOut.Close();
                System.IO.StreamReader stIn = new System.IO.StreamReader(httpRequest.GetResponse().GetResponseStream());
                strResponse = stIn.ReadToEnd();       //{"data":{"id":null,"status":false,"model":null},"details":["INVALID IMEI"],"status":false,"message":"DeviceManager"}
                stIn.Close();
            }
            catch (Exception ex)
            {
                strResponse = "*2 User Creation Api Catch Error-" + ex.Message;
            }
            return strResponse;
        }
        public static string BoomApi_Put_Method(string BoomAPIAuthToken, string boomAPIURL, string jsonRequest_BoomMobileActivation, string userAgent)
        {
            string activationresult = "";
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(boomAPIURL);
                httpWebRequest.Timeout = 180000;
                httpWebRequest.Headers.Add("Authorization", "Bearer " + BoomAPIAuthToken);
                httpWebRequest.UserAgent = userAgent;        
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = Convert.ToString(jsonRequest_BoomMobileActivation);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (Convert.ToString(httpResponse.StatusCode) == "OK")
                {
                    Stream newStream = httpResponse.GetResponseStream();
                    StreamReader sr = new StreamReader(newStream);
                    activationresult = sr.ReadToEnd();
                    return activationresult;
                }
                return activationresult;
            }
            catch (Exception ex)
            {
                return "*2 " + ex.Message + activationresult;
            }
        }
        public static ErrorFormat BoomApi_Get_Method(string BoomAPIAuthToken, string boomAPIURL, string userAgent)
        {
            int errorcode = 101;
            string result = "";
            ErrorFormat errorFormat = new ErrorFormat();
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(boomAPIURL);
                httpWebRequest.Headers.Add("Authorization", "Bearer " + BoomAPIAuthToken);
                httpWebRequest.UserAgent = userAgent;
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream newStream = httpResponse.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(newStream))
                        {
                            result = sr.ReadToEnd();
                            errorcode = 100;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorcode = 101;
                result = ex.Message;
            }
            errorFormat.Error_code = errorcode;
            errorFormat.Error_msg = result;
            return errorFormat; // Return the ErrorFormat object
        }
        public static ErrorFormat BoomApi_Post_Method(string BoomAPIAuthToken, string boomAPIURL,string jsonRequest_BoomMobileActivation)
        {
            int errorcode = 101;
            string result = "";
            ErrorFormat errorFormat = new ErrorFormat();
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(boomAPIURL);
                httpRequest.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest_BoomMobileActivation);
                httpRequest.Accept = "*/*";
                httpRequest.Headers["Authorization"] = "Bearer " + BoomAPIAuthToken + "";
                httpRequest.KeepAlive = false;
                httpRequest.ContentType = "application/json";
                httpRequest.ContentLength = byteArray.Length;

                System.IO.StreamWriter stOut = new System.IO.StreamWriter(httpRequest.GetRequestStream(), System.Text.Encoding.ASCII);
                stOut.Write(jsonRequest_BoomMobileActivation);
                stOut.Flush();
                stOut.Close();
                System.IO.StreamReader stIn = new System.IO.StreamReader(httpRequest.GetResponse().GetResponseStream());
                result = stIn.ReadToEnd();       
                stIn.Close();
                errorcode = 100;
            }
            catch (Exception ex)
            {
                errorcode = 101;
                result = ex.Message;
            }
            errorFormat.Error_code = errorcode;
            errorFormat.Error_msg = result;
            return errorFormat; // Return the ErrorFormat object
        }

        public static async Task  Post_Method( string boomAPIURL, string jsonRequest_BoomMobileActivation)
        {
            int errorcode = 101;
            string result = "";
            ErrorFormat errorFormat = new ErrorFormat();
            try
            {
                var httpRequest = (HttpWebRequest)WebRequest.Create(boomAPIURL);
                httpRequest.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest_BoomMobileActivation);
                httpRequest.Accept = "*/*";
                //httpRequest.Headers["Authorization"] = "Bearer " + BoomAPIAuthToken + "";
                httpRequest.KeepAlive = false;
                httpRequest.ContentType = "application/json";
                httpRequest.ContentLength = byteArray.Length;

                System.IO.StreamWriter stOut = new System.IO.StreamWriter(httpRequest.GetRequestStream(), System.Text.Encoding.ASCII);
                stOut.Write(jsonRequest_BoomMobileActivation);
                stOut.Flush();
                stOut.Close();
                System.IO.StreamReader stIn = new System.IO.StreamReader(httpRequest.GetResponse().GetResponseStream());
                result = stIn.ReadToEnd();
                stIn.Close();
                errorcode = 100;
            }
            catch (Exception ex)
            {
                errorcode = 101;
                result = ex.Message;
            }
            errorFormat.Error_code = errorcode;
            errorFormat.Error_msg = result;
            //return errorFormat; // Return the ErrorFormat object
        }
    }
}
