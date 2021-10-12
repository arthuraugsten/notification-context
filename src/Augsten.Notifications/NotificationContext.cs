using System;
using System.Collections.Generic;

namespace Augsten.Notifications
{
    public sealed class NotificationContext : INotificationContext
    {
        private readonly List<Notification> _notifications = new List<Notification>();
        private readonly NotificationOptions _options;

        public NotificationContext(NotificationOptions options)
            => _options = options;

        public bool HasNotification { get; private set; }
        public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

        public void Add(string message)
        {
            if (!ValidaMessage(() => string.IsNullOrWhiteSpace(message)))
                return;

            _notifications.Add(new Notification(message));
            HasNotification = true;
        }

        public void Add(Notification notification)
        {
            if (!ValidaMessage(() => notification is null))
                return;

            _notifications.Add(notification);
            HasNotification = true;
        }

        private bool ValidaMessage(Func<bool> validator)
        {
            var invalidMessage = validator.Invoke();
            if (invalidMessage && _options.IgnoreInvalidMessages)
                return false;

            if (invalidMessage && _options.ThrowExceptionOnInvalidMessage)
                throw new InvalidOperationException("You cant add an invalid message.");

            return true;
        }
    }
}
