using Cart.Service.API.Models;
using FluentValidation;

namespace Cart.Service.API.Validators
{
    public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
    {
        public CreateCartRequestValidator()
        {
            RuleFor(t => t.CustomerID).NotEmpty();
            RuleFor(t => t.Item).NotNull().SetValidator(new CartItemValidator());
        }

        public class CartItemValidator : AbstractValidator<CreateCartRequest.CartItem>
        {   
            public CartItemValidator()
            {
                RuleFor(t => t.Quantity).GreaterThan(0);
                RuleFor(t => t.SKU).NotEmpty();
            }
        }
    }
}