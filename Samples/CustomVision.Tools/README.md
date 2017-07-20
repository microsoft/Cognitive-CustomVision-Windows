# Demo Programs for Customvision.ai


This project presents three demo programs for using customvision.ai's APIs: a training program, a prediction program, and a program for downloading project images.

After compilation, three executable binaries are generated respectively for these three demos under `.\Bin`, and you can run them in the command prompt with certain options specified. For example, to create a new project and train it with images in a local folder:


```bash
$ .\Training.exe -w img\ -s local -l image_info.csv -b https://iris.com/training -d project_info.json -c 4 -k TrainingKey
```

## Basics

For basic information about these three programs, you can run

```bash
$ .\Training.exe --help
```

```bash
$ .\Prediction.exe --help
```

```bash
$ .\ImagesDownloader.exe --help
```

to read the basic help documentation. For more details, please refer to the following chapters.

## Common Options

The demo programs share a common set of options:

- **-p**: The project id.
- **-g**: The names of image tags to operate on. This serves as an image filter by tags. Tag names should be separated by "," and no space is allowed, e.g. "tagname1,tagname2,tagname3,...". If this option is not specified, use all images
- **-l**: An image metainfo file, described below.
- **-m**: The way images are organized. It can be "dir" or "metainfo", and "dir" is the default value. This specifies whether we tag the images by saving them into folders with the tag names ("dir") or by the metainfo file. This option is ignored if the image source is “url”. For the case where an image can have multiple tags, images will/should be duplicated into each tag folder they belong to.
- **-w**: The working directory. Default: ".\"
- **-s**: The image source, which is one of three values. Default: "local" 
	- url: Load images from URLs in image metainfo file
	- local: Load images within work directory
	- existing: Do not load new images.

## Image Metainfo File
Each row represents an image, and the row format depends on the selected image source.
- **local**: The row format is "filename,tagname1,tagname2,..."
- **url**: The row format is "url,tagname1,tagname2,..."

>**Note:**
>An image can have no tags specified.

## Training Options
- **-k**: The training key.
- **-c**: The batch size. Specifies the number of images uploaded in each batch. Default: 4
- **-d**: The project info JSON file name, which is used for creating a project. If you are working with an existing project (specified by "-p"), this option will be ignored. Default: project_info.json
	- Example content of a project JSON file:
```
	{
		"name":"A food classifier",
		"description":"This is a food classifier",
		"domain":"general"
	}
```
- **-g**: Note: this serves as a filter of images to upload, not to specify the tags used in training

If a project id is specified by "-p", then the training and image uploading happen within this existing project; otherwise, a new project will be created from the project info JSON file specified by "-d".

Before training, if image source specified is NOT "existing", images will be uploaded to the project from either URLs or local images. Otherwise, image uploading will be skipped.

## Prediction Options
- **-i**: The iteration id. Default: the iteration id that is specified as default in web portal.
- **-k**: The prediction key.
- **-o**: The prediction file name. Default: "predictions.csv"
- **-r**: The milli-seconds between predictions. Customvision.ai has a rate limit for API requests, and this option provides simple rate limiting. Default: 500
- **-t**: This option specifies the probability threshold used in precision and recall computation. Default: 0.9

This program can make predictions for images loaded from urls or from the local disk. The predictions are written into the csv file specified by "-o". Precision and recall will be computed if true tags are available.

If metainfo is provided (by "-l") for images, then precision and recall will be calculated as well. If tag filter is specified (by "-g"), but metainfo is missing, then tag filter will be ignored.

> An common use case: sometimes you may have images under a folder (e.g., `imgs`) without tag information and all you want to do is to get predictions for them. All you have to do is to specify the image organization to be "metainfo" and a real metainfo file (specified by "-l") is not needed, e.g.,
```{base}
.\Prediction.exe -k predictionkey -p projectid -w imgs -m metainfo
```

## Image Download Options
- **-i**: The iteration id. Default: the newest iteration.
- **-k**: The training key.
- **-d**: The project info JSON file name. It has the same format with the one in training options. Default: project_info.json.

If **-m** is specified as 
- "metainfo", then an image meta info file will be created as well and the file name is specified by "-l", the file format of which is described above; images will be all downloaded in the working directory
- "dir", for each tag name, a folder will be created within the working directory and the images belonging to this tag will be saved in the corresponding folder (for untagged ones, they are stored in the folder called "UntaggedImages").

A project info will be created with name specified by "-d" as well.