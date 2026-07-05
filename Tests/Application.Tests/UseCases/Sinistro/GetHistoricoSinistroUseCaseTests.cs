using AutoMapper;
using FluentAssertions;
using Moq;
using Segfy.Application.DTOs.HistoricoSinistro;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.UseCases.Sinistros.GetHistoricoSinistro;
using Xunit;
using DomainHistorico = segfy.Domain.Entities.HistoricoSinistros;

namespace Segfy.Application.Tests.UseCases.Sinistros;

public class GetHistoricoSinistroUseCaseTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private GetHistoricoSinistroUseCase CreateUseCase()
    {
        return new GetHistoricoSinistroUseCase(
            _sinistroRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistirHistorico()
    {
        var historico = new List<DomainHistorico>();
        var dtos = new List<HistoricoSinistroDTO>();

        _sinistroRepository
            .Setup(x => x.GetHistoricoSinistro(1))
            .ReturnsAsync(historico);

        _mapper
            .Setup(x => x.Map<IEnumerable<HistoricoSinistroDTO>>(historico))
            .Returns(dtos);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(1);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarHistoricoDTO()
    {
        var historico = new List<DomainHistorico>
        {
            CriarHistorico(),
            CriarHistorico()
        };

        var dtos = new List<HistoricoSinistroDTO>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        _sinistroRepository
            .Setup(x => x.GetHistoricoSinistro(1))
            .ReturnsAsync(historico);

        _mapper
            .Setup(x => x.Map<IEnumerable<HistoricoSinistroDTO>>(historico))
            .Returns(dtos);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(1);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarHistoricoPeloSinistroIdInformado()
    {
        var historico = new List<DomainHistorico>();

        _sinistroRepository
            .Setup(x => x.GetHistoricoSinistro(10))
            .ReturnsAsync(historico);

        _mapper
            .Setup(x => x.Map<IEnumerable<HistoricoSinistroDTO>>(historico))
            .Returns(new List<HistoricoSinistroDTO>());

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(10);

        _sinistroRepository.Verify(
            x => x.GetHistoricoSinistro(10),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveChamarMapperUmaVez()
    {
        var historico = new List<DomainHistorico>
        {
            CriarHistorico()
        };

        var dtos = new List<HistoricoSinistroDTO>
        {
            new() { Id = 1 }
        };

        _sinistroRepository
            .Setup(x => x.GetHistoricoSinistro(1))
            .ReturnsAsync(historico);

        _mapper
            .Setup(x => x.Map<IEnumerable<HistoricoSinistroDTO>>(historico))
            .Returns(dtos);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(1);

        _mapper.Verify(
            x => x.Map<IEnumerable<HistoricoSinistroDTO>>(historico),
            Times.Once);
    }

    private static DomainHistorico CriarHistorico()
    {
        return new DomainHistorico(
            1,
            null,
            segfy.Domain.Enums.StatusSinistro.ABERTO,
            "Sinistro aberto.");
    }
}