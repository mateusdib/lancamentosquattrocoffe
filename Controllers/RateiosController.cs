using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LancamentosQuattroCoffe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RateiosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string centroCusto, [FromQuery] decimal valor)
        {

            if (valor <= 0)
                return BadRequest("O valor da despesa deve ser maior que zero.");

            var percentuaisSocietariosJson = Environment.GetEnvironmentVariable("percentuaisSocietariosEnv");

            if (string.IsNullOrWhiteSpace(percentuaisSocietariosJson))
                return StatusCode(500, "Variável de ambiente 'percentuaisSocietariosEnv' não está definida.");

            Dictionary<string, Dictionary<string, decimal>> dados;
            try
            {
                dados = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(percentuaisSocietariosJson);
            }
            catch (JsonException)
            {
                return StatusCode(500, "Erro ao desserializar os percentuais societários. Verifique o formato do JSON na variavel de ambiente.");
            }

            if (!dados.ContainsKey(centroCusto))
                return BadRequest("Centro de custo inválido.");

            var percentuais = dados[centroCusto];

            var rateios = percentuais.ToDictionary(
                x => x.Key,
                x => Math.Round((x.Value / 100m) * valor, 2)
            );

            return Ok(rateios);
        }
    }
}
