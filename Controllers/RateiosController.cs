using Microsoft.AspNetCore.Mvc;

namespace LancamentosQuattroCoffe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RateiosController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string centroCusto, [FromQuery] decimal valor)
        {
            var rateios = new List<Tuple<string, decimal>>();

            switch (centroCusto)
            {
                case "Mantibio Geral":
                    rateios.Add(new Tuple<string, decimal>("Equipe A", valor * 0.5m));
                    rateios.Add(new Tuple<string, decimal>("Equipe B", valor * 0.5m));
                    break;

                case "Capao Sujo Geral":
                    rateios.Add(new Tuple<string, decimal>("Capão Norte", valor * 0.6m));
                    rateios.Add(new Tuple<string, decimal>("Capão Sul", valor * 0.4m));
                    break;

                case "Percentual Societario":
                    rateios.Add(new Tuple<string, decimal>("Sócio 1", valor * 0.7m));
                    rateios.Add(new Tuple<string, decimal>("Sócio 2", valor * 0.3m));
                    break;

                case "Combustivel Percentual societario":
                    rateios.Add(new Tuple<string, decimal>("Carro 1", valor * 0.4m));
                    rateios.Add(new Tuple<string, decimal>("Carro 2", valor * 0.6m));
                    break;

                default:
                    return BadRequest("Centro de custo inválido.");
            }

            return Ok(rateios);
        }
    }
}
