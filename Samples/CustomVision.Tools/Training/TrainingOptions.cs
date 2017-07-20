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
using CommandLine;
using Common;

namespace Training
{
    /// <summary>
    /// Options controlling prediction
    /// </summary>
    public class TrainingOptions : Options
    {
        [Option('c', Default = 4, Required = false, HelpText = "Batch size for image uploading.")] 
        public int UploadBatchSize { get; set; }

        [Option('d', Default = "project_info.json", Required = false, HelpText = "JSON file name of the project information for creating a new project.")] 
        public string ProjectInfoFileName { get; set; }

        /// <remarks>
        /// Hide this option for training
        /// </remarks>
        [Option('i', Hidden = true, Required = false)]
        public override Guid InternalIterationId { get; set; } = Guid.Empty;

        [Option('g', Default = null, Required = false, HelpText = "Tag names of the images to upload. All images will be uploaded, if not specified ", Separator = ',')]
        public override ICollection<string> AllowedTagNames { get; set; }

        [Option('k', Required = true, HelpText = "Training key.")] 
        public string Trainingkey { get; set; }

        /// <remarks>
        /// Overriding this property out of adding a default value to the option
        /// </remarks>>
        [Option('l', Default = "metainfo.csv", Required = false, HelpText = "File name of the image metainfo.")]
        public override string ImageMetaInfoFileName { get; set; }

        public override bool Validate(out string errorMessage)
        {
            if (!base.Validate(out errorMessage))
            {
                return false;
            }

            if (!Directory.Exists(WorkDir))
            {
                errorMessage = $"{ErrorMsgPrefix}\n Working directory \"{WorkDir}\" does not exists.";
            }
            else if (ProjectId == Guid.Empty && string.IsNullOrEmpty(ProjectInfoFileName))
            {
                errorMessage = "ERROR(S):\n Cannot retrieve a existing project or create a new one, as project id and project info JSON file are both missing.";
            }
            else if (ImageOrganization == "metainfo" || ImageSource == "url")
            {
                if (!File.Exists(WorkDir + ImageMetaInfoFileName))
                {
                    errorMessage = $"ERROR(S):\n Metainfo file {ImageMetaInfoFileName} does not exists under directory {WorkDir}.";
                }
                else if (ProjectId == Guid.Empty && !File.Exists(WorkDir + ProjectInfoFileName))
                {
                    errorMessage = $"ERROR(S):\n Project info file {ProjectInfoFileName} does not exists under directory {WorkDir}.";
                }
            }

            return errorMessage == null;
        }
    }
}
