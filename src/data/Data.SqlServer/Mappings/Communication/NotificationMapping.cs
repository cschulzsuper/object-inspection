using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Communication;
using System;
using ChristianSchulz.ObjectInspection.Data.SqlServer.Mappings;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Communication;

public class NotificationMapping : IEntityTypeConfiguration<Notification>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;

    public NotificationMapping(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<NotificationPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey,
                nameof(Notification.Date),
                nameof(Notification.Time));

        builder
            .ToTable(nameof(Notification), _state.CurrentOrganization);

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
            .Property(x => x.Target)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Text)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.Date)
            .IsRequired();

        builder
            .Property(x => x.Time)
            .IsRequired();
    }
}