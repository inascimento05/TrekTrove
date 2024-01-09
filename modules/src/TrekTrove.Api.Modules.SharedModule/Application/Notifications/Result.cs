using FluentValidator;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Modules.SharedModule.Application.Notifications
{
    [ExcludeFromCodeCoverage]
    public class Result : Notifiable
    {
        public ErrorCode? Error { get; set; }

        public Result()
        {
            Error = null;
        }

        public Result(IReadOnlyCollection<Notification> notifications)
            : this()
        {
            AddNotifications(notifications);
        }

        public Result(IReadOnlyCollection<Notification> notifications, ErrorCode error)
            : this(notifications)
        {
            Error = error;
            AddNotifications(notifications);
        }

        public void AddNotification(string error)
        {
            AddNotification(null, error);
        }

        public void AddNotification(string error, ErrorCode errorCode)
        {
            AddNotification(null, error);
            Error = errorCode;
        }

        public void AddNotification(string error, ErrorCode code, ErrorCode errorCode)
        {
            AddNotification(string.Empty, error, code);
            Error = errorCode;
        }

        public void AddNotification(string property, string message, ErrorCode errorCode)
        {
            AddNotification(property, message);
            Error = errorCode;
        }

        public void AddNotification(Notification notification, ErrorCode errorCode)
        {
            AddNotification(notification);
            Error = errorCode;
        }

        public void AddNotifications(IReadOnlyCollection<Notification> notifications, ErrorCode errorCode)
        {
            AddNotifications(notifications);
            Error = errorCode;
        }

    }
}
