using AutoMapper;
using Shop.Api.DTOs.BrandDTOs;
using Shop.Api.DTOs.ProductDTOs;
using Shop.Core.Entities;

namespace Shop.Api.Profiles
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Product, ProductGetDTO>();
            CreateMap<ProductPostDTO, Product>();
            CreateMap<BrandPostDTO, Brand>();
            //Productdan ProoductgetAllItem yaradanda icindeki spesifik bir propa menimsetme elemek ucun member islenir.
            //Birinci hissesinde yaranacah obyektin hansi propertisi secilir o gosderilir, sorada ikinci hissede hansi obyetkden yaradilirsa o obyetktin propunun sertine uygun beraberlesir
            CreateMap<Product,ProductGetAllItemDTO>().ForMember(dest=>dest.HasDiscount,m=>m.MapFrom(s=>s.DiscountPercent>0));
            

            CreateMap<Brand, BrandGetAllItemDTO>();
            CreateMap<Brand,BrandInProductGetDTO>();
        }
    }
}
