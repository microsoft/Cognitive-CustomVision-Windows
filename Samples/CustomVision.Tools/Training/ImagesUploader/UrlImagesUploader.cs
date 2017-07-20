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
using Common.Helper;
using Common.Model;
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;
using Microsoft.Rest;

namespace Training.ImagesUploader
{
    internal class UrlImagesUploader : ImagesUploaderInBatches
    {
        public UrlImagesUploader(ITrainingApi trainingApi, int batchSize,int retryTimes = 3) : base(trainingApi, batchSize)
        {
            RetryTimes = retryTimes;
        }

        private int RetryTimes { get; }

        protected override async Task<CreateImageSummaryModel> UploadImagesByTagsAsync(IEnumerable<Image> images, Guid projectId, ICollection<Guid> tagIds)
        {
            var imageArray = images.ToList();
            Func<Task<CreateImageSummaryModel>> createImagesFromUrls = async () => await TrainingApi.CreateImagesFromUrlsAsync(
                projectId,
                new ImageUrlCreateBatch(tagIds: tagIds.ToList(), urls: imageArray.Select(x => x.Path).ToList()));

            return await RetryHelper.RetryFuncAsync(
                createImagesFromUrls,
                new[] { typeof(HttpOperationException), typeof(TaskCanceledException) },
                RetryTimes);
        }
    }
}
