using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Demo.Domain.Model
{
	public class Orders
{
    public long ID { get; set; }
    public long ProductID { get; set; }
    public long UserID { get; set; }
    public decimal? Price { get; set; }
    public int? Quantidade { get; set; }
}

public class ValidatorOrders : AbstractValidator<Orders>
{
    public ValidatorOrders()
    {
        RuleFor(x => x.ProductID).NotEmpty().WithMessage("Favor preencher o campo");
        RuleFor(x => x.UserID).NotEmpty().WithMessage("Favor preencher o campo");
    }
}
}