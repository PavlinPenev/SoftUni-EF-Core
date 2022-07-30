namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using Artillery.DataProcessor.ImportDto;
    using AutoMapper;
    using System.Linq;

    class ArtilleryProfile : Profile
    {
        public ArtilleryProfile()
        {
            #region Export Mappings
            CreateMap<Country, ExportCountryDto>();
            CreateMap<Gun, ExportGunDto>()
                .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer.ManufacturerName))
                .ForMember(dest => dest.GunType, opt => opt.MapFrom(src => src.GunType.ToString()))
                .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => src.CountriesGuns.Select(cg => cg.Country)));
            #endregion
        }
    }
}