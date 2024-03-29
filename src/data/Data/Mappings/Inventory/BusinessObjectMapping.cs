﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChristianSchulz.ObjectInspection.Application.Inventory;
using ChristianSchulz.ObjectInspection.Application.Operation;
using System.Linq;

namespace ChristianSchulz.ObjectInspection.Data.Mappings.Inventory;

public class BusinessObjectMapping : IEntityTypeConfiguration<BusinessObject>
{
    public string PartitionKey = nameof(PartitionKey);

    private readonly ObjectInspectionContextState _state;
    private readonly ExtensionProvider _extensions;

    public BusinessObjectMapping(ObjectInspectionContextState state, ExtensionProvider extensions)
    {
        _state = state;
        _extensions = extensions;
    }

    public void Configure(EntityTypeBuilder<BusinessObject> builder)
    {
        builder
            .Property<string>(PartitionKey)
            .HasValueGenerator<BusinessObjectPartitionKeyValueGenerator>();

        builder
            .HasKey(PartitionKey, nameof(BusinessObject.UniqueName));

        builder
            .ToContainer(_state.CurrentOrganization)
            .HasPartitionKey(PartitionKey);

        builder
            .HasDiscriminator<string>("discriminator")
            .HasValue("business-object");

        builder
            .Property(x => x.ETag)
            .IsETagConcurrency();

        builder
            .Property(x => x.UniqueName)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.DistinctionType)
            .HasMaxLength(140)
            .IsRequired();

        builder
            .Property(x => x.DisplayName)
            .HasMaxLength(140)
            .IsRequired();

        var extension = _extensions.Get(ExtensionAggregateTypes.BusinessObject);

        if (extension != null)
        {
            foreach (var extensionField in extension.Fields)
            {
                var extensionFieldClrType = ExtensionFieldDataTypes.GetClrType(extensionField.DataType);
                var extensionFieldDataName = extensionField.DataName;

                var propertyAlreadyPresent = builder.Metadata
                    .GetProperties()
                    .Any(x => x.Name.Equals(extensionFieldDataName, System.StringComparison.InvariantCultureIgnoreCase));

                if (!propertyAlreadyPresent)
                {
                    builder.IndexerProperty(
                        extensionFieldClrType,
                        extensionFieldDataName);
                }
            }
        }
    }
}