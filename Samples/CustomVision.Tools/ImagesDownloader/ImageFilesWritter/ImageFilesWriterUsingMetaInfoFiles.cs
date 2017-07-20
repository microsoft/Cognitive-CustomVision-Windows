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
using System.Linq;

namespace ImagesDownloader.ImageFilesWritter
{
    /// <summary>
    /// For writing images to disk and saving image info such as path and tag info in metainfo file
    /// </summary>
    internal class ImageFilesWriterUsingMetaInfoFiles : IImageFilesWriter
    {
        public ImageFilesWriterUsingMetaInfoFiles(string workDir, string metaInfoFileName)
        {
            WorkDir = workDir;
            MetaInfoFileName = metaInfoFileName;
        }

        private string MetaInfoFileName { get; }

        private string WorkDir { get; }

        public void WriteImagesToDisk(IEnumerable<Common.Model.Image> images)
        {
            Console.WriteLine($"Created image metainfo file: {WorkDir + MetaInfoFileName}");
            using (var metaInfoFile = new StreamWriter(WorkDir + MetaInfoFileName))
            {
                foreach (var image in images)
                {
                    var fileName = image.Id + ".jpg";
                    
                    image.OperateOnImageStream(Image.FromStream).Save(WorkDir + fileName, ImageFormat.Jpeg);
                    Console.WriteLine($"Downloaded and saved {fileName} with tags being \"{image.TagNames?.Aggregate((x, y) => x + "," + y)}\".");
                    WriteRowInMetaFile(metaInfoFile, image, fileName);
                }
            }
        }

        private static void WriteRowInMetaFile(TextWriter metaInfoFile, Common.Model.Image image, string fileName)
        {
            var tagNames = image.TagNames?.ToList();
            var row = tagNames != null && tagNames.Any()
                ? tagNames.Aggregate(string.Empty, (x, y) => $"{x},{y}")
                : string.Empty;
            metaInfoFile.WriteLine(fileName + row);
        }
    }
}
