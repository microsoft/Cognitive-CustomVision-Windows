# Microsoft Custom Vision Service: Windows Client Library & Sample
>**This repo is out of date and will be retired on Oct 1, 2019**
>
>This repo exists to hold source for the older non Azure integrated version of the Cognitive Services Custom Vision SDK. The older versions will be deprecated on Oct 1, 2019. 
> See our [March 26 release notes](https://docs.microsoft.com/en-us/azure/cognitive-services/custom-vision-service/release-notes) for more information.
>
>The latest versions can be found via NuGet:
>
>* [Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training)
>* [Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction](https://www.nuget.org/packages/Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction/)
>
>Source for those packages can be found in the [Azure SDK For .NET](https://github.com/Azure/azure-sdk-for-net) repo and the latest samples are available with [Azure Samples for Custom Vision](https://github.com/Azure-Samples/cognitive-services-dotnet-sdk-samples/tree/master/CustomVision).


This repo contains the Windows client library & sample for the Microsoft Custom Vision Service, an offering within [Microsoft Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/)

## The Prediction Client Library
The prediction client library is an automatically generated C\# wrapper that allows you to make predictions against trained endpoints.

The easiest way to get the prediction client library is to get the [Microsoft.Cognitive.CustomVision.Prediction](https://www.nuget.org/packages/Microsoft.Cognitive.CustomVision.Prediction/) package from [nuget](<http://nuget.org>). 

## The Training Client Library
The training client library is automatically generated C\# wrapper that allows you to create, manage and train Custom Vision projects programatically. All operations on the [website](<https://customvision.ai>) are exposed through this library, allowing you to automate all aspects of the Custom Vision Service.

The easiest way to get the training client library is to get the [Microsoft.Cognitive.CustomVision.Training](https://www.nuget.org/packages/Microsoft.Cognitive.CustomVision.Training/) package from [nuget](<http://nuget.org>).

## The Sample
The sample is a Windows Console application that shows how to create and train a project, then make a prediction against the newly trained endpoint.

### Build the sample
1. Start Microsoft Visual Studio 2015 and select File > Open > Project/Solution.

2. Starting in the folder where you clone the repository, go to Samples > CustomVision.Sample

3. Double-click the Visual Studio 2015 Solution (.sln) file CustomVision.Sample.

4. Press Ctrl+Shift+B, or select Build > Build Solution.

### Run the sample

After the build is complete, press F5 to run the sample.

The sample will prompt for your training key. You can obtain your training key from the [website](https://customvision.ai):
1. Open a project, and go to the project settings (the gear icon)

2. Copy the value listed underneath 'Training Key'.

After entering your training key, hit enter, and the program will run.

The sample uses the Training Client Library to create a new project, add two tags and associated images, and train it. Once trained, the program will use the Prediction Client Library to predict a test image.

The results of the test will be printed out to the screen, and should look like the below image

<img src="Samples/CustomVision.Sample/screenshot.png" width="50%"/>

The sample does not delete the project after it has run. If you visit the [website](https://customvision.ai) you will be able to see and explore the newly created project.

## Contributing
We welcome contributions. Feel free to file issues and pull requests on the repo and we'll address them as we can. Learn more about how you can help on our [Contribution Rules & Guidelines](</CONTRIBUTING.md>). 

You can reach out to us anytime with questions and suggestions using our communities below:
 - **Support questions:** [StackOverflow](<https://stackoverflow.com/questions/tagged/microsoft-cognitive>)
 - **Feedback & feature requests:** [Cognitive Services UserVoice Forum](<https://cognitive.uservoice.com>)

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.


## License
All Microsoft Cognitive Services SDKs and samples are licensed with the MIT License. For more details, see
[LICENSE](</LICENSE.md>).

Sample images are licensed separately, please refer to [LICENSE-IMAGE](</LICENSE-IMAGE.md>).


## Developer Code of Conduct
Developers using Cognitive Services, including this client library & sample & tools, are expected to follow the “Developer Code of Conduct for Microsoft Cognitive Services”, found at [http://go.microsoft.com/fwlink/?LinkId=698895](http://go.microsoft.com/fwlink/?LinkId=698895).
