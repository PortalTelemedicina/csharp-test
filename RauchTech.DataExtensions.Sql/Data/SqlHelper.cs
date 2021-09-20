using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Demo.Common.Configuration;
using System;
using System.Data;
using System.Linq;

namespace Demo.DataExtensions.Sql
{
    public class SqlHelper: ISqlHelper
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public SqlHelper(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetValue("ConnectionStrings:ConnectionBase")[0];
        }


        public int ExecuteNonQuery(SqlCommand command)
        {
            int retorno;
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);

                command.Connection = connection;
                command.CommandTimeout = 0;

                connection.Open();
                retorno = command.ExecuteNonQuery();

                command.Dispose();
                connection.Dispose();
            }
            catch
            {
                throw;
            }

            return retorno;
        }

        public int ExecuteNonQuery(string script, SqlParameter[] parameters = null)
        {
            int retorno;
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = script;
                command.CommandTimeout = 0;

                if (parameters?.Count() > 0)
                {
                    command.Parameters.AddRange(parameters);
                    command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    command.CommandType = CommandType.Text;
                }

                connection.Open();
                retorno = command.ExecuteNonQuery();

                command.Dispose();
                connection.Dispose();
            }
            catch
            {
                throw;
            }

            return retorno;
        }


        /// <summary>
        /// Executa scripts e procedures que retornam um único dado (inserts com id, counts, selects de um único dado)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(SqlCommand command)
        {
            object retorno;
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);

                command.Connection = connection;
                command.CommandTimeout = 0;

                connection.Open();
                retorno = command.ExecuteScalar();

                command.Dispose();
                connection.Dispose();
            }
            catch
            {
                throw;
            }

            return retorno;
        }

        /// <summary>
        /// Executa scripts e procedures que retornam um único dado (inserts com id, counts, selects de um único dado)
        /// </summary>
        /// <param name="script"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string script, SqlParameter[] parameters = null)
        {
            object retorno;
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = script;
                command.CommandTimeout = 0;

                if (parameters?.Count() > 0)
                {
                    command.Parameters.AddRange(parameters);
                    command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    command.CommandType = CommandType.Text;
                }

                connection.Open();
                retorno = command.ExecuteScalar();

                command.Dispose();
                connection.Dispose();
            }
            catch
            {
                throw;
            }

            return retorno;
        }


        public DataSet ExecuteDataSet(SqlCommand command)
        {
            DataSet retorno;

            try
            {
                retorno = new DataSet();

                SqlConnection connection = new SqlConnection(_connectionString);

                command.Connection = connection;
                command.CommandTimeout = 0;

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(retorno);

                command.Dispose();
                connection.Dispose();
            }
            catch
            {
                throw;
            }

            return retorno;
        }
        public DataSet ExecuteDataSet(string script, SqlParameter[] parameters = null)
        {
            DataSet retorno;

            try
            {
                retorno = new DataSet();

                SqlConnection connection = new SqlConnection(_connectionString);
                SqlCommand command = new SqlCommand();

                command.Connection = connection;
                command.CommandText = script;
                command.CommandTimeout = 0;

                if (parameters?.Count() > 0)
                {
                    command.Parameters.AddRange(parameters);
                    command.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    command.CommandType = CommandType.Text;
                }

                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                _ = adapter.Fill(retorno);

                command.Dispose();
                connection.Dispose();
            }
            catch
            {
                throw;
            }

            return retorno;
        }
    }
}
