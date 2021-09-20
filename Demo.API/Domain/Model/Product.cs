using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Domain.Model
{
    public class Product
    {
        public long ID { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }

    public class ValidatorProduct : AbstractValidator<Product>
    {
        public ValidatorProduct()
        {
            RuleFor(x => x.Nome).Must(x => x.Length <= 50).WithMessage("Tamanho máximo excedido: 50");
            RuleFor(x => x.Descricao).Must(x => x.Length <= 100).WithMessage("Tamanho máximo excedido: 100");
        }
    }
}
