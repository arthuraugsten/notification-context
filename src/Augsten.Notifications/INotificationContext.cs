using System.Collections.Generic;

namespace Augsten.Notifications
{
    /// <summary>
    /// Represents a notification context. It stores notification objects created during the request scope to represent an error state.
    /// A context can stores multiples notifications for each request.
    /// This object has a request scope. At the end of the request, all data will be discarded.
    /// </summary>
    public interface INotificationContext
    {
        /// <summary>
        /// A bool value indicating if the current context has stored any notification.
        /// </summary>
        bool HasNotification { get; }

        /// <summary>
        /// A list of <see cref="Notification" /> objects. It stores the objects created during the request scope.
        /// </summary>
        IReadOnlyCollection<Notification> Notifications { get; }

        /// <summary>
        /// Add a notification in the context of the current request.
        /// </summary>
        /// <param name="message">A message representing an error state, this message will be converted into a <see cref="Notification" /> object</param>
        void Add(string message);

        /// <summary>
        /// Add a notification object in the context of the current request.
        /// </summary>
        /// <param name="item">An <see cref="Notification" /> object that represents an error state in the current request.</param>
        void Add(Notification notification);
    }
}
