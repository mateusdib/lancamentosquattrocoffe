namespace LancamentosQuattroCoffe.Service.Lancamento
{
    public interface ILancamentoService
    {
        Task<long> SalvarAsync(Models.Lancamento lancamento);
        Task<IEnumerable<Models.Lancamento>> GetByUserId(int idUsuario, int qtdItens);
        Task<IEnumerable<Models.Lancamento>> GetByUserIdAndStatusPagamento(int idUsuario, bool pago, int qtdItens);
        Task AtualizarStatusLancamentoAsync(int idLancamento, bool pago);
        Task DeleteLancamentoAsync(int idLancamento);
    }
}
