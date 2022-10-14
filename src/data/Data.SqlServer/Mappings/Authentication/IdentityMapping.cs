using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Authentication;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Authentication;

public class IdentityMapping : IEntityTypeConfiguration<Identity>
{
    public string PartitionKey = nameof(PartitionKey);

    public void Configure(EntityTypeBuilder<Identity> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<IdentityPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(Identity.UniqueName));

        builder
            .ToTable(nameof(Identity), "_auth");

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
            .Property(x => x.MailAddress)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Secret)
            .HasMaxLength(140)
            .IsRequired();
    }
}