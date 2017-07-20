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
using Common;
using Common.Extensions;
using Microsoft.Cognitive.CustomVision;
using Training.Extensions;
using Common.Model;

namespace Training
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = Options.InitializeOptions<TrainingOptions>(args);
            if (options == null)
            {
                return;
            }

            var trainingApi = new TrainingApi(new TrainingApiCredentials(options.Trainingkey));
            if (options.BaseUri != null)
            {
                Console.WriteLine($"The default base uri is {trainingApi.BaseUri}. Changed it to {options.BaseUri}.");
                trainingApi.BaseUri = options.BaseUri;
            }

            Project project;
            if (options.ProjectId != Guid.Empty)
            {
                if (!trainingApi.CheckIfProjectExists(options.ProjectId))
                {
                    Console.WriteLine($"Cannot get the project with id {options.ProjectId}. Please check if the Guid is right.");
                    return;
                }

                project = new Project(trainingApi.GetProject(options.ProjectId));
            }
            else
            {
                Console.WriteLine("ProjectId is not provided. Creating a new project...");
                var projectInfo = Project.ReadProjectInfo(options.WorkDir + options.ProjectInfoFileName);
                project = trainingApi.CreateProjectAsync(projectInfo).Result;
            }
            
            Console.WriteLine($"Project Id: {project.Id}");

            if (options.ImageSource != "existing")
            {
                var uploadResult = trainingApi.ReadAndUploadImagesAsync(project, options, options.AllowedTagNames).Result;
                if (!uploadResult.IsBatchSuccessful)
                {
                    Console.WriteLine("No image uploaded successfully. Please check if those images have been uploaded before.");
                    return;
                }
            }

            trainingApi.TrainProject(project.Id);

            Console.WriteLine("The classifier is now training. You can check its status in the web portal.\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
