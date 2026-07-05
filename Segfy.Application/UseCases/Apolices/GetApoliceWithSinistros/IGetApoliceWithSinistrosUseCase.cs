using Segfy.Application.DTOs.Apolice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.GetApoliceWithSinistros
{
    public interface IGetApoliceWithSinistrosUseCase
    {
        Task<ApoliceComSinistrosDTO?> ExecuteAsync(int id);
    }
}
