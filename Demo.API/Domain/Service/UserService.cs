using Demo.API.Domain.Repository;
using Demo.API.Domain.Model;
using System;
using System.Collections.Generic;

namespace Demo.API.Domain.Service
{
    public class UserService
    {
        private readonly UsersRepository _userRepository;

        public UserService(UsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region Change Data

        public User Insert(User user)
        {
            try
            {
                if (user.ID == 0)
                {
                    user = _userRepository.Insert(user);
                }
                else
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do PUT");
                }
            }
            catch
            {
                throw;
            }

            return user;
        }
        public User Login(Login login)
        {
            User user;
            try
            {
                user = new User();
                if (!string.IsNullOrEmpty(login.Email))
                {
                    user = _userRepository.Login(login);
                }
                else
                {
                    throw new Exception("Insira o email por favor");
                }
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
