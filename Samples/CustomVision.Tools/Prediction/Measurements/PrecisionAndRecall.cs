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
using System.Linq;
using System.Threading.Tasks;
using Common.Model;
using Microsoft.Cognitive.CustomVision.Models;

namespace Prediction.Measurements
{
    public class PrecisionAndRecall
    {
        public PrecisionAndRecall(int numPositives, int numTrues, int numTruePositives)
        {
            NumPositives = numPositives;
            NumTrues = numTrues;
            NumTruePositives = numTruePositives;
        }

        /// <summary>
        /// Gets or sets the number of positive predictions
        /// </summary>
        public int NumPositives { get; set; }

        /// <summary>
        /// Gets or sets the number of true predictions
        /// </summary>
        public int NumTrues { get; set; }

         /// <summary>
        /// Gets or sets the number of true positive predictions
        /// </summary>
        public int NumTruePositives { get; set; }

        /// <summary>
        /// Gets the prediction precision
        /// </summary>
        public double Precision => (double)NumTruePositives / NumPositives;

        /// <summary>
        /// Gets the prediction recall 
        /// </summary>
        public double Recall => (double)NumTruePositives / NumTrues;

        /// <summary>
        /// Gets the prediction F1 score
        /// </summary>
        public double F1Score => 2 * Precision * Recall / (Precision + Recall);

        /// <summary>
        /// Runs the <paramref name="predict"/> on <paramref name="images"/> and compute the <see cref="PrecisionAndRecall"/> using <paramref name="threshold"/>
        /// </summary>
        /// <param name="images">Images to predict and evaluate</param>
        /// <param name="threshold">Threshold for computing precision and recall</param>
        /// <param name="predict">Prediction function</param>
        /// <param name="allowedTagNames">Tags considered in computation</param>
        /// <returns>Precision and recall</returns>
        public static async Task<PrecisionAndRecall> ComputeAsync(
            IEnumerable<Image> images,
            double threshold,
            Func<IEnumerable<Image>, Task<IEnumerable<ImagePredictionResultModel>>> predict,
            ICollection<string> allowedTagNames)
        {
            var imageList = images.ToList();
            var predictions = await predict(imageList);

            return Compute(imageList, threshold, predictions, allowedTagNames);
        }

        private static PrecisionAndRecall Compute(
            IEnumerable<Image> images,
            double threshold,
            IEnumerable<ImagePredictionResultModel> predictions,
            ICollection<string> allowedTagNames)
        {
            var imageList = images.ToList();

            var resultSummary = imageList.Zip(
                predictions, 
                (image, prediction) =>
                {
                    var predictedLabels = prediction?.Predictions
                        .Where(x => x.Probability > threshold && (allowedTagNames == null || allowedTagNames.Count == 0 || allowedTagNames.Contains(x.Tag)))
                        .Select(x => x.Tag)
                        .ToList();

                    if (predictedLabels == null)
                    {
                        return new PrecisionAndRecall(0, 0, 0);
                    }

                    var trueLabels = new HashSet<string>(image.TagNames);
                    var truePositives = new HashSet<string>(predictedLabels);
                    truePositives.IntersectWith(trueLabels);

                    return new PrecisionAndRecall(predictedLabels.Count, trueLabels.Count, truePositives.Count);
                });

            var numPositives = 0;
            var numTrues = 0;
            var numTruePositives = 0;

            foreach (var result in resultSummary)
            {
                numPositives += result.NumPositives;
                numTrues += result.NumTrues;
                numTruePositives += result.NumTruePositives;
            }

            return new PrecisionAndRecall(numPositives, numTrues, numTruePositives);
        }
    }
}
