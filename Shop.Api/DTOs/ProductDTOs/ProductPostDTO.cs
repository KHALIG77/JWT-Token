using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.DTOs.ProductDTOs
{
    
    public class ProductPostDTO
    {
        
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
       
       
    }
    public class ProductPostDTOValidation:AbstractValidator<ProductPostDTO>

    {
        public ProductPostDTOValidation()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20).MinimumLength(5);
            RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(x => x.CostPrice).NotEmpty();
            RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0).NotEmpty();
            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.DiscountPercent > 0)
                {
                    var price = x.SalePrice * (100 - x.DiscountPercent) / 100;
                    if(x.CostPrice> price)
                    {
                        context.AddFailure(nameof(x.DiscountPercent), "DiscountPercent incorrect");
                    }

                }
            });

        }

    }
}
