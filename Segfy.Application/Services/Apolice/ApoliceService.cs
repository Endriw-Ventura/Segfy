using AutoMapper;
using segfy.Domain.Enums;
using segfy.Domain.Exceptions;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using DomainApolice = segfy.Domain.Entities.Apolice;


namespace Segfy.Application.Services.Apolice
{
    public class ApoliceService(
        IApoliceRepository apoliceRepository, 
        IUnitOfWork unitOfWork, 
        IMapper mapper
        ) : IApoliceService
    {
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task AtualizarStatusAsync(int apoliceId, StatusApolice novoStatus)
        {
            var apolice = await _apoliceRepository.GetApoliceByIdAsyncTracked(apoliceId);
            if (apolice is null)
                throw new DomainException("Apolice não encontrada.");

            switch (novoStatus)
            {
                case StatusApolice.ATIVA:
                    apolice.Ativar();
                    break;
                case StatusApolice.EXPIRADA:
                    apolice.Expirar();
                    break;
                case StatusApolice.CANCELADA:
                    apolice.Cancelar();
                    break;
                default:
                    throw new DomainException("Status inválido.");
            }
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ApoliceDTO> CriarApoliceAsync(CreateApoliceDTO request)
        {
            var newApolice = _mapper.Map<DomainApolice>(request);
            await _apoliceRepository.AddApoliceAsync(newApolice);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ApoliceDTO>(newApolice);
        }

        public async Task<IEnumerable<ApoliceDTO>> GetAllAsync(StatusApolice? status, DateTime? data, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var apolices = await _apoliceRepository.GetAllAsync(status, data, page, pageSize);
            return _mapper.Map<IEnumerable<ApoliceDTO>>(apolices);
        }

        public async Task<ApoliceDTO?> GetApoliceByIdAsync(int id)
        {
            var apolice = await _apoliceRepository.GetApoliceByIdAsync(id);

            if (apolice is null)
                return null;

            return _mapper.Map<ApoliceDTO>(apolice);
        }

        public async Task<ApoliceComSinistrosDTO?> GetApoliceComSinistrosAsync(int id)
        {
            var apolice = await _apoliceRepository.GetByIdWithSinistrosAsync(id);
            return _mapper.Map<ApoliceComSinistrosDTO>(apolice);
        }
    }
}
