using Cart.Service.API.Models;
using FluentValidation;

namespace Cart.Service.API.Validators
{
    public class CartCheckoutRequestValidator : AbstractValidator<CartCheckoutRequest>
    {
        public CartCheckoutRequestValidator()
        {
            RuleFor(t => t.CurrencyCode).NotEmpty()
                .Must(value => value.ToUpper() == "BRL" || value.ToUpper() == "EUR" || value.ToUpper() == "USD")
                .WithMessage("It should be BRL, EUR or USD");
        }
    }
}