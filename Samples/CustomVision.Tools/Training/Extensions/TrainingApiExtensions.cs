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

using System.Collections.Generic;
using System.Threading.Tasks;
using Common.DataProvider;
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;
using Training.ImagesUploader;
using Common.Model;

namespace Training.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ITrainingApi"/>
    /// </summary>
    internal static class TrainingApiExtensions
    {
        /// <summary>
        /// Read and upload images to <paramref name="project"/> based on <paramref name="options"/>
        /// </summary>
        public static async Task<CreateImageSummaryModel> ReadAndUploadImagesAsync(this ITrainingApi trainingApi,
            Project project, TrainingOptions options, ICollection<string> allowedTagNames)
        {
            var images = ImagesLoaderGenerator.GenerateImagesLoader(options, allowedTagNames).LoadImages();
            return await ImagesUploaderGenerator.GenerateImagesUploader(options, trainingApi).UploadImagesAsync(images, project.Id);
        }
    }
}
