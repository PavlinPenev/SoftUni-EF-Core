
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using AutoMapper;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data.Models.Enums;
    using Newtonsoft.Json;
    using Artillery.DataProcessor.ExportDto;
    using AutoMapper.QueryableExtensions;
    using System.Collections.Generic;

    public class Serializer
    {
        private static IMapper mapper;

        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .ToList()
                .Select(s => new ExportShellDto
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns.Select(g => new ExportShellGunDto
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    }).Where(g => g.GunType == GunType.AntiAircraftGun.ToString())
                    .OrderByDescending(g => g.GunWeight)
                    .ToList()
                }).ToList().OrderBy(s => s.ShellWeight);

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            GenerateMapper();

            var guns = context.Guns
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .ProjectTo<ExportGunDto>(mapper.ConfigurationProvider)
                .ToList()
                .OrderBy(g => g.BarrelLength)
                .ToList();

            foreach (var gun in guns)
            {
                gun.Countries = gun.Countries
                    .Where(c => c.ArmySize > 4500000)
                    .ToList()
                    .OrderBy(c => c.ArmySize)
                    .ToList();
            }

            return XmlSerializer<List<ExportGunDto>>(guns, Constants.GUNS);
        }

        #region Serializer Methods
        private static string XmlSerializer<T>(T dto, string rootTag)
        {
            var sb = new StringBuilder();

            var root = new XmlRootAttribute(rootTag);
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var serializer = new XmlSerializer(typeof(T), root);

            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, dto, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        private static void GenerateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cnfg =>
            {
                cnfg.AddProfile<ArtilleryProfile>();
            });

            mapper = config.CreateMapper();
        }
        #endregion
    }
}
