using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProductShop.App
{
    public class StartUp
    {
        private static void Main()
        {
            using (var db = new ProductShopContext())
            {
                //db.Database.EnsureCreated();
                //db.Database.EnsureDeleted();

                //Json Import
                //Console.WriteLine(ImportJsonUsers());
                //Console.WriteLine(ImportJsonCategories());
                //Console.WriteLine(ImportJsonProducts());
                //SetJsonCategories();

                //Json Export
                //GetProductsInRange();
                //GetSoldProducts();
                //GetCategoryByProductCount();

                //XML Import
                //Console.WriteLine(ImportXmlUsers());
                //Console.WriteLine(ImportXmlCategories());
                //Console.WriteLine(ImprotXmlProducts());

                //XML Export
                //GetUsersAndProductsXml();
                //GetCategoriesByProductsCountXml();
                //GetProductsInRangeXml();
            }
        }
        //XML Processing
        public static void GetProductsInRangeXml()
        {
            using (var db = new ProductShopContext())
            {
                var products = db.Products
                    .Include(p => p.Seller)
                    .Where(p => p.Price >= 500 && p.Price <= 1000).Select(p => new
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                    })
                    .OrderBy(p => p.Price)
                    .ToArray();

                var xDoc = new XDocument();
                xDoc.Add(new XElement("products"));

                foreach (var p in products)
                {
                    xDoc.Root.Add(new XElement("product", new XAttribute("name", p.Name), new XAttribute("price", $"{p.Price}"), new XAttribute("buyer", p.Seller)));
                }

                File.WriteAllText("ProductsInRange(500-1000).xml", xDoc.ToString());
            }
        }

        public static void GetCategoriesByProductsCountXml()
        {
            using (var db = new ProductShopContext())
            {
                var categories = db.Categories
                    .Include(c => c.Products)
                    .OrderBy(c => c.Name)
                    .Select(c => new
                    {
                        category = c.Name,
                        productsCount = c.Products.Count,
                        averagePrice = $"{c.Products.Sum(p => p.Product.Price) / c.Products.Count:f2}",
                        totalRevenue = $"{c.Products.Sum(p => p.Product.Price):f2}"
                    }).ToArray();

                var xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("categories"));

                foreach (var c in categories)
                {
                    var category = new XElement("category", new XAttribute("name", c.category));
                    category.Add(new XElement("product-count", c.productsCount),
                        new XElement("average-price", c.averagePrice),
                        new XElement("total-revenue", c.totalRevenue));
                    xmlDoc.Root.Add(category);
                }
                File.WriteAllText("CategoriesByProductsCount.xml", xmlDoc.ToString());
            }
        }

        public static void GetUsersAndProductsXml()
        {
            using (var db = new ProductShopContext())
            {
                var users = db.Users
                .Where(u => u.SoldProducts.Count > 0)
                .Include(u => u.SoldProducts)
                .OrderByDescending(u => u.SoldProducts.Count)
                .ThenBy(u => u.LastName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.SoldProducts.Count,
                        products = u.SoldProducts.Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    }
                });

                var usersToSerialize = new
                {
                    usersCount = users.Count(),
                    users
                };

                var xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("users", new XAttribute("count", usersToSerialize.usersCount)));

                foreach (var u in usersToSerialize.users)
                {
                    var user = new XElement("user");
                    if (u.firstName != null)
                    {
                        user.Add(new XAttribute("first-name", u.firstName));
                    }
                    user.Add(new XAttribute("last-name", u.lastName));
                    if (u.age != null)
                    {
                        user.Add(new XAttribute("age", u.age));
                    }

                    var soldProducts = new XElement("sold-products", new XAttribute("count", u.soldProducts.count));

                    foreach (var p in u.soldProducts.products)
                    {
                        var product = new XElement("product",
                            new XAttribute("name", p.name),
                            new XAttribute("price", p.price));
                        soldProducts.Add(product);
                    }

                    user.Add(soldProducts);
                    xmlDoc.Root.Add(user);
                }
                File.WriteAllText("UsersAndProducts.xml", xmlDoc.ToString());
            }
        }

        public static string ImprotXmlProducts()
        {
            var xmlString = File.ReadAllText("Files/products.xml");

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var catProd = new List<CategoryProducts>();

            using (var db = new ProductShopContext())
            {

                var userIds = db.Users.Select(u => u.UserId).ToArray();
                var categoryIds = db.Categories.Select(c => c.CategoryId).ToArray();

                Random rnd = new Random();

                foreach (var e in elements)
                {
                    var name = e.Element("name").Value;
                    var price = decimal.Parse(e.Element("price").Value);

                    var sellerIndex = rnd.Next(0, userIds.Length);
                    var sellerId = userIds[sellerIndex];

                    var product = new Product()
                    {
                        Name = name,
                        Price = price,
                        SellerId = sellerId
                    };

                    var categoryIndex = rnd.Next(0, categoryIds.Length);
                    var categoryId = categoryIds[categoryIndex];

                    var catProduct = new CategoryProducts()
                    {
                        Product = product,
                        CategoryId = categoryId
                    };

                    catProd.Add(catProduct);
                }
                db.AddRange(catProd);

                db.SaveChanges();
            }
            return $"{catProd.Count} prodcuts were imported";
        }

        public static string ImportXmlCategories()
        {
            var xmlString = File.ReadAllText("Files/categories.xml");

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var categories = new List<Category>();

            foreach (var e in elements)
            {
                var category = new Category()
                {
                    Name = e.Element("name").Value
                };

                categories.Add(category);
            }

            using (var db = new ProductShopContext())
            {
                db.Categories.AddRange(categories);

                db.SaveChanges();
            }
            return $"{categories.Count} categories were imported";
        }

        public static string ImportXmlUsers()
        {
            var xmlString = File.ReadAllText("Files/users.xml");

            var xmlDoc = XDocument.Parse(xmlString);

            var elements = xmlDoc.Root.Elements();

            var users = new List<User>();

            foreach (var xElement in elements)
            {
                var firstName = xElement.Attribute("firstName")?.Value;
                var lastName = xElement.Attribute("lastName").Value;

                int? age = null;
                if (xElement.Attribute("age") != null)
                {
                    age = int.Parse(xElement.Attribute("age").Value);
                }

                var user = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                };
                users.Add(user);
            }

            using (var db = new ProductShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            return $"{users.Count} users were imported";
        }

        //Json Processing
        public static void GetCategoryByProductCount()
        {
            using (var db = new ProductShopContext())
            {
                var categories = db.Categories
                    .OrderBy(c => c.Name)
                    .Select(c => new
                    {
                        c.Name,
                        productsCount = c.Products.Count,
                        avaragePrice = c.Products.Sum(p => p.Product.Price) / c.Products.Count,
                        totalReveneue = c.Products.Sum(p => p.Product.Price)
                    })
                    .ToArray();

                var jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("CategoriesByProductCount.json", jsonString);
            }
        }

        public static void GetSoldProducts()
        {
            using (var db = new ProductShopContext())
            {
                var users = db.Users
                    .Where(u => u.SoldProducts.Any(p => p.BuyerId != null))
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        SoldProducts = u.SoldProducts.Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = p.Buyer.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        })
                    })
                    .ToArray();

                var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("SoldProductsInfo.json", jsonString);
            }
        }

        public static void GetProductsInRange()
        {
            using (var db = new ProductShopContext())
            {
                var products = db.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Seller = p.Seller.FirstName + " " + p.Seller.LastName
                    })
                    .ToArray();

                var jsonString = JsonConvert.SerializeObject(products, Formatting.Indented, new JsonSerializerSettings()
                {
                    DefaultValueHandling = DefaultValueHandling.Ignore
                });

                File.WriteAllText("PricesInRange.json", jsonString);
            }
        }

        public static void SetJsonCategories()
        {
            using (var db = new ProductShopContext())
            {
                var random = new Random();

                var productIds = db.Products.AsNoTracking().Select(p => p.ProductId).OrderBy(p => p).ToArray();
                var categoryIds = db.Categories.AsNoTracking().Select(c => c.CategoryId).ToArray();
                int categoryCount = categoryIds.Length;

                var categoryProducts = new List<CategoryProducts>();

                foreach (var p in productIds)
                {
                    for (int i = 0; i < random.Next(0, categoryCount); i++)
                    {
                        int index = random.Next(0, categoryCount);
                        while (categoryProducts.Any(cp => cp.ProductId == p && cp.CategoryId == categoryIds[index]))
                        {
                            index = random.Next(0, categoryCount);
                        }

                        var catPr = new CategoryProducts()
                        {
                            ProductId = p,
                            CategoryId = categoryIds[index]
                        };
                        categoryProducts.Add(catPr);
                    }
                }

                db.CategoryProducts.AddRange(categoryProducts);
                db.SaveChanges();
            }
        }

        public static string ImportJsonCategories()
        {
            Category[] categories = ImportJson<Category>("Files/categories.json");
            using (var db = new ProductShopContext())
            {
                db.Categories.AddRange(categories);

                db.SaveChanges();
            }

            return $"{categories.Length} categories were imported";
        }

        public static string ImportJsonProducts()
        {
            Product[] products = ImportJson<Product>("Files/products.json");

            Random rnd = new Random();

            using (var db = new ProductShopContext())
            {
                var userIds = db.Users.Select(u => u.UserId).ToArray();

                foreach (var product in products)
                {
                    var index = rnd.Next(0, userIds.Length);
                    var sellerId = userIds[index];

                    var buyerId = sellerId;
                    while (buyerId == sellerId)
                    {
                        var buyerIndex = rnd.Next(0, userIds.Length);
                        buyerId = userIds[buyerIndex];
                    }

                    product.SellerId = sellerId;
                    product.BuyerId = buyerId;
                }

                db.Products.AddRange(products);

                db.SaveChanges();
            }

            return $"{products.Length} products were imported";
        }

        public static string ImportJsonUsers()
        {
            User[] users = ImportJson<User>("Files/users.json");
            using (var db = new ProductShopContext())
            {
                db.Users.AddRange(users);

                db.SaveChanges();
            }

            return $"{users.Length} users were imported";
        }

        public static T[] ImportJson<T>(string path)
        {
            string jsonString = File.ReadAllText(path);

            T[] objects = JsonConvert.DeserializeObject<T[]>(jsonString);

            return objects;
        }
    }
}