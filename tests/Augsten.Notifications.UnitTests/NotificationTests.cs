using FluentAssertions;
using Xunit;

namespace Augsten.Notifications.UnitTests
{
    public sealed class NotificationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("abc")]
        public void Should_build_correctly_when_passing_message_only(string message)
        {
            var notification = new Notification(message);
            notification.Message.Should().Be(message);
        }

        [Theory]
        [InlineData("", "abc")]
        [InlineData(" ", null)]
        [InlineData(null, " ")]
        [InlineData("abc", "")]
        public void Should_build_correctly_when_passing_code_and_message(string code, string message)
        {
            var notification = new Notification(code, message);
            notification.Should().BeEquivalentTo(new { Code = code, Message = message });
        }
    }
}
