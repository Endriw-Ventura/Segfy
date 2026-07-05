using segfy.Domain.Enums;
using Segfy.Application.DTOs.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.GetAllSinistros
{
    public interface IGetAllSinistrosUseCase
    {
        Task<IEnumerable<SinistroDTO>> ExecuteAsync (StatusSinistro? status, DateTime? data, int page, int pageSize);
    }
}
