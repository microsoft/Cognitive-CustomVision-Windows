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
    /// Interface defining a predictor that can predict for a single image
    /// </summary>
    internal interface ISinglePredictor
    {
        /// <summary>
        /// Predict tags for one image <paramref name="image"/>, using project <paramref name="projectId"/> and iteration <paramref name="iterationId"/>
        /// </summary>
        /// <param name="image">Image to predict</param>
        /// <param name="projectId">Project id used for prediction</param>
        /// <param name="iterationId">Iteration id used for prediction. Can be null if a default one is specified</param>
        /// <returns></returns>
        Task<ImagePredictionResultModel> PredictWithImageAsync(Image image, Guid projectId, Guid? iterationId = null);
    }
}
