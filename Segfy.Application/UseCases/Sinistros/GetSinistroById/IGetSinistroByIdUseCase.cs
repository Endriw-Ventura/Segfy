using Segfy.Application.DTOs.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Sinistros.GetSinistroById
{
    public interface IGetSinistroByIdUseCase
    {
        Task<SinistroDTO?> ExecuteAsync(int id);
    }
}
