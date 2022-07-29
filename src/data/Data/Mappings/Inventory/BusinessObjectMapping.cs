using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Inventory;
using Super.Paula.Application.Setup;

namespace Super.Paula.Data.Mappings.Inventory
{
    public class BusinessObjectMapping : IEntityTypeConfiguration<BusinessObject>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;
        private readonly ExtensionProvider _extensions;

        public BusinessObjectMapping(PaulaContextState state, ExtensionProvider extensions)
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
                .Property(x => x.DisplayName)
                .HasMaxLength(140)
                .IsRequired();

            builder
                .Property(x => x.Inspector)
                .HasMaxLength(140)
                .IsRequired();

            var extension = _extensions[ExtensionTypes.BusinessObject];

            if (extension != null)
            {
                foreach (var extensionField in extension.Fields)
                {
                    var extensionFieldClrType = ExtensionFieldTypes.GetClrType(extensionField.Type);
                    var extensionFieldName = extensionField.Name;

                    builder.IndexerProperty(
                        extensionFieldClrType, 
                        extensionFieldName);
                }
            }
        }
    }
}
