using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DataLogic1.DBModels
{
    public partial class ShoppingCartContext : DbContext
    {
        private readonly string _connectionString;
        public ShoppingCartContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCart> TblCarts { get; set; }
        public virtual DbSet<TblDeliveryCharge> TblDeliveryCharges { get; set; }
        public virtual DbSet<TblDiscount> TblDiscounts { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblProductCategory> TblProductCategories { get; set; }
        public virtual DbSet<TblProductCostTable> TblProductCostTables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                // optionsBuilder.UseSqlServer("Data Source=DESKTOP-5Q08ST9;Initial Catalog=ShoppingCart;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblCart>(entity =>
            {
                entity.HasKey(e => e.CartId)
                    .HasName("PK_CartID");

                entity.ToTable("tblCart");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.AddWeekendsDiscount)
                    .HasColumnType("decimal(8, 2)")
                    .HasColumnName("Add_WeekendsDiscount");

                entity.Property(e => e.CostPerUnit).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.DeliveryCharges).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.FkProductId).HasColumnName("FK_ProductID");

                entity.Property(e => e.NormalDayDiscount).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.TotalCost).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.TotalDiscount).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.FkProduct)
                    .WithMany(p => p.TblCarts)
                    .HasForeignKey(d => d.FkProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblCart__FK_Prod__4F7CD00D");
            });

            modelBuilder.Entity<TblDeliveryCharge>(entity =>
            {
                entity.HasKey(e => e.DeliveryChargesId)
                    .HasName("PK_DeliveryChargesID");

                entity.ToTable("tblDeliveryCharges");

                entity.Property(e => e.DeliveryChargesId).HasColumnName("DeliveryChargesID");

                entity.Property(e => e.FkProductcategoryId).HasColumnName("FK_ProductcategoryID");

                entity.HasOne(d => d.FkProductcategory)
                    .WithMany(p => p.TblDeliveryCharges)
                    .HasForeignKey(d => d.FkProductcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDelive__FK_Pr__44FF419A");
            });

            modelBuilder.Entity<TblDiscount>(entity =>
            {
                entity.HasKey(e => e.DiscountId)
                    .HasName("PK_DiscountID");

                entity.ToTable("tblDiscount");

                entity.Property(e => e.DiscountId).HasColumnName("DiscountID");

                entity.Property(e => e.AddWeekendsDiscount).HasColumnName("Add_WeekendsDiscount");

                entity.Property(e => e.FkProductId).HasColumnName("FK_ProductID");

                entity.HasOne(d => d.FkProduct)
                    .WithMany(p => p.TblDiscounts)
                    .HasForeignKey(d => d.FkProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblDiscou__FK_Pr__3F466844");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK_ProductID");

                entity.ToTable("tblProducts");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.FkProductcategoryId).HasColumnName("FK_ProductcategoryID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkProductcategory)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.FkProductcategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblProduc__FK_Pr__398D8EEE");
            });

            modelBuilder.Entity<TblProductCategory>(entity =>
            {
                entity.HasKey(e => e.ProductcategoryId)
                    .HasName("PK_ProductcategoryID");

                entity.ToTable("tblProductCategory");

                entity.Property(e => e.ProductcategoryId).HasColumnName("ProductcategoryID");

                entity.Property(e => e.ProductCategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblProductCostTable>(entity =>
            {
                entity.HasKey(e => e.ProductCostId)
                    .HasName("PK_ProductCostID");

                entity.ToTable("tblProductCostTable");

                entity.Property(e => e.ProductCostId).HasColumnName("ProductCostID");

                entity.Property(e => e.FkProductId).HasColumnName("FK_ProductID");

                entity.HasOne(d => d.FkProduct)
                    .WithMany(p => p.TblProductCostTables)
                    .HasForeignKey(d => d.FkProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblProduc__FK_Pr__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
