// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: https://azure.microsoft.com/en-us/services/cognitive-services
// 
// Microsoft Cognitive Services GitHub:
// https://github.com/Microsoft/Cognitive-CustomVision-Windows
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using System;
using Common;
using Common.DataProvider;
using Microsoft.Cognitive.CustomVision;
using Prediction.Measurements;
using Prediction.Predictors;

namespace Prediction
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = Options.InitializeOptions<PredictionOptions>(args);
            if (options == null)
            {
                return;
            }

            var images = ImagesLoaderGenerator.GenerateImagesLoader(options, options.AllowedTagNames).LoadImages();
            var predictionEndpoint = new PredictionEndpoint(new PredictionEndpointCredentials(options.PredictionKey));

            if (options.BaseUri != null)
            {
                Console.WriteLine($"The default base uri is {predictionEndpoint.BaseUri}. Changed it to {options.BaseUri}.");
                predictionEndpoint.BaseUri = options.BaseUri;
            }

            PrecisionAndRecall precisionAndRecall;

            using (var predictor = ConstructPredictor(predictionEndpoint, options.WorkDir + options.PredictionFileName, options.MilliSecondsBetweenPredictions))
            {
                 var batchPredictor = new BatchPredictorWithRateLimit(predictor, options.MilliSecondsBetweenPredictions);
                 precisionAndRecall = PrecisionAndRecall.ComputeAsync(
                        images,
                        options.Threshold,
                        async imgs => await batchPredictor.PredictWithImagesAsync(imgs, options.ProjectId, options.IterationId),
                        options.AllowedTagNames).Result;
            }

            Console.WriteLine($"Precision: {precisionAndRecall.Precision}");
            Console.WriteLine($"Recall: {precisionAndRecall.Recall}");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static SinglePredictorWithPredictionsToFile ConstructPredictor(IPredictionEndpoint predictionEndpoint, string predictionFilePath, TimeSpan milliSecondsBetweenPrediction)
        {
            return new SinglePredictorWithPredictionsToFile(
                new SinglePredictorWithConsoleOutput(new SinglePredictorWithRetry(new SinglePredictor(predictionEndpoint), milliSecondsBetweenPrediction)), predictionFilePath);
        }
    }
}
