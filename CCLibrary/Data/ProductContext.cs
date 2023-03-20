using System.Linq;
using Microsoft.EntityFrameworkCore;
using CCLibrary.Products;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(Data.Database.DbSource);

        /// <summary>
        /// Get the product by ID.
        /// </summary>
        public Product? GetProduct(long id)
        {
            return Products.FirstOrDefault(product => product.Id.Equals(id));
        }

        /// <summary>
        /// Add a product to the database.
        /// </summary>
        public void AddProduct(Product product)
        {
            Products.Add(product);
            SaveChanges();
        }

        /// <summary>
        /// Remove product from the database.
        /// </summary>
        /// <exception cref="ProductNotFoundException"></exception>
        public void DeleteProduct(long id)
        {
            Product? product = GetProduct(id);
            if (product != null)
            {
                Products.Remove(product);
                SaveChanges();
            }
            else
                throw new ProductNotFoundException();
        }
    }
}
