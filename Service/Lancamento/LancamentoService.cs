using Dapper;
using LancamentosQuattroCoffe.Models;
using LancamentosQuattroCoffe.Queries;
using System.Data;

namespace LancamentosQuattroCoffe.Service.Lancamento
{
    public class LancamentoService : ILancamentoService
    {
        private readonly IDbConnection _connection;

        public LancamentoService(IDbConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<IEnumerable<Models.Lancamento>> GetByUserId(int idUser, int qtdItens)
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var result = await connection.QueryAsync<Models.Lancamento>(LancamentoQueries.GetLancamentoById, new { IdUser = idUser, Quantidade = qtdItens });
                foreach (var item in result)
                {
                    item.Valor = Math.Round(item.Valor * (item.Percentual / 100.0m), 2);
                }
                return result;
              
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<Models.Lancamento>> GetByUserIdAndStatusPagamento(int idUser, bool pago,int qtdItens)
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                var result = await connection.QueryAsync<Models.Lancamento>(LancamentoQueries.GetByUserAndStatusPagamento, new { IdUser = idUser, Pago = pago, Quantidade = qtdItens });
                
                foreach (var item in result)
                {
                    item.Valor = Math.Round(item.Valor * (item.Percentual / 100.0m), 2);
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<long> SalvarAsync(Models.Lancamento lancamento)
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                var id = await connection.ExecuteScalarAsync<long>(
                    LancamentoQueries.InsertLancamento,
                    new
                    {
                        lancamento.Descricao,
                        lancamento.Valor,
                        lancamento.IdCentroDeCusto,
                        lancamento.DataLancamento,
                        lancamento.DataVencimento,
                        lancamento.DataPagamento,
                        Excluido = false,
                        lancamento.IdUserIncluder,
                        IsPago = lancamento.IsPago ?? false,
                        IsRecorrente = lancamento.IsRecorrente ?? false,
                        lancamento.IdCategoria,
                        lancamento.Observacao,
                    });

                return id;
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        public async Task AtualizarStatusLancamentoAsync(int idLancamento, bool pago)
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
              
                await connection.ExecuteAsync(LancamentoQueries.UpdateLancamento, new
                {
                    idLancamento = idLancamento,
                    pago = pago,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task DeleteLancamentoAsync(int idLancamento)
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                await connection.ExecuteAsync(LancamentoQueries.DeleteLancamento, new
                {
                    idLancamento = idLancamento,
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return await connection.QueryAsync<Categoria>("SELECT * FROM CATEGORIA");
             

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<CentroDeCusto>> GetCentroDeCustoAsync()
        {
            try
            {
                using var connection = _connection;
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return await connection.QueryAsync<CentroDeCusto>("SELECT * FROM centro_de_custo");


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
