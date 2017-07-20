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
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Model;
using Microsoft.Cognitive.CustomVision.Models;

namespace Prediction.Predictors
{
    /// <summary>
    /// Predicting tags for a batch of images, with controllable prediction rate 
    /// </summary>
    internal class BatchPredictorWithRateLimit
    {
        public BatchPredictorWithRateLimit(ISinglePredictor predictor, TimeSpan gapTimeBetweenPrediction)
        {
            Predictor = predictor;
            GapTimeBetweenPrediction = gapTimeBetweenPrediction;
        }

        private ISinglePredictor Predictor { get; }

        private TimeSpan GapTimeBetweenPrediction { get; }

        /// <summary>
        /// Predict tags for <paramref name="images"/> using project <paramref name="projectId"/> and iteration <paramref name="iterationId"/>
        /// </summary>
        /// <param name="images">Images to predict</param>
        /// <param name="projectId">Project id used for prediction</param>
        /// <param name="iterationId">Iteration id used for prediction. Can be null, if a default iteration is specified</param>
        /// <returns></returns>
        public async Task<IEnumerable<ImagePredictionResultModel>> PredictWithImagesAsync(IEnumerable<Image> images, Guid projectId, Guid? iterationId = null)
        {
            var result = new List<ImagePredictionResultModel>();
            foreach (var image in images)
            {
                await Task.Delay(GapTimeBetweenPrediction);
                result.Add(await Predictor.PredictWithImageAsync(image, projectId, iterationId));
            }

            return result;
        }
    }
}
