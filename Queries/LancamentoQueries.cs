using Microsoft.AspNetCore.Http;

namespace LancamentosQuattroCoffe.Queries
{
    public static class LancamentoQueries
    {
        public const string InsertLancamento = @"
        INSERT INTO lancamentos (
            descricao, valor, id_centro_de_custo, data_lancamento,
            data_vencimento, data_pagamento, excluido,
            id_user_includer, is_pago, is_recorrente,
            data_criacao, data_edicao, id_categoria, observacao
        ) VALUES (
            @Descricao, @Valor, @IdCentroDeCusto, @DataLancamento,
            @DataVencimento, @DataPagamento, @Excluido,
            @IdUserIncluder, @IsPago, @IsRecorrente,
            NOW(), NOW(), @IdCategoria, @Observacao
        )
        RETURNING id;
    ";

        public const string UpdateLancamento = @"
                UPDATE lancamentos
                SET 
                is_pago = @pago
                WHERE id = @idLancamento";
    
              public const string GetLancamentoById = @"
               SELECT  l.id as Id,
                          l.descricao as Descricao ,
          u.nome AS NomeUserIncluder,
          u.nome AS NomeUsuarioRateio,
          c.descricao as DescricaoCentroDeCusto ,
          c.id as IdCentroDeCusto,
          ru.percentual as Percentual,
          l.valor AS Valor,
          l.valor AS ValorLancado,
          l.is_pago as IsPago,
          l.excluido as Excluido,
          l.data_vencimento as DataVencimento,
          l.data_pagamento as DataPagamento,
          l.data_lancamento as DataLancamento,
          ca.nome as Categoria,
          l.id_user_includer as IdUserIncluder,
          l.is_recorrente as IsRecorrente,
          l.id_categoria as IdCategoria
                 FROM lancamentos l
                 JOIN centro_de_custo c ON l.id_centro_de_custo = c.id
                 JOIN rateio r ON c.id = r.id_centro_de_custo
                 JOIN rateio_usuario ru ON r.id = ru.id_rateio
                 JOIN usuarios u ON u.id = ru.id_usuario
                 JOIN usuarios us on us.id = l.id_user_includer
                 JOIN categoria ca on ca.id = l.id_categoria
                WHERE u.id = @IdUser and l.excluido = false
                       ORDER BY data_lancamento desc
                    LIMIT @Quantidade";

        //public const string GetByUserAndStatusPagamento = @"
        //       SELECT 
        //                  l.descricao as Descricao ,
        //  u.nome AS NomeUserIncluder,
        //  u.nome AS NomeUsuarioRateio,
        //  c.descricao as DescricaoCentroDeCusto ,
        //  c.id as IdCentroDeCusto,
        //  ru.percentual as Percentual,
        //  ROUND(l.valor * (ru.percentual / 100.0), 2) AS Valor,
        //  l.is_pago as IsPago,
        //  l.data_vencimento as DataVencimento,
        //  l.data_pagamento as DataPagamento,
        //  l.data_lancamento as DataLancamento,
        //  ca.nome as Categoria,
        //  l.id_user_includer as IdUserIncluder,
        //  l.excluido as Excluido,
        //  l.is_recorrente as IsRecorrente,
        //  l.id_categoria as IdCategoria
        //         FROM lancamentos l
        //         JOIN centro_de_custo c ON l.id_centro_de_custo = c.id
        //         JOIN rateio r ON c.id = r.id_centro_de_custo
        //         JOIN rateio_usuario ru ON r.id = ru.id_rateio
        //         JOIN usuarios u ON u.id = ru.id_usuario
        //         JOIN usuarios us on us.id = l.id_user_includer
        //         JOIN categoria ca on ca.id = l.id_categoria
        //        WHERE u.id = @IdUser  and l.is_pago = @Pago and l.excluido = false
        //               ORDER BY data_lancamento
        //            LIMIT @Quantidade";

        public const string GetByUserAndStatusPagamento = @"SELECT 
l.id as Id,
    l.descricao as Descricao,
    us.nome AS NomeUserIncluder,
    u.nome AS NomeUsuarioRateio,
    c.descricao as DescricaoCentroDeCusto,
    c.id as IdCentroDeCusto,
    ru.percentual as Percentual,
    l.valor AS Valor,
    l.valor AS ValorLancado,
    l.is_pago as IsPago,
    l.data_vencimento as DataVencimento,
    l.data_pagamento as DataPagamento,
    l.data_lancamento as DataLancamento,
    ca.nome as Categoria,
    l.id_user_includer as IdUserIncluder,
    l.excluido as Excluido,
    l.is_recorrente as IsRecorrente,
    l.id_categoria as IdCategoria
FROM lancamentos l
JOIN centro_de_custo c ON l.id_centro_de_custo = c.id
JOIN rateio r ON c.id = r.id_centro_de_custo
JOIN rateio_usuario ru ON r.id = ru.id_rateio
JOIN usuarios u ON u.id = ru.id_usuario
JOIN usuarios us ON us.id = l.id_user_includer
JOIN categoria ca ON ca.id = l.id_categoria
WHERE u.id = @IdUser
  AND l.is_pago = @Pago
  AND l.excluido = false
ORDER BY l.data_lancamento desc
LIMIT @Quantidade";


        public const string DeleteLancamento = @"UPDATE lancamentos
                SET 
                excluido = true
                WHERE id = @idLancamento";
    }

}
