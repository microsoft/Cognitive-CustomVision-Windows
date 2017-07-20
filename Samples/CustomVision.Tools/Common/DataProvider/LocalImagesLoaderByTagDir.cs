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
using System.IO;
using System.Linq;
using Common.Model;

namespace Common.DataProvider
{
    /// <summary>
    /// Load images from subfolders and use the folder name as the tag name
    /// </summary>
    public class LocalImagesLoaderByTagDir : LocalImagesLoader
    {
        public LocalImagesLoaderByTagDir(string workDir, ICollection<string> allowedTagNames = null) : base(workDir, allowedTagNames)
        {
        }

        /// <summary>
        /// Get the names of directories within <paramref name="dir"/> as tag names and filter out the ones not in <paramref name="allowedTagNames"/>
        /// if <paramref name="allowedTagNames"/> is null, then not filter is applied
        /// </summary>
        /// <param name="dir">Directory within which sub-directories' names are used as tag names</param>
        /// <param name="allowedTagNames">Allowed tag names</param>
        /// <returns>Tag names in <paramref name="dir"/></returns>
        public static IEnumerable<string> GetTagNamesFromDir(string dir, ICollection<string> allowedTagNames)
        {
            var tagNames = Directory.GetDirectories(dir).Select(x => x.Split('\\').Last());
            return (allowedTagNames == null || allowedTagNames.Count == 0) ? tagNames : tagNames.Where(allowedTagNames.Contains);
        }

        public override IEnumerable<Image> LoadImages()
        {
            var tagNames = GetTagNamesFromDir(WorkDir, AllowedTagNames);
            foreach (var tagName in tagNames)
            {
                var imageFilePaths = GetAllImagePathsInFolder(WorkDir + tagName, LegalImageExtensions);
                foreach (var imageFilePath in imageFilePaths)
                {
                    yield return new Image
                    {
                        Path = imageFilePath,
                        TagNames = new[] { tagName } 
                    };
                }
            }
        }
    }
}
