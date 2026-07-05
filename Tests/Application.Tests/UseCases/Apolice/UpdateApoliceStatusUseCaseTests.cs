using FluentAssertions;
using Moq;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.UseCases.Apolices.UpdateApoliceStatus;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;

namespace Segfy.Application.Tests.UseCases.Apolice;

public class UpdateApoliceStatusUseCaseTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private UpdateApoliceStatusUseCase CreateUseCase()
    {
        return new UpdateApoliceStatusUseCase(
            _apoliceRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarException_QuandoApoliceNaoExiste()
    {
        // Arrange
        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(123))
            .ReturnsAsync((DomainApolice?)null);

        var useCase = CreateUseCase();

        // Act
        Func<Task> act = async () =>
            await useCase.ExecuteAsync(123, StatusApolice.EXPIRADA);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Apolice não encontrada.");

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveAtivarApolice()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var useCase = CreateUseCase();

        // Act
        await useCase.ExecuteAsync(apolice.Id, StatusApolice.ATIVA);

        // Assert
        apolice.Status.Should().Be(StatusApolice.ATIVA);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveExpirarApolice()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var useCase = CreateUseCase();

        // Act
        await useCase.ExecuteAsync(apolice.Id, StatusApolice.EXPIRADA);

        // Assert
        apolice.Status.Should().Be(StatusApolice.EXPIRADA);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveCancelarApolice()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var useCase = CreateUseCase();

        // Act
        await useCase.ExecuteAsync(apolice.Id, StatusApolice.CANCELADA);

        // Assert
        apolice.Status.Should().Be(StatusApolice.CANCELADA);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarApolicePeloIdInformado()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var useCase = CreateUseCase();

        // Act
        await useCase.ExecuteAsync(apolice.Id, StatusApolice.EXPIRADA);

        // Assert
        _apoliceRepository.Verify(
            x => x.GetApoliceByIdAsyncTracked(apolice.Id),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveSalvar_QuandoStatusForInvalido()
    {
        // Arrange
        var apolice = CriarApolice();
        var statusInvalido = (StatusApolice)999;

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var useCase = CreateUseCase();

        // Act
        Func<Task> act = async () =>
            await useCase.ExecuteAsync(apolice.Id, statusInvalido);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Status inválido.");

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    private static DomainApolice CriarApolice()
    {
        return new DomainApolice(
            "123",
            "Nome",
            DateTime.Now.AddMonths(-2),
            DateTime.Now.AddMonths(2));
    }
}