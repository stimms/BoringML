using Microsoft.ML;
using Microsoft.ML.Trainers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ml_prediction.ml
{
    public class ModelBuilder
    {
        public PredictionEngine<PredictiveModel, Prediction> Build()
        {
            // create a machine learning context
            var context = new MLContext();

            // load the dataset in memory
            Console.WriteLine("Loading data...");
            var data = context.Data.LoadFromTextFile<PredictiveModel>(
                Path.Combine(Environment.CurrentDirectory, "value.csv"),
                hasHeader: true,
                separatorChar: ',');

            // split the data into 80% training and 20% testing partitions
            var partitions = context.Data.TrainTestSplit(data, testFraction: 0.2);


            IEstimator<ITransformer> estimator = context.Transforms.Conversion
                .MapValueToKey(outputColumnName: "countryIdEncoded", inputColumnName: "CountryId")
                .Append(context.Transforms.Conversion.MapValueToKey(outputColumnName: "colorIdEncoded", inputColumnName: "ColorId"));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "countryIdEncoded",
                MatrixRowIndexColumnName = "colorIdEncoded",
                LabelColumnName = "PetId",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(context.Recommendation().Trainers.MatrixFactorization(options));

            ITransformer model = trainerEstimator.Fit(partitions.TrainSet);

            var predictionEngine = context.Model.CreatePredictionEngine<PredictiveModel, Prediction>(model);

            var prediction = model.Transform(partitions.TestSet);
            var metrics = context.Regression.Evaluate(prediction, labelColumnName: "PetId", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());

            return predictionEngine;
        }
    }
}
