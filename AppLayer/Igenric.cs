using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer
{
    public  interface Igenric
    {
         string Decrypt(string cipherText);
         string Encrypt(string clearText);
         dynamic ConvertDataTableToDynamic(DataTable dataTable);
         dynamic ConvertDataSetToDynamic(DataSet ds);
        string ConvertToJson<T>(T obj);
        void Log(string message);
    }
}
