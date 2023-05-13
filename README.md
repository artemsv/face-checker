# FaceChecker

## Overview
FaceChecker is a .NET 4.7.2 library that is intended to be used as an authentication step to have a secured login experience. It is based on the OpenCV and its .NET wrapper ( https://github.com/shimat/opencvsharp ) and leverages face recognition features to get a proper face snapshot.

## Usage

There are several usage cases:

### Embedded WinForms Form 

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

    var faceChecker = new Library.FaceChecker(faceCheckerParameters);
    var res = faceChecker.CaptureFace();

    if (res.Code == FaceCaptureResultCode.Success)
        res.Image.Save("c:\\image.bmp");
```

## License

FaceChecker is licensed under the MIT License. See the LICENSE file for more information. 

