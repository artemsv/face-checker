using SmartBase.FaceChecker.Library;
using System;
using System.Windows.Forms;

namespace SmartBase.FaceChecker.TestHost
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
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
        }
    }
}
