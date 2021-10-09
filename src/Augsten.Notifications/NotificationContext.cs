using System.Collections.Generic;

namespace Augsten.Notifications
{
    public sealed class NotificationContext : INotificationContext
    {
        private readonly List<Notification> _notifications = new();

        public bool HasNotification { get; private set; }
        public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

        public void Add(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            _notifications.Add(new Notification(message));
            HasNotification = true;
        }

        public void Add(Notification notification)
        {
            if (notification is null)
                return;

            _notifications.Add(notification);
            HasNotification = true;
        }
    }
}
