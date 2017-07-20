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
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;

namespace Training.ImagesUploader
{
    /// <summary>
    /// A decorator for <see cref="IImagesUploader"/> that allows for uploading images in several sub-batches
    /// </summary>
    internal abstract class ImagesUploaderInBatches : IImagesUploader 
    {
        protected ImagesUploaderInBatches(ITrainingApi trainingApi, int batchSize)
        {
            BatchSize = batchSize;
            TrainingApi = trainingApi;
        }

        private int BatchSize { get; }

        public ITrainingApi TrainingApi { get; }

        public async Task<CreateImageSummaryModel> UploadImagesAsync(IEnumerable<Image> images, Guid projectId)
        {
            // The images in the same batch share the same set of labels
            var imageBatchesByTags = images.GroupBy(image => ConcatTags(image.TagNames), image => image);
            var uploadResults = new List<CreateImageResultModel>();
            var tagNameToId = (await TrainingApi.GetTagsAsync(projectId)).Tags.ToDictionary(x => x.Name, x => x.Id);
            var numImagesUploaded = 0;

            foreach (var imageBatch in imageBatchesByTags)
            {
                var cnt = 0;
                var subBatchesByIndex = imageBatch.Select((x, idx) => new { image = x, index = idx }).GroupBy(x => x.index / BatchSize, x => x.image).Select(x => x.ToList());

                Console.WriteLine($"Uploading images with labels being {imageBatch.Key}.");

                // subBatch is in UploadBatchSize 
                foreach (var subBatch in subBatchesByIndex)
                {
                    var tagNames = subBatch.First().TagNames;
                    if (tagNames == null || tagNames.Count == 0)
                    {
                        continue;
                    }

                    var tagIds = tagNames.Select(x => GetTagId(projectId, x, tagNameToId).Result).ToList();
                    var batchResult = await UploadImagesByTagsAsync(subBatch, projectId, tagIds);

                    if (batchResult == null)
                    {
                        Console.WriteLine("Failed to receive a response for this batch. Some of the images might be uploaded.");
                        continue;
                    }

                    cnt += batchResult.Images.Count(x => string.Equals(x.Status, "OK", StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine($"Finished uploading {cnt} images in total.");
                    uploadResults.AddRange(batchResult.Images);
                }

                numImagesUploaded += cnt;
            }

            return new CreateImageSummaryModel(isBatchSuccessful: numImagesUploaded != 0, images: uploadResults);
        }

        private async Task<Guid> GetTagId(Guid projectId, string tagName, IDictionary<string, Guid> tagNameToId)
        {
            if (!tagNameToId.ContainsKey(tagName))
            {
                Console.Write($"Create tag with name being {tagName}");
                tagNameToId[tagName] = (await TrainingApi.CreateTagAsync(projectId, tagName)).Id;
                Console.WriteLine($" tag id being {tagNameToId[tagName]}");
            }

            return tagNameToId[tagName];
        }  

        private static string ConcatTags(IEnumerable<string> tags)
        {
            return tags.OrderBy(x => x).Aggregate(
                string.Empty,
                (x, y) =>
                {
                    var sep = x == string.Empty ? string.Empty : ",";
                    return $"{x}{sep}{y}";
                });
        }

        protected abstract Task<CreateImageSummaryModel> UploadImagesByTagsAsync(IEnumerable<Image> images, Guid projectId, ICollection<Guid> tagIds);
    }
}
