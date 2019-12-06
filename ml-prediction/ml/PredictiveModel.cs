using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ml_prediction.ml
{
    public class PredictiveModel
    {

        [LoadColumn(0)] public int CountryId { get; set; }
        [LoadColumn(1)] public int ColorId { get; set; }
        [LoadColumn(2)] public Single PetId { get; set; }
    }

    public class Prediction
    {
        public Single PetId { get; set; }
        public Single Score { get; set; }
    }
}
