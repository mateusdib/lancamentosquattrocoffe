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
            _config = config;
            _spreadsheetId = _config["Google:SpreadsheetId"];
            var json = Environment.GetEnvironmentVariable(@"{
  ""type"": ""service_account"",
  ""project_id"": ""pragmatic-cat-198719"",
  ""private_key_id"": ""bde79f3fcd3aa3cfed89e23a72d5104d43ee4587"",
  ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQCVyeVXa9nb10vm\nH/qff/CJxE/5RINgcEC6sLMuggtGayvfZg2XLNPnPZIEgyRSkWKE02LAhPDSD0mr\n9OZUoLrUEFH6CiHaWO/+MbEa61rN34BGcoEKLwzfQ6Ffr2G5W8wonF7xN8p11idg\ngdUQ1FFl1elOEi5OulQpkcoNe0cPA+9/nx9VBPbq//D5KtW8UFuFwKbCEAa6dgFP\nrGm/A1L//6b7kZkykcQAjf2VoKi3kt3URKp+5jO0k1EVcBT+bM4BrNZlQcunTLTp\neXeJ+3VqoHN2pGSW0WCIOgMzKSrSPklvGf95VmQrXq50z6rnU/RkDwxI3TxekWUu\nFBJ5c+5lAgMBAAECggEASCkqLAoAvAjF/1jToAh+RJiHIKKRmiGqBWaAA/1RWjzi\nN28OSBCoC2Rdln/pPT/eEvhVQEMrUz9bMhS66/U83FH6dDdyZKLpB3BADqRoq/qq\n7QVXxiSxklOeCB9ROH+RZNUkZhgHGOqxMKyLjV6D0kgR6/MVTQc16YrbKNCFhKUX\ni3Bkx5avmYk735jP8zO3hhqjYWZAV46jO5CIxByyEUl9L2DEfwrq8dsy7shJ17Ki\ntDVB+UssqC11/HfHjMMxr/PK4uUUwGeyTJ25gmJtGtCGEsPBSghXXAlD7UMCuDgV\nfBc8B5y6tbsb3PNS2Z+ybyLO98ve18eG6bMtRA9hFwKBgQDCg8qPAgEWK3gB8hMy\n40owLflCAV9vemrq3OxSMZJUpDK/jdDJ4gY/FFsytSRI2uynwW341PbiKFWEKREo\n3Z/iV5WoSWZ5JAh2vb8xS62GBcCces3jQLRBG1D/5rZNLlLKBiUt4GM93iXVkriH\nyDEXhy90EMkqLYF2sbACXF5lOwKBgQDFItkbUTk2gxvvhhAm2eLjXu3Z+XJuMwR4\nDO+g37VvOgEkYUuMXJg7sbU2ZslG3j0Zi7SmVP6yRe/vV/6UQw9py9mGluBfY44r\nOBjAZPUTfH0jWYGzKOFjvjtK2a0H7nQFGTzR1XbTfuhDCnfjJsUQsUoKMV2znGSl\nUZDoAxFA3wKBgGQusLL7G0gp10NZRJ4fBmTOKOTgBCFMcajJZEXpFi7V6vVRu0lf\nSkxO1bHGLgwABYdZTqSsOwO8Nl89L4NWsjRHW+My+r5F2r4deE2RZqvyZoOxlD0J\nZE1sHknOr/IMe8Nlaty4ByUkN2RKjxQP/YjarYwf4RwCF/3kAn0UyeFlAoGAfAbA\nsP8ZD8LNAJVH9CmBJavD2S+nXs2aMi1xVbVpYOENduX/sf9Ph772U5O3bm7D4h6T\nOVKgK2j025BxCrZmLBmkcZja8SiheW+BownhZrEbyfG2OBGwPCCjDSSGSEctl0eZ\njCrQ558gYY1kf/UUZrwj6OXGHgp3B8uZb17+q+ECgYBp+Vdf53y3h6f6c2NeOiKW\nHzyDCj6VKK0v2Fy/SUcvTrI55XVyL3kJd3pwmCRtgXEFoUPSmFS/QmADY4jJ9CLt\nYKNeB1KB3oYTxX3lJ7pPILQaI19OlDDMgWy/H0wkaMCWrX0pLQGS0eMY8scTSfZT\njnEjS5NRMc6n/jK6LxOSnA==\n-----END PRIVATE KEY-----\n"",
  ""client_email"": ""670954940961-compute@developer.gserviceaccount.com"",
  ""client_id"": ""114871753611658683772"",
  ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
  ""token_uri"": ""https://oauth2.googleapis.com/token"",
  ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
  ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/670954940961-compute%40developer.gserviceaccount.com"",
  ""universe_domain"": ""googleapis.com""
}
");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var credential = GoogleCredential.FromStream(stream)
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
                    new List<object> { lancamento.Data, lancamento.Descricao, lancamento.Valor }
                }
            };

            var appendRequest = _sheetsService.Spreadsheets.Values.Append(valueRange, _spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var response = await appendRequest.ExecuteAsync();

            return Ok(new { message = "Dados salvos com sucesso!" });
        }
    }
}