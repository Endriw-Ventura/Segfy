using segfy.Domain.Enums;
using Segfy.Application.DTOs.Apolice;

namespace Segfy.Application.UseCases.Apolices.GetAll
{
    public interface IGetAllApoliciesUseCase
    {
        Task<IEnumerable<ApoliceDTO>> ExecuteAsync(StatusApolice? status, DateTime? data, int page, int pageSize);
    }
}
