using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorModelo : ControllerBase
    {
        //Calcaito de jose, quien ademas me dijo que tenia que ser asi para que funcionase asi que nada con Dios
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _model;

        public ControladorModelo(PredictionEnginePool<ModelInput, ModelOutput> model)
        {
            _model = model;
        }

        [HttpGet]
        public ModelOutput Predict(string text)
        {
            ModelInput input = new ModelInput { Text = text };

            ModelOutput output = _model.Predict(input);

            return output;
        }
    }
}
