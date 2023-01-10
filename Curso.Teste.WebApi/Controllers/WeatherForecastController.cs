using Microsoft.AspNetCore.Mvc;

namespace Curso.Teste.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PrevisoesDoTempoController : ControllerBase
    {
        private static string[] _condicoesClimaticas = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private static WeatherForecast[] _previsoesDoTempo = _condicoesClimaticas.Select((summary, index) => new WeatherForecast
        {
            Id = index,
            Date = DateTime.Now.AddDays(Random.Shared.Next(0, _condicoesClimaticas.Length - 1)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summary
        }).ToArray();

        [HttpGet]
        public IEnumerable<WeatherForecast> ObterTodas()
        {
            return _previsoesDoTempo;
        }

        [HttpGet]
        public WeatherForecast ObterPeloId([FromQuery] int id)
        {
            var previsaoDoTempo = _previsoesDoTempo.FirstOrDefault(previsaoDoTempo => previsaoDoTempo.Id == id);
            if (previsaoDoTempo == null)
                throw new Exception("Não encontramos uma previsão com esse id!");

            return previsaoDoTempo;
        }


        [HttpPost]
        public void Adicionar([FromBody] WeatherForecast previsaoDoTempo)
        {
            _previsoesDoTempo = _previsoesDoTempo.Append(previsaoDoTempo).ToArray();
        }

        [HttpPut]
        public void Ajustar([FromBody] WeatherForecast previsaoDoTempo)
        {
            var indiceDoPrevisaoQueSeraSubstituido = Array.FindIndex(array: _previsoesDoTempo, startIndex: 0, count: 1, match: forecast => forecast.Id == previsaoDoTempo.Id);
            if (indiceDoPrevisaoQueSeraSubstituido == -1)
                throw new Exception("Não encontramos a previsão a ser atualizada");

            _previsoesDoTempo[indiceDoPrevisaoQueSeraSubstituido] = previsaoDoTempo;
        }

        [HttpDelete]
        public void RemoverTodas()
        {
            _previsoesDoTempo = Array.Empty<WeatherForecast>();
        }

        [HttpGet]
        public void RemoverPeloId([FromQuery] List<int> ids)
        {
            _previsoesDoTempo = _previsoesDoTempo.Where((previsaoDoTempo) => !ids.Contains(previsaoDoTempo.Id)).ToArray();
        }
    }
}