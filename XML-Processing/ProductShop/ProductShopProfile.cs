using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            #region Import Dtos
            CreateMap<UserDto, User>();
            CreateMap<ProductDto, Product>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CategoryProductDto, CategoryProduct>();
            #endregion

            #region Export Dtos
            CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(dest => dest.BuyerFullName, 
                    opt => opt.MapFrom(src => $"{src.Buyer.FirstName} {src.Buyer.LastName}"));
            CreateMap<Product, ExportProductDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => $"{src.Price.ToString("0.##")}"));
            CreateMap<Product, ExportProductDtoV2>();
            CreateMap<User, ExportUserSoldProductsDto>()
                .ForMember(dest => dest.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));
            CreateMap<Category, ExportCategoryByProductsCountDto>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(dest => dest.AveragePrice, 
                    opt => opt.MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(dest => dest.TotalRevenue, 
                    opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));
            CreateMap<ICollection<Product>, ExportSoldProductsDto>()
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src));
            CreateMap<User, ExportUserWithProductsDto>()
                .ForMember(dest => dest.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));
            #endregion
        }
    }
}
