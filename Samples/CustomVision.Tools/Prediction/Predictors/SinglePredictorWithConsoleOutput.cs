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
using System.Threading.Tasks;
using Common.Model;
using Microsoft.Cognitive.CustomVision.Models;

namespace Prediction.Predictors
{
    /// <summary>
    /// A decorator for <see cref="ISinglePredictor"/> that writes helpful information to console during prediction
    /// </summary>
    internal class SinglePredictorWithConsoleOutput : SinglePredictorDecorator
    {
        public SinglePredictorWithConsoleOutput(ISinglePredictor predictor) : base(predictor)
        {
        }

        public override async Task<ImagePredictionResultModel> PredictWithImageAsync(Image image, Guid projectId, Guid? iterationId = null)
        {
            Console.Write($"Prediction for {image.Path}...");
            var prediction = await Predictor.PredictWithImageAsync(image, projectId, iterationId);
            if (prediction == null)
            {
                Console.WriteLine("Failed.");
                Console.WriteLine($"Please make sure : \n- the project id {projectId} is valid; \n- prediction endpoint url is right;");
                if (iterationId == null)
                {
                    Console.WriteLine("- a default iteration is specified;");
                }

                return null;
            }

            Console.WriteLine("Succeeded!");
            Console.Write("Predicted labels and probabilities: ");

            foreach (var label in prediction.Predictions)
            {
                Console.Write($"[Label: {label.Tag}, Probability: {label.Probability}]");
            }

            Console.WriteLine(string.Empty);
            Console.Write("True labels: ");
            foreach (var label in image.TagNames)
            {
                Console.Write($"[Label: {label}]");
            }

            Console.WriteLine(string.Empty);
            return prediction;
        }
    }
}
