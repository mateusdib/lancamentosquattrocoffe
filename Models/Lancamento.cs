namespace LancamentosQuattroCoffe.Models
{
    public class Lancamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = null!;
        public int IdCentroDeCusto { get; set; }
        public string? NomeUserIncluder { get; set; }
        public string? NomeUsuarioRateio { get; set; }
        public string? DescricaoCentroDeCusto { get; set; }
        public decimal Percentual { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorLancado { get; set; }
        public bool? IsPago { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string? Categoria { get; set; }
        public int IdUserIncluder { get; set; }
        public bool Excluido { get; set; }
        public bool? IsRecorrente { get; set; }

        public int IdCategoria { get; set; }
        public string? Observacao { get; set; } 
    }
}