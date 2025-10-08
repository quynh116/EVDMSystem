using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EVMDealerSystem.DataAccess.Models;

public partial class EVMDealerSystemContext : DbContext
{
    public EVMDealerSystemContext()
    {
    }

    public EVMDealerSystemContext(DbContextOptions<EVMDealerSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Dealer> Dealers { get; set; }

    public virtual DbSet<Evm> Evms { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleRequest> VehicleRequests { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__appointm__3213E83F6E3B1126");

            entity.ToTable("appointments");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DealerStaffId).HasColumnName("dealer_staff_id");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__custo__7E37BEF6");

            entity.HasOne(d => d.DealerStaff).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DealerStaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__deale__7F2BE32F");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__vehic__00200768");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__customer__3213E83F26DBB8F0");

            entity.ToTable("customers");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DealerStaffId).HasColumnName("dealer_staff_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.DealerStaff).WithMany(p => p.Customers)
                .HasForeignKey(d => d.DealerStaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__customers__deale__5EBF139D");
        });

        modelBuilder.Entity<Dealer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__dealers__3213E83FE6E82356");

            entity.ToTable("dealers");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.ContractNumber)
                .HasMaxLength(100)
                .HasColumnName("contract_number");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EvmId).HasColumnName("evm_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.SalesTarget)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("sales_target");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Evm).WithMany(p => p.Dealers)
                .HasForeignKey(d => d.EvmId)
                .HasConstraintName("FK__dealers__evm_id__3F466844");
        });

        modelBuilder.Entity<Evm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__evm__3213E83FA6B513C9");

            entity.ToTable("evm");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__feedback__3213E83F44B9D8E6");

            entity.ToTable("feedbacks");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.FeedbackType)
                .HasMaxLength(50)
                .HasColumnName("feedback_type");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .HasColumnName("subject");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Customer).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedbacks__custo__787EE5A0");

            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedbacks__order__797309D9");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__inventor__3213E83FB728D904");

            entity.ToTable("inventories");

            entity.HasIndex(e => e.VinNumber, "UQ__inventor__37F2DBC7C58A44EF").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DealerId).HasColumnName("dealer_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");
            entity.Property(e => e.VinNumber)
                .HasMaxLength(255)
                .HasColumnName("vin_number");

            entity.HasOne(d => d.Dealer).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.DealerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventori__deale__534D60F1");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__inventori__vehic__52593CB8");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__orders__3213E83F35C40827");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.DealerId).HasColumnName("dealer_id");
            entity.Property(e => e.DealerStaffId).HasColumnName("dealer_staff_id");
            entity.Property(e => e.DeliveredAt).HasColumnName("delivered_at");
            entity.Property(e => e.InventoryId).HasColumnName("inventory_id");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .HasColumnName("order_status");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("payment_status");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .HasColumnName("payment_type");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__customer__6383C8BA");

            entity.HasOne(d => d.Dealer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DealerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__dealer_i__66603565");

            entity.HasOne(d => d.DealerStaff).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DealerStaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__dealer_s__6477ECF3");

            entity.HasOne(d => d.Inventory).WithMany(p => p.Orders)
                .HasForeignKey(d => d.InventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__inventor__6754599E");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__vehicle___656C112C");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__payments__3213E83F32E7AC4A");

            entity.ToTable("payments");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("transaction_date");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__payments__order___6C190EBB");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__promotio__3213E83FA1B868C7");

            entity.ToTable("promotions");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DiscountPercent)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount_percent");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(50)
                .HasColumnName("discount_type");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__creat__59063A47");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Promotions)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__promotion__vehic__5812160E");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__roles__3213E83F5DCA9357");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F7D20C0BC");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164F25A86AE").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.DealerId).HasColumnName("dealer_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Dealer).WithMany(p => p.Users)
                .HasForeignKey(d => d.DealerId)
                .IsRequired(false)
                .HasConstraintName("FK__users__dealer_id__46E78A0C");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__45F365D3");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicles__3213E83FFED70FF2");

            entity.ToTable("vehicles");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.BatteryCapacity)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("battery_capacity");
            entity.Property(e => e.Category)
                .HasMaxLength(100)
                .HasColumnName("category");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EvmId).HasColumnName("evm_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .HasColumnName("image_url");
            entity.Property(e => e.LaunchDate).HasColumnName("launch_date");
            entity.Property(e => e.ModelName)
                .HasMaxLength(255)
                .HasColumnName("model_name");
            entity.Property(e => e.RangePerCharge).HasColumnName("range_per_charge");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .HasColumnName("version");

            entity.HasOne(d => d.Evm).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.EvmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vehicles__evm_id__4CA06362");
        });

        modelBuilder.Entity<VehicleRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vehicle___3213E83F17EEA724");

            entity.ToTable("vehicle_requests");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.ApprovedAt).HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DealerId).HasColumnName("dealer_id");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.VehicleRequestApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__vehicle_r__appro__74AE54BC");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.VehicleRequestCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vehicle_r__creat__70DDC3D8");

            entity.HasOne(d => d.Dealer).WithMany(p => p.VehicleRequests)
                .HasForeignKey(d => d.DealerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vehicle_r__deale__72C60C4A");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.VehicleRequests)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__vehicle_r__vehic__71D1E811");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
