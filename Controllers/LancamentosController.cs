using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using LancamentosQuattroCoffe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text;

namespace LancamentosQuattroCoffe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LancamentosController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly string _spreadsheetId;
        private readonly SheetsService _sheetsService;
        private readonly Service.Lancamento.ILancamentoService _lancamentoService;

        public LancamentosController(IConfiguration config, Service.Lancamento.ILancamentoService lancamentoService)
        {
            _lancamentoService = lancamentoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lancamento lancamento)
        {
            var result = await _lancamentoService.SalvarAsync(lancamento);
            if (result != 0)
            {
                return Ok(new { message = "Dados salvos com sucesso!" });

            }
            else
            {
                return UnprocessableEntity();
            }

        }
        [HttpDelete("DeleteLancamentoById", Name = "DeleteLancamentoById")]
        public async Task<IActionResult> DeleteLancamento([FromQuery] int idLancamento)
        {
            await _lancamentoService.DeleteLancamentoAsync(idLancamento);
            return Ok("Excluido com sucesso!");

        }
        //[HttpGet]
        //public async Task<IActionResult> Get()
        //{
        //    //var range = "A2:H"; // Dados começam da linha 2 até a F (Data, Descrição, Categoria, Valor, CentroDeCusto, Status)
        //    //var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);
        //    //var response = await request.ExecuteAsync();

        //    //var values = response.Values;
        //    //var lista = new ConcurrentBag<Lancamento>();

        //    //if (values != null && values.Count > 0)
        //    //{
        //    //    // Pegando apenas os últimos 20


        //    //    Parallel.ForEach(values, row =>
        //    //    {

        //    //        var lancamento = new Lancamento
        //    //        {
        //    //            Id = Int32.TryParse(row[0]?.ToString(), out var id) ? id : 0,
        //    //            DataLancamento = DateTime.TryParse(row[1]?.ToString(), out var data) ? data : DateTime.Now,
        //    //            Descricao = row[2]?.ToString(),
        //    //            Categoria = row[3]?.ToString(),
        //    //            Valor = ConverterToDecimal(row[4]?.ToString()),
        //    //            CentroDeCusto = row[5]?.ToString(),
        //    //            Status = row[6]?.ToString() ?? "noExist",
        //    //        };

        //    //        lista.Add(lancamento);
        //    //    });


        //    // }
        //   // return Ok(lista.Where(x => x.Status.ToUpper().Contains("PENDENTE")).OrderBy(x => x.Id));
        //    return Ok();
        // }
        [HttpGet("getByUserId",Name = "getByUserId")]
        public async Task<IActionResult> GetByUserId([FromQuery] int idUser, [FromQuery] int totalItens)
        {
            return Ok(await _lancamentoService.GetByUserId(idUser, totalItens));
        }
        [HttpGet("GetByUserIdAndStatusPagamento", Name = "GetByUserIdAndStatusPagamento")]
        public async Task<IActionResult> GetByUserIdAndStatusPagamento([FromQuery] int idUser, [FromQuery]  bool pago, [FromQuery] int totalItens)
        {
            return Ok(await _lancamentoService.GetByUserIdAndStatusPagamento(idUser, pago, totalItens));
        }
        [HttpPut("postLancamentoByIdStatus", Name = "postLancamentoByIdStatus")]
        public async Task<IActionResult> UpdateLancamento( int idLancamento,  bool pago)
        {
            await _lancamentoService.AtualizarStatusLancamentoAsync(idLancamento, pago);
            return Ok("Dados atualizados com sucesso!");
        }

        [HttpGet("getCentroDeCusto", Name = "getCentroDeCusto")]
        public async Task<IActionResult> getCentroDeCusto()
        {
            return Ok(await _lancamentoService.GetCentroDeCustoAsync());
        }
        [HttpGet("getCategorias", Name = "getCategorias")]
        public async Task<IActionResult> getCategorias()
        {
            return Ok(await _lancamentoService.GetCategoriasAsync());
        }
        private static decimal ConverterToDecimal(string valorTexto)
        {
            if (string.IsNullOrWhiteSpace(valorTexto))
                return 0m;

            // Remove "R$", espaços e pontos (milhar), troca vírgula por ponto
            var valorLimpo = valorTexto
                .Replace("R$", "")
                .Replace(" ", "")
                .Replace(".", "")
                .Replace(",", ".");

            if (decimal.TryParse(valorLimpo, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var resultado))
            {
                return resultado;
            }

            return 0m;
        }

    }
}