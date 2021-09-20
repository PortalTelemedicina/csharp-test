using Demo.API.Domain.Repository;
using Demo.API.Domain.Model;
using System;
using System.Collections.Generic;

namespace Demo.API.Domain.Service
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        #region Change Data

        public Product Insert(Product product)
        {
            try
            {
                if (product.ID == 0)
                {
                    product = _productRepository.Insert(product);
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

            return product;
        }
        public Product Update(Product product)
        {
            try
            {
                if (product.ID == 0)
                {
                    throw new Exception("ID diferente de 0, avalie a utilização do POST");
                }
                else
                {
                    product = _productRepository.Update(product);
                }
            }
            catch
            {
                throw;
            }

            return product;
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
                    _productRepository.Delete(id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Retrieve Repository

        public Product Get(long id)
        {
            Product product;

            try
            {
                product = _productRepository.Get(id);
            }
            catch
            {
                throw;
            }

            return product;
        }

        public List<Product> Get(string nome = null, string descricao = null, decimal? price = null, DateTime? created = null, DateTime? modified = null)
        {
            List<Product> products;
            try
            {
                products = new List<Product>();
                products = _productRepository.Get(nome: nome, descricao: descricao, price: price, created: created, modified: modified);
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

