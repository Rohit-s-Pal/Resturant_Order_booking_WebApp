using System.Data;
using System.Data.SqlClient;

namespace Estonia.Data
{
    public class Dataaccess
    {
        public static DataTable GetDataTable(String cmdText, CommandType cmdType, SqlParameter[] parameters, string database)
        {
            try
            {
                string conString = database;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.CommandTimeout = 1400;
                        if (cmdType == null) cmd.CommandType = CommandType.StoredProcedure; else cmd.CommandType = cmdType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                if (null != parameter) cmd.Parameters.Add(parameter);
                            }
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            con.Close();
                            con.Dispose();
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetDataSet(String cmdText, CommandType cmdType, SqlParameter[] parameters, string connectionString)
        {
            try
            {
                //Set Connection string
                string conString = connectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.CommandTimeout = 1400;
                        if (cmdType == null) cmd.CommandType = CommandType.StoredProcedure; else cmd.CommandType = cmdType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                if (null != parameter) cmd.Parameters.Add(parameter);
                            }
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            return ds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ExecuteScalar(String cmdText, CommandType cmdType, SqlParameter[] parameters, string connectionString)
        {
            try
            {
                string conString = connectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(cmdText, con))
                    {
                        cmd.CommandTimeout = 1400;
                        if (cmdType == null) cmd.CommandType = CommandType.StoredProcedure; else cmd.CommandType = cmdType;
                        if (parameters != null)
                        {
                            foreach (SqlParameter parameter in parameters)
                            {
                                if (null != parameter) cmd.Parameters.Add(parameter);
                            }
                        }
                        object objScaler = cmd.ExecuteScalar();
                        con.Close();
                        con.Dispose();
                        return objScaler;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
