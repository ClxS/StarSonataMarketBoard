namespace SSMB.SQL.Configurations
{
    using System;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class OrderConfiguration : IEntityTypeConfiguration<OrderEntry>
    {
        public void Configure(EntityTypeBuilder<OrderEntry> builder)
        {
            builder.HasKey(e => e.EntryId);

            builder.Property(e => e.EntryId).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(e => e.GalaxyName).HasMaxLength(70).IsRequired();

            builder.Property(e => e.BaseName).HasMaxLength(70).IsRequired();

            builder.Property(e => e.IsUserBase).IsRequired();

            builder.Property(e => e.Price).IsRequired();

            builder.Property(e => e.Quantity).IsRequired();

            builder.Property(e => e.OrderType)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),
                       v => (OrderType)Enum.Parse(typeof(OrderType), v))
                   .IsUnicode(false);
        }
    }
}
