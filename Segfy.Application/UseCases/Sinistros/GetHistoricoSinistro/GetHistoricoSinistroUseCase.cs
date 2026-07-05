using AutoMapper;
using Segfy.Application.DTOs.HistoricoSinistro;
using Segfy.Application.Interfaces.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.GetHistoricoSinistro
{
    public class GetHistoricoSinistroUseCase(ISinistroRepository sinistroRepository, IMapper mapper) : IGetHistoricoSinistroUseCase
    {
        private readonly ISinistroRepository _sinistroRepository = sinistroRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<IEnumerable<HistoricoSinistroDTO>> ExecuteAsync(int id)
        {
            var historico = await _sinistroRepository.GetHistoricoSinistro(id);
            return _mapper.Map<IEnumerable<HistoricoSinistroDTO>>(historico);
        }
    }
}
