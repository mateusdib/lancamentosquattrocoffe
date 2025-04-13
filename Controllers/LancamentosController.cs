using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using LancamentosQuattroCoffe.Models;
using Microsoft.AspNetCore.Mvc;
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
            var range = "A1";
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>>
                {
                    new List<object> { lancamento.Descricao, lancamento.Categoria, lancamento.Valor }
                }
            };

            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var response = await appendRequest.ExecuteAsync();

            return Ok(new { message = "Dados salvos com sucesso!" });
        }

    }
}