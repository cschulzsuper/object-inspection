using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Super.Paula.Application.Communication;

namespace Super.Paula.Data.Mappings.Communication
{
    public class NotificationMapping : IEntityTypeConfiguration<Notification>
    {
        public string PartitionKey = nameof(PartitionKey);

        private readonly PaulaContextState _state;

        public NotificationMapping(PaulaContextState state)
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
                .ToContainer(_state.CurrentOrganization)
                .HasPartitionKey(PartitionKey);

            builder
                .HasDiscriminator<string>("discriminator")
                .HasValue("notification");

            builder
                .Property(x => x.ETag)
                .IsETagConcurrency();

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
}
