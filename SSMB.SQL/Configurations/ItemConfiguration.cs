namespace SSMB.SQL.Configurations
{
    using System;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(e => e.ItemId);

            builder.Property(e => e.ItemId).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();

            builder.Property(e => e.Description).IsRequired();

            builder.Property(e => e.ScrapValue).HasDefaultValue(-1).IsRequired();

            builder.Property(e => e.StructuredDescription).IsRequired();

            builder.Property(e => e.Type)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),
                       v => (ItemType)Enum.Parse(typeof(ItemType), v))
                   .IsUnicode(false);

            builder.Property(e => e.Cost).IsRequired();

            builder.Property(e => e.Weight).IsRequired();

            builder.Property(e => e.Space).IsRequired();

            builder.Property(e => e.Quality)
                   .IsRequired()
                   .HasConversion(
                       v => v.ToString(),
                       v => (Quality)Enum.Parse(typeof(Quality), v))
                   .IsUnicode(false);

            builder
                .HasMany(e => e.Orders)
                .WithOne(o => o.Item)
                .IsRequired()
                .HasForeignKey(o => o.ItemId);
        }
    }
}
