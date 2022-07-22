using AutoMapper;
using AutoMapper.QueryableExtensions;
using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;

        public static void Main(string[] args)
        {
            var dbContext = new ProductShopContext();

            ResetDatabase(dbContext);

            Console.WriteLine(GetUsersWithProducts(dbContext));
        }

        public static void ResetDatabase(ProductShopContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Seed(context);
        }

        public static void Seed(ProductShopContext context)
        {
            #region PrepareXMLs
            var userXmlString = File.ReadAllText("Datasets/users.xml");
            var productsXmlString = File.ReadAllText("Datasets/products.xml");
            var categoriesXmlString = File.ReadAllText("Datasets/categories.xml");
            var categoriesProductsXmlString = File.ReadAllText("Datasets/categories-products.xml");
            #endregion

            Console.WriteLine(ImportUsers(context, userXmlString));
            Console.WriteLine(ImportProducts(context, productsXmlString));
            Console.WriteLine(ImportCategories(context, categoriesXmlString));
            Console.WriteLine(ImportCategoryProducts(context, categoriesProductsXmlString));
        }

        #region Import Data
        public static string ImportUsers(ProductShopContext context, string xmlInput) 
        {
            var userDtos = Deserializer<List<UserDto>>(Constants.Users, xmlInput);

            GenerateMapper();

            var users = mapper.Map<List<User>>(userDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, users.Count());
        }

        public static string ImportProducts(ProductShopContext context, string xmlInput)
        {
            var productDtos = Deserializer<List<ProductDto>>(Constants.Products, xmlInput);

            GenerateMapper();

            var products = mapper.Map<List<Product>>(productDtos);

            context.AddRange(products);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, products.Count());
        }

        public static string ImportCategories(ProductShopContext context, string xmlInput)
        {
            var categoryDtos = Deserializer<List<CategoryDto>>(Constants.Categories, xmlInput);

            GenerateMapper();

            var categories = mapper.Map<List<Category>>(categoryDtos);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, categories.Count());
        }

        public static string ImportCategoryProducts(ProductShopContext context, string xmlInput)
        {
            var categoryProductDtos = Deserializer<List<CategoryProductDto>>(Constants.CategoryProducts, xmlInput)
                .Where(cp =>
                        context.Categories.Find(cp.CategoryId) != null
                        && context.Products.Find(cp.ProductId) != null)
                .ToList();

            GenerateMapper();

            var categoryProducts = mapper.Map<List<CategoryProduct>>(categoryProductDtos);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, categoryProducts.Count());
        }
        #endregion

        #region Query Tasks
        public static string GetProductsInRange(ProductShopContext context)
        {
            GenerateMapper();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
                .ToList();

            return Serializer<List<ExportProductInRangeDto>>(products, Constants.Products);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            GenerateMapper();

            var users = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserSoldProductsDto>(mapper.ConfigurationProvider)
                .ToList();

            return Serializer<List<ExportUserSoldProductsDto>>(users, Constants.Users);
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            GenerateMapper();

            var categories = context.Categories
                .ProjectTo<ExportCategoryByProductsCountDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToList();

            return Serializer<List<ExportCategoryByProductsCountDto>>(categories, Constants.Categories);

        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            GenerateMapper();

            var users = context.Users
                .ToList()
                .Where(u => u.ProductsSold.Count > 0)
                .OrderByDescending(u => u.ProductsSold.Count)
                .Take(10)
                .Select(u => new ExportUserWithProductsDto
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportSoldProductsDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(ps => new ExportProductDtoV2
                        {
                            Name = ps.Name,
                            Price = ps.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToList()
                    }
                })
                .ToList();


            var result = new ExportResultUserWithProductsDto
            {
                Count = context.Users.Count(u => u.ProductsSold.Count > 0),
                Users = users
            };

            return Serializer<ExportResultUserWithProductsDto>(result, Constants.Users);
        }
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
                cnfg.AddProfile<ProductShopProfile>();
            });

            mapper = config.CreateMapper();
        }
        #endregion
    }
}