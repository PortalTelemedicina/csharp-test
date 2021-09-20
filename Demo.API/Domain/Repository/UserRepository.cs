using Demo.API.Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Demo.DataExtensions.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Demo.API.Domain.Repository
{
    public class UsersRepository
    {
        private readonly IConfiguration _config;
        private readonly ISqlHelper _sqlHelper;

        public UsersRepository(IConfiguration configuration, ISqlHelper sqlHelper)
        {
            _config = configuration;
            _sqlHelper = sqlHelper;
        }

        #region LoadModel

        private List<User> Load(DataSet data)
        {
            List<User> users;
            User user;

            try
            {
                users = new List<User>();

                foreach (DataRow row in data.Tables[0].Rows)
                {
                    user = new User();

                    user.ID = row.Field<long>("ID");
                    user.Nome = row.Field<string>("Nome");
                    user.Email = row.Field<string>("Email");
                    user.Senha = row.Field<string>("Senha");
                    user.Perfil = row.Field<Perfil>("Perfil");
                    user.Created = row.Field<DateTime>("Created");
                    user.Modified = row.Field<DateTime>("Modified");
                    user.Last_Login = row.Field<DateTime?>("Last_Login");

                    users.Add(user);
                }
            }
            catch
            {
                throw;
            }

            return users;
        }

        #endregion

        #region Change Data

        public User Insert(User user)
        {
            SqlCommand command;

            try
            {
                command = new SqlCommand(" INSERT INTO Users " +
                            " (" +
                                "  Nome" +
                                " ,Email" +
                                " ,Senha" +
                                " ,Perfil" +
                                " ,Created" +
                                " ,Modified" +
                            " )" +
                            " OUTPUT inserted.ID " +
                            " VALUES " +
                            " (" +
                                "  @Nome" +
                                " ,@Email" +
                                " ,@Senha" +
                                " ,@Perfil" +
                                " ,@Created" +
                                " ,@Modified" +
                            " )");

                command.Parameters.AddWithValue("Nome", user.Nome.AsDbValue());
                command.Parameters.AddWithValue("Email", user.Email.AsDbValue());
                command.Parameters.AddWithValue("Senha", user.Senha.AsDbValue());
                command.Parameters.AddWithValue("Perfil", user.Perfil.AsDbValue());
                command.Parameters.AddWithValue("Created", DateTime.Now);
                command.Parameters.AddWithValue("Modified", DateTime.Now);
                user.Last_Login = null;
                user.ID = (long)_sqlHelper.ExecuteScalar(command);
            }
            catch
            {
                throw;
            }

            return user;
        }

        public User Login(Login login)
        {
            SqlCommand command;
            User user;

            try
            {
                user = new User();
                user = Get(login);
                if (user!=null)
                {
                    command = new SqlCommand(" UPDATE Users SET " +

                               "  Nome = @Nome" +
                               " ,Email = @Email" +
                               " ,Senha = @Senha" +
                               " ,Perfil = @Perfil" +
                               " ,Created = @Created" +
                               " ,Modified = @Modified" +
                               " ,Last_Login = @Last_Login" +

                               " WHERE ID = @ID");

                    command.Parameters.AddWithValue("ID", user.ID.AsDbValue());
                    command.Parameters.AddWithValue("Nome", user.Nome.AsDbValue());
                    command.Parameters.AddWithValue("Email", user.Email.AsDbValue());
                    command.Parameters.AddWithValue("Senha", user.Senha.AsDbValue());
                    command.Parameters.AddWithValue("Perfil", user.Perfil.AsDbValue());
                    command.Parameters.AddWithValue("Created", user.Created.AsDbValue());
                    command.Parameters.AddWithValue("Modified", DateTime.Now);
                    command.Parameters.AddWithValue("Last_login", DateTime.Now);

                    _sqlHelper.ExecuteNonQuery(command);

                }
                else
                {
                    throw new Exception( "Email e/ou senha invalidos");
                }
            }
            catch
            {
                throw;
            }

            return user;
        }





        public User Get(Login login)
        {
            SqlCommand command;
            DataSet dataSet;

            User user;

            try
            {
                command = new SqlCommand(" SELECT * FROM Users WHERE Email = @Email and Senha = @Senha");
                command.Parameters.AddWithValue("Email", login.Email);
                command.Parameters.AddWithValue("Senha", login.Senha);

                dataSet = _sqlHelper.ExecuteDataSet(command);

                user = Load(dataSet).FirstOrDefault();

            }
            catch
            {
                throw;
            }

            return user;
        }


        #endregion

    }
}
