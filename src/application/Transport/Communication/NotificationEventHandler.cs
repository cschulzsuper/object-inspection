using Microsoft.Extensions.DependencyInjection;
using Super.Paula.Application.Administration.Events;
using Super.Paula.Application.Auditing.Events;
using Super.Paula.Application.Communication.Responses;
using Super.Paula.Shared;
using Super.Paula.Shared.Orchestration;
using System.Threading.Tasks;

namespace Super.Paula.Application.Communication;

public class NotificationEventHandler : INotificationEventHandler
{
    public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectorCreationEvent @event)
    {
        var notificationManager = context.Services.GetRequiredService<INotificationManager>();
        var notificationBroadcaster = context.Services.GetRequiredService<INotificationBroadcaster>();

        var (date, time) = DateTimeNumbers.GlobalNow;

        var notification = new Notification
        {
            Inspector = @event.Inspector,
            Date = date,
            Time = time,
            Target = $"business-objects/{@event.UniqueName}",
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

        await notificationBroadcaster.SendNotificationCreationAsync(response);

    }

    public async Task HandleAsync(EventHandlerContext context, BusinessObjectInspectorDeletionEvent @event)
    {
        var notificationManager = context.Services.GetRequiredService<INotificationManager>();
        var notificationBroadcaster = context.Services.GetRequiredService<INotificationBroadcaster>();

        var (date, time) = DateTimeNumbers.GlobalNow;

        var notification = new Notification
        {
            Inspector = @event.Inspector,
            Date = date,
            Time = time,
            Target = $"business-objects/{@event.UniqueName}",
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

        await notificationBroadcaster.SendNotificationCreationAsync(response);
    }

    public async Task HandleAsync(EventHandlerContext context, InspectorBusinessObjectImmediacyDetectionEvent @event)
    {
        var notificationManager = context.Services.GetRequiredService<INotificationManager>();
        var notificationBroadcaster = context.Services.GetRequiredService<INotificationBroadcaster>();

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

        await notificationBroadcaster.SendNotificationCreationAsync(response);
    }

    public async Task HandleAsync(EventHandlerContext context, InspectorBusinessObjectOverdueDetectionEvent @event)
    {
        var notificationManager = context.Services.GetRequiredService<INotificationManager>();
        var notificationBroadcaster = context.Services.GetRequiredService<INotificationBroadcaster>();

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

        await notificationBroadcaster.SendNotificationCreationAsync(response);
    }
}