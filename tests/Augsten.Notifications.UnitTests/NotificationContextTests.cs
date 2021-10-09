using FluentAssertions;
using Xunit;

namespace Augsten.Notifications.UnitTests
{
    public sealed class NotificationContextTests
    {
        private readonly NotificationContext _context = new();

        [Fact]
        public void Deve_inicializar_lista_interna_de_notificacoes()
        {
            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Deve_ignorar_strings_invalidas_na_adicao_de_mensagens(string input)
        {
            _context.Add(input);
            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Fact]
        public void Deve_adicionar_mensagem_por_string()
        {
            const string input = "abc";

            _context.Add(input);
            _context.Notifications.Should().BeEquivalentTo(new[] { new Notification(input) });
            _context.HasNotification.Should().BeTrue();
        }

        [Fact]
        public void Deve_ignorar_notificacao_nula_ao_adicionar()
        {
            _context.Add(default(Notification));
            _context.Notifications.Should().BeEmpty();
            _context.HasNotification.Should().BeFalse();
        }

        [Fact]
        public void Deve_adicionar_notificacao()
        {
            var notificacao = new Notification("abc");

            _context.Add(notificacao);
            _context.Notifications.Should().BeEquivalentTo(new[] { notificacao });
            _context.HasNotification.Should().BeTrue();
        }
    }
}
