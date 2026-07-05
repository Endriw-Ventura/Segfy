using FluentAssertions;
using Moq;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.UseCases.Sinistros.UpdateSinistroStatus;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Tests.UseCases.Sinistros;

public class UpdateSinistroStatusUseCaseTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();

    private UpdateSinistroStatusUseCase CreateUseCase()
    {
        return new UpdateSinistroStatusUseCase(
            _sinistroRepository.Object,
            _unitOfWork.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarException_QuandoSinistroNaoExiste()
    {
        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(1))
            .ReturnsAsync((DomainSinistro?)null);

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.ExecuteAsync(
            1,
            StatusSinistro.EM_ANALISE,
            null,
            null);

        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Sinistro não encontrado.");

        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveAlterarStatusParaEmAnalise()
    {
        var sinistro = CriarSinistro();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.EM_ANALISE,
            null,
            null);

        sinistro.Status.Should().Be(StatusSinistro.EM_ANALISE);
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveAlterarStatusParaAprovado()
    {
        var sinistro = CriarSinistro();
        sinistro.SendForAnalisys();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.APROVADO,
            null,
            null);

        sinistro.Status.Should().Be(StatusSinistro.APROVADO);
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveAlterarStatusParaEncerrado_QuandoValorAprovadoInformado()
    {
        var sinistro = CriarSinistro();
        sinistro.SendForAnalisys();
        sinistro.Aprove();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.ENCERRADO,
            null,
            3000);

        sinistro.Status.Should().Be(StatusSinistro.ENCERRADO);
        sinistro.ValorAprovado.Should().Be(3000);
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveEncerrar_QuandoValorAprovadoNaoInformado()
    {
        var sinistro = CriarSinistro();
        sinistro.SendForAnalisys();
        sinistro.Aprove();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.ENCERRADO,
            null,
            null);

        await act.Should().ThrowAsync<DomainException>();

        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveAlterarStatusParaNegado_QuandoMotivoInformado()
    {
        var sinistro = CriarSinistro();
        sinistro.SendForAnalisys();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.NEGADO,
            "Documentação insuficiente",
            null);

        sinistro.Status.Should().Be(StatusSinistro.NEGADO);
        sinistro.MotivoNegativa.Should().Be("Documentação insuficiente");
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveNegar_QuandoMotivoNaoInformado()
    {
        var sinistro = CriarSinistro();
        sinistro.SendForAnalisys();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.NEGADO,
            null,
            null);

        await act.Should().ThrowAsync<DomainException>();

        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveSalvar_QuandoFluxoDeStatusForInvalido()
    {
        var sinistro = CriarSinistro();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.APROVADO,
            null,
            null);

        await act.Should().ThrowAsync<DomainException>();

        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarSinistroPeloIdInformado()
    {
        var sinistro = CriarSinistro();

        _sinistroRepository
            .Setup(x => x.GetSinistroByIdAsyncTracked(sinistro.Id))
            .ReturnsAsync(sinistro);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(
            sinistro.Id,
            StatusSinistro.EM_ANALISE,
            null,
            null);

        _sinistroRepository.Verify(
            x => x.GetSinistroByIdAsyncTracked(sinistro.Id),
            Times.Once);
    }

    private static DomainSinistro CriarSinistro()
    {
        var apolice = new DomainApolice(
            "AP-001",
            "João Silva",
            DateTime.Now.AddMonths(-2),
            DateTime.Now.AddMonths(10));

        return new DomainSinistro(
            "SIN-001",
            DateTime.Now,
            "Colisão",
            5000,
            apolice);
    }
}