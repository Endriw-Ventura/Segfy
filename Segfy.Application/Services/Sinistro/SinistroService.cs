using AutoMapper;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.HistoricoSinistro;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Interfaces.Sinistro;
using DomainSinistro = segfy.Domain.Entities.Sinistro;

namespace Segfy.Application.Services.Sinistro
{
    public class SinistroService(
        ISinistroRepository sinistroRepository, 
        IApoliceRepository apoliceRepository, 
        IUnitOfWork unitOfWork,
        IMapper mapper) : ISinistroService
    {
        private readonly ISinistroRepository _sinistroRepository = sinistroRepository;
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<SinistroDTO> AbrirSinistroAsync(CreateSinistroDTO sinistro)
        {
            var apolice = await _apoliceRepository.GetApoliceByIdAsync(sinistro.ApoliceId);
            
            if (apolice is null)
                throw new DomainException("Apolice não encontrada.");
            
            var newSinistro = new DomainSinistro(
                sinistro.NumeroSinistro,
                sinistro.DataSinistro,
                sinistro.Descricao,
                sinistro.ValorSolicitado,
                apolice
            );

            await _sinistroRepository.AddSinistroAsync(newSinistro);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SinistroDTO>(newSinistro);
        }

        public async Task AtualizarStatusAsync(
        int sinistroId,
        StatusSinistro novoStatus,
        string? motivoNegativa,
        decimal? valorAprovado)
        {
            var sinistro = await _sinistroRepository.GetSinistroByIdAsyncTracked(sinistroId);

            if (sinistro is null)
                throw new DomainException("Sinistro não encontrado.");

            switch (novoStatus)
            {
                case StatusSinistro.EM_ANALISE:
                    sinistro.EnviarParaAnalise();
                    break;
                case StatusSinistro.APROVADO:
                    sinistro.Aprovar();
                    break;
                case StatusSinistro.ENCERRADO:
                    sinistro.Encerrar(valorAprovado);
                    break;
                case StatusSinistro.NEGADO:
                    sinistro.Negar(motivoNegativa);
                    break;
                default:
                    throw new DomainException("Status inválido.");
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<SinistroDTO>> GetAllAsync(StatusSinistro? status, DateTime? data, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var sinistros = await _sinistroRepository.GetAllAsync(status, data, page, pageSize);
            return _mapper.Map<IEnumerable<SinistroDTO>>(sinistros);
        }

        public async Task<IEnumerable<HistoricoSinistroDTO>> GetHistoricoSinistro(int id)
        {
           var historico = await _sinistroRepository.GetHistoricoSinistro(id);
            return _mapper.Map<IEnumerable<HistoricoSinistroDTO>>(historico);
        }

        public async Task<SinistroDTO?> GetSinistroByIdAsync(int id)
        {
           var sinistro = await _sinistroRepository.GetSinistroByIdAsync(id);
            if (sinistro is null)
                return null;

            return _mapper.Map<SinistroDTO>(sinistro);
        }
    }
}
