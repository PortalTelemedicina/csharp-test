using Demo.API.Domain.Model;
using Demo.DataExtensions.Sql;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Domain.Repository
{
    public class ProductRepository
    {
        private readonly IConfiguration _config;
        private readonly ISqlHelper _sqlHelper;

        public ProductRepository(IConfiguration configuration, ISqlHelper sqlHelper)
        {
            _config = configuration;
            _sqlHelper = sqlHelper;
        }

        #region LoadModel

        private List<Product> Load(DataSet data)
        {
            List<Product> products;
            Product product;

            try
            {
                products = new List<Product>();

                foreach (DataRow row in data.Tables[0].Rows)
                {
                    product = new Product();

                    product.ID = row.Field<long>("ID");
                    product.Nome = row.Field<string>("Nome");
                    product.Descricao = row.Field<string>("Descricao");
                    product.Price = row.Field<decimal?>("Price");
                    product.Created = row.Field<DateTime?>("Created");
                    product.Modified = row.Field<DateTime?>("Modified");

                    products.Add(product);
                }
            }
            catch
            {
                throw;
            }

            return products;
        }

        #endregion

        #region Change Data

        public Product Insert(Product product)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand(" INSERT INTO Product " +
                            " (" +
                                "  Nome" +
                                " ,Descricao" +
                                " ,Price" +
                                " ,Created" +
                                " ,Modified" +
                            " )" +
                            " OUTPUT inserted.ID " +
                            " VALUES " +
                            " (" +
                                "  @Nome" +
                                " ,@Descricao" +
                                " ,@Price" +
                                " ,@Created" +
                                " ,@Modified" +
                            " )");

                command.Parameters.AddWithValue("Nome", product.Nome.AsDbValue());
                command.Parameters.AddWithValue("Descricao", product.Descricao.AsDbValue());
                command.Parameters.AddWithValue("Price", product.Price.AsDbValue());
                command.Parameters.AddWithValue("Created", product.Created.AsDbValue());
                command.Parameters.AddWithValue("Modified", product.Modified.AsDbValue());

                product.ID = (long)_sqlHelper.ExecuteScalar(command);
            }
            catch
            {
                throw;
            }

            return product;
        }

        public Product Update(Product product)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand(" UPDATE Product SET " +

                            "  Nome = @Nome" +
                            " ,Descricao = @Descricao" +
                            " ,Price = @Price" +
                            " ,Created = @Created" +
                            " ,Modified = @Modified" +

                            " WHERE ID = @ID");

                command.Parameters.AddWithValue("ID", product.ID.AsDbValue());
                command.Parameters.AddWithValue("Nome", product.Nome.AsDbValue());
                command.Parameters.AddWithValue("Descricao", product.Descricao.AsDbValue());
                command.Parameters.AddWithValue("Price", product.Price.AsDbValue());
                command.Parameters.AddWithValue("Created", product.Created.AsDbValue());
                command.Parameters.AddWithValue("Modified", product.Modified.AsDbValue());

                _sqlHelper.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }

            return product;
        }

        public bool Delete(long id)
        {
            SqlCommand command;

            int result;

            try
            {
                command = new SqlCommand(" DELETE from Product where ID = @ID ");

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

        public Product Get(long id)
        {
            SqlCommand command;
            DataSet dataSet;

            Product product;

            try
            {
                command = new SqlCommand(" SELECT * FROM Product WHERE ID = @ID");
                command.Parameters.AddWithValue("ID", id.AsDbValue());

                dataSet = _sqlHelper.ExecuteDataSet(command);

                product = Load(dataSet).FirstOrDefault();

            }
            catch
            {
                throw;
            }

            return product;
        }

        public List<Product> Get(string nome = null, string descricao = null, decimal? price = null, DateTime? created = null, DateTime? modified = null)
        {
            SqlCommand commandCount;
            SqlCommand commandWhere;
            DataSet dataSet;

            List<string> clauses;
            List<Product> products;

            int count;

            try
            {
                products= new List<Product>();

                commandCount = new SqlCommand(" SELECT COUNT(DISTINCT A.ID) " +
                                " FROM Product A ");

                commandWhere = new SqlCommand(" SELECT DISTINCT A.* " +
                                " FROM Product A ");

                clauses = new List<string>();

                //Internal Columns
                if (!string.IsNullOrEmpty(nome))
                {
                    clauses.Add($"A.Nome LIKE '%' + @Nome + '%'");
                    commandCount.Parameters.AddWithValue($"Nome", nome.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Nome", nome.AsDbValue());
                }
                if (!string.IsNullOrEmpty(descricao))
                {
                    clauses.Add($"A.Descricao LIKE '%' + @Descricao + '%'");
                    commandCount.Parameters.AddWithValue($"Descricao", descricao.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Descricao", descricao.AsDbValue());
                }
                if (price.HasValue)
                {
                    clauses.Add($"A.Price = @Price");
                    commandCount.Parameters.AddWithValue($"Price", price.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Price", price.AsDbValue());
                }
                if (created.HasValue)
                {
                    clauses.Add($"A.Created = @Created");
                    commandCount.Parameters.AddWithValue($"Created", created.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Created", created.AsDbValue());
                }
                if (modified.HasValue)
                {
                    clauses.Add($"A.Modified = @Modified");
                    commandCount.Parameters.AddWithValue($"Modified", modified.AsDbValue());
                    commandWhere.Parameters.AddWithValue($"Modified", modified.AsDbValue());
                }

                //Outer Columns

                if (clauses.Count > 0)
                {
                    commandCount.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
                    commandWhere.CommandText += $" WHERE { string.Join(" AND ", clauses)}";
                }

                

                count = (int)_sqlHelper.ExecuteScalar(commandCount);

                dataSet = _sqlHelper.ExecuteDataSet(commandWhere);

                products = Load(dataSet);
            }
            catch
            {
                throw;
            }

            return products;
        }

        #endregion

    }

}
