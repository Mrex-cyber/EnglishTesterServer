using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Connection
{
    public class SqlQueryParameter
    {
        /// <summary>
        /// Название параметра
        /// </summary>
        public string Parameter { get; set; }
        /// <summary>
        /// Зеачение
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Тип
        /// </summary>
        public SqlDbType Type { get; set; }

        /// <summary>
        /// Назва структури, якщо Type == Structured
        /// </summary>
        public string StructureName { get; set; }
    }



    public class MsSql
    {
        readonly string connectionString = "";

        public MsSql(string connectionString)
        {
            this.connectionString = connectionString;

            List<string> g = new List<string>();
            
        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        public IEnumerable<IDataRecord> Read(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                if(connection.State != ConnectionState.Open) 
                {
                    connection.Open();
                }

                using (var command = new SqlCommand(query, connection))
                {
                    foreach (SqlQueryParameter item in parametrs)
                    {
                        SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);

                        if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                            param1.Value = item.Value.ToString().AsDBFloat();
                        else
                            param1.Value = item.Value;

                        if (item.Type == SqlDbType.Structured)
                        {
                            param1.TypeName = item.StructureName;
                        }

                        command.Parameters.Add(param1);
                    }

                    command.CommandTimeout = timeout;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tmp_record = (IDataRecord)reader;

                            yield return tmp_record;
                        }
                    }
                }
            };
        }

        public async Task<System.Data.DataTable> getDataTable(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {

            System.Data.SqlClient.SqlConnection dataConnection = new SqlConnection(ConnectionString);

            DataTable rez = new DataTable();


            SqlCommand sqlCommand = new SqlCommand(query, dataConnection);

            foreach (SqlQueryParameter item in parametrs)
            {
                SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);

                if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                    param1.Value = item.Value.ToString().AsDBFloat();
                else
                    param1.Value = item.Value;

                if (item.Type == SqlDbType.Structured)
                {
                    param1.TypeName = item.StructureName;
                }

                sqlCommand.Parameters.Add(param1);
            }

            try
            {
                System.Data.DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.SelectCommand.CommandTimeout = timeout;
                await Task.Run(() => da.Fill(ds));
                da.Dispose();
                rez = ds.Tables[0];
                ds.Dispose();
            }
            catch (Exception er)
            {
                string error = "";
                error += "Error at getDataTable: " + er.Message;
                error += $"{Environment.NewLine}{query}";
				throw new NotImplementedException(error, er);
            }
            finally { dataConnection.Close(); }


            return rez;
        }

        public System.Data.DataTable getDataTableSync(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {

            System.Data.SqlClient.SqlConnection dataConnection = new SqlConnection(ConnectionString);

            DataTable rez = new DataTable();


            SqlCommand sqlCommand = new SqlCommand(query, dataConnection);

            foreach (SqlQueryParameter item in parametrs)
            {
                SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);

                if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                    param1.Value = item.Value.ToString().AsDBFloat();
                else
                    param1.Value = item.Value;

                if (item.Type == SqlDbType.Structured)
                {
                    param1.TypeName = item.StructureName;
                }

                sqlCommand.Parameters.Add(param1);
            }

            try
            {
                System.Data.DataSet ds = new DataSet();
                System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.SelectCommand.CommandTimeout = timeout;
                da.Fill(ds);
                da.Dispose();
                rez = ds.Tables[0];
                ds.Dispose();
            }
            catch (Exception er)
            {
                string error = "";
                error += "Error at getDataTable: " + er.Message;
                throw new NotImplementedException(error, er);
            }
            finally { dataConnection.Close(); }


            return rez;
        }

        public async Task<System.Data.DataSet> getDataSet(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {
            System.Data.SqlClient.SqlConnection dataConnection = new SqlConnection(ConnectionString);

            System.Data.DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand(query, dataConnection);

            foreach (SqlQueryParameter item in parametrs)
            {
                SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);
                if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                    param1.Value = item.Value.ToString().AsDBFloat();
                else
                    param1.Value = item.Value;

                if (item.Type == SqlDbType.Structured)
                {
                    param1.TypeName = item.StructureName;
                }

                sqlCommand.Parameters.Add(param1);
            }

            try
            {
                System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.SelectCommand.CommandTimeout = timeout;
                await Task.Run(() => da.Fill(ds));
                da.Dispose();
            }
            catch (Exception er)
            {
                string error = "";
                error += "Error at getDataSet: " + er.Message;
                throw new NotImplementedException(error, er);
            }
            finally
            {
                dataConnection.Close();
            }

            return ds;
        }

        public System.Data.DataSet getDataSetSync(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {
            System.Data.SqlClient.SqlConnection dataConnection = new SqlConnection(ConnectionString);

            System.Data.DataSet ds = new DataSet();

            SqlCommand sqlCommand = new SqlCommand(query, dataConnection);

            foreach (SqlQueryParameter item in parametrs)
            {
                SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);
                if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                    param1.Value = item.Value.ToString().AsDBFloat();
                else
                    param1.Value = item.Value;

                if (item.Type == SqlDbType.Structured)
                {
                    param1.TypeName = item.StructureName;
                }

                sqlCommand.Parameters.Add(param1);
            }

            try
            {
                System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                da.SelectCommand.CommandTimeout = timeout;
                da.Fill(ds);
                da.Dispose();
            }
            catch (Exception er)
            {
                string error = "";
                error += "Error at getDataSet: " + er.Message;
                throw new NotImplementedException(error, er);
            }
            finally
            {
                dataConnection.Close();
            }

            return ds;
        }
        public async Task<bool> executeNonQuery(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {
            System.Data.SqlClient.SqlConnection dataConnection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommand = new SqlCommand(query, dataConnection);

            foreach (SqlQueryParameter item in parametrs)
            {
                SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);
                if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                    param1.Value = item.Value.ToString().AsDBFloat();
                else
                    param1.Value = item.Value;

                if (item.Type == SqlDbType.Structured)
                {
                    param1.TypeName = item.StructureName;
                }

                sqlCommand.Parameters.Add(param1);
            }

            try
            {
                dataConnection.Open();
                sqlCommand.CommandTimeout = timeout;
                await sqlCommand.ExecuteNonQueryAsync();
                dataConnection.Close();
                return true;
            }
            catch (Exception er)
            {
                string error = "";
                error += "Error at executeNonQuery: " + er.Message;
                throw new NotImplementedException(error, er);
            }
        }

        public bool executeNonQuerySync(string query, List<SqlQueryParameter> parametrs, int timeout = 700)
        {
            System.Data.SqlClient.SqlConnection dataConnection = new SqlConnection(ConnectionString);

            SqlCommand sqlCommand = new SqlCommand(query, dataConnection);

            foreach (SqlQueryParameter item in parametrs)
            {
                SqlParameter param1 = new SqlParameter(item.Parameter, item.Type);
                if (item.Type == SqlDbType.Float && item.Value != (object)DBNull.Value)
                    param1.Value = item.Value.ToString().AsDBFloat();
                else
                    param1.Value = item.Value;

                if (item.Type == SqlDbType.Structured)
                {
                    param1.TypeName = item.StructureName;
                }

                sqlCommand.Parameters.Add(param1);
            }

            try
            {
                dataConnection.Open();
                sqlCommand.CommandTimeout = timeout;
                sqlCommand.ExecuteNonQuery();
                dataConnection.Close();
                return true;
            }
            catch (Exception er)
            {
                string error = "";
                error += "Error at executeNonQuery: " + er.Message;
                throw new NotImplementedException(error, er);
            }
        }
    }
}
