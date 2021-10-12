using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Augsten.Notifications.Web.UnitTests
{
    public sealed class PipelineValidatorTests
    {
        private PipelineValidator<CommandMock, CommandResultMock> _pipelineValidator;
        private IEnumerable<IValidator<CommandMock>> _validators;

        private readonly Mock<INotificationContext> _notificationContext = new();
        private readonly Mock<RequestHandlerDelegate<CommandResultMock>> _delegate = new();
        private readonly CommandMock _request = new("Teste");

        public static List<object[]> CenarioValidadores { get; } = new(2)
        {
            new object[] { Enumerable.Empty<IValidator<CommandMock>>() },
            new object[] { default(List<IValidator<CommandMock>>) }
        };

        [Fact]
        public async Task Should_throw_exception_when_next_request_delegate_is_null()
        {
            _validators = Enumerable.Empty<IValidator<CommandMock>>();
            _pipelineValidator = new PipelineValidator<CommandMock, CommandResultMock>(_validators, _notificationContext.Object);

            var excecao = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _pipelineValidator.Handle(_request, default, default));

            excecao.Message.Should().Be("Value cannot be null. (Parameter 'next')");
            _notificationContext.VerifyNoOtherCalls();
            _delegate.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(CenarioValidadores))]
        public async Task Should_return_success_when_doesnt_have_validators_registered(IEnumerable<IValidator<CommandMock>> validadores)
        {
            var resultadoEsperado = new CommandResultMock();

            _validators = Enumerable.Empty<IValidator<CommandMock>>();
            _delegate.Setup(t => t.Invoke()).ReturnsAsync(resultadoEsperado);
            _pipelineValidator = new PipelineValidator<CommandMock, CommandResultMock>(validadores, _notificationContext.Object);

            var resultado = await _pipelineValidator.Handle(_request, default, _delegate.Object);

            _notificationContext.VerifyNoOtherCalls();
            _delegate.Verify(t => t.Invoke(), Times.Once);

            resultado.Should().BeSameAs(resultadoEsperado);
        }

        [Fact]
        public async Task Should_return_null_when_validation_fails()
        {
            _validators = new[] { new CommandMockValidation() };

            _pipelineValidator = new PipelineValidator<CommandMock, CommandResultMock>(_validators, _notificationContext.Object);

            var resultado = await _pipelineValidator.Handle(new(string.Empty), default, _delegate.Object);

            _notificationContext.Verify(t => t.Add(CommandMockValidation.Mensagem), Times.Once);
            _delegate.VerifyNoOtherCalls();

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_the_request_delegate_result_when_validation_doest_fail()
        {
            var resultadoEsperado = new CommandResultMock();

            _validators = new[] { new CommandMockValidation() };
            _delegate.Setup(t => t.Invoke()).ReturnsAsync(resultadoEsperado);
            _pipelineValidator = new PipelineValidator<CommandMock, CommandResultMock>(_validators, _notificationContext.Object);

            var resultado = await _pipelineValidator.Handle(_request, default, _delegate.Object);

            _notificationContext.VerifyNoOtherCalls();
            _delegate.Verify(t => t.Invoke(), Times.Once);
            resultado.Should().BeSameAs(resultadoEsperado);
        }

        public sealed record CommandMock(string Name) : IRequest<CommandResultMock>;
        public sealed record CommandResultMock { }

        public sealed class CommandMockValidation : AbstractValidator<CommandMock>
        {
            public const string Mensagem = "Message";

            public CommandMockValidation()
                => RuleFor(t => t.Name).NotEmpty().WithMessage(Mensagem);
        }
    }
}
