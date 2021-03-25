using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    public class OrderMap : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(u => u.Id);

            builder.Property(e => e.Input).IsRequired();

            builder.Property(e => e.Output).IsRequired();
        }
    }
}