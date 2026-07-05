using Segfy.Application.DTOs.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.CreateSinistro
{
    public interface ICreateSinistroUseCase
    {
        Task<GetSinistroDTO> ExecuteAsync(CreateSinistroDTO sinistro);
    }
}
