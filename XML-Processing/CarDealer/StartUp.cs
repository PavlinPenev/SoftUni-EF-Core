using AutoMapper;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            var dbContext = new CarDealerContext();

            ResetDatabase(dbContext);

            Console.WriteLine();
        }

        public static void ResetDatabase(CarDealerContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Seed(context);
        }

        public static void Seed(CarDealerContext context)
        {
            #region PrepareXMLs
            var carsXmlString = File.ReadAllText("Datasets/cars.xml");
            var customersXmlString = File.ReadAllText("Datasets/customers.xml");
            var partsXmlString = File.ReadAllText("Datasets/parts.xml");
            var salesXmlString = File.ReadAllText("Datasets/sales.xml");
            var suppliersXmlString = File.ReadAllText("Datasets/suppliers.xml");
            #endregion

            ImportSuppliers(context, suppliersXmlString);
            ImportParts(context, partsXmlString);
            ImportCars(context, carsXmlString);
            ImportCustomers(context, customersXmlString);
            ImportSales(context, salesXmlString);
        }

        #region Import Data
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            GenerateMapper(); 

            var suppliersDto = Deserializer<List<ImportSupplierDto>>(Constants.Suppliers, inputXml);

            var suppliers = mapper.Map<List<Supplier>>(suppliersDto);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return String.Format(Constants.Notifications.SuccessfullyImported, suppliers.Count());
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            GenerateMapper();

            var partsDtos = Deserializer<List<ImportPartDto>>(Constants.Parts, inputXml);
        
            var parts = mapper.Map<List<Part>>(partsDtos)
                .Where(p => context.Suppliers.Find(p.SupplierId) != null);

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return String.Format(Constants.Notifications.SuccessfullyImported, parts.Count());
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            GenerateMapper();

            var carsDtos = Deserializer<List<ImportCarDto>>(Constants.Cars, inputXml);

            var existingPartsIds = context.Parts.Select(p => p.Id).ToList();

            var cars = new List<Car>();

            foreach (var car in carsDtos)
            {
                var currentCar = mapper.Map<Car>(car);

                foreach (var part in car.PartsIds.Distinct())
                {
                    if (existingPartsIds.Contains(part.PartId)
                        && !currentCar.PartCars.Any(pc => pc.PartId == part.PartId))
                    {
                        var currentCarPart = new PartCar
                        {
                            PartId = part.PartId,
                            CarId = currentCar.Id
                        };

                        currentCar.PartCars.Add(currentCarPart);
                    }
                }

                cars.Add(currentCar);
            }

            context.AddRange(cars);
            context.SaveChanges();

            return String.Format(Constants.Notifications.SuccessfullyImported, cars.Count());
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            GenerateMapper();

            var customersDtos = Deserializer<List<ImportCustomerDto>>(Constants.Customers, inputXml);

            var customers = mapper.Map<List<Customer>>(customersDtos);

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return String.Format(Constants.Notifications.SuccessfullyImported, customers.Count());
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            GenerateMapper();

            var salesDtos = Deserializer<List<ImportSaleDto>>(Constants.Sales, inputXml);

            var sales = mapper.Map<List<Sale>>(salesDtos)
                .Where(s => context.Cars.Any(c => c.Id == s.CarId));

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return String.Format(Constants.Notifications.SuccessfullyImported, sales.Count());
        }
        #endregion

        #region Query Tasks

        #endregion

        #region Private Methods
        private static T Deserializer<T>(string rootTag, string inputXml)
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

        private static string Serializer<T>(T dto, string rootTag)
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
                cnfg.AddProfile<CarDealerProfile>();
            });

            mapper = config.CreateMapper();
        }
        #endregion
    }
}