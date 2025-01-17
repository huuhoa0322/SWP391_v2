using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineMarketPlace.Models;

public partial class OnlineShoppingContext : DbContext
{
    public OnlineShoppingContext()
    {
    }

    public OnlineShoppingContext(DbContextOptions<OnlineShoppingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<RatingAndReview> RatingAndReviews { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }



    private string GetConnectionString()

    {

        IConfiguration config = new ConfigurationBuilder()

        .SetBasePath(Directory.GetCurrentDirectory())

        .AddJsonFile("appsettings.json", true, true)

        .Build();

        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];


        return strConn;

    }// use code in json file to connect to database


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F186F8D9E");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parentId");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("category_parentid_foreign");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3213E83FA76FDD14");

            entity.ToTable("Discount");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndAt)
                .HasColumnType("datetime")
                .HasColumnName("endAt");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.StartAt)
                .HasColumnType("datetime")
                .HasColumnName("startAt");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.Product).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("discount_productid_foreign");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F94D0A5F9");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.DeletedBy).HasColumnName("deletedBy");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Inventory).HasColumnName("inventory");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.QuantitySold).HasColumnName("quantitySold");
            entity.Property(e => e.SellerId).HasColumnName("sellerId");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_categoryid_foreign");

            entity.HasOne(d => d.Seller).WithMany(p => p.Products)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_sellerid_foreign");
        });

        modelBuilder.Entity<RatingAndReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RatingAn__3213E83F61C5CD85");

            entity.ToTable("RatingAndReview");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Review)
                .HasMaxLength(255)
                .HasColumnName("review");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RatingAndReviews)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratingandreview_createdby_foreign");

            entity.HasOne(d => d.Product).WithMany(p => p.RatingAndReviews)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ratingandreview_productid_foreign");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shop__3213E83F4D2B2E68");

            entity.ToTable("Shop");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.DeletedBy).HasColumnName("deletedBy");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Logo)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("logo");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("ownerId");

            entity.HasOne(d => d.Owner).WithMany(p => p.Shops)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("shop_ownerid_foreign");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F66D73ED9");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deletedAt");
            entity.Property(e => e.DeletedBy).HasColumnName("deletedBy");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
