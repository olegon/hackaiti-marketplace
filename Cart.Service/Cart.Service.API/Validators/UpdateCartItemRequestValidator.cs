using Cart.Service.API.Models;
using FluentValidation;

namespace Cart.Service.API.Validators
{
    public class UpdateCartItemRequestValidator : AbstractValidator<UpdateCartItemRequest>
    {
        public UpdateCartItemRequestValidator()
        {
            RuleFor(t => t.SKU).NotEmpty();
        }
    }
}