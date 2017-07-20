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

namespace Common.DataProvider
{
    /// <summary>
    /// Generator of <see cref="IImagesLoader"/>
    /// </summary>
    public class ImagesLoaderGenerator
    {
        /// <summary>
        /// Generate an instance of <see cref="IImagesLoader"/>
        /// </summary>
        /// <param name="options">Options</param>
        /// <param name="allowedTagNames">Allowed tags. Images without any allowed tag will be omitted</param>
        /// <returns>Desired instance of <see cref="IImagesLoader"/></returns>
        public static IImagesLoader GenerateImagesLoader(Options options, ICollection<string> allowedTagNames = null)
        {
            switch (options.ImageSource)
            {
                case "local":
                    switch (options.ImageOrganization)
                    {
                        case "metainfo":
                            return new LocalImagesLoaderByMetaInfoFile(options.WorkDir, options.ImageMetaInfoFileName, allowedTagNames);
                        case "dir":
                            return new LocalImagesLoaderByTagDir(options.WorkDir, allowedTagNames);
                        default:
                            throw new InvalidOperationException($"Invalid value for {nameof(options.ImageOrganization)}: {options.ImageOrganization}");
                    }

                case "url":
                    return new UrlImagesLoader(options.WorkDir + options.ImageMetaInfoFileName, allowedTagNames);
                default:
                    throw new InvalidOperationException($"{options.ImageSource} is not valid.");
            }
        }
    }
}
