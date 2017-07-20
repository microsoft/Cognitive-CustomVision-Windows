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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Model;
using Microsoft.Cognitive.CustomVision.Models;

namespace Prediction.Predictors
{
    /// <summary>
    /// A decorator for <see cref="ISinglePredictor"/> that writes predictions to csv file
    /// </summary>
    internal class SinglePredictorWithPredictionsToFile : SinglePredictorDecorator, IDisposable
    {
        public SinglePredictorWithPredictionsToFile(ISinglePredictor predictor, string outputFileName) : base(predictor)
        {
            OutputFileName = outputFileName;
            OutputFileStream = new StreamWriter(OutputFileName);
        }

        public SinglePredictorWithPredictionsToFile(ISinglePredictor predictor, StreamWriter outputFileStream) : base(predictor)
        {
            OutputFileStream = outputFileStream;
        }

        private string OutputFileName { get; }

        private StreamWriter OutputFileStream { get; }  

        public override async Task<ImagePredictionResultModel> PredictWithImageAsync(Image image, Guid projectId, Guid? iterationId = null)
        {
            var prediction = await Predictor.PredictWithImageAsync(image, projectId, iterationId);
            if (prediction != null)
            {
                var predictionLine = prediction.Predictions.Select(x => $"{x.Tag}:{x.Probability}").Aggregate((x, y) => $"{x},{y}");
                await OutputFileStream.WriteLineAsync($"{image.Path},{predictionLine}");
            }

            return prediction;
        }

        public void Dispose()
        {
            if (OutputFileName != null)
            {
                OutputFileStream.Dispose();
            }
        }
    }
}
