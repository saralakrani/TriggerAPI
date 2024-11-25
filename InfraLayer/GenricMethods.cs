using AppLayer;
using Grpc.Core;
using InfraLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructureLayer
{
    public class GenricMethods :Igenric
    {
        // Not being used for Now
        #region encrypt decrypt password
        public string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "CIRCUMFRANCES6546753";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch(Exception ex)
            { 
                return "*2 ," + ex.Message;
            }
           
        }

        public string Encrypt(string clearText)
        {
            try
            {
                string EncryptionKey = "CIRCUMFRANCES6546753";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch(Exception ex) 
            {
                return "*2 ," + ex.Message;
            }
            
        }
        #endregion

        #region Convert Datatable and Dataset Into  json  dynamic 
        public dynamic ConvertDataTableToDynamic(DataTable dataTable)
        {
            try
            {
                dynamic dynamicObject = new ExpandoObject();
                var dynamicProperties = dynamicObject as IDictionary<string, object>;

                foreach (DataRow row in dataTable.Rows)
                {
                    var rowData = dynamicObject as IDictionary<string, object>;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        string columnName = column.ColumnName;
                        object columnValue = row[column];

                        rowData[columnName] = columnValue;
                    }
                }

                return dynamicObject;
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }
        public dynamic ConvertDataSetToDynamic(DataSet dataSet)
        {
            try
            {
                dynamic dynamicObject = new ExpandoObject();

                var dynamicTables = new List<dynamic>();

                foreach (DataTable table in dataSet.Tables)
                {
                    dynamic dynamicTable = new ExpandoObject();
                    var dynamicRows = new List<dynamic>();

                    foreach (DataRow row in table.Rows)
                    {
                        dynamic dynamicRow = new ExpandoObject();
                        var rowData = dynamicRow as IDictionary<string, object>;

                        foreach (DataColumn column in table.Columns)
                        {
                            string columnName = column.ColumnName;
                            object columnValue = row[column];

                            rowData[columnName] = columnValue;
                        }

                        dynamicRows.Add(dynamicRow);
                    }

                    ((IDictionary<string, object>)dynamicTable).Add(table.TableName, dynamicRows);
                    dynamicTables.Add(dynamicTable);
                }

                  ((IDictionary<string, object>)dynamicObject).Add("Tables", dynamicTables);


                return dynamicObject;
            }
            catch(Exception ex)
            {
                return null;
            }
}



        #endregion

        #region json Convert dynamic 
        public string ConvertToJson<T>(T obj)
        {
            // Serialize the object to JSON
            string json = JsonConvert.SerializeObject(obj);

            return json;
        }
        #endregion
        public void Log(string message)
        {
            try
            {
                string logConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt");

                // Write to the text file
                using (StreamWriter sw = File.AppendText(logConfigFilePath))
                {
                    sw.WriteLine($"{DateTime.Now}: {message}");
                    sw.WriteLine("------------------------------------------------Gap-----------------------------------------------");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
