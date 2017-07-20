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
using CommandLine.Text;
using Common.Extensions;
using Microsoft.Cognitive.CustomVision;
using Microsoft.Rest;

namespace Common
{
    /// <summary>
    /// Options that control the behavior of the program
    /// </summary>
    public class Options
    {
        protected const string ErrorMsgPrefix = "ERROR: ";

        [Option('b', Required = false, HelpText = "Training or prediction endpoint uri.  Defaults to customvision.ai endpoint.")]
        public virtual Uri BaseUri { get; set; }

        [Option('g', Required = false, HelpText = "Tag names of the images.", Separator = ',')]
        public virtual ICollection<string> AllowedTagNames { get; set; }

        [Option('i', Required = false, HelpText = "Id of the iteration.")]
        public virtual Guid InternalIterationId { get; set; } = Guid.Empty;

        public Guid? IterationId => InternalIterationId == Guid.Empty ? (Guid?) null : InternalIterationId;

        [Option('l', Required = false, HelpText = "File name of the image metainfo.")]
        public virtual string ImageMetaInfoFileName { get; set; }

        [Option('m', Default = "dir", Required = false, HelpText = "The way images are organized and stored locally. Can be \"dir\" or \"metainfo\".")]
        public string ImageOrganization {
            get
            {
                return _imageOrganization;
            }
            set
            {
                if (value != "metainfo" && value != "dir")
                {
                    throw new InvalidOperationException($"Invalid value for {nameof(ImageOrganization)}: {value}");
                }

                _imageOrganization = value;
            }
        }

        [Option('p', Required = false, HelpText = "Id of the project.")]
        public virtual Guid ProjectId { get; set; }

        [Option('s', Default = "local", Required = false, HelpText = "Source of images. Can be \"local\", \"url\", or \"existing\".")]
        public virtual string ImageSource {
            get
            {
                return _imageSource;
            }
            set
            {
                if (!new[] {"local", "url", "existing"}.Contains(value))
                {
                    throw new InvalidOperationException($"Invalid value for {nameof(ImageSource)}: {value}");
                }

                _imageSource = value;
            }
        }

        [Option('w', Default = @".\", Required = false, HelpText = "Work directory within which this program operates.")]
        public string WorkDir
        {
            get
            {
                return _workDir;
            }
            set
            {
                _workDir = value.EndsWith(@"\") ? value : value + @"\";
            }
        }

        private string _imageOrganization { get; set; }

        private string _imageSource { get; set; }

        private string _workDir { get; set; }

        public static T InitializeOptions<T>(string[] args) where T : Options
        {
            T options = null;
            try
            {
                Parser.Default.ParseArguments<T>(args)
                    .WithParsed(downloadOptions =>
                    {
                        options = downloadOptions;
                    });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ErrorMsgPrefix + ex.InnerException?.InnerException?.Message);
                return null;
            }

            if (options == null)
            {
                return null;
            }

            string errorMessage;
            if (options.Validate(out errorMessage))
            {
                return options;
            }

            Console.WriteLine(errorMessage);
            OutputHelpScreen<T>();
            return null;
        }

        protected static void OutputHelpScreen<T>() where T : Options
        {
            Parser.Default.ParseArguments<T>(new [] { "--help" });
        }

        /// <summary>
        /// Basic validation over the option.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <returns>Whether this option is valid.</returns>
        public virtual bool Validate(out string errorMessage)
        {
            errorMessage = null;
            return true;
        }

        public virtual bool ValidateWithTrainingApi(ITrainingApi trainingApi, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                trainingApi.GetProjects();
            }
            catch (HttpOperationException ex)
            {
                if (ex.Message.Contains("NotFound"))
                {
                    errorMessage = $"{ErrorMsgPrefix}{ex.Message} It is likely the base url {BaseUri} is wrong.";
                }
                else if (ex.Message.Contains("Unauthorized"))
                {
                    errorMessage = $"{ErrorMsgPrefix}{ex.Message}. It is likely the training key is wrong.";
                }
                else
                {
                    errorMessage = ex.Message;
                }

                return false;
            }

            if (ProjectId != Guid.Empty && !trainingApi.CheckIfProjectExists(ProjectId))
            {
                errorMessage = $"{ErrorMsgPrefix}Project with id {ProjectId} does not exist.";
            }
            else if (IterationId != null && !trainingApi.CheckIfIterationExists(ProjectId, IterationId.Value))
            {
                errorMessage = $"{ErrorMsgPrefix}Iteration with id {IterationId} does not exist within project with id {ProjectId}.";
            }

            return errorMessage == null;
        }
    }
}
