using AutoMapper;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.GetSinistroById
{
    public class GetSinistroByIdUseCase(ISinistroRepository sinistroRepository, IMapper mapper) : IGetSinistroByIdUseCase
    {
        private readonly ISinistroRepository _sinistroRepository = sinistroRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<SinistroDTO?> ExecuteAsync(int id)
        {
            var sinistro = await _sinistroRepository.GetSinistroByIdAsync(id);
            if (sinistro is null)
                return null;

            return _mapper.Map<SinistroDTO>(sinistro);
        }
    }
}
