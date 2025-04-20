namespace LancamentosQuattroCoffe.Models
{
    public class Lancamento
    {
        public int? Id { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public decimal Valor { get; set; }
        public string CentroDeCusto { get; set; }
        public DateTime? DataLancamento { get; set; }
        public string? Status { get; set; }
    }
}
