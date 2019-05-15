using System;
using System.Collections.Generic;
using System.Text;

namespace SSMB.SQL.Configurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class AlertConditionConfiguration : IEntityTypeConfiguration<AlertCondition>
    {
        public void Configure(EntityTypeBuilder<AlertCondition> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(e => e.Value).HasColumnName("Value").IsRequired();

            builder.Property(e => e.Field)
                   .IsRequired()
                   .HasConversion(
                       v => (byte)v,
                       v => (AlertField)v)
                   .IsUnicode(false);
        }
    }
}
