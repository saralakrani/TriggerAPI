using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace InfraStructureLayer
{
    public class DataBase
    {
        SqlConnection con;
        SqlCommand cmd;
        public DataBase()
        {
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/ServerConfig.txt");
            string conString = reader.ReadLine();
            con = new SqlConnection(conString);
        }
        public DataTable FetchData(string sqlQuery)
        {
            cmd = new SqlCommand(sqlQuery, con);
            cmd.CommandType = CommandType.Text;
            try
            {
                DataTable dt = new DataTable("ResultDataTable");
                con.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                cmd.Dispose();
            }
        }
        public object RunCommand(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = con;
                cmd.CommandTimeout = 1800;
                con.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public Int32 RunExecuteNoneQuery(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = con;
                cmd.CommandTimeout = 1800;
                con.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public DataTable ExecuteSqlData(string sqlQuery, SqlCommand cmd = null)
        {
            if (sqlQuery != String.Empty)
            {
                cmd = new SqlCommand(sqlQuery);
                cmd.CommandType = CommandType.Text;
            }
            try
            {
                DataTable dt = new DataTable("ResultDataTable");
                cmd.Connection = con;
                con.Open();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                cmd.Dispose();
            }
        }
        public DataSet ReturnDataSet(SqlCommand objCmd)
        {
            try
            {
                DataSet objDS = new DataSet();
                SqlDataAdapter objDA = null;
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.Connection = con;
                objCmd.CommandTimeout = 1800;
                con.Open();
                using (objDA = new SqlDataAdapter(objCmd))
                {
                    objDA.Fill(objDS);
                    return objDS;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                // cmd.Dispose();
            }
        }
    }
}
