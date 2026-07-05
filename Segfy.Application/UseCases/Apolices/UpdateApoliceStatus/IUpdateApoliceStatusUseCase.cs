using segfy.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.UseCases.Apolices.UpdateApoliceStatus
{
    public interface IUpdateApoliceStatusUseCase
    {
        Task ExecuteAsync(int apoliceId, StatusApolice novoStatus);
    }
}
