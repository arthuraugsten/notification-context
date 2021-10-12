using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Augsten.Notifications.UnitTests
{
    public sealed class NotificationContextTests
    {
        private const string ErrorMessage = "You cant add an invalid message.";
        private readonly NotificationOptions _options = new();
        private readonly NotificationContext _context;

        public NotificationContextTests()
            => _context = new(_options);

        public static List<object[]> InvalidTextScenario { get; } = new()
        {
            new object[] { string.Empty },
            new object[] { " " },
            new object[] { default(string) },
        };

        [Fact]
        public void Should_initialize_an_empty_notification_list()
        {
            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(InvalidTextScenario))]
        public void Should_ignore_invalid_messages_when_IgnoreInvalidMessages_is_set(string input)
        {
            _options.IgnoreInvalidMessages = true;
            _options.ThrowExceptionOnInvalidMessage = false;

            _context.Add(input);
            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(InvalidTextScenario))]
        public void Should_throw_exception_when_IgnoreInvalidMessages_is_set(string input)
        {
            _options.IgnoreInvalidMessages = false;
            _options.ThrowExceptionOnInvalidMessage = true;

            var exception = Assert.Throws<InvalidOperationException>(() => _context.Add(input));
            exception.Message.Should().Be(ErrorMessage);

            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Fact]
        public void Should_add_new_message_when_a_valid_string()
        {
            const string input = "abc";

            _options.IgnoreInvalidMessages = true;
            _options.ThrowExceptionOnInvalidMessage = true;

            _context.Add(input);
            _context.Notifications.Should().BeEquivalentTo(new[] { new Notification(input) });
            _context.HasNotification.Should().BeTrue();
        }

        [Fact]
        public void Should_ignore_when_notification_is_null()
        {
            _options.IgnoreInvalidMessages = true;
            _options.ThrowExceptionOnInvalidMessage = false;

            _context.Add(default(Notification));
            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Fact]
        public void Should_throw_exception_when_notification_is_null()
        {
            _options.IgnoreInvalidMessages = false;
            _options.ThrowExceptionOnInvalidMessage = true;

            var exception = Assert.Throws<InvalidOperationException>(() => _context.Add(default(Notification)));
            exception.Message.Should().Be(ErrorMessage);

            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Fact]
        public void Deve_adicionar_notificacao()
        {
            _options.IgnoreInvalidMessages = true;
            _options.ThrowExceptionOnInvalidMessage = true;

            var notificacao = new Notification("abc");

            _context.Add(notificacao);
            _context.Notifications.Should().BeEquivalentTo(new[] { notificacao });
            _context.HasNotification.Should().BeTrue();
        }
    }
}
