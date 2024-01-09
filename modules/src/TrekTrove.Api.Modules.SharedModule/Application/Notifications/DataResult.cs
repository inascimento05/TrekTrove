using FluentValidator;

namespace TrekTrove.Api.Modules.SharedModule.Application.Notifications
{
    public class DataResult<T> : Result
    {
        public T? Data { get; set; }

        public DataResult() : base()
        {
        }

        public DataResult(IReadOnlyCollection<Notification> notifications)
            : base(notifications)
        {
        }

        public DataResult(IReadOnlyCollection<Notification> notifications, T data)
           : this(notifications)
        {
            Data = data;
        }
    }
}
