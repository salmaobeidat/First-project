using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace First_project.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutu> Aboutus { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Credential> Credentials { get; set; }

    public virtual DbSet<Homepage> Homepages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VisaChecher> VisaChechers { get; set; }

    public virtual DbSet<Visainfo> Visainfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##RecipeBlog;PASSWORD=Test321;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##RECIPEBLOG")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Aboutu>(entity =>
        {
            entity.HasKey(e => e.AboutUs).HasName("SYS_C008642");

            entity.ToTable("ABOUTUS");

            entity.Property(e => e.AboutUs)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUT_US");
            entity.Property(e => e.AboutusContent)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("ABOUTUS_CONTENT");
            entity.Property(e => e.AboutusImagePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("ABOUTUS_IMAGE_PATH");
            entity.Property(e => e.Header)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("HEADER");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("SYS_C008601");

            entity.ToTable("CATEGORY_");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CategoryImagePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_IMAGE_PATH");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_NAME");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("SYS_C008638");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.ContactId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACT_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.MessageContent)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("MESSAGE_CONTENT");
            entity.Property(e => e.Subject)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<Credential>(entity =>
        {
            entity.HasKey(e => e.CredentialId).HasName("SYS_C008624");

            entity.ToTable("CREDENTIALS");

            entity.HasIndex(e => e.Email, "EMAILUNIQUE").IsUnique();

            entity.HasIndex(e => e.UserId, "SYS_C008625").IsUnique();

            entity.Property(e => e.CredentialId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CREDENTIAL_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithOne(p => p.Credential)
                .HasForeignKey<Credential>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CREDENTIALS_FOREIGN_KEY");
        });

        modelBuilder.Entity<Homepage>(entity =>
        {
            entity.HasKey(e => e.HomeId).HasName("SYS_C008640");

            entity.ToTable("HOMEPAGE");

            entity.Property(e => e.HomeId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOME_ID");
            entity.Property(e => e.BackgroundImagePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("BACKGROUND_IMAGE_PATH");
            entity.Property(e => e.SliderHeader)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SLIDER_HEADER");
            entity.Property(e => e.SliderParagraph)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("SLIDER_PARAGRAPH");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("SYS_C008652");

            entity.ToTable("ORDERS");

            entity.HasIndex(e => e.PaymentId, "SYS_C008653").IsUnique();

            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDER_ID");
            entity.Property(e => e.OrderDate)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp")
                .HasColumnName("ORDER_DATE");
            entity.Property(e => e.PaymentId)
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENT_ID");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Payment).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("VOFOREIGN_KEY");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Orders)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RFOREIGN_KEY");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("UOFOREIGN_KEY");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId).HasName("SYS_C008618");

            entity.ToTable("RECIPE");

            entity.Property(e => e.RecipeId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.Creationdate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CREATIONDATE");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Ingredients)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("INGREDIENTS");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.RecipeImagePath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("RECIPE_IMAGE_PATH");
            entity.Property(e => e.RecipeName)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("RECIPE_NAME");
            entity.Property(e => e.RepcipePdfPath)
                .HasMaxLength(3276)
                .IsUnicode(false)
                .HasColumnName("REPCIPE_PDF_PATH");
            entity.Property(e => e.StatusId)
                .HasColumnType("NUMBER")
                .HasColumnName("STATUS_ID");
            entity.Property(e => e.TimeTakes)
                .HasColumnType("NUMBER(4,2)")
                .HasColumnName("TIME_TAKES");
            entity.Property(e => e.TotalCalories)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTAL_CALORIES");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CFOREIGN_KEY");

            entity.HasOne(d => d.Status).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("RSFOREIGN_KEY");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("UFOREIGN_KEY");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("SYS_C008567");

            entity.ToTable("ROLE");

            entity.Property(e => e.RoleId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("SYS_C008629");

            entity.ToTable("STATUS");

            entity.Property(e => e.StatusId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("STATUS_ID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("STATUS_NAME");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestimonialId).HasName("SYS_C008631");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.TestimonialId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIAL_ID");
            entity.Property(e => e.CreationDate)
                .HasColumnType("DATE")
                .HasColumnName("CREATION_DATE");
            entity.Property(e => e.StatusId)
                .HasColumnType("NUMBER")
                .HasColumnName("STATUS_ID");
            entity.Property(e => e.TestimonialContent)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIAL_CONTENT");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Status).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("TSFOREIGN_KEY");

            entity.HasOne(d => d.User).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("TUFOREIGN_KEY");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("SYS_C008607");

            entity.ToTable("USERS");

            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Birthdate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDATE");
            entity.Property(e => e.Gender)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.Profileimagepath)
                .HasMaxLength(700)
                .IsUnicode(false)
                .HasColumnName("PROFILEIMAGEPATH");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.Specialization)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("SPECIALIZATION");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FOREIGN_KEY");
        });

        modelBuilder.Entity<VisaChecher>(entity =>
        {
            entity.HasKey(e => e.VisaChecherId).HasName("SYS_C008661");

            entity.ToTable("VISA_CHECHER");

            entity.HasIndex(e => e.CardNumber, "SYS_C008662").IsUnique();

            entity.Property(e => e.VisaChecherId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("VISA_CHECHER_ID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER")
                .HasColumnName("BALANCE");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CARD_HOLDER_NAME");
            entity.Property(e => e.CardNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Cvc)
                .HasColumnType("NUMBER")
                .HasColumnName("CVC");
        });

        modelBuilder.Entity<Visainfo>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("SYS_C008647");

            entity.ToTable("VISAINFO");

            entity.HasIndex(e => e.CardNumber, "SYS_C008648").IsUnique();

            entity.HasIndex(e => e.UserId, "SYS_C008649").IsUnique();

            entity.Property(e => e.PaymentId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENT_ID");
            entity.Property(e => e.CardHolderName)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("CARD_HOLDER_NAME");
            entity.Property(e => e.CardNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Cvc)
                .HasColumnType("NUMBER")
                .HasColumnName("CVC");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithOne(p => p.Visainfo)
                .HasForeignKey<Visainfo>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("UVFOREIGN_KEY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
