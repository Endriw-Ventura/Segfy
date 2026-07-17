using AutoMapper;
using FluentAssertions;
using Moq;
using segfy.Domain.Entities;
using Segfy.Application.Apolices.Commands.CreateApolice;
using Segfy.Application.Apolices.Results;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolices;
using Segfy.Domain.Enums;
using Xunit;

namespace Tests.Application.Tests.Apolices
{
    public sealed class CreateApoliceCommandHandlerTests
    {
        private readonly Mock<IApoliceRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateApoliceCommandHandler _handler;

        public CreateApoliceCommandHandlerTests()
        {
            _repositoryMock = new Mock<IApoliceRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateApoliceCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCommandIsValid_ShouldCreateApoliceCorrectly()
        {
            var dataAtual = new DateTime(2026, 7, 17);
            var command = new CreateApoliceCommand(
                "001",
                "João",
                dataAtual.AddMonths(-2),
                dataAtual.AddMonths(1),
                Ramo.AUTOMOVEL
                );


            _repositoryMock
                    .Setup(repository => repository.AddApoliceAsync(
                        It.IsAny<Apolice>(),
                        It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

            _mapperMock
                .Setup(mapper => mapper.Map<CreateApoliceResult>(
                    It.IsAny<Apolice>()))
                        .Returns((Apolice apolice) => new CreateApoliceResult(
                            apolice.Id,
                            apolice.NumeroApolice,
                            apolice.NomeSegurado)
                        );

            _unitOfWorkMock
                    .Setup(unitOfWork => unitOfWork.SaveChangesAsync(
                        It.IsAny<CancellationToken>()))
                            .Returns(Task<int>.FromResult(1));

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.NumeroApolice.Should().Be(command.NumeroApolice);
            result.NomeSegurado.Should().Be(command.NomeSegurado);

            _repositoryMock.Verify(
                repository => repository.AddApoliceAsync(
                    It.Is<Apolice>(apolice =>
                        apolice.NumeroApolice == command.NumeroApolice &&
                        apolice.NomeSegurado == command.NomeSegurado &&
                        apolice.DataInicio == command.DataInicio &&
                        apolice.DataFim == command.DataFim &&
                        apolice.Ramo == command.Ramo),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _unitOfWorkMock.Verify(
                unitOfWork => unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _mapperMock.Verify(
                mapper => mapper.Map<CreateApoliceResult>(
                    It.IsAny<Apolice>()),
                Times.Once);
        }
    }
}
