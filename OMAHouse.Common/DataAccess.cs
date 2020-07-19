using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace OMAHouse.Common
{
    public class DataAccess
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public static IConfigurationRoot Configuration { get; set; }
        static IConfigurationRoot builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        public static string ConnectionString = builder["ConnectionString:OMAHouse"].ToString();

        public bool IsLogin()
        {
            //Session["UserLoginID"].ToString() != "" || Session["UserLoginID"].ToString() != string.Empty || 
            //if (Session["UserLoginID"] != null)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        public bool IsEmailValid(string Email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }

        }

        public DataTable GetDatatable(string SPName, string[] paramNames, object[] paramValues)
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(SPName, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < paramNames.Length; i++)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = paramNames[i];
                    p.Value = paramValues[i];
                    cmd.Parameters.Add(p);
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);

                da.Fill(dt);
                return dt;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public DataTable GetDatatable(string SPName)
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(SPName, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);

                da.Fill(dt);
                return dt;
            }
            catch
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                return null;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public int ExecuteNonQuery(string SPName)
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            try
            {
                int rowsAffected = 0;
                cnn.Open();
                SqlCommand cmd = new SqlCommand(SPName, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rowsAffected = cmd.ExecuteNonQuery();

                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                if (cnn != null)
                {
                    cnn.Dispose();
                }
                return rowsAffected;
            }
            catch
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                return -1;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public int ExecuteNonQuery(string SPName, string[] paramNames, object[] paramValues)
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            try
            {
                int rowsAffected = 0;
                cnn.Open();
                SqlCommand cmd = new SqlCommand(SPName, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < paramNames.Length; i++)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = paramNames[i];
                    p.Value = paramValues[i];
                    cmd.Parameters.Add(p);
                }
                rowsAffected = cmd.ExecuteNonQuery();

                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                if (cnn != null)
                {
                    cnn.Dispose();
                }
                return rowsAffected;
            }
            catch
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                return -1;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public DataSet GetDataset(string SPName)
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(SPName, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);

                da.Fill(ds);
                return ds;
            }
            catch
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                return null;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public DataSet GetDataset(string SPName, string[] paramNames, object[] paramValues)
        {
            SqlConnection cnn = new SqlConnection(ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand(SPName, (SqlConnection)cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                for (int i = 0; i < paramNames.Length; i++)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = paramNames[i];
                    p.Value = paramValues[i];
                    cmd.Parameters.Add(p);
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter((SqlCommand)cmd);

                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
                return null;
            }
            finally
            {
                if (cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
        }

        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
