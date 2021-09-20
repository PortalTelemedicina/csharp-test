using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Demo.DataExtensions.Sql;
using Demo.Domain.Model;

namespace Demo.Domain.Repository
{
	public class OrdersRepository
{
    private readonly IConfiguration _config;
    private readonly ISqlHelper _sqlHelper;

    public OrdersRepository(IConfiguration configuration, ISqlHelper sqlHelper)
    {
        _config = configuration;
        _sqlHelper = sqlHelper;
    }

    #region LoadModel

    private List<Orders> Load(DataSet data)
    {
        List<Orders> orderss;
        Orders orders;

        try
        {
            orderss = new List<Orders>();

            foreach (DataRow row in data.Tables[0].Rows)
            {
                orders = new Orders();

                orders.ID = row.Field<long>("ID");
                orders.ProductID = row.Field<long>("ProductID");
                orders.UserID = row.Field<long>("UserID");
                orders.Price = row.Field<decimal?>("Price");
                orders.Quantidade = row.Field<int?>("Quantidade");

                orderss.Add(orders);
            }
        }
        catch
        {
            throw;
        }

        return orderss;
    }

    #endregion

    #region Change Data

    public Orders Insert(Orders orders)
    {
        SqlCommand command;

        try
        {
            command = new SqlCommand(" INSERT INTO Orders " +
                        " (" +
                            "  ProductID" +
                            " ,UserID" +
                            " ,Price" +
                            " ,Quantidade" +
                        " )" +
                        " OUTPUT inserted.ID " +
                        " VALUES " +
                        " (" +
                            "  @ProductID" +
                            " ,@UserID" +
                            " ,@Price" +
                            " ,@Quantidade" +
                        " )");

            command.Parameters.AddWithValue("ProductID", orders.ProductID.AsDbValue());
            command.Parameters.AddWithValue("UserID", orders.UserID.AsDbValue());
            command.Parameters.AddWithValue("Price", orders.Price.AsDbValue());
            command.Parameters.AddWithValue("Quantidade", orders.Quantidade.AsDbValue());

            orders.ID = (long)_sqlHelper.ExecuteScalar(command);
        }
        catch
        {
            throw;
        }

        return orders;
    }

    public Orders Update(Orders orders)
    {
        SqlCommand command;

        try
        {
            command = new SqlCommand(" UPDATE Orders SET " +

                        "  ProductID = @ProductID" +
                        " ,UserID = @UserID" +
                        " ,Price = @Price" +
                        " ,Quantidade = @Quantidade" +

                        " WHERE ID = @ID");

            command.Parameters.AddWithValue("ID", orders.ID.AsDbValue());
            command.Parameters.AddWithValue("ProductID", orders.ProductID.AsDbValue());
            command.Parameters.AddWithValue("UserID", orders.UserID.AsDbValue());
            command.Parameters.AddWithValue("Price", orders.Price.AsDbValue());
            command.Parameters.AddWithValue("Quantidade", orders.Quantidade.AsDbValue());

            _sqlHelper.ExecuteNonQuery(command);
        }
        catch
        {
            throw;
        }

        return orders;
    }

    public bool Delete(long id)
    {
        SqlCommand command;

        int result;

        try
        {
            command = new SqlCommand(" DELETE from Orders where ID = @ID ");

            command.Parameters.AddWithValue("ID", id.AsDbValue());
            result = _sqlHelper.ExecuteNonQuery(command);
        }
        catch
        {
            throw;
        }

        return result > 0;
    }

    #endregion

    #region Retrieve Data

    public Orders Get(long id)
    {
        SqlCommand command;
        DataSet dataSet;

        Orders orders;

        try
        {
            command = new SqlCommand(" SELECT * FROM Orders WHERE ID = @ID");
            command.Parameters.AddWithValue("ID", id.AsDbValue());

            dataSet = _sqlHelper.ExecuteDataSet(command);

            orders = Load(dataSet).FirstOrDefault();

        }
        catch
        {
            throw;
        }

        return orders;
    }

    public List<Orders> Get(long? productID = null, long? userID = null, decimal? price = null, int? quantidade = null)
    {
        SqlCommand commandCount;
        SqlCommand commandWhere;
        DataSet dataSet;

        List<string> clauses;
        List<Orders> orders;

            int count;

        try
        {
                orders = new List<Orders>();

            commandCount = new SqlCommand(" SELECT COUNT(DISTINCT A.ID) " +
                            " FROM Orders A ");

            commandWhere = new SqlCommand(" SELECT DISTINCT A.* " +
                            " FROM Orders A ");

            clauses = new List<string>();

            //Internal Columns
            if (productID.HasValue)
            {
                clauses.Add($"A.ProductID = @ProductID");
                commandCount.Parameters.AddWithValue($"ProductID", productID.AsDbValue());
                commandWhere.Parameters.AddWithValue($"ProductID", productID.AsDbValue());
            }
            if (userID.HasValue)
            {
                clauses.Add($"A.UserID = @UserID");
                commandCount.Parameters.AddWithValue($"UserID", userID.AsDbValue());
                commandWhere.Parameters.AddWithValue($"UserID", userID.AsDbValue());
            }
            if (price.HasValue)
            {
                clauses.Add($"A.Price = @Price");
                commandCount.Parameters.AddWithValue($"Price", price.AsDbValue());
                commandWhere.Parameters.AddWithValue($"Price", price.AsDbValue());
            }
            if (quantidade.HasValue)
            {
                clauses.Add($"A.Quantidade = @Quantidade");
                commandCount.Parameters.AddWithValue($"Quantidade", quantidade.AsDbValue());
                commandWhere.Parameters.AddWithValue($"Quantidade", quantidade.AsDbValue());
            }

            //Outer Columns

            if (clauses.Count > 0)
            {
                commandCount.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
                commandWhere.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
            }


            
            count = (int)_sqlHelper.ExecuteScalar(commandCount);

            dataSet = _sqlHelper.ExecuteDataSet(commandWhere);


                orders = Load(dataSet);
        }
        catch
        {
            throw;
        }

        return orders;
    }

    #endregion

}

}