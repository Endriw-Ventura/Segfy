using AutoMapper;
using FluentAssertions;
using Moq;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.UseCases.Apolices.GetApoliceWithSinistros;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;

namespace Segfy.Application.Tests.UseCases.Apolice;

public class GetApoliceWithSinistrosUseCaseTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private GetApoliceWithSinistrosUseCase CreateUseCase()
    {
        return new GetApoliceWithSinistrosUseCase(
            _apoliceRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNull_QuandoApoliceNaoExiste()
    {
        _apoliceRepository
            .Setup(x => x.GetByIdWithSinistrosAsync(123))
            .ReturnsAsync((DomainApolice?)null);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(123);

        result.Should().BeNull();

        _mapper.Verify(
            x => x.Map<ApoliceComSinistrosDTO>(It.IsAny<DomainApolice>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarDTO_QuandoApoliceExiste()
    {
        var apolice = CriarApolice();

        var dto = new ApoliceComSinistrosDTO
        {
            Id = apolice.Id,
            NumeroApolice = "123"
        };

        _apoliceRepository
            .Setup(x => x.GetByIdWithSinistrosAsync(apolice.Id))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceComSinistrosDTO>(apolice))
            .Returns(dto);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(apolice.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(apolice.Id);
        result.NumeroApolice.Should().Be("123");

        _mapper.Verify(
            x => x.Map<ApoliceComSinistrosDTO>(apolice),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarApoliceComSinistrosPeloIdInformado()
    {
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetByIdWithSinistrosAsync(apolice.Id))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<ApoliceComSinistrosDTO>(apolice))
            .Returns(new ApoliceComSinistrosDTO { Id = apolice.Id });

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(apolice.Id);

        _apoliceRepository.Verify(
            x => x.GetByIdWithSinistrosAsync(apolice.Id),
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