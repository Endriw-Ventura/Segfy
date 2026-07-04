using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;
using Segfy.Application.DTOs.HistoricoSinistro;
using Segfy.Application.DTOs.Sinistro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Segfy.Application.Interfaces.Apolice
{
    public interface IApoliceService
    {
        Task<ApoliceDTO> CreateApoliceAsync(CreateApoliceDTO request);
        Task UpdateStatusAsync(
       int apoliceId,
       StatusApolice novoStatus);
        Task<IEnumerable<ApoliceDTO>> GetAllAsync(StatusApolice? status, DateTime? data, int page, int pageSize);
        Task<ApoliceDTO?> GetApoliceByIdAsync(int id);
        Task<ApoliceComSinistrosDTO?> GetApoliceWithSinistrosAsync(int id);
        Task<bool> CheckForDuplicateNumeroApolice(string numeroApolice);
    }
}
