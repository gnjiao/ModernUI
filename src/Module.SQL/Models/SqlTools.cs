using System;
using System.Data;
using System.Data.SqlClient;

namespace Module.SQL.Models
{
    public class SqlTools : IDisposable
    {
        private SqlTransaction _dbTrans;

        private string _error;

        private string _strSql;

        private string ConnectString { get; set; }

        private SqlConnection GetDbConnection { get; set; }

        public void ChangeDatabase(string tableName)
        {
            try
            {
                GetDbConnection = new SqlConnection(ConnectString);

                GetDbConnection.Open();

                GetDbConnection.ChangeDatabase(tableName);                
            }
            catch
            {
                // ignored
            }
            finally
            {
                GetDbConnection.Close();
            }
        }        

        public SqlTools(string strConn)
        {
            ConnectString = strConn;
        }

        ~SqlTools()
        {
            Dispose();
        }

        public void Setup(string strConn)
        {
            ConnectString = strConn;
        }

        public bool IsOpen
        {
            get
            {
                try
                {
                    GetDbConnection = new SqlConnection(ConnectString);

                    GetDbConnection.Open();

                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    GetDbConnection.Close();
                }
            }
        }

        public int ExecSql(string sql)
        {
            try
            {
                GetDbConnection = new SqlConnection(ConnectString);
                GetDbConnection.Open();

                int nResult;
                using (var cmd = GetDbConnection.CreateCommand())
                {
                    cmd.CommandText = sql;

                    nResult = cmd.ExecuteNonQuery();
                }

                return nResult;
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                return -1;
            }
            finally
            {
                GetDbConnection.Close();
            }
        }

        public DataSet SelectDataBase(string tempStrSql, string tempTableName)
        {
            DataSet ds = new DataSet();
            //ds.Tables[0].TableName = ""';
            this._strSql = tempStrSql;
            GetDbConnection = new SqlConnection(ConnectString);
            SqlDataAdapter da = new SqlDataAdapter(this._strSql, this.GetDbConnection);
            ds.Clear();
            da.Fill(ds, tempTableName);
            return ds;
        }

        public bool UpdateDataBaseFromDataSet(string strTblName, DataSet ds)
        {
            SqlDataAdapter myAdapter = new SqlDataAdapter();

            try
            {
                using (SqlCommand myCommand = new SqlCommand("select * from " + strTblName, this.GetDbConnection))
                {
                    myAdapter.SelectCommand = myCommand;

                    SqlCommandBuilder myCommandBuilder = new SqlCommandBuilder(myAdapter);

                    myAdapter.Update(ds, strTblName);
                }
                return true;
            }
            catch (Exception ex)
            {
                string s = ex.Message.ToString();
                return false;
            }
            finally
            {
                GetDbConnection.Close();
            }
        }

        public SqlDataReader GetSqlDataReader(string sql)
        {
            try
            {
                SqlDataReader sdr;

                GetDbConnection = new SqlConnection(ConnectString);
                GetDbConnection.Open();

                using (SqlCommand cmd = GetDbConnection.CreateCommand())
                {
                    cmd.CommandText = sql;

                    sdr = cmd.ExecuteReader();
                }

                return sdr;
            }
            catch (Exception ex)
            {
                _error = ex.Message;

                return null;
            }
            finally
            {
                GetDbConnection.Close();
            }
        }

        public DataSet GetDataSet(string sql)
        {
            try
            {
                DataSet ds;

                GetDbConnection = new SqlConnection(ConnectString);
                GetDbConnection.Open();

                using (SqlCommand cmd = GetDbConnection.CreateCommand())
                {
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = sql;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))//new SqlDataAdapter(sql, SqlConnectString))
                    {
                        ds = new DataSet();

                        sda.Fill(ds);
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                _error = ex.Message;

                return null;
            }
            finally
            {
                GetDbConnection.Close();
            }
        }

        public string GetLastError()
        {
            return _error;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            // TODO:  添加 SqlTools.Dispose 实现
            if (_dbTrans != null)
            {
                _dbTrans.Dispose();

                _dbTrans = null;
            }

            if (GetDbConnection != null)
            {
                GetDbConnection.Dispose();

                GetDbConnection = null;
            }
        }

        #endregion
    }
}
