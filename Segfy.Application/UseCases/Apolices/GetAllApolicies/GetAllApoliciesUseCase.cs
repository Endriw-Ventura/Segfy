using AutoMapper;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.GetAll
{
    public class GetAllApoliciesUseCase(IApoliceRepository apoliceRepository, IMapper mapper) : IGetAllApoliciesUseCase
    {
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<ApoliceDTO>> ExecuteAsync(StatusApolice? status, DateTime? data, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var apolices = await _apoliceRepository.GetAllAsync(status, data, page, pageSize);
            return _mapper.Map<IEnumerable<ApoliceDTO>>(apolices);
        }
    }
}
