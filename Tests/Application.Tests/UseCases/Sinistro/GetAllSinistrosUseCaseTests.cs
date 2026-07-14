using AutoMapper;
using FluentAssertions;
using Moq;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.UseCases.Sinistros.GetAllSinistros;
using Segfy.Domain.Enums;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Tests.UseCases.Sinistros;

public class GetAllSinistrosUseCaseTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private GetAllSinistrosUseCase CreateUseCase()
    {
        return new GetAllSinistrosUseCase(
            _sinistroRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistiremSinistros()
    {
        var sinistros = new List<DomainSinistro>();
        var dtos = new List<GetSinistroDTO>();

        _sinistroRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(sinistros);

        _mapper
            .Setup(x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()))
            .Returns(dtos);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(null, null, 1, 10);

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _sinistroRepository.Verify(
            x => x.GetAllAsync(null, null, 1, 10),
            Times.Once);

        _mapper.Verify(
            x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSinistrosDTO()
    {
        var sinistros = new List<DomainSinistro>
        {
            CriarSinistro("SIN-001"),
            CriarSinistro("SIN-002")
        };

        var dtos = new List<GetSinistroDTO>
        {
            new()
            {
                Id = sinistros[0].Id,
                NumeroSinistro = "SIN-001"
            },
            new()
            {
                Id = sinistros[1].Id,
                NumeroSinistro = "SIN-002"
            }
        };

        _sinistroRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(sinistros);

        _mapper
            .Setup(x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()))
            .Returns(dtos);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(null, null, 1, 10);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(dtos);

        result.Should().ContainSingle(x =>
            x.NumeroSinistro == "SIN-001");

        result.Should().ContainSingle(x =>
            x.NumeroSinistro == "SIN-002");
    }

    [Fact]
    public async Task ExecuteAsync_DevePassarFiltrosParaRepository()
    {
        var status = StatusSinistro.EM_ANALISE;
        var data = DateTime.Today;

        var sinistros = new List<DomainSinistro>();
        var dtos = new List<GetSinistroDTO>();

        _sinistroRepository
            .Setup(x => x.GetAllAsync(status, data, 2, 20))
            .ReturnsAsync(sinistros);

        _mapper
            .Setup(x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()))
            .Returns(dtos);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(status, data, 2, 20);

        _sinistroRepository.Verify(
            x => x.GetAllAsync(status, data, 2, 20),
            Times.Once);

        _sinistroRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ExecuteAsync_DeveNormalizarPaginacao_QuandoValoresInvalidos()
    {
        var sinistros = new List<DomainSinistro>();
        var dtos = new List<GetSinistroDTO>();

        _sinistroRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(sinistros);

        _mapper
            .Setup(x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()))
            .Returns(dtos);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(null, null, 0, 0);

        _sinistroRepository.Verify(
            x => x.GetAllAsync(null, null, 1, 10),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveChamarMapperUmaVez()
    {
        var sinistros = new List<DomainSinistro>
        {
            CriarSinistro("SIN-001")
        };

        var dtos = new List<GetSinistroDTO>
        {
            new()
            {
                Id = sinistros[0].Id,
                NumeroSinistro = "SIN-001"
            }
        };

        _sinistroRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(sinistros);

        _mapper
            .Setup(x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()))
            .Returns(dtos);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(null, null, 1, 10);

        _mapper.Verify(
            x => x.Map<IEnumerable<GetSinistroDTO>>(
                It.IsAny<IEnumerable<DomainSinistro>>()),
            Times.Once);
    }

    private static DomainSinistro CriarSinistro(string numeroSinistro)
    {
        var apolice = new DomainApolice(
            "AP-001",
            "João Silva",
            DateTime.Now.AddMonths(-2),
            DateTime.Now.AddMonths(10),
            Ramo.AUTOMOVEL);

        return new DomainSinistro(
            numeroSinistro,
            DateTime.Now,
            "Colisão",
            5000,
            apolice);
    }
}
