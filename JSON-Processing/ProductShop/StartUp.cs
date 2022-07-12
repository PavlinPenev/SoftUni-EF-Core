using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
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
            #region PrepareJsons
            var usersJsonString = File.ReadAllText("users.json");
            var productsJsonString = File.ReadAllText("products.json");
            var categoriesJsonString = File.ReadAllText("categories.json");
            var categoriesProductsJsonString = File.ReadAllText("categories-products.json");
            #endregion

            ImportUsers(context, usersJsonString);
            ImportProducts(context, productsJsonString);
            ImportCategories(context, categoriesJsonString);
            ImportCategoryProducts(context, categoriesProductsJsonString);
        }

        #region Import Data
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesAndProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoriesAndProducts);

            context.SaveChanges();

            return String.Format(Constants.SuccessfullyImported, categoriesAndProducts.Count);
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<ProductShopProfile>();
            var mapperCfg = new MapperConfiguration(cfg);

            var mapper = new Mapper(mapperCfg);

            var mockCategories = JsonConvert.DeserializeObject<List<CategoryDto>>(inputJson);

            var mappedCategories = mapper.DefaultContext.Mapper.Map<List<Category>>(mockCategories)
                .Where(x => x.Name != null).ToList();

            context.AddRange(mappedCategories);

            context.SaveChanges();

            return String.Format(Constants.SuccessfullyImported, mappedCategories.Count);
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var mockProducts = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(mockProducts);

            context.SaveChanges();

            return String.Format(Constants.SuccessfullyImported, mockProducts.Count);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var mockUsers = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(mockUsers);

            context.SaveChanges();

            return String.Format(Constants.SuccessfullyImported, mockUsers.Count);
        }
        #endregion

        #region Query Tasks
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                            .Where(x => x.Price >= 500 && x.Price <= 1000)
                            .OrderBy(x => x.Price)
                            .Select(x => new
                            {
                                x.Name,
                                x.Price,
                                Seller = x.Seller.FirstName + " " + x.Seller.LastName,
                                
                            })
                            .ToList();

            var jsonValue = JsonConvert.SerializeObject(products, GetJsonSettings());

            return jsonValue;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users                            
                            .Select(u => new
                            {
                                u.FirstName,
                                u.LastName,
                                SoldProducts = u.ProductsSold
                                .Where(i => i.BuyerId != null)
                                .Select(ps => new
                                {
                                    ps.Name,
                                    ps.Price,
                                    BuyerFirstName = ps.Buyer.FirstName,
                                    BuyerLastName = ps.Buyer.LastName

                                })
                            })
                            .OrderBy(u => u.LastName)
                            .ThenBy(u => u.FirstName)
                            .ToList()
                            .Where(u => u.SoldProducts.Count() > 0);

            var jsonValue = JsonConvert.SerializeObject(users, GetJsonSettings());

            return jsonValue;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                                .OrderByDescending(c => c.CategoryProducts.Count())
                                .Select(c => new
                                {
                                    Category = c.Name,
                                    ProductsCount = c.CategoryProducts.Count(),
                                    AveragePrice = $"{c.CategoryProducts.Average(cp => cp.Product.Price):f2}",
                                    TotalRevenue = $"{c.CategoryProducts.Sum(cp => cp.Product.Price):f2}"
                                })
                                .ToList();

            var jsonValue = JsonConvert.SerializeObject(categories, GetJsonSettings());

            return jsonValue;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ToList()
                .Where(x => x.ProductsSold.Any(p => p.BuyerId != null))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Age = x.Age,
                    SoldProducts = new
                    {
                        Count = x.ProductsSold.Where(p => p.BuyerId != null).Count(),
                        Products = x.ProductsSold.Where(p => p.BuyerId != null).Select(p => new
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToList()
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .ToList();

            var result = new
            {
                UsersCount = users.Count(),
                Users = users
            };

            var jsonValue = JsonConvert.SerializeObject(result, GetJsonSettings());

            return jsonValue;
        }
        #endregion

        private static JsonSerializerSettings GetJsonSettings()
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
    }
}