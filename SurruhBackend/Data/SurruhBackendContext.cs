using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SurruhBackend.Models
{
    public class SurruhBackendContext : DbContext
    {
        public SurruhBackendContext (DbContextOptions<SurruhBackendContext> options)
            : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ImageData> ImageData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageData_Tag>()
                .HasKey(t => new { t.ImageDataId, t.TagId });

            modelBuilder.Entity<ProductOption_ImageData>()
                .HasKey(t => new { t.ProductOptionId, t.ImageDataId });

            modelBuilder.Entity<ImageData_Tag>()
                .HasOne(idt => idt.ImageData)
                .WithMany(id => id.ImageData_Tags)
                .HasForeignKey(idt => idt.ImageDataId);

            modelBuilder.Entity<ProductOption_ImageData>()
                .HasOne(poid => poid.ProductOption)
                .WithMany(po => po.ProductOption_ImageData)
                .HasForeignKey(poid => poid.ProductOptionId);

            modelBuilder.Entity<ImageData_Tag>()
                .HasOne(idt => idt.Tag)
                .WithMany(t => t.ImageData_Tags)
                .HasForeignKey(idt => idt.TagId);

            modelBuilder.Entity<ProductOption_ImageData>()
                .HasOne(poid => poid.ImageData)
                .WithMany(id => id.ProductOption_ImageData)
                .HasForeignKey(poid => poid.ImageDataId);
        }
    }
}
