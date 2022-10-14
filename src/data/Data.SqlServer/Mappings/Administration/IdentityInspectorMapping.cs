using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Administration;
using System;
using IdGen;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Administration;

public class IdentityInspectorMapping : IEntityTypeConfiguration<IdentityInspector>
{
    private readonly string PartitionKey = nameof(PartitionKey);

    public void Configure(EntityTypeBuilder<IdentityInspector> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<IdentityInspectorPartitionKeyValueGenerator>();

        builder
             .HasKey(PartitionKey,
                nameof(IdentityInspector.Organization),
                nameof(IdentityInspector.Inspector));

        builder
            .ToTable(nameof(IdentityInspector),"_administration");

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
            .Property(x => x.Organization)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Inspector)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Activated)
            .IsRequired();
    }
}