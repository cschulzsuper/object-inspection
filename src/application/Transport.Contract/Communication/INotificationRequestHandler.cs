using ChristianSchulz.ObjectInspection.Application.Communication.Requests;
using ChristianSchulz.ObjectInspection.Application.Communication.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChristianSchulz.ObjectInspection.Application.Communication;

public interface INotificationRequestHandler
{
    ValueTask<NotificationResponse> GetAsync(string inspector, int date, int time);
    IAsyncEnumerable<NotificationResponse> GetAll();
    IAsyncEnumerable<NotificationResponse> GetAllForInspector(string inspector);

    ValueTask<NotificationResponse> CreateAsync(string inspector, NotificationRequest request);
    ValueTask ReplaceAsync(string inspector, int date, int time, NotificationRequest request);
    ValueTask DeleteAsync(string inspector, int date, int time, string etag);
}