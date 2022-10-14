using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Orchestration;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Orchestration;

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
            .ToTable(nameof(Worker), "_orchestration");

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
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.IterationDelay)
            .IsRequired();
    }
}