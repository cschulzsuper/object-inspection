using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

public class EventProcessingMapping : IEntityTypeConfiguration<EventProcessing>
{
    public string PartitionKey = nameof(PartitionKey);

    public void Configure(EntityTypeBuilder<EventProcessing> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<EventProcessingPartitionKeyValueGenerator>();

        builder
             .HasKey(PartitionKey, nameof(EventProcessing.EventId), nameof(EventProcessing.Subscriber), nameof(EventProcessing.Name));

        builder
            .ToTable(nameof(EventProcessing), "_orchestration");

        builder
            .Property(x => x.Id)
            .HasValueGenerator<IdValueGenerator>()
            ;

        builder
            .Property(x => x.ETag)
            .IsRowVersion()
            .HasConversion(
                v => Convert.FromBase64String(v),
                v => Convert.ToBase64String(v));

        builder
            .Property(x => x.EventId)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Name)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Subscriber)
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