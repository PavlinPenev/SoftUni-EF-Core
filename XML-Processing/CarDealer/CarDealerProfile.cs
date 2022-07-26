using AutoMapper;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Linq;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            #region Import Mappings
            CreateMap<ImportSupplierDto, Supplier>();
            CreateMap<ImportPartDto, Part>();
            CreateMap<ImportCarDto, Car>();
            CreateMap<ImportCustomerDto, Customer>();
            CreateMap<ImportSaleDto, Sale>();
            #endregion

            #region Export Mappings
            CreateMap<Car, ExportCarWithDistanceDto>();
            CreateMap<Car, ExportCarFromMakeBmwDto>();
            CreateMap<Supplier, ExportLocalSupplierDto>()
                .ForMember(dest => dest.PartsCount, opt => opt.MapFrom(src => src.Parts.Count()));
            CreateMap<Part, ExportPartDto>();
            CreateMap<Car, ExportCarWithListOfPartsDto>()
                .ForMember(dest => dest.Parts,
                    opt => opt.MapFrom(src => src.PartCars.Select(pc => pc.Part).OrderByDescending(p => p.Price)));
            CreateMap<Customer, ExportCustomerTotalSalesDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BoughtCars, opt => opt.MapFrom(src => src.Sales.Count()))
                .ForMember(
                    dest => dest.SpentMoney,
                    opt => opt.MapFrom(src => 
                        src.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))));
            CreateMap<Car, ExportCarForSalesDto>();
            CreateMap<Sale, ExportSalesWithAppliedDiscountDto>()
                .ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car))
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => src.Discount))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Car.PartCars.Sum(pc => pc.Part.Price)))
                .ForMember(dest => dest.PriceWithDiscount,
                    opt => opt.MapFrom(src =>
                        (src.Car.PartCars.Sum(pc => pc.Part.Price) - ((src.Car.PartCars.Sum(pc => pc.Part.Price) * (src.Discount / 100))))));
            #endregion
        }
    }
}
