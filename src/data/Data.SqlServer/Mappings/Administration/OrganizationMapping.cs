using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Administration;
using System.Collections.Concurrent;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Administration;

public class OrganizationMapping : IEntityTypeConfiguration<Organization>
{
    public string PartitionKey = nameof(PartitionKey);

    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<OrganizationPartitionKeyValueGenerator>();
        builder
            .HasKey(PartitionKey, nameof(Organization.UniqueName));

        builder
            .ToTable(nameof(Organization), "_administration");

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
            .Property(x => x.DisplayName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.ChiefInspector)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Activated)
            .IsRequired();
    }
}