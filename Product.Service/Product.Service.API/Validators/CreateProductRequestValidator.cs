using FluentValidation;
using Product.Service.API.Model;

namespace Product.Service.API.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(t => t.ImageURL).NotEmpty();
            RuleFor(t => t.ShortDescription).NotEmpty();
            RuleFor(t => t.Name).NotEmpty();
            RuleFor(t => t.Price).NotNull().SetValidator(new ProductPriceValidator());
            RuleFor(t => t.SKU).NotEmpty();
        }

        public class ProductPriceValidator : AbstractValidator<CreateProductRequest.ProductPrice>
        {
            public ProductPriceValidator()
            {
                RuleFor(t => t.Amount).GreaterThan(0);
                RuleFor(t => t.Scale).GreaterThanOrEqualTo(0);
                RuleFor(t => t.CurrencyCode).NotEmpty()
                    .Must(value => value.ToUpper() == "BRL" || value.ToUpper() == "EUR" || value.ToUpper() == "USD")
                    .WithMessage("It should be BRL, EUR or USD");
            }
        }
    }
}