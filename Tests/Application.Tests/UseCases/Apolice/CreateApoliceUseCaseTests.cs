using AutoMapper;
using FluentAssertions;
using Moq;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.UseCases.Apolices.Create;
using Segfy.Domain.Enums;
using Xunit;
using DomainApolice = segfy.Domain.Entities.Apolice;

namespace Segfy.Application.Tests.UseCases.Apolices;

public class CreateApoliceUseCaseTests
{
    private readonly Mock<IApoliceRepository> _apoliceRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();

    private CreateApoliceUseCase CreateUseCase()
    {
        return new CreateApoliceUseCase(
            _apoliceRepository.Object,
            _unitOfWork.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveCriarApolice_ComSucesso()
    {
        var request = new CreateApoliceDTO
        {
            NumeroApolice = "AP-001",
            NomeSegurado = "João Silva",
            DataInicio = DateTime.Now.AddMonths(-1),
            DataFim = DateTime.Now.AddMonths(11),
            Ramo = Ramo.AUTOMOVEL
        };

        var response = new ApoliceDTO
        {
            NumeroApolice = request.NumeroApolice,
            NomeSegurado = request.NomeSegurado
        };

        _apoliceRepository
            .Setup(x => x.CheckForDuplicateNumeroApolice(
                request.NumeroApolice))
            .ReturnsAsync(false);

        _mapper
            .Setup(x => x.Map<ApoliceDTO>(
                It.IsAny<DomainApolice>()))
            .Returns(response);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(request);

        result.Should().NotBeNull();
        result.NumeroApolice.Should().Be("AP-001");
        result.NomeSegurado.Should().Be("João Silva");

        _apoliceRepository.Verify(
            x => x.AddApoliceAsync(
                It.Is<DomainApolice>(a =>
                    a.NumeroApolice == request.NumeroApolice &&
                    a.NomeSegurado == request.NomeSegurado)),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _mapper.Verify(
            x => x.Map<ApoliceDTO>(
                It.Is<DomainApolice>(a =>
                    a.NumeroApolice == request.NumeroApolice &&
                    a.NomeSegurado == request.NomeSegurado)),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveSalvarAposAdicionarApolice()
    {
        var request = CriarRequest();
        var response = new ApoliceDTO();

        _apoliceRepository
            .Setup(x => x.CheckForDuplicateNumeroApolice(
                request.NumeroApolice))
            .ReturnsAsync(false);

        _mapper
            .Setup(x => x.Map<ApoliceDTO>(
                It.IsAny<DomainApolice>()))
            .Returns(response);

        var useCase = CreateUseCase();

        await useCase.ExecuteAsync(request);

        _apoliceRepository.Verify(
            x => x.AddApoliceAsync(
                It.Is<DomainApolice>(a =>
                    a.NumeroApolice == request.NumeroApolice &&
                    a.NomeSegurado == request.NomeSegurado &&
                    a.DataInicio == request.DataInicio &&
                    a.DataFim == request.DataFim &&
                    a.Ramo == request.Ramo)),
            Times.Once);

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarApoliceDTO()
    {
        var request = CriarRequest();

        var expectedDto = new ApoliceDTO
        {
            NumeroApolice = request.NumeroApolice,
            NomeSegurado = request.NomeSegurado
        };

        _apoliceRepository
            .Setup(x => x.CheckForDuplicateNumeroApolice(
                request.NumeroApolice))
            .ReturnsAsync(false);

        _mapper
            .Setup(x => x.Map<ApoliceDTO>(
                It.IsAny<DomainApolice>()))
            .Returns(expectedDto);

        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(request);

        result.Should().BeEquivalentTo(expectedDto);

        _mapper.Verify(
            x => x.Map<ApoliceDTO>(
                It.Is<DomainApolice>(a =>
                    a.NumeroApolice == request.NumeroApolice &&
                    a.NomeSegurado == request.NomeSegurado)),
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NaoDeveChamarSaveChanges_QuandoAddApoliceFalhar()
    {
        var request = CriarRequest();

        _apoliceRepository
            .Setup(x => x.CheckForDuplicateNumeroApolice(
                request.NumeroApolice))
            .ReturnsAsync(false);

        _apoliceRepository
            .Setup(x => x.AddApoliceAsync(
                It.IsAny<DomainApolice>()))
            .ThrowsAsync(new Exception("Erro ao adicionar apólice."));

        var useCase = CreateUseCase();

        Func<Task> act = async () =>
            await useCase.ExecuteAsync(request);

        await act.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Erro ao adicionar apólice.");

        _unitOfWork.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);

        _mapper.Verify(
            x => x.Map<ApoliceDTO>(
                It.IsAny<DomainApolice>()),
            Times.Never);
    }

    private static CreateApoliceDTO CriarRequest()
    {
        return new CreateApoliceDTO
        {
            NumeroApolice = "AP-001",
            NomeSegurado = "João Silva",
            DataInicio = DateTime.Now.AddMonths(-1),
            DataFim = DateTime.Now.AddMonths(11),
            Ramo = Ramo.AUTOMOVEL
        };
    }

    private static DomainApolice CriarApolice(CreateApoliceDTO request)
    {
        return new DomainApolice(
            request.NumeroApolice,
            request.NomeSegurado,
            request.DataInicio,
            request.DataFim,
            Ramo.AUTOMOVEL);
    }
}
