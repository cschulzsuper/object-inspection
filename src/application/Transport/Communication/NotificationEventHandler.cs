using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Application.Inventory.Events;
using Super.Paula.Application.Orchestration;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication
{
    public class NotificationEventHandler : INotificationEventHandler
    {
        public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectorEvent @event)
        {
            var notificationManager = context.Services.GetRequiredService<INotificationManager>();
            var notificationAnnouncer = context.Services.GetRequiredService<INotificationAnnouncer>();

            var (date, time) = DateTimeNumbers.GlobalNow;

            if (!string.IsNullOrWhiteSpace(@event.OldInspector) &&
                @event.OldInspector != @event.NewInspector)
            {
                var notification = new Notification
                {
                    Inspector = @event.OldInspector,
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{@event.UnqiueName}",
                    Text = $"You are not longer the inspector for {@event.DisplayName}!"
                };

                await notificationManager.InsertAsync(notification);

                var response = new NotificationResponse
                {
                    Date = notification.Date,
                    Time = notification.Time,
                    Text = notification.Text,
                    Inspector = notification.Inspector,
                    Target = notification.Target
                };

                await notificationAnnouncer.AnnounceCreationAsync(response);
            }

            if (!string.IsNullOrWhiteSpace(@event.NewInspector) &&
                @event.NewInspector != @event.OldInspector)
            {
                var notification = new Notification
                {
                    Inspector = @event.NewInspector,
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{@event.UnqiueName}",
                    Text = $"You are now the inspector for {@event.DisplayName}!"
                };

                await notificationManager.InsertAsync(notification);

                var response = new NotificationResponse
                {
                    Date = notification.Date,
                    Time = notification.Time,
                    Text = notification.Text,
                    Inspector = notification.Inspector,
                    Target = notification.Target
                };

                await notificationAnnouncer.AnnounceCreationAsync(response);
            }
        }

        public async Task HandleAsync(EventHandlerContext context, InspectorBusinessObjectEvent @event)
        {
            var notificationManager = context.Services.GetRequiredService<INotificationManager>();
            var notificationAnnouncer = context.Services.GetRequiredService<INotificationAnnouncer>();

            if (@event.NewAuditSchedulePending == true &&
                @event.NewAuditSchedulePending != @event.OldAuditSchedulePending &&
                @event.NewAuditScheduleDelayed == false &&
                @event.OldAuditScheduleDelayed == false)
            {
                var (date, time) = DateTimeNumbers.GlobalNow;

                var notification = new Notification
                {
                    Inspector = @event.UniqueName,
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{@event.BusinessObject}",
                    Text = $"An inspection audit for {@event.BusinessObjectDisplayName} imminent!"
                };

                await notificationManager.InsertAsync(notification);

                var response = new NotificationResponse
                {
                    Date = notification.Date,
                    Time = notification.Time,
                    Text = notification.Text,
                    Inspector = notification.Inspector,
                    Target = notification.Target
                };

                await notificationAnnouncer.AnnounceCreationAsync(response);
            }

            if (@event.NewAuditScheduleDelayed == true &&
                @event.NewAuditScheduleDelayed != @event.OldAuditScheduleDelayed)
            {

                var (date, time) = DateTimeNumbers.GlobalNow;

                var notification = new Notification
                {
                    Inspector = @event.UniqueName,
                    Date = date,
                    Time = time,
                    Target = $"business-objects/{@event.BusinessObject}",
                    Text = $"An inspection audit for {@event.BusinessObjectDisplayName} overdue!"
                };

                await notificationManager.InsertAsync(notification);

                var response = new NotificationResponse
                {
                    Date = notification.Date,
                    Time = notification.Time,
                    Text = notification.Text,
                    Inspector = notification.Inspector,
                    Target = notification.Target
                };

                await notificationAnnouncer.AnnounceCreationAsync(response);
            }
        }
    }
}
