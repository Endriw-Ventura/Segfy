using AutoMapper;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.UseCases.Apolices.GetAll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.GetApoliceById
{
    public class GetApoliceByIdUseCase(IApoliceRepository apoliceRepository, IMapper mapper) : IGetApoliceByIdUseCase
    {
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApoliceDTO?> ExecuteAsync(int id)
        {
            var apolice = await _apoliceRepository.GetApoliceByIdAsync(id);

            if (apolice is null)
                return null;

            return _mapper.Map<ApoliceDTO>(apolice);
        }
    }
}
