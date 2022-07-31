namespace Theatre
{
    using AutoMapper;
    using System;
    using System.Globalization;
    using System.Linq;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;
    using Theatre.DataProcessor.ImportDto;

    class TheatreProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public TheatreProfile()
        {
            CreateMap<Cast, ExportCastDto>();
            CreateMap<Play, ExportPlayDto>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString("c", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.ToString()))
                .ForMember(dest => dest.Actors, opt => opt.MapFrom(src => src.Casts.Where(c => c.IsMainCharacter).ToList()))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating == 0 ? "Premier" : src.Rating.ToString()));
        }
    }
}
