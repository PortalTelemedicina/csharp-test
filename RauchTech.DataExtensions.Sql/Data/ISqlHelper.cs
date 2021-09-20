using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Demo.DataExtensions.Sql
{
    public interface ISqlHelper
    {
        int ExecuteNonQuery(SqlCommand command);

        int ExecuteNonQuery(string script, SqlParameter[] parameters = null);


        /// <summary>
        /// Executa scripts e procedures que retornam um único dado (inserts com id, counts, selects de um único dado)
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        object ExecuteScalar(SqlCommand command);

        /// <summary>
        /// Executa scripts e procedures que retornam um único dado (inserts com id, counts, selects de um único dado)
        /// </summary>
        /// <param name="script"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ExecuteScalar(string script, SqlParameter[] parameters = null);

        DataSet ExecuteDataSet(SqlCommand command);
        DataSet ExecuteDataSet(string script, SqlParameter[] parameters = null);
    }
}
