using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<JsonCarDto, Car>();

            CreateMap<Customer, CustomerSalesDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BoughtCars, opt => opt.MapFrom(src => src.Sales.Count))
                .ForMember(
                    dest => dest.SpentMoney,
                    opt => opt.MapFrom(
                        src => src.Sales.Sum(
                            s => s.Car.PartCars.Select(p => p.Part.Price).Sum())));
        }
    }
}
