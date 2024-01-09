using FluentValidator;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorData
    {
        public ErrorCode? ErrorCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;


        public static explicit operator ErrorData(Notification notification)
        {
            return new ErrorData
            {
                Message = notification.Message,
                Path = notification.Property
            };
        }
    }
}
