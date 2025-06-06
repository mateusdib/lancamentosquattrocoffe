namespace LancamentosQuattroCoffe.Models
{
    public class RateioUsuario
    {
        public int Id { get; set; }
        public int IdRateio { get; set; }
        public int IdUsuario { get; set; }
        public decimal Percentual { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
    }
}
