-- 1. Ranking dos ramos com maior percentual de sinistros negados nos últimos 6 meses

SELECT
    a.Ramo,
    COUNT(s.Id) AS TotalSinistros,
    SUM(CASE WHEN s.Status = 'NEGADO' THEN 1 ELSE 0 END) AS TotalNegados,
    CAST(
        SUM(CASE WHEN s.Status = 'NEGADO' THEN 1 ELSE 0 END) * 100.0 / COUNT(s.Id)
        AS DECIMAL(10, 2)
    ) AS PercentualNegados
FROM Sinistros s
INNER JOIN Apolices a ON a.Id = s.ApoliceId
WHERE s.DataSinistro >= DATEADD(MONTH, -6, GETDATE())
GROUP BY a.Ramo
ORDER BY PercentualNegados DESC;

-- 2. Top 10 clientes com maior soma de ValorEstimado em sinistros em análise ou aprovados

SELECT TOP 10
    a.NomeSegurado AS Cliente,
    SUM(s.ValorSolicitado) AS SomaValorEstimado
FROM Sinistros s
INNER JOIN Apolices a ON a.Id = s.ApoliceId
WHERE s.Status IN ('EM_ANALISE', 'APROVADO')
GROUP BY a.NomeSegurado
ORDER BY SomaValorEstimado DESC;


-- 3. Tempo médio de resolução em dias de sinistros encerrados, agrupado por ramo

SELECT
    a.Ramo,
    AVG(CAST(DATEDIFF(DAY, s.DataSinistro, h.CriadoEm) AS DECIMAL(10, 2))) AS TempoMedioResolucaoDias
FROM Sinistros s
INNER JOIN Apolices a ON a.Id = s.ApoliceId
INNER JOIN HistoricoSinistros h ON h.SinistroId = s.Id
WHERE s.Status = 'ENCERRADO'
  AND h.StatusNovo = 'ENCERRADO'
GROUP BY a.Ramo
ORDER BY TempoMedioResolucaoDias ASC;