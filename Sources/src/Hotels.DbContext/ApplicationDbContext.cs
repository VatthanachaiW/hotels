using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotels.Entities;
using Hotels.Entities.Audits;
using Hotels.Entities.Masters;
using Hotels.Entities.Profiles;
using Hotels.IDbConnections;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hotels.DbConnections
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserClaim,
        ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var auditEntries = OnBeforeSaveChange();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            await OnAfterSaveChangesAsync(auditEntries);
            return result;
        }

        public async Task<bool> SaveChangesAsync() => await base.SaveChangesAsync() > 0;


        private Task OnAfterSaveChangesAsync(List<AuditTrailEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0) return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var property in auditEntry.TemporaryProperties)
                {
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[property.Metadata.Name] = property.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                    }
                }

                Set<AuditTrail>().Add(auditEntry.ToAuditTrail());
            }

            return SaveChangesAsync();
        }

        private List<AuditTrailEntry> OnBeforeSaveChange()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditTrailEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditTrail || entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged) continue;

                var auditEntry = new AuditTrailEntry(entry)
                {
                    TableName = entry.Metadata.Relational().TableName,
                    Action = Enum.GetName(typeof(EntityState), entry.State),
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propName] = property.OriginalValue;
                                auditEntry.NewValues[propName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Set<AuditTrail>().Add(auditEntry.ToAuditTrail());
            }

            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AuditTrail>(AuditTrailLogConfigure);

            builder.Entity<RoomType>(RoomTypeConfigure);
            builder.Entity<Room>(RoomConfigure);
            builder.Entity<RoomPrice>(RoomPriceConfigure);
            builder.Entity<Receipt>(ReceiptConfigure);
            builder.Entity<Hotel>(HotelConfigure);

            base.OnModelCreating(builder);
        }

        private void AuditTrailLogConfigure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.ToTable("tb_audit_trails");

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasColumnName("audit_trail_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.TimeStamp)
                .HasColumnName("audit_trail_timpstamp")
                .IsRequired();
            builder.Property(p => p.Action)
                .HasColumnName("audit_trail_action")
                .IsRequired();
            builder.Property(p => p.TableName)
                .HasColumnName("audit_trail_tablename")
                .IsRequired();
            builder.Property(p => p.KeyValues)
                .HasColumnName("audit_trail_key_values")
                .IsRequired();
            builder.Property(p => p.OldValues)
                .HasColumnName("audit_trail_old_values")
                .IsRequired();
            builder.Property(p => p.NewValues)
                .HasColumnName("audit_trail_new_values")
                .IsRequired();
        }

        private void RoomTypeConfigure(EntityTypeBuilder<RoomType> builder)
        {
            builder.ToTable("tb_room_types");

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasColumnName("room_type_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.RoomTypeName)
                .HasColumnName("room_type_name")
                .IsRequired();

            builder.Property(p => p.IsActive)
                .HasColumnName("room_type_is_active")
                .HasDefaultValue(true);

            builder.Property(p => p.CreateBy)
                .HasColumnName("room_type_create_by")
                .IsRequired();
            builder.Property(p => p.CreateOn)
                .HasColumnName("room_type_create_On")
                .IsRequired();

            builder.Property(p => p.ModifiedBy)
                .HasColumnName("room_type_modified_by");

            builder.Property(p => p.ModifiedOn)
                .HasColumnName("room_type_modified_on");
        }

        private void RoomConfigure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("tb_rooms");

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasColumnName("room_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.RoomCode).HasColumnName("room_code");
            builder.Property(p => p.Description).HasColumnName("room_description");
            builder.Property(p => p.RoomTypeId).HasColumnName("room_type_id");

            builder.Property(p => p.IsActive)
                .HasColumnName("room_is_active")
                .HasDefaultValue(true);

            builder.Property(p => p.CreateBy)
                .HasColumnName("room_create_by")
                .IsRequired();
            builder.Property(p => p.CreateOn)
                .HasColumnName("room_create_On")
                .IsRequired();

            builder.Property(p => p.ModifiedBy)
                .HasColumnName("room_modified_by");

            builder.Property(p => p.ModifiedOn)
                .HasColumnName("room_modified_on");

            builder.HasOne(o => o.RoomType).WithMany(m => m.Rooms).HasForeignKey(f => f.RoomTypeId);
        }

        private void RoomPriceConfigure(EntityTypeBuilder<RoomPrice> builder)
        {
            builder.ToTable("tb_room_prices");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasColumnName("room_price_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.RoomId).HasColumnName("room_id");
            builder.Property(p => p.PriceDate).HasColumnName("price_date");
            builder.Property(p => p.Price).HasColumnName("price");

            builder.Property(p => p.IsActive)
                .HasColumnName("room_price_is_active")
                .HasDefaultValue(true);

            builder.Property(p => p.CreateBy)
                .HasColumnName("room_price_create_by")
                .IsRequired();
            builder.Property(p => p.CreateOn)
                .HasColumnName("room_price_create_On")
                .IsRequired();

            builder.Property(p => p.ModifiedBy)
                .HasColumnName("room_price_modified_by");

            builder.Property(p => p.ModifiedOn)
                .HasColumnName("room_price_modified_on");

            builder.HasOne(o => o.Room).WithMany(m => m.RoomPrices).HasForeignKey(f => f.RoomId);
        }

        private void ReceiptConfigure(EntityTypeBuilder<Receipt> builder)
        {
            builder.ToTable("tb_receipt");

            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasColumnName("receipt_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.ReceiptNo).HasColumnName("receipt_no");
            builder.Property(p => p.ReceiptDate).HasColumnName("receipt_date");
            builder.Property(p => p.RoomId).HasColumnName("room_id");
            builder.Property(p => p.PriceId).HasColumnName("price_id");
            builder.Property(p => p.Firstname).HasColumnName("firstname");
            builder.Property(p => p.Lastname).HasColumnName("lastname");
            builder.Property(p => p.Address).HasColumnName("address");
            builder.Property(p => p.PostalCode).HasColumnName("postal_code");
            builder.Property(p => p.Mobile).HasColumnName("mobile");
            builder.Property(p => p.Email).HasColumnName("email");
            builder.Property(p => p.CheckIn).HasColumnName("check_in");
            builder.Property(p => p.CheckOut).HasColumnName("check_out");
            builder.Property(p => p.PaidPrice).HasColumnName("paid_price");


            builder.Property(p => p.IsActive)
                .HasColumnName("receipt_is_active")
                .HasDefaultValue(true);

            builder.Property(p => p.CreateBy)
                .HasColumnName("receipt_create_by")
                .IsRequired();
            builder.Property(p => p.CreateOn)
                .HasColumnName("receipt_create_On")
                .IsRequired();

            builder.Property(p => p.ModifiedBy)
                .HasColumnName("receipt_modified_by");

            builder.Property(p => p.ModifiedOn)
                .HasColumnName("receipt_modified_on");

            builder.HasOne(o => o.Room).WithMany(m => m.Receipts).HasForeignKey(f => f.RoomId);
            builder.HasOne(o => o.Price).WithMany(m => m.Receipts).HasForeignKey(f => f.PriceId);
        }

        private void HotelConfigure(EntityTypeBuilder<Hotel> builder)
        {
            builder.ToTable("tb_hotels");
            builder.HasKey(k => k.Id);
            builder.Property(p => p.Id)
                .HasColumnName("hotel_id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.HotelName).HasColumnName("hotel_name");
            builder.Property(p => p.Address).HasColumnName("hotel_address");
            builder.Property(p => p.PostalCode).HasColumnName("hotel_postal_code");
            builder.Property(p => p.Phone).HasColumnName("hotel_phone");
            builder.Property(p => p.Fax).HasColumnName("hotel_fax");
            builder.Property(p => p.Email).HasColumnName("hotel_email");
        }
    }
}