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
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Cognitive.CustomVision.Training.Models;

namespace Common.Model
{
    /// <summary>
    /// Project info model
    /// </summary>
    public class ProjectInfo :Project
    {
        public ProjectInfo()
        {
            Name = "Custom Vision Project";
            Description = "No description";
        }

        public ProjectInfo(Project projectModel)
        {
            Name = projectModel.Name;
            Description = projectModel.Description;

            Id = projectModel.Id;
            ThumbnailUri = projectModel.ThumbnailUri;
        }

        public new Guid Id { get; set; }

        public new string ThumbnailUri { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; } = "General";

        public static ProjectInfo ReadProjectInfo(string projectInfoFileName)
        {
            if (string.IsNullOrEmpty(projectInfoFileName))
            {
                return new ProjectInfo();
            }

            try
            {
                using (var r = new StreamReader(projectInfoFileName))
                {
                    return JsonConvert.DeserializeObject<ProjectInfo>(r.ReadToEnd());
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Project info file {projectInfoFileName} not found. The default one will be used.");
                return new ProjectInfo();
            }
        }

        public async Task WriteProjectInfo(string projectInfoFileName)
        {
            using (var writer = File.CreateText(projectInfoFileName))
            {
                var json = JsonConvert.SerializeObject(this);
                await writer.WriteAsync(json);
            }
        }
    }
}
