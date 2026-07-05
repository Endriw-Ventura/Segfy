using AutoMapper;
using FluentAssertions;
using Moq;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.UseCases.Apolices.GetAll;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;

namespace Segfy.Application.Tests.UseCases.Apolice;

public class GetAllApolicesUseCaseTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private GetAllApoliciesUseCase CreateUseCase()
    {
        return new GetAllApoliciesUseCase(
            _apoliceRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistiremApolices()
    {
        var apolices = new List<DomainApolice>();
        var dtos = new List<ApoliceDTO>();

        _apoliceRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(apolices);

        _mapper
            .Setup(x => x.Map<IEnumerable<ApoliceDTO>>(apolices))
            .Returns(dtos);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(null, null, 1, 10);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarApolicesDTO()
    {
        var apolices = new List<DomainApolice>
        {
            CriarApolice("AP-001"),
            CriarApolice("AP-002")
        };

        var dtos = new List<ApoliceDTO>
        {
            new() { Id = apolices[0].Id, NumeroApolice = "AP-001" },
            new() { Id = apolices[1].Id, NumeroApolice = "AP-002" }
        };

        _apoliceRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(apolices);

        _mapper
            .Setup(x => x.Map<IEnumerable<ApoliceDTO>>(apolices))
            .Returns(dtos);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(null, null, 1, 10);

        result.Should().HaveCount(2);
        result.First().NumeroApolice.Should().Be("AP-001");
    }

    [Fact]
    public async Task ExecuteAsync_DevePassarFiltrosParaRepository()
    {
        var status = StatusApolice.ATIVA;
        var data = DateTime.Today;

        var apolices = new List<DomainApolice>();
        var dtos = new List<ApoliceDTO>();

        _apoliceRepository
            .Setup(x => x.GetAllAsync(status, data, 2, 20))
            .ReturnsAsync(apolices);

        _mapper
            .Setup(x => x.Map<IEnumerable<ApoliceDTO>>(apolices))
            .Returns(dtos);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(status, data, 2, 20);

        _apoliceRepository.Verify(
            x => x.GetAllAsync(status, data, 2, 20),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveNormalizarPaginacao_QuandoValoresInvalidos()
    {
        var apolices = new List<DomainApolice>();
        var dtos = new List<ApoliceDTO>();

        _apoliceRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(apolices);

        _mapper
            .Setup(x => x.Map<IEnumerable<ApoliceDTO>>(apolices))
            .Returns(dtos);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(null, null, 0, 0);

        _apoliceRepository.Verify(
            x => x.GetAllAsync(null, null, 1, 10),
            Times.Once);
    }

    private static DomainApolice CriarApolice(string numero)
    {
        return new DomainApolice(
            numero,
            "Nome",
            DateTime.Now.AddMonths(-2),
            DateTime.Now.AddMonths(2));
    }
}