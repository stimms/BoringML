using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ml_prediction.ml;

namespace ml_prediction.Pages
{
    public class PetController : Controller
    {
        private PredictionEngine<PredictiveModel, Prediction> engine;
        public PetController(PredictionEngine<PredictiveModel, Prediction> engine)
        {
            this.engine = engine;
        }

        [Route("/api/pets")]
        public IActionResult Index(int countryId, int colorId)
        {
            var prediction = engine.Predict(new PredictiveModel { CountryId = countryId, ColorId = countryId, PetId = 1 });
            var pets = new List<string> {
                "Cat",
                "Dog",
                "Monkey",
                "Bat",
                "Gerbil",
                "Graboid"
            };

            return new JsonResult(new { items = pets, predicted = Math.Round(prediction.Score) });
        }
    }
}