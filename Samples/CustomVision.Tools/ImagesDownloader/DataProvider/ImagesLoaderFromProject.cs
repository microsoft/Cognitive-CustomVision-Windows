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
using Common.DataProvider;
using Common.Model;
using Microsoft.Cognitive.CustomVision;
using Microsoft.Cognitive.CustomVision.Models;

namespace ImagesDownloader.DataProvider
{
    internal class ImagesLoaderFromProject : ImagesLoader
    {
        private const int ImageDownloadBatchSize = 50;

        public ImagesLoaderFromProject(ITrainingApi trainingApi, Guid projectId, Guid? iterationId, IEnumerable<string> allowedTagNames) : base(allowedTagNames)
        {
            TrainingApi = trainingApi;
            ProjectId = projectId;
            IterationId = iterationId;
        }

        private ITrainingApi TrainingApi { get; }

        private Guid ProjectId { get; }

        private Guid? IterationId { get; }

        public override IEnumerable<Image> LoadImages()
        {
            var tagIds = GetTagIds(AllowedTagNames).ToList();
            if (TagFilterEnabled && tagIds.Count == 0)
            {
                throw new InvalidOperationException("No tag provided is valid");
            }

            return tagIds.Count == 0 ? GetAllImages() : GetImagesByTags(tagIds);
        }

        private IEnumerable<Image> GetAllImages()
        {
            var taggedImages = GetImagesInBatch((batchSize, skip) => TrainingApi.GetAllTaggedImages(ProjectId, IterationId, take: batchSize, skip: skip));
            var untaggedImages = GetImagesInBatch((batchSize, skip) => TrainingApi.GetAllUntaggedImages(ProjectId, IterationId, take: batchSize, skip: skip));
            return taggedImages.Concat(untaggedImages);
        }

        private IEnumerable<Image> GetImagesByTags(ICollection<Guid> tagIds)
        {
            var images = GetImagesInBatch((batchSize, skip) => TrainingApi.GetImagesByTags(ProjectId, IterationId, take: batchSize, skip: skip, tagIds: tagIds.Select(x => x.ToString()).ToList()));
            return images.Select(x =>
            {
                var allowed = x.TagIds.Select(tagIds.Contains).ToList();
                x.TagIds = x.TagIds.Where((val,i) => allowed[i]).ToList();
                x.TagNames = x.TagNames.Where((val, i) => allowed[i]).ToList();
                return x;
            });
        }

        private IEnumerable<Image> GetImagesInBatch(Func<int, int, IEnumerable<ImageModel>> getAnImageModelBatch)
        {
            var tagIdToName = TrainingApi.GetTags(ProjectId, IterationId).Tags.ToDictionary(x => x.Id, x => x.Name);

            var batchIndex = 0;

            IList<ImageModel> imageModels;
            do
            {
                imageModels = getAnImageModelBatch(ImageDownloadBatchSize, batchIndex * ImageDownloadBatchSize).ToList();
                foreach (var imageModel in imageModels)
                {
                    yield return new Image
                    {
                        Id = imageModel.Id,
                        Path = imageModel.ImageUri,
                        TagIds = imageModel.Labels?.Select(x => x.TagId).ToList(),
                        TagNames = imageModel.Labels?.Select(x => tagIdToName[x.TagId]).ToList()
                    };
                }

                batchIndex++;
            } while (imageModels.Count != 0);
        }

        private IEnumerable<Guid> GetTagIds(IEnumerable<string> tagNames)
        {
            var tagNameToDictionary = TrainingApi.GetTags(ProjectId, IterationId).Tags.ToDictionary(x => x.Name, x => x.Id);

            return tagNames?.Select(x =>
            {
                Guid id;
                return tagNameToDictionary.TryGetValue(x, out id) ? id : Guid.Empty;
            }).Where(x => x != Guid.Empty).ToList() ?? Enumerable.Empty<Guid>();
        }
    }
}
