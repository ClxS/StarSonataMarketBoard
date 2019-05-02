namespace SSMB.SQL.Configurations
{
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    class OrderBatchConfiguration : IEntityTypeConfiguration<OrderBatch>
    {
        public void Configure(EntityTypeBuilder<OrderBatch> builder)
        {
            builder.HasKey(e => e.BatchId);

            builder.Property(e => e.BatchId).HasColumnName("Id").ValueGeneratedOnAdd();

            builder.Property(e => e.ItemId).IsRequired();

            builder.Property(e => e.Date).IsRequired();

            builder.HasMany(e => e.Entries).WithOne(o => o.Batch).HasForeignKey("BatchId").IsRequired();

            builder.HasMany(e => e.LtsEntries).WithOne(o => o.Batch).HasForeignKey("BatchId").IsRequired();

            builder.HasOne(e => e.Item).WithMany(i => i.Orders);

            builder.Ignore(e => e.PurchaseEntries);

            builder.Ignore(e => e.SaleEntries);
        }
    }
}
