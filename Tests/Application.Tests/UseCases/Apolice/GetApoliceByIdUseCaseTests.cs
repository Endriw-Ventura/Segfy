using AutoMapper;
using FluentAssertions;
using Moq;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.UseCases.Apolices.GetApoliceById;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;

namespace Segfy.Application.Tests.UseCases.Apolice;

public class GetApoliceByIdUseCaseTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private GetApoliceByIdUseCase CreateUseCase()
    {
        return new GetApoliceByIdUseCase(
            _apoliceRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNull_QuandoApoliceNaoExiste()
    {
        // Arrange
        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(123))
            .ReturnsAsync((DomainApolice?)null);

        var useCase = CreateUseCase();

        // Act
        var result = await useCase.ExecuteAsync(123);

        // Assert
        result.Should().BeNull();

        _mapper.Verify(
            x => x.Map<ApoliceDTO>(It.IsAny<DomainApolice>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarApoliceDTO_QuandoApoliceExiste()
    {
        // Arrange
        var apolice = CriarApolice();

        var dto = new ApoliceDTO
        {
            Id = apolice.Id,
            NumeroApolice = "123"
        };

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(apolice.Id))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceDTO>(apolice))
            .Returns(dto);

        var useCase = CreateUseCase();

        // Act
        var result = await useCase.ExecuteAsync(apolice.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(apolice.Id);
        result.NumeroApolice.Should().Be("123");

        _mapper.Verify(
            x => x.Map<ApoliceDTO>(apolice),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarApolicePeloIdInformado()
    {
        // Arrange
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(apolice.Id))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceDTO>(apolice))
            .Returns(new ApoliceDTO { Id = apolice.Id });

        var useCase = CreateUseCase();

        // Act
        await useCase.ExecuteAsync(apolice.Id);

        // Assert
        _apoliceRepository.Verify(
            x => x.GetApoliceByIdAsync(apolice.Id),
            Times.Once);
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