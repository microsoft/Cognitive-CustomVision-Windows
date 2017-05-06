// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: https://www.microsoft.com/cognitive-services
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Cognitive.CustomVision.Models;
using Microsoft.Rest;

namespace Microsoft.Cognitive.CustomVision
{
    public static partial class TrainingApiExtensions
    {
        /// <summary>
        /// Adds the provided images to the current project iteration
        /// </summary>
        /// <param name='operations'>
        /// The operations group for this extension method.
        /// </param>
        /// <param name='projectId'>
        /// The project id.
        /// </param>
        /// <param name='imageData'>
        /// </param>
        /// <param name='tagIds'>
        /// The tags ids to associate with the image batch.
        /// </param>
        public static CreateImageSummaryModel CreateImagesFromData(this ITrainingApi operations, System.Guid projectId, IEnumerable<Stream> imageData, IList<Guid> tagIds = default(IList<Guid>))
        {
            return operations.CreateImagesFromDataAsync(projectId, imageData, tagIds).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Adds the provided images to the current project iteration
        /// </summary>
        /// <param name='operations'>
        /// The operations group for this extension method.
        /// </param>
        /// <param name='projectId'>
        /// The project id.
        /// </param>
        /// <param name='imageData'>
        /// </param>
        /// <param name='tagIds'>
        /// The tags ids to associate with the image batch.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        public static async Task<CreateImageSummaryModel> CreateImagesFromDataAsync(this ITrainingApi operations, System.Guid projectId, IEnumerable<Stream> imageData, IList<Guid> tagIds = default(IList<Guid>), CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var _result = await operations.CreateImagesFromDataWithHttpMessagesAsync(projectId, imageData, tagIds, null, cancellationToken).ConfigureAwait(false))
            {
                return _result.Body;
            }
        }

        /// <summary>
        /// Adds the provided images to the current project iteration
        /// </summary>
        /// <param name='operations'>
        /// The operations group for this extension method.
        /// </param>
        /// <param name='projectId'>
        /// The project id.
        /// </param>
        /// <param name='imageData'>
        /// </param>
        /// <param name='tagIds'>
        /// The tags ids to associate with the image batch.
        /// </param>
        /// <param name='customHeaders'>
        /// Headers that will be added to request.
        /// </param>
        public static HttpOperationResponse<CreateImageSummaryModel> CreateImagesFromDataWithHttpMessages(this ITrainingApi operations, System.Guid projectId, IEnumerable<Stream> imageData, IList<Guid> tagIds = default(IList<Guid>), Dictionary<string, List<string>> customHeaders = null)
        {
            return operations.CreateImagesFromDataWithHttpMessagesAsync(projectId, imageData, tagIds, customHeaders, CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
