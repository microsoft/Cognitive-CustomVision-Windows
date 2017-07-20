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
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;
using Microsoft.Rest;

namespace Prediction.Predictors
{
    /// <summary>
    /// Predictor that can predict for a single image using <see cref="IPredictionEndpoint"/>
    /// </summary>
    internal class SinglePredictor : ISinglePredictor
    {
        public SinglePredictor(IPredictionEndpoint predictionEndpoint)
        {
            PredictionEndpoint = predictionEndpoint;
        }

        private IPredictionEndpoint PredictionEndpoint { get; }

        public async Task<ImagePredictionResultModel> PredictWithImageAsync(Image image, Guid projectId, Guid? iterationId = null)
        {
            try
            {
                return image.IsPathAUri
                    ? await PredictionEndpoint.PredictImageUrlAsync(projectId, new ImageUrl(image.Path), iterationId)
                    : await image.OperateOnImageStreamAsync(x => PredictionEndpoint.PredictImageAsync(projectId, x, iterationId));
            }
            catch (HttpOperationException)
            {
                return null;
            }
        }
    }
}
