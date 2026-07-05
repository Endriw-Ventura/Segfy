using AutoMapper;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.GetApoliceWithSinistros
{
    public class GetApoliceWithSinistrosUseCase(IApoliceRepository apoliceRepository, IMapper mapper) : IGetApoliceWithSinistrosUseCase
    {
        private readonly IApoliceRepository _apoliceRepository = apoliceRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ApoliceComSinistrosDTO?> ExecuteAsync(int id)
        {
            var apolice = await _apoliceRepository.GetByIdWithSinistrosAsync(id);
            return _mapper.Map<ApoliceComSinistrosDTO>(apolice);
        }
    }
}
