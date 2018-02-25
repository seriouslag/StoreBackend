using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SurruhBackend.Models
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options): base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product_Tag>()
                .HasKey(t => new { t.ProductId, t.TagId });

            modelBuilder.Entity<ProductOption_Image>()
                .HasKey(t => new { t.ProductOptionId, t.ImageId });

            modelBuilder.Entity<Product_Tag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductOption_Image>()
                .HasOne(poi => poi.ProductOption)
                .WithMany(po => po.Images)
                .HasForeignKey(poi => poi.ProductOptionId);

            modelBuilder.Entity<Product_Tag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Products)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<ProductOption_Image>()
                .HasOne(poi => poi.Image)
                .WithMany(i => i.ProductOption)
                .HasForeignKey(poi => poi.ImageId);

        } 
    }
}
