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

        public LancamentosController(IConfiguration config)
        {
            _spreadsheetId = "1ui79wvlER3bK2TBycYDIjWbX6bMPRAiUgLsXKljcsf4";


            var jsonCredentials = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_JSON");

            if (string.IsNullOrEmpty(jsonCredentials))
            {
                throw new InvalidOperationException("A variável de ambiente GOOGLE_APPLICATION_CREDENTIALS_JSON não foi definida.");
            }

            // Criação das credenciais a partir do JSON
            var credential = GoogleCredential.FromJson(jsonCredentials)
                .CreateScoped(SheetsService.Scope.Spreadsheets);

            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Lancamentos .NET"
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Lancamento lancamento)
        {
            var range = "A2";
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> { "=ROW()-1", DateTime.Now.Date.ToShortDateString(), lancamento.Descricao, lancamento.Categoria, lancamento.Valor,lancamento.CentroDeCusto,"Pendente" }
                }
            };

            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var response = await appendRequest.ExecuteAsync();

            return Ok(new { message = "Dados salvos com sucesso!" });
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var range = "A2:H"; // Dados começam da linha 2 até a F (Data, Descrição, Categoria, Valor, CentroDeCusto, Status)
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);
            var response = await request.ExecuteAsync();

            var values = response.Values;
            var lista = new ConcurrentBag<Lancamento>();

            if (values != null && values.Count > 0)
            {
                // Pegando apenas os últimos 20


                Parallel.ForEach(values, row =>
                {

                    var lancamento = new Lancamento
                    {
                        Id = Int32.TryParse(row[0]?.ToString(), out var id) ? id : 0,
                        DataLancamento = DateTime.TryParse(row[1]?.ToString(), out var data) ? data : DateTime.Now,
                        Descricao = row[2]?.ToString(),
                        Categoria = row[3]?.ToString(),
                        Valor = ConverterToDecimal(row[4]?.ToString()),
                        CentroDeCusto = row[5]?.ToString(),
                        Status = row[6]?.ToString() ?? "noExist",
                    };

                    lista.Add(lancamento);
                });

                //foreach (var row in values)
                //{
                //    if (row.Count < 6) continue;

                //    var lancamento = new Lancamento
                //    {
                //        DataLancamento = DateTime.TryParse(row[0]?.ToString(), out var data) ? data : DateTime.Now,
                //        Descricao = row[1]?.ToString(),
                //        Categoria = row[2]?.ToString(),
                //        Valor = ConverterToDecimal(row[3]?.ToString()),
                //        CentroDeCusto = row[4]?.ToString(),
                //        Status = row[5]?.ToString()
                //    };

                //    lista.Add(lancamento);
                //}
            }

            return Ok(lista.Where(x => x.Status.ToUpper().Contains("PENDENTE")).OrderBy(x=>x.Id));
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
