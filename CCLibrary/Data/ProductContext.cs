using System.Linq;
using Microsoft.EntityFrameworkCore;
using CCLibrary.Products;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public class ProductContext : DbContext
    {
        protected DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(Data.Database.DbSource);

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
