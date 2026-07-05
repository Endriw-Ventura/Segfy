using AutoMapper;
using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using Segfy.Application.Interfaces.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.GetAllSinistros
{
    public class GetAllSinistrosUseCase(ISinistroRepository sinistroRepository, IMapper mapper) : IGetAllSinistrosUseCase
    {
        private readonly ISinistroRepository _sinistroRepository = sinistroRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<SinistroDTO>> ExecuteAsync (StatusSinistro? status, DateTime? data, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var sinistros = await _sinistroRepository.GetAllAsync(status, data, page, pageSize);
            return _mapper.Map<IEnumerable<SinistroDTO>>(sinistros);
        }
    }
}
