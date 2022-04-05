using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Data.Mappings.Orchestration
{
    public class ContinuationMapping : IEntityTypeConfiguration<Continuation>
    {
        public string PartitionKey = nameof(PartitionKey);

        public void Configure(EntityTypeBuilder<Continuation> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<WorkerPartitionKeyValueGenerator>();

            builder
                 .HasKey(PartitionKey, nameof(Continuation.Id));

            builder
                .ToContainer("_orchestration")
                .HasPartitionKey(PartitionKey);

            builder
                .HasDiscriminator<string>("discriminator")
                .HasValue("continuation");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

            builder
                .Property(x => x.Id)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Name)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.OperationId)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Data)
                .HasMaxLength(4000)
                .IsRequired();

            builder
                .Property(x => x.State)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.User)
                .HasMaxLength(4000)
                .IsRequired();

            builder
                .Property(x => x.CreationDate)
                .IsRequired();

            builder
                .Property(x => x.CreationTime)
                .IsRequired();
        }
    }
}
