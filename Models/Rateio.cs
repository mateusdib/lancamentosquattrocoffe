﻿namespace LancamentosQuattroCoffe.Models
{
    public class Rateio
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public int IdCentroDeCusto { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataEdicao { get; set; }
    }
}
