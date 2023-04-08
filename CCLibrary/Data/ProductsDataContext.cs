using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CCLibrary.Products;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public partial class DataContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Consumable> Consumables { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<EnergyDrink> EnergyDrinks { get; set; }
        public DbSet<AlcoholDrink> AlcoholDrinks { get; set; }

        /// <summary>
        /// Get product reference.
        /// Use with caution!
        /// </summary>
        /// <exception cref="ProductNotFoundException"></exception>
        public Product GetProductRef(long id)
        {
            return Products.FirstOrDefault(product =>  product.Id == id)
                ?? throw new ProductNotFoundException();
        }

        /// <summary>
        /// Get the product by ID.
        /// </summary>
        public Product GetProduct(long id)
        {
            Product product = GetProductRef(id);

            return product.Copy();
        }

        /// <summary>
        /// Add a product to the database.
        /// </summary>
        public void AddProduct(Product product)
        {
            Products.Add(product.Copy());
            SaveChanges();
        }

        /// <summary>
        /// Remove product from the database.
        /// </summary>
        public void DeleteProduct(long id)
        {
            Product product = GetProductRef(id);

            Products.Remove(product);
            SaveChanges();
        }

        /// <summary>
        /// Modify existing product.
        /// </summary>
        /// <exception cref="ProductNotFoundException"></exception>
        public void ModifyProduct(Product product)
        {
            if (!Products.Contains(product))
                throw new ProductNotFoundException();

            Products.Update(product);
        }
    }
}
