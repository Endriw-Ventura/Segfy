using segfy.Domain.Enums;
using Segfy.Domain.Enums;

namespace Segfy.Application.Apolices.Results
{
    public sealed record ApoliceResult(
         int Id,
         string NumeroApolice,
         string NomeSegurado, 
         DateTime DataInicio,
         DateTime DataFim ,
         StatusApolice Status,
         Ramo Ramo);
}
