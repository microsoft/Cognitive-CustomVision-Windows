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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImagesDownloader.ImageFilesWritter
{
    /// <summary>
    /// For writing images to corresponding folders based on their tags
    /// e.g. an image with tags: food, hot will be saved in both "food" and "hot" folders
    /// </summary>
    internal class ImageFilesWriterUsingTagDir : IImageFilesWriter
    {
        public ImageFilesWriterUsingTagDir(string workDir)
        {
            WorkDir = workDir;
        }

        private string DirNameForImagesWithoutTags => WorkDir + "UntaggedImages\\";

        private string WorkDir { get; }

        public void WriteImagesToDisk(IEnumerable<Common.Model.Image> images)
        {
            foreach (var image in images)
            {
                var img = image.OperateOnImageStream(Image.FromStream);
                var fileName = image.Id + ".jpg";

                if (image.TagNames == null || image.TagNames.Count == 0)
                {
                    Directory.CreateDirectory(DirNameForImagesWithoutTags);
                    Console.WriteLine($"Downloaded and saved {DirNameForImagesWithoutTags + fileName}.");
                    img.Save(DirNameForImagesWithoutTags + fileName, ImageFormat.Jpeg);
                    continue;
                }

                foreach (var tagName in image.TagNames)
                {
                    Directory.CreateDirectory(WorkDir + tagName);
                    var imagePath = WorkDir + tagName + "\\" + fileName;
                    Console.WriteLine($"Downloaded and saved {imagePath}.");
                    img.Save(imagePath, ImageFormat.Jpeg);
                }
            }
        }
    }
}
