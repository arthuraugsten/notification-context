using System.Diagnostics.CodeAnalysis;

namespace Augsten.NotificationContext
{
    /// <summary>
    /// The object represents a notification for an error state during the request.
    /// </summary>
    public sealed class Notification
    {
        /// <summary>
        /// Construct a notification object representing an error with the specified message.
        /// </summary>
        /// <param name="message">Specify an error message for the notification.</param>
        public Notification([AllowNull] string? message)
            => Message = message;

        /// <summary>
        /// Construct a notification object representing an error with the code and message specified.
        /// </summary>
        /// <param name="code">Specify an error code for the notification.</param>
        /// <param name="message">Specify an error message for the notification.</param>
        public Notification([AllowNull] string? code, [AllowNull] string? message) : this(message)
            => Code = code;

        /// <summary>
        /// Represents a code for the error.
        /// </summary>
        public string? Code { get; }

        /// <summary>
        /// Represents a message for the error.
        /// </summary>
        public string? Message { get; }
    }
}
