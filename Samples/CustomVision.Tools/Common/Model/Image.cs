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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Model
{
    /// <summary>
    /// Model for image
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or sets the image id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the image path. Can be url or local path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets whether the path is a uri
        /// </summary>
        public bool IsPathAUri => Uri.IsWellFormedUriString(Path, UriKind.Absolute);

        /// <summary>
        /// Gets or sets tag names of the image
        /// </summary>
        public ICollection<string> TagNames { get; set; }

        /// <summary>
        /// Gets or sets tag ids of the image
        /// </summary>
        public ICollection<Guid> TagIds { get; set; }

        /// <summary>
        /// Helper function that allows a function <paramref name="funcAsync"/> to operate on the streams of <paramref name="images"/>.
        /// </summary>
        /// <typeparam name="T">Type of return value</typeparam>
        /// <param name="funcAsync">Function to run</param>
        /// <param name="images">Images which <paramref name="funcAsync"/> runs on the streams of</param>
        /// <returns>Result</returns>
        public static async Task<T> OperateOnImageStreamsAsync<T>(Func<IEnumerable<Stream>, Task<T>> funcAsync, IEnumerable<Image> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            using (var httpClient = new HttpClient())
            {
                var imagesStreams = images.Select(x => x.IsPathAUri ? httpClient.GetStreamAsync(x.Path).Result : File.OpenRead(x.Path)).ToList();
                var result = await funcAsync(imagesStreams);

                foreach (var imagesStream in imagesStreams)
                {
                    imagesStream.Dispose();
                }

                return result;
            }
        }

        /// <summary>
        /// Runs <paramref name="func"/> on the image stream
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="func">Function to run</param>
        /// <returns>Result</returns>
        public T OperateOnImageStream<T>(Func<Stream, T> func)
        {
            if (IsPathAUri)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var imageContent = httpClient.GetStreamAsync(Path).Result)
                    {
                        return func(imageContent);
                    }
                }
            }

            using (var imageContent = File.OpenRead(Path))
            {
                return func(imageContent);
            }
        }

        /// <summary>
        /// Runs <paramref name="funcAsync"/> on image streams
        /// </summary>
        /// <typeparam name="T">Return value type</typeparam>
        /// <param name="funcAsync">Function to run</param>
        /// <returns>Result</returns>
        public async Task<T> OperateOnImageStreamAsync<T>(Func<Stream, Task<T>> funcAsync)
        {
            if (IsPathAUri)
            {
                using (var httpClient = new HttpClient())
                {
                    using (var imageContent = httpClient.GetStreamAsync(Path).Result)
                    {
                        return await funcAsync(imageContent);
                    }
                }
            }

            using (var imageContent = File.OpenRead(Path))
            {
                return await funcAsync(imageContent);
            }
        }
    }
}
