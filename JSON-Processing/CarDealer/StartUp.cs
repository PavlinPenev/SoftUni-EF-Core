using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static IMapper mapper;

        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();

            ResetDatabase(dbContext);

            Console.WriteLine(GetSalesWithAppliedDiscount(dbContext));
        }

        private static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Seed(context);
        }

        private static void Seed(CarDealerContext context)
        {
            #region Prepare Jsons
            var suppliersJson = File.ReadAllText("suppliers.json");
            var partsJson = File.ReadAllText("parts.json");
            var carsJson = File.ReadAllText("cars.json");
            var customersJson = File.ReadAllText("customers.json");
            var salesJson = File.ReadAllText("sales.json");
            #endregion

            ImportSuppliers(context, suppliersJson);
            ImportParts(context, partsJson);
            ImportCars(context, carsJson);
            ImportCustomers(context, customersJson);
            ImportSales(context, salesJson);
        }

        #region Import Data
        public static string ImportSuppliers(CarDealerContext context, string suppliersJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(suppliersJson);

            context.Suppliers.AddRange(suppliers);

            context.SaveChanges();

            var suppliersCount = suppliers.Count();

            return String.Format(Constants.SuccessfullyImported, suppliersCount);
        }

        public static string ImportParts(CarDealerContext context, string partsJson)
        {
            var supplierIds = context.Suppliers.Select(x => x.Id).ToList();

            var parts = JsonConvert.DeserializeObject<List<Part>>(partsJson)
                .Where(p => supplierIds.Contains(p.SupplierId))
                .ToList();

            context.Parts.AddRange(parts);

            context.SaveChanges();

            var partsCount = parts.Count();

            return String.Format(Constants.SuccessfullyImported, partsCount);
        }

        public static string ImportCars(CarDealerContext context, string carsJson)
        {
            GenerateMapper();

            var jsonCars = JsonConvert.DeserializeObject<List<JsonCarDto>>(carsJson);

            var existingPartsId = context.Parts.Select(x => x.Id).ToHashSet();

            var cars = new List<Car>();

            foreach (var jsonCar in jsonCars)
            {
                var car = mapper.Map<Car>(jsonCar);

                foreach (var part in jsonCar.PartsId.Distinct())
                {
                    if (existingPartsId.Contains(part))
                    {
                        var currentCarPart = new PartCar
                        {
                            CarId = car.Id,
                            PartId = part
                        };

                        car.PartCars.Add(currentCarPart);
                    }
                }

                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();

            var carsCount = cars.Count();

            return String.Format(Constants.SuccessfullyImported, carsCount);
        }

        public static string ImportCustomers(CarDealerContext context, string customersJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(customersJson);

            context.Customers.AddRange(customers);

            context.SaveChanges();

            var customersCount = customers.Count();

            return String.Format(Constants.SuccessfullyImported, customersCount);
        }

        public static string ImportSales(CarDealerContext context, string salesJson)
        {
            var sales = JsonConvert.DeserializeObject<List<Sale>>(salesJson);

            context.Sales.AddRange(sales);

            context.SaveChanges();

            var salesCount = sales.Count();

            return String.Format(Constants.SuccessfullyImported, salesCount);
        }
        #endregion

        #region Query Tasks
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                                .OrderBy(c => c.BirthDate)
                                .ThenBy(c => c.IsYoungDriver)
                                .Select(c => new
                                {
                                    c.Name,
                                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    c.IsYoungDriver
                                })
                                .ToList();

             var jsonResult = JsonConvert.SerializeObject(customers, GetJsonSettingsNoCasing());

            return jsonResult;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                        .ToList()
                        .Where(c => c.Make.Contains("Toyota", StringComparison.InvariantCultureIgnoreCase))
                        .OrderBy(c => c.Model)
                        .ThenByDescending(c => c.TravelledDistance)
                        .Select(c => new
                        {
                            c.Id,
                            c.Make,
                            c.Model,
                            c.TravelledDistance
                        })
                        .ToList();

            var jsonResult = JsonConvert.SerializeObject(cars, GetJsonSettingsNoCasing());

            return jsonResult;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                                .Where(s => !s.IsImporter)
                                .Select(s => new
                                {
                                    s.Id,
                                    s.Name,
                                    PartsCount = s.Parts.Count
                                })
                                .ToList();

            var jsonResult = JsonConvert.SerializeObject(suppliers, GetJsonSettingsNoCasing());

            return jsonResult;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                        .Select(cd => new
                        {
                            car = new
                            {
                                cd.Make,
                                cd.Model,
                                cd.TravelledDistance
                            },
                            parts = cd.PartCars.Select(p => new
                            {
                                p.Part.Name,
                                Price = $"{p.Part.Price:f2}"
                            })
                        })
                        .ToList();

            var jsonResult = JsonConvert.SerializeObject(cars, GetJsonSettingsNoCasing());

            return jsonResult;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            GenerateMapper();

            var customers = context.Customers
                                .Where(c => c.Sales.Any())
                                .ProjectTo<CustomerSalesDto>(mapper.ConfigurationProvider)
                                .OrderByDescending(cs => cs.SpentMoney)
                                .ThenByDescending(cs => cs.BoughtCars);

            var jsonResult = JsonConvert.SerializeObject(customers, GetJsonSettingsCamelCaseWithNullValueHandling());

            return jsonResult;
        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                        .Take(10)
                        .Select(s => new
                        {
                            car = new
                            {
                                s.Car.Make,
                                s.Car.Model,
                                s.Car.TravelledDistance
                            },
                            customerName = s.Customer.Name,
                            Discount = $"{s.Discount:f2}",
                            price = $"{s.Car.PartCars.Sum(pc => pc.Part.Price):f2}",
                            priceWithDiscount = $"{s.Car.PartCars.Sum(pc => pc.Part.Price) - ((s.Car.PartCars.Sum(pc => pc.Part.Price) * s.Discount) / 100):f2}"
                        })
                        .ToList();

            var jsonResult = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return jsonResult;
        }
        #endregion

        #region Configurations
        private static void GenerateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cnfg =>
            {
                cnfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }

        private static JsonSerializerSettings GetJsonSettingsCamelCaseWithNullValueHandling()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        private static JsonSerializerSettings GetJsonSettingsCamelCase()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }

        private static JsonSerializerSettings GetJsonSettingsNoCasing()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };
        }
        #endregion
    }
}