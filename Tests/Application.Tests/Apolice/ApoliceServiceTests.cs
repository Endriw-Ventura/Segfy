using AutoMapper;
using FluentAssertions;
using Moq;
using segfy.Domain.Entities;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Services.Apolice;
using Xunit;

namespace Segfy.Application.Tests.Services;

public class ApoliceServiceTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();

    private ApoliceService CreateService()
    {
        return new ApoliceService(
            _apoliceRepository.Object,
            _unitOfWork.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task CriarApoliceAsync_DeveCriarApolice_ComSucesso()
    {
        // Arrange
        var request = new CreateApoliceDTO
        {
            NumeroApolice = "AP-001",
            DataInicio = DateTime.UtcNow,
            DataFim = DateTime.UtcNow.AddYears(1)
        };

        var apolice = CriarApolice();
        var apoliceDto = new ApoliceDTO { Id = apolice.Id };

        _mapper
            .Setup(x => x.Map<Apolice>(request))
            .Returns(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceDTO>(apolice))
            .Returns(apoliceDto);

        var service = CreateService();

        // Act
        var result = await service.CriarApoliceAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(apolice.Id);

        _apoliceRepository.Verify(
            x => x.AddApoliceAsync(apolice),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveAtivarApolice()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var service = CreateService();

        // Act
        await service.AtualizarStatusAsync(apolice.Id, StatusApolice.ATIVA);

        // Assert
        apolice.Status.Should().Be(StatusApolice.ATIVA);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveCancelarApolice()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var service = CreateService();

        // Act
        await service.AtualizarStatusAsync(apolice.Id, StatusApolice.CANCELADA);

        // Assert
        apolice.Status.Should().Be(StatusApolice.CANCELADA);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveExpirarApolice()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(apolice.Id))
            .ReturnsAsync(apolice);

        var service = CreateService();

        // Act
        await service.AtualizarStatusAsync(apolice.Id, StatusApolice.EXPIRADA);

        // Assert
        apolice.Status.Should().Be(StatusApolice.EXPIRADA);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveLancarException_QuandoApoliceNaoExiste()
    {
        // Arrange
        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsyncTracked(99))
            .ReturnsAsync((Apolice?)null);

        var service = CreateService();

        // Act
        Func<Task> act = async () =>
            await service.AtualizarStatusAsync(99, StatusApolice.ATIVA);

        // Assert
        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Apolice não encontrada.");

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeApoliceDTO()
    {
        // Arrange
        var apolices = new List<Apolice>
        {
            CriarApolice(),
            CriarApolice()
        };

        var dtos = new List<ApoliceDTO>
        {
            new() { Id = apolices[0].Id },
            new() { Id = apolices[1].Id }
        };

        _apoliceRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(apolices);

        _mapper
            .Setup(x => x.Map<IEnumerable<ApoliceDTO>>(apolices))
            .Returns(dtos);

        var service = CreateService();

        // Act
        var result = await service.GetAllAsync(null, null, 1, 10);

        // Assert
        result.Should().HaveCount(2);

        _apoliceRepository.Verify(
            x => x.GetAllAsync(null, null, 1, 10),
            Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_DeveNormalizarPaginacao_QuandoValoresInvalidos()
    {
        // Arrange
        var apolices = new List<Apolice>();

        _apoliceRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(apolices);

        _mapper
            .Setup(x => x.Map<IEnumerable<ApoliceDTO>>(apolices))
            .Returns(new List<ApoliceDTO>());

        var service = CreateService();

        // Act
        var result = await service.GetAllAsync(null, null, 0, 0);

        // Assert
        result.Should().BeEmpty();

        _apoliceRepository.Verify(
            x => x.GetAllAsync(null, null, 1, 10),
            Times.Once);
    }

    [Fact]
    public async Task GetApoliceByIdAsync_DeveRetornarNull_QuandoNaoEncontrada()
    {
        // Arrange
        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(1))
            .ReturnsAsync((Apolice?)null);

        var service = CreateService();

        // Act
        var result = await service.GetApoliceByIdAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetApoliceByIdAsync_DeveRetornarDTO_QuandoEncontrada()
    {
        // Arrange
        var apolice = CriarApolice();
        var dto = new ApoliceDTO { Id = apolice.Id };

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(apolice.Id))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceDTO?>(apolice))
            .Returns(dto);

        var service = CreateService();

        // Act
        var result = await service.GetApoliceByIdAsync(apolice.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(apolice.Id);
    }

    [Fact]
    public async Task GetApoliceComSinistrosAsync_DeveRetornarNull_QuandoNaoEncontrada()
    {
        // Arrange
        _apoliceRepository
            .Setup(x => x.GetByIdWithSinistrosAsync(1))
            .ReturnsAsync((Apolice?)null);

        var service = CreateService();

        // Act
        var result = await service.GetApoliceComSinistrosAsync(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetApoliceComSinistrosAsync_DeveRetornarDTO_QuandoEncontrada()
    {
        // Arrange
        var apolice = CriarApolice();
        var dto = new ApoliceComSinistrosDTO { Id = apolice.Id };

        _apoliceRepository
            .Setup(x => x.GetByIdWithSinistrosAsync(apolice.Id))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceComSinistrosDTO>(apolice))
            .Returns(dto);

        var service = CreateService();

        // Act
        var result = await service.GetApoliceComSinistrosAsync(apolice.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(apolice.Id);
    }

    private static Apolice CriarApolice()
    {
        return new Apolice(
             "123",
            "Nome",
            DateTime.UtcNow,
            DateTime.UtcNow.AddYears(1)
        );
    }
}