using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Orchestration;

namespace Super.Paula.Data.Mappings.Orchestration
{
    public class WorkerMapping : IEntityTypeConfiguration<Worker>
    {
        public string PartitionKey = nameof(PartitionKey);

        public void Configure(EntityTypeBuilder<Worker> builder)
        {
            builder
                .Property<string>(PartitionKey)
                .HasValueGenerator<WorkerPartitionKeyValueGenerator>();

            builder
                 .HasKey(PartitionKey, nameof(Worker.UniqueName));

            builder
                .ToContainer("_orchestration")
                .HasPartitionKey(PartitionKey);

            builder
                .HasDiscriminator<string>("discriminator")
                .HasValue("worker");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

            builder
                .Property(x => x.UniqueName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.IterationDelay)
                .IsRequired();

            builder
                .Property(x => x.State)
                .HasMaxLength(140)
                .IsRequired();
        }
    }
}
