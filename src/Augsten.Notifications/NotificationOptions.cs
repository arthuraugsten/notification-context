namespace Augsten.Notifications
{
    /// <summary>
    /// This options will configure the default behavior of notification context.
    /// </summary>
    public sealed class NotificationOptions
    {
        /// <summary>
        /// This option will indicate if the context will ignore invalid messages. It will have more priority instead of <see cref="ThrowExceptionOnInvalidMessage"/>
        /// </summary>
        public bool IgnoreInvalidMessages { get; set; } = true;

        /// <summary>
        /// This option will indicate if the context will throw an exception when receive an invalid message. It will have less priority instead of <see cref="IgnoreInvalidMessages"/>
        /// </summary>
        public bool ThrowExceptionOnInvalidMessage { get; set; } = false;
    }
}
