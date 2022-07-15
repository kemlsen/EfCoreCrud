using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace entityframework
{
    public class ShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer("Data Source=DESKTOP-RF82O7V;Initial Catalog=ShopDb;Integrated Security=True");

        }

    }
    //entity class
    public class Product
    {
        public int ProductId { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            DeleteProduct(6);
        }
        static void AddProducts()
        {
            using (var db = new ShopContext())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Samsung S6", Price = 3000 },
                    new Product { Name = "Samsung S7", Price = 4000 },
                    new Product { Name = "Samsung S8", Price = 5000 },
                    new Product { Name = "Samsung S9", Price = 6000 }
                };
                //foreach (var p in products)
                //{

                //    db.Products.Add(p);

                //} ya da
                db.Products.AddRange(products);
                db.SaveChanges();
                Console.WriteLine("Veriler eklendi.");
            }
        }
        static void AddProduct()
        {
            using (var db = new ShopContext())
            {
                var p = new Product { Name = "Samsung S10", Price = 8000 };
                db.Products.Add(p);
                db.SaveChanges();
                Console.WriteLine("Veri eklendi.");
            }
        }
        static void GetAllProduct()
        {
            using (var context = new ShopContext())
            {
                var products = context
                    .Products
                    .Select(p => new
                    {
                        p.Name,
                        p.Price
                    })
                    .ToList();
                foreach (var p in products)
                {
                    Console.WriteLine($"Name: {p.Name}  price: {p.Price}");
                }
            }
        }
        static void GetProductBy(int id)
        {
            using (var context = new ShopContext())
            {
                var result = context.Products
                                    .Where(p => p.ProductId == id)
                                    .Select(p =>
                                                 new
                                                 {
                                                     p.Name,
                                                     p.Price
                                                 })
                                    .First();

                Console.WriteLine($"Name: {result.Name}  price: {result.Price}");

            }
        }
        static void GetProductByName(string name)
        {
            using (var context = new ShopContext())
            {
                var result = context.Products
                                    .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                                    .Select(p =>
                                                 new
                                                 {
                                                     p.Name,
                                                     p.Price
                                                 })
                                    .ToList();
                foreach (var p in result)
                {
                    Console.WriteLine($"Name: {p.Name}  price: {p.Price}");
                }


            }
        }
        static void UpdateProduct()
        {
            using (var db = new ShopContext())
            {
                var p = db.Products.Where(i => i.ProductId == 1).FirstOrDefault();
                if (p!=null)
                {
                    p.Price = 2400;
                    db.Products.Update(p);
                    db.SaveChanges();
                }
            }

            //using(var db=new ShopContext())
            //{
            //    var entity = new Product() { ProductId = 1 };
            //    db.Products.Attach(entity);
            //    entity.Price = 3000;
            //    db.SaveChanges();
            //}

                //using (var db = new ShopContext())
                //{
                //    var p = db.Products.Where(i => i.ProductId == 1).FirstOrDefault();
                //    if (p != null)
                //    {
                //        p.Price *= 1.2m;
                //        db.SaveChanges();
                //        Console.WriteLine("Güncelleme yapıldı");
                //    }
                //}
        }

        static void DeleteProduct(int id)
        {
            using (var db = new ShopContext())
            {
                var p = new Product() { ProductId = 5 };
                db.Products.Remove(p);
            }
                
           
            //using(var db=new ShopContext())
            //{
            //    var p=db.Products.FirstOrDefault(i=>i.ProductId== id);
            //    if (p!=null)
            //    {
            //        db.Products.Remove(p);
            //        db.SaveChanges();
            //        Console.WriteLine("Veri Silindi");
            //    }
            //}
        }
    }
}
