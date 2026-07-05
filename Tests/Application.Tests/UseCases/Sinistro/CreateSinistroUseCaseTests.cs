using AutoMapper;
using FluentAssertions;
using Moq;
using segfy.Domain.Entities;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.UseCases.Sinistros.CreateSinistro;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Tests.UseCases.Sinistros;

public class CreateSinistroUseCaseTests
{
    private readonly Mock<ISinistroRepository> _sinistroRepository = new();
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();

    private CreateSinistroUseCase CreateUseCase()
    {
        return new CreateSinistroUseCase(
            _sinistroRepository.Object,
            _apoliceRepository.Object,
            _unitOfWork.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveCriarSinistro_ComSucesso()
    {
        var request = CriarRequest();
        var apolice = CriarApolice();

        var response = new SinistroDTO
        {
            Id = 1,
            NumeroSinistro = request.NumeroSinistro,
            Descricao = request.Descricao,
            ValorSolicitado = request.ValorSolicitado
        };

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(request.ApoliceId))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()))
            .Returns(response);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(request);

        result.Should().NotBeNull();
        result.NumeroSinistro.Should().Be(request.NumeroSinistro);
        result.Descricao.Should().Be(request.Descricao);
        result.ValorSolicitado.Should().Be(request.ValorSolicitado);

        _sinistroRepository.Verify(
            x => x.AddSinistroAsync(It.IsAny<DomainSinistro>()),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _mapper.Verify(
            x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarException_QuandoApoliceNaoExiste()
    {
        var request = CriarRequest();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(request.ApoliceId))
            .ReturnsAsync((DomainApolice?)null);

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.ExecuteAsync(request);

        await act.Should()
            .ThrowAsync<DomainException>()
            .WithMessage("Apolice não encontrada.");

        _sinistroRepository.Verify(
            x => x.AddSinistroAsync(It.IsAny<DomainSinistro>()),
            Times.Never);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);

        _mapper.Verify(
            x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveSalvar_QuandoAddSinistroFalhar()
    {
        var request = CriarRequest();
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(request.ApoliceId))
            .ReturnsAsync(apolice);

        _sinistroRepository
            .Setup(x => x.AddSinistroAsync(It.IsAny<DomainSinistro>()))
            .ThrowsAsync(new Exception("Erro ao adicionar sinistro."));

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.ExecuteAsync(request);

        await act.Should().ThrowAsync<Exception>();

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveBuscarApolicePeloIdInformado()
    {
        var request = CriarRequest();
        var apolice = CriarApolice();

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(request.ApoliceId))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()))
            .Returns(new SinistroDTO { Id = 1 });

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(request);

        _apoliceRepository.Verify(
            x => x.GetApoliceByIdAsync(request.ApoliceId),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarSinistroDTO()
    {
        var request = CriarRequest();
        var apolice = CriarApolice();

        var expectedDto = new SinistroDTO
        {
            Id = 1,
            NumeroSinistro = request.NumeroSinistro,
            Descricao = request.Descricao,
            ValorSolicitado = request.ValorSolicitado
        };

        _apoliceRepository
            .Setup(x => x.GetApoliceByIdAsync(request.ApoliceId))
            .ReturnsAsync(apolice);

        _mapper
            .Setup(x => x.Map<SinistroDTO>(It.IsAny<DomainSinistro>()))
            .Returns(expectedDto);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(request);

        result.Should().BeEquivalentTo(expectedDto);
    }

    private static CreateSinistroDTO CriarRequest()
    {
        return new CreateSinistroDTO
        {
            ApoliceId = 1,
            NumeroSinistro = "SIN-001",
            DataSinistro = DateTime.Now,
            Descricao = "Colisão",
            ValorSolicitado = 5000
        };
    }

    private static DomainApolice CriarApolice()
    {
        return new DomainApolice(
            "AP-001",
            "João Silva",
            DateTime.Now.AddMonths(-2),
            DateTime.Now.AddMonths(10),
            Domain.Enums.Ramo.AUTOMOVEL);
    }
}