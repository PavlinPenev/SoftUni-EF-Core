using AutoMapper;
using BookShop.Data.Models;
using BookShop.DataProcessor.ExportDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BookShop.DataProcessor
{
    public class BookShopProfile : Profile
    {
        public BookShopProfile()
        {
            CreateMap<Book, ExportBookDto>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.PublishedOn.ToString("d", CultureInfo.InvariantCulture)));
        }
    }
}
