using FluentValidation;
using System;
using System.Collections.Generic;

namespace Demo.API.Domain.Model
{
    public class Login
    {

            public string Email { get; set; }
            public string Senha { get; set; }
        }

        public class ValidatorLogin : AbstractValidator<Login>
        {
            public ValidatorLogin()
            {
                RuleFor(x => x.Email).Must(x => x.Length <= 100).WithMessage("Tamanho máximo excedido: 100");
                RuleFor(x => x.Senha).NotEmpty().WithMessage("Favor preencher o campo");
            }
        }
    }


