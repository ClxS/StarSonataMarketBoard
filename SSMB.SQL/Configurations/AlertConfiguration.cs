using System;
using System.Collections.Generic;
using System.Text;

namespace SSMB.SQL.Configurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class AlertConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(e => e.Name).HasMaxLength(50);

            builder.Property(e => e.UserId).HasColumnName("UserId").IsRequired();

            builder.Property(e => e.ItemId).HasColumnName("ItemId").IsRequired();

            builder.HasOne(e => e.Item).WithMany(i => i.Alerts);

            builder
                .HasMany(e => e.Conditions)
                .WithOne(o => o.Alert)
                .IsRequired()
                .HasForeignKey(o => o.AlertId);
        }
    }
}
