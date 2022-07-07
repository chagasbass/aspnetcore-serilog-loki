using Flunt.Notifications;
using RestoqueMinimal.Extensions.Shared.Enums;

namespace RestoqueMinimal.Extensions.Shared.Notifications
{
    public interface INotificationServices
    {
        public StatusCodeOperation StatusCode { get; set; }

        void AddNotification(Notification notification);
        void AddNotification(Notification notification, StatusCodeOperation statusCode);
        void AddNotifications(IEnumerable<Notification> notifications);
        void AddNotifications(IEnumerable<Notification> notifications, StatusCodeOperation statusCode);
        void AddStatusCode(StatusCodeOperation statusCode);
        IEnumerable<Notification> GetNotifications();
        bool HasNotifications();
        void ClearNotifications();
    }
}
