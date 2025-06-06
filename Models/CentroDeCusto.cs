namespace LancamentosQuattroCoffe.Models
{
    public class CentroDeCusto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = null!;
        public int IdRateio { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
    }
}
