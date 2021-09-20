using Demo.API.Domain.Repository;
using Demo.API.Domain.Model;
using System;
using System.Collections.Generic;
using Demo.Domain.Repository;
using Demo.Domain.Model;

namespace Demo.API.Domain.Service
{
    public class OrderService
    {
        private readonly OrdersRepository _ordersRepository;

        public OrderService(OrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        #region Change Data

        public Orders Insert(Orders orders)
        {
            try
            {
                if (orders.ID == 0)
                {
                    orders = _ordersRepository.Insert(orders);
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

            return orders;
        }
        public Orders Update(Orders orders)
        {
            try
            {
                if (orders.ID == 0)
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do POST");
                }
                else
                {
                    orders = _ordersRepository.Update(orders);
                }
            }
            catch
            {
                throw;
            }

            return orders;
        }

        public void Delete(long id)
        {
            try
            {
                if (id == 0)
                {
                    throw new Exception("ID inválido");
                }
                else
                {
                    _ordersRepository.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieve Repository

        public Orders Get(long id)
        {
            Orders orders;

            try
            {
                orders = _ordersRepository.Get(id);
            }
            catch
            {
                throw;
            }

            return orders;
        }

        public List<Orders> Get(long? productID = null, long? userID = null, decimal? price = null, int? quantidade = null)
        {
            List<Orders> Orders;
            try
            {
                Orders = new List<Orders>();
                Orders = _ordersRepository.Get(productID: productID, userID: userID, price: price, quantidade: quantidade);
            }
            catch
            {
                throw;
            }

            return Orders;
        }

        #endregion
    }
}
