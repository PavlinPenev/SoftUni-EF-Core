namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using AutoMapper;
    using Newtonsoft.Json;

    public class Deserializer
    {

        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var countryDtos = XmlDeserializer<List<ImportCountryDto>>(Constants.COUNTRIES, xmlString).ToList();

            var countries = new HashSet<Country>();

            var sb = new StringBuilder();

            foreach (var country in countryDtos)
            {
                var currentCountry = new Country
                {
                    CountryName = country.CountryName,
                    ArmySize = country.ArmySize
                };

                if (!IsValid(currentCountry))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                countries.Add(currentCountry);
                sb.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var manufacturerDtos = XmlDeserializer<List<ImportManufacturerDto>>(Constants.MANUFACTURERS, xmlString);

            var manufacturers = new HashSet<Manufacturer>();

            var sb = new StringBuilder();

            var manufacturerNamesImported = new List<string>();

            foreach (var manufacturer in manufacturerDtos)
            {
                var currentManufacturer = new Manufacturer
                {
                    ManufacturerName = manufacturer.ManufacturerName,
                    Founded = manufacturer.Founded
                };

                if (!IsValid(currentManufacturer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (manufacturerNamesImported.Contains(manufacturer.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                manufacturerNamesImported.Add(manufacturer.ManufacturerName);

                var townCountry = manufacturer.Founded.Split(", ").TakeLast(2).ToList();

                var townName = townCountry[0];

                var countryName = townCountry[1];

                manufacturers.Add(currentManufacturer);
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, $"{townName}, {countryName}")); ;
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var shellDtos = XmlDeserializer<List<ImportShellDto>>(Constants.SHELLS, xmlString);

            var shells = new HashSet<Shell>();

            var sb = new StringBuilder();

            foreach (var shell in shellDtos)
            {
                var currentShell = new Shell
                {
                    ShellWeight = shell.ShellWeight,
                    Caliber = shell.Caliber
                };

                if (!IsValid(currentShell))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                shells.Add(currentShell);
                sb.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var jsonGuns = JsonConvert.DeserializeObject<List<ImportGunDto>>(jsonString);

            var guns = new HashSet<Gun>();

            var sb = new StringBuilder();

            foreach (var gun in jsonGuns)
            {
                var isValidGunType = Enum.TryParse<GunType>(gun.GunType, out GunType gunType);

                if (!isValidGunType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var currentGun = new Gun
                {
                    ManufacturerId = gun.ManufacturerId,
                    GunWeight = gun.GunWeight,
                    BarrelLength = gun.BarrelLength,
                    NumberBuild = gun.NumberBuild,
                    Range = gun.Range,
                    GunType = gunType,
                    ShellId = gun.ShellId
                };

                foreach (var countryId in gun.Countries.Select(c => c.Id).ToHashSet())
                {
                    currentGun.CountriesGuns.Add(new CountryGun
                    {
                        Gun = currentGun,
                        CountryId = countryId
                    });
                }

                if (!IsValid(currentGun))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                guns.Add(currentGun);
                sb.AppendLine(String.Format(SuccessfulImportGun, currentGun.GunType, currentGun.GunWeight, currentGun.BarrelLength));
            }

            context.AddRange(guns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

        #region Deserializer Methods
        private static T XmlDeserializer<T>(string rootTag, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootTag);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            T dtos;

            using (StringReader reader = new StringReader(inputXml))
            {
                dtos = (T)serializer.Deserialize(reader);
            }

            return dtos;
        }
        #endregion
    }
}
