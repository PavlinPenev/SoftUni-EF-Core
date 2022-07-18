using AutoMapper;
using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // Console.WriteLine(GetUsersWithProducts(dbContext));
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
            var serializer = SerializerInit(Constants.Users, typeof(List<UserDto>));

            List<UserDto> userDtos = null;

            using (StringReader reader = new StringReader(xmlInput))
            {
                userDtos = (List<UserDto>)serializer.Deserialize(reader);
            }

            GenerateMapper();

            var users = mapper.Map<List<User>>(userDtos);

            context.Users.AddRange(users);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, users.Count());
        }

        public static string ImportProducts(ProductShopContext context, string xmlInput)
        {
            var serializer = SerializerInit(Constants.Products, typeof(List<ProductDto>));

            var productDtos = new List<ProductDto>();

            using (StringReader reader = new StringReader(xmlInput))
            {
                productDtos = 
                    ((List<ProductDto>)serializer.Deserialize(reader))
                    //.Where(pd => pd.BuyerId != 0)
                    .ToList();
            }

            GenerateMapper();

            var products = mapper.Map<List<Product>>(productDtos);

            context.AddRange(products);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, products.Count());
        }

        public static string ImportCategories(ProductShopContext context, string xmlInput)
        {
            var serializer = SerializerInit(Constants.Categories, typeof(List<CategoryDto>));

            var categoryDtos = new List<CategoryDto>();

            using (StringReader reader = new StringReader(xmlInput))
            {
                categoryDtos = ((List<CategoryDto>)serializer.Deserialize(reader)).Where(c => c.Name != null).ToList();
            }

            GenerateMapper();

            var categories = mapper.Map<List<Category>>(categoryDtos);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, categories.Count());
        }

        public static string ImportCategoryProducts(ProductShopContext context, string xmlInput)
        {
            var serializer = SerializerInit(Constants.CategoryProducts, typeof(List<CategoryProductDto>));

            var categoryProductDtos = new List<CategoryProductDto>();

            using (StringReader reader = new StringReader(xmlInput))
            {
                categoryProductDtos = 
                    ((List<CategoryProductDto>)serializer.Deserialize(reader))
                    .Where(cp => 
                        context.Categories.Find(cp.CategoryId) != null
                        && context.Products.Find(cp.ProductId) != null)
                    .ToList();
            }

            GenerateMapper();

            var categoryProducts = mapper.Map<List<CategoryProduct>>(categoryProductDtos);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return String.Format(Constants.NotificationsConstants.SuccessfullyImported, categoryProducts.Count());
        }
        #endregion

        #region Query Tasks

        #endregion

        #region Private Methods
        private static XmlSerializer SerializerInit(string rootTag, Type resultDataType)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootTag);
            XmlSerializer serializer = new XmlSerializer(resultDataType, root);

            return serializer;
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