using AutoMapper;
using FluentAssertions;
using Moq;
using segfy.Domain.Entities;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.HistoricoSinistro;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.Services.Sinistro;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;
using DomainSinistro = segfy.Domain.Entities.Sinistro;
namespace Tests.Application.Tests.Sinistro;

public class SinistroServiceTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();

    private SinistroService CreateService()
    {
        return new SinistroService(
            _sinistroRepository.Object,
            _apoliceRepository.Object,
            _unitOfWork.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task AbrirSinistroAsync_DeveCriarSinistro_QuandoApoliceExiste()
    {
        var apolice = new DomainApolice("123", "Nome", DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(1));

        var dto = new CreateSinistroDTO
        {
            ApoliceId = apolice.Id,
            NumeroSinistro = "SIN-001",
            DataSinistro = DateTime.UtcNow,
            Descricao = "Colisão",
            ValorSolicitado = 5000
        };

        var sinistroDto = new SinistroDTO { Id = 1 };

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(dto.ApoliceId))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()))
            .Returns(sinistroDto);

        var service = CreateService();

        var result = await service.CreateSinistroAsync(dto);

        result.Should().NotBeNull();
        result.Id.Should().Be(1);

        _sinistroRepository.Verify(
            x => x.AddSinistroAsync(It.IsAny<DomainSinistro>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AbrirSinistroAsync_NaoDeveCriarSinistro_QuandoApoliceNaoExiste()
    {
        var dto = new CreateSinistroDTO
        {
            ApoliceId = 0,
            NumeroSinistro = "SIN-001",
            DataSinistro = DateTime.UtcNow,
            Descricao = "Colisão",
            ValorSolicitado = 5000
        };

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(dto.ApoliceId))
            .ReturnsAsync((DomainApolice?)null);

        var service = CreateService();

        Func<Task> act = async () => await service.CreateSinistroAsync(dto);

        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Apolice não encontrada.");

        _sinistroRepository.Verify(
            x => x.AddSinistroAsync(It.IsAny<DomainSinistro>()),
            Times.Never);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveEnviarParaAnalise()
    {
        var sinistro = CriarSinistroValido();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var service = CreateService();

        await service.UpdateStatusAsync(
            sinistro.Id,
            StatusSinistro.EM_ANALISE,
            null,
            null);

        sinistro.Status.Should().Be(StatusSinistro.EM_ANALISE);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveAprovarSinistro()
    {
        var sinistro = CriarSinistroValido();
        sinistro.SendForAnalisys();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var service = CreateService();

        await service.UpdateStatusAsync(
            sinistro.Id,
            StatusSinistro.APROVADO,
            null,
            null);

        sinistro.Status.Should().Be(StatusSinistro.APROVADO);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveEncerrarSinistro_QuandoValorAprovadoInformado()
    {
        var sinistro = CriarSinistroValido();
        sinistro.SendForAnalisys();
        sinistro.Aprove();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var service = CreateService();

        await service.UpdateStatusAsync(
            sinistro.Id,
            StatusSinistro.ENCERRADO,
            null,
            3000);

        sinistro.Status.Should().Be(StatusSinistro.ENCERRADO);
        sinistro.ValorAprovado.Should().Be(3000);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_NaoDeveEncerrarSinistro_QuandoValorAprovadoNaoInformado()
    {
        var sinistro = CriarSinistroValido();
        sinistro.SendForAnalisys();
        sinistro.Aprove();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var service = CreateService();

        Func<Task> act = async () => await service.UpdateStatusAsync(
            sinistro.Id,
            StatusSinistro.ENCERRADO,
            null,
            null);

        await act.Should().ThrowAsync<DomainException>();

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveNegarSinistro_QuandoMotivoInformado()
    {
        var sinistro = CriarSinistroValido();
        sinistro.SendForAnalisys();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var service = CreateService();

        await service.UpdateStatusAsync(
            sinistro.Id,
            StatusSinistro.NEGADO,
            "Documentação insuficiente",
            null);

        sinistro.Status.Should().Be(StatusSinistro.NEGADO);
        sinistro.MotivoNegativa.Should().Be("Documentação insuficiente");

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task AtualizarStatusAsync_NaoDeveNegarSinistro_QuandoMotivoNaoInformado()
    {
        var sinistro = CriarSinistroValido();
        sinistro.SendForAnalisys();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var service = CreateService();

        Func<Task> act = async () => await service.UpdateStatusAsync(
            sinistro.Id,
            StatusSinistro.NEGADO,
            null,
            null);

        await act.Should().ThrowAsync<DomainException>();

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task AtualizarStatusAsync_DeveLancarException_QuandoSinistroNaoExiste()
    {
        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(99))
            .ReturnsAsync((DomainSinistro?)null);

        var service = CreateService();

        Func<Task> act = async () => await service.UpdateStatusAsync(
            99,
            StatusSinistro.EM_ANALISE,
            null,
            null);

        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Sinistro não encontrado.");

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeSinistroDTO()
    {
        var sinistros = new List<DomainSinistro>
        {
            CriarSinistroValido(),
            CriarSinistroValido()
        };

        var dtos = new List<SinistroDTO>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        };

        _sinistroRepository
            .Setup(x => x.GetAllAsync(null, null, 1, 10))
            .ReturnsAsync(sinistros);

        _mapper
            .Setup(x => x.Map<IEnumerable<SinistroDTO>>(sinistros))
            .Returns(dtos);

        var service = CreateService();

        var result = await service.GetAllAsync(null, null, 1, 10);

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSinistroByIdAsync_DeveRetornarNull_QuandoNaoEncontrado()
    {
        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsync(1))
            .ReturnsAsync((DomainSinistro?)null);

        var service = CreateService();

        var result = await service.GetSinistroByIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSinistroByIdAsync_DeveRetornarDTO_QuandoEncontrado()
    {
        var sinistro = CriarSinistroValido();
        var dto = new SinistroDTO { Id = sinistro.Id };

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsync(sinistro.Id))
            .ReturnsAsync(sinistro);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(sinistro))
            .Returns(dto);

        var service = CreateService();

        var result = await service.GetSinistroByIdAsync(sinistro.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(sinistro.Id);
    }

    [Fact]
    public async Task GetHistoricoSinistro_DeveRetornarHistoricoDTO()
    {
        var historico = new List<HistoricoSinistros>
        {
            new HistoricoSinistros(1, StatusSinistro.ABERTO, StatusSinistro.EM_ANALISE, "Enviado para análise"),
            new HistoricoSinistros(1, StatusSinistro.EM_ANALISE, StatusSinistro.APROVADO, "Aprovado"),
        };

        var historicoDto = new List<HistoricoSinistroDTO>
        {
            new() { Id = 1 }
        };

        _sinistroRepository
            .Setup(x => x.GetHistoricoSinistro(1))
            .ReturnsAsync(historico);

        _mapper
            .Setup(x => x.Map<IEnumerable<HistoricoSinistroDTO>>(historico))
            .Returns(historicoDto);

        var service = CreateService();

        var result = await service.GetHistoricoSinistro(1);

        result.Should().HaveCount(1);
    }

    private static DomainSinistro CriarSinistroValido()
    {
        var apolice = new DomainApolice("123", "Nome", DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(1));

        return new DomainSinistro(
            "SIN-001",
            DateTime.UtcNow,
            "Colisão",
            5000,
            apolice);
    }
}