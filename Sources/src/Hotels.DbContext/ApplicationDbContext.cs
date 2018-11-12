using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hotels.Entities;
using Hotels.Entities.Audits;
using Hotels.Entities.Masters;
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

            base.OnModelCreating(builder);
        }

        private void AuditTrailLogConfigure(EntityTypeBuilder<AuditTrail> builder)
        {
            builder.ToTable("tb_Audit_Trails");

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
            builder.ToTable("tb_Room_types");

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
    }
}