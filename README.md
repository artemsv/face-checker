# FaceChecker

## Overview
FaceChecker is a .NET 4.7.2 library that is intended to be used as an authentication step to have a secured login experience. It is based on the OpenCV and its .NET wrapper ( https://github.com/shimat/opencvsharp ) and leverages face recognition features to get a proper face snapshot.

## Usage

There are several usage cases:

### Embedded WinForms Form 

Prefferred way to use the library if the client needs to display captured face. 

```csharp
const int screenWidth = 640;
const int screenHeight = 480;

var faceCheckerParameters = new FaceCheckerParameters
{
    Width = screenWidth,
    Height = screenHeight,
    Left = (Screen.PrimaryScreen.Bounds.Width - screenWidth) /2,
    Top = (Screen.PrimaryScreen.Bounds.Height - screenHeight) / 2,
    LogCallback = Console.WriteLine,
    CloseTimeoutInMs = 15000
};

var faceChecker = new FaceChecker(faceCheckerParameters);
var res = faceChecker.CaptureFace();

if (res.Code == FaceCaptureResultCode.Success)
    res.Image.Save("c:\\image.bmp");
```

### Using raw FaceCapturer 

Gives the full access to the underlying face capturing functionality. Client needs to organize image processing loop. 

```csharp
var faceCheckerParameters = new FaceCapturerParameters
{
    Width = 640,
    Height = 480,
    LogCallback = Console.WriteLine,
};

using (var faceCapturer = new FaceCapturer(parameters))
{
    faceCapturer.Start();

    while (!faceCapturer.CaptureFace())
    {
        Task.Delay(100);
    }

    if (faceCapturer.FaceImage != null)
        faceCapturer.FaceImage.Save("c:\\face.bmp");
}
```

## License

FaceChecker is licensed under the MIT License. See the LICENSE file for more information. 

