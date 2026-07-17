using segfy.Domain.Enums;

namespace Segfy.Application.Sinistros.Results
{
    public sealed record GetHistoricoSinistroResult(
     StatusSinistro? StatusAnterior, 
     StatusSinistro StatusNovo, 
     string? Observacao, 
     DateTime CriadoEm 
    );
}
