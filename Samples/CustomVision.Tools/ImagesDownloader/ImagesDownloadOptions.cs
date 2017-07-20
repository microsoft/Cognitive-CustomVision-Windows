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
using CommandLine;
using Common;
using Microsoft.Cognitive.CustomVision;
using Training;

namespace ImagesDownloader
{
    /// <summary>
    /// Options controlling image downloading
    /// </summary>
    internal class ImagesDownloadOptions : Options
    {
        [Option('d', Default = "project_info.json", Required = false, HelpText = "JSON file name of the project information.")]
        public string ProjectInfoFileName { get; set; }

        [Option('g', Default = null, Required = false, HelpText = "Tag names of the images to download. All images will be downloaded, if not specified.", Separator = ',')]
        public override ICollection<string> AllowedTagNames { get; set; }

        [Option('i', Required = false, HelpText = "(Default: latest iteration) Id of the iteration.")]
        public override Guid InternalIterationId { get; set; } = Guid.Empty;

        [Option('k', Required = true, HelpText = "Training key.")]
        public string Trainingkey { get; set; }

        /// <remarks>
        /// Overriding this property out of adding a default value to the option
        /// </remarks>>
        [Option('l', Default = "metainfo.csv", Required = false, HelpText = "File name of the image metainfo.")]
        public override string ImageMetaInfoFileName { get; set; }

        /// <remarks>
        /// <see cref="ProjectId"/> is required for image downloading
        /// </remarks>
        [Option('p', Required = true, HelpText = "Id of the project.")]
        public override Guid ProjectId { get; set; }

        /// <remarks>
        /// Hide this option 
        /// </remarks>>
        [Option('s', Hidden = true, Required = false, HelpText = "Source of images. Can be \"local\", \"url\", or \"existing\".")]
        public override string ImageSource { get; set; }

        public override bool Validate(out string errorMessage)
        {
            if (!base.Validate(out errorMessage))
            {
                return false;
            }

            if (!Directory.Exists(WorkDir))
            {
                Directory.CreateDirectory(WorkDir);
                Console.WriteLine($"{WorkDir} does not exist and is created.");
            }

            return true;
        }

        public override bool ValidateWithTrainingApi(ITrainingApi trainingApi, out string errorMessage)
        {
            if (!base.ValidateWithTrainingApi(trainingApi, out errorMessage))
            {
                return false;
            }

            if (AllowedTagNames != null && AllowedTagNames.Count != 0)
            {
                var existingTagNames = new HashSet<string>(trainingApi.GetTagsAsync(ProjectId, IterationId).Result.Tags.Select(x => x.Name));
                var invalidTagNames = AllowedTagNames.Where(x => !existingTagNames.Contains(x)).ToList();
                if (invalidTagNames.Count != 0)
                {
                    errorMessage = $"{ErrorMsgPrefix}Some tag names are invalid: {invalidTagNames.Aggregate((x, y) => x + "," + y)}";
                }
            }

            return errorMessage == null;
        }
    }
}
