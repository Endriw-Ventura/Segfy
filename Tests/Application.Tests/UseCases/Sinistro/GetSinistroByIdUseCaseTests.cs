using AutoMapper;
using FluentAssertions;
using Moq;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.UseCases.Sinistros.GetSinistroById;
using Segfy.Domain.Enums;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Tests.UseCases.Sinistros;

public class GetSinistroByIdUseCaseTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private GetSinistroByIdUseCase CreateUseCase()
    {
        return new GetSinistroByIdUseCase(
            _sinistroRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNull_QuandoSinistroNaoExiste()
    {
        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsync(1))
            .ReturnsAsync((DomainSinistro?)null);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(1);

        result.Should().BeNull();

        _mapper.Verify(
            x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarDTO_QuandoSinistroExiste()
    {
        var sinistro = CriarSinistro();

        var dto = new SinistroDTO
        {
            Id = sinistro.Id,
            NumeroSinistro = "SIN-001",
            Descricao = "Colisão",
            ValorSolicitado = 5000
        };

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsync(sinistro.Id))
            .ReturnsAsync(sinistro);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(sinistro))
            .Returns(dto);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(sinistro.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(sinistro.Id);
        result.NumeroSinistro.Should().Be("SIN-001");

        _mapper.Verify(
            x => x.Map<SinistroDTO>(sinistro),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarSinistroPeloIdInformado()
    {
        var sinistro = CriarSinistro();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsync(sinistro.Id))
            .ReturnsAsync(sinistro);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(sinistro))
            .Returns(new SinistroDTO { Id = sinistro.Id });

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(sinistro.Id);

        _sinistroRepository.Verify(
            x => x.GetSinistroByIdAsync(sinistro.Id),
            Times.Once);
    }

    private static DomainSinistro CriarSinistro()
    {
        var apolice = new DomainApolice(
            "AP-001",
            "João Silva",
            DateTime.Now.AddMonths(-2),
            DateTime.Now.AddMonths(10),
            Ramo.AUTOMOVEL);

        return new DomainSinistro(
            "SIN-001",
            DateTime.Now,
            "Colisão",
            5000,
            apolice);
    }
}