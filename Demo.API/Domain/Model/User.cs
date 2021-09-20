using FluentValidation;
using System;
using System.Collections.Generic;

namespace Demo.API.Domain.Model
{
    public class User
    {
        #region Data Base Parse

        //(Optional, used to parse Db Column Names against Model Property Names)
        public static readonly Dictionary<string, string> ColumnsLibrary = new Dictionary<string, string>
        {
            { "id", "ID"},
            { "nome", "Nome"},
            { "email", "Email"},
            { "senha", "Senha"},
            { "perfil", "Perfil"}
        };

        #endregion

            public long ID { get; set; }
            public string Nome { get; set; }
            public string Email { get; set; }
            public string Senha { get; set; }
            public Perfil Perfil { get; set; }
            public DateTime? Created { get; set; }
            public DateTime? Modified { get; set; }
            public DateTime? Last_Login { get; set; }
    }
            public enum Perfil
            { 
                ADM = 1,
                PDV = 2,
                FINANCEIRO = 3
            }

        public class ValidatorUsers : AbstractValidator<User>
        {
            public ValidatorUsers()
            {
                RuleFor(x => x.Nome).NotEmpty().WithMessage("Favor preencher o campo");
                RuleFor(x => x.Nome).Must(x => x.Length <= 150).WithMessage("Tamanho máximo excedido: 150");
                RuleFor(x => x.Email).Must(x => x.Length <= 100).WithMessage("Tamanho máximo excedido: 100");
                RuleFor(x => x.Senha).NotEmpty().WithMessage("Favor preencher o campo");
                RuleFor(x => x.Senha).Must(x => x.Length <= 50).WithMessage("Tamanho máximo excedido: 50");
                RuleFor(x => x.Perfil).NotEmpty().WithMessage("Favor preencher o campo");
                RuleFor(x => x.Created).NotEmpty().WithMessage("Favor preencher o campo");
                RuleFor(x => x.Last_Login).NotEmpty().WithMessage("Favor preencher o campo");
        }
        }
    }


