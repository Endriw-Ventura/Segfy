using Segfy.Application.DTOs.Apolice;
using Segfy.Application.Interfaces.Apolice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.GetApoliceById
{
    public interface IGetApoliceByIdUseCase
    {
        public Task<ApoliceDTO?> ExecuteAsync(int id);
    }
}
