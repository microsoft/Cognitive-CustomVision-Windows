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
using System.Configuration;
using System.IO;
using System.Linq;

namespace Common.DataProvider
{
    /// <summary>
    /// Image loader that loads images from local disk
    /// </summary>
    public abstract class LocalImagesLoader : ImagesLoader
    {
        protected static readonly IEnumerable<string> LegalImageExtensions = ConfigurationManager.AppSettings["ImageExtensions"]?.Split(',').Select(x => "*." + x);

        protected LocalImagesLoader(string workDir, IEnumerable<string> allowedTagNames) : base(allowedTagNames)
        {
            WorkDir = workDir;
        }

        protected string WorkDir { get; }

        protected static IEnumerable<string> GetAllImagePathsInFolder(string folderPath, IEnumerable<string> extensions)
        {
            return extensions.SelectMany(x => Directory.EnumerateFiles(folderPath, x));
        }
    }
}
