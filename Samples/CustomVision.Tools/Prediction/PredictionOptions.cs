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

namespace Prediction
{
    /// <summary>
    /// Options controlling prediction
    /// </summary>
    internal class PredictionOptions : Options
    {
        [Option('g', Default = null, Required = false, HelpText = "Tag names of the images to predict. All images will be used for prediction, if not specified.", Separator = ',')]
        public override ICollection<string> AllowedTagNames { get; set; }

        /// <remarks>
        /// Note that a iteration must be marked as default if <see cref="InternalIterationId"/> is not specified, for prediction
        /// </remarks>
        [Option('i', Required = false, HelpText = "(Default: default iteration) Id of the iteration.")]
        public override Guid InternalIterationId { get; set; } = Guid.Empty;

        [Option('k', Required = true, HelpText = "Prediction key.")]
        public string PredictionKey { get; set; }

        [Option('o', Default = "predictions.csv", Required = false, HelpText = "Prediction file name.")]
        public string PredictionFileName { get; set; }

        /// <remarks>
        /// <see cref="ProjectId"/> is required for making predictions
        /// </remarks>
        [Option('p', Required = true, HelpText = "Id of the project.")]
        public override Guid ProjectId { get; set; }

        [Option('r', Required = false, HelpText = "(Default: 0.5s) Milli-seconds between predictions.")]
        public TimeSpan MilliSecondsBetweenPredictions { get; set; } = new TimeSpan(0, 0, 0, 0, 500);

        [Option('t', Default = 0.9f, HelpText = "Threshold for computing precision and recall.")]
        public double Threshold { get; set; }

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
            else if (ImageSource == "local"
                && ImageOrganization == "metainfo"
                && !string.IsNullOrEmpty(ImageMetaInfoFileName)
                && !File.Exists(WorkDir + ImageMetaInfoFileName))
            {
                errorMessage = $"{ErrorMsgPrefix}Metainfo file {ImageMetaInfoFileName} does not exists under directory {WorkDir}.";
            }

            return errorMessage == null;
        }
    }
}
