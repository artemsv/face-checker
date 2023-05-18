using System;
using System.Threading.Tasks;
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
                Left = (Screen.PrimaryScreen.Bounds.Width - screenWidth) / 2,
                Top = (Screen.PrimaryScreen.Bounds.Height - screenHeight) / 2,
                LogCallback = Console.WriteLine,
                CloseTimeoutInMs = 15000,

                // debug feature
                HighlightFaceAndEyes = true
            };

            _01_EmbeddedForm(faceCheckerParameters);
            //_02_DirectFaceCapturer(faceCheckerParameters);
        }

        private static void _01_EmbeddedForm(FaceCheckerParameters parameters)
        {
            var faceChecker = new FaceChecker(parameters);
            var res = faceChecker.CaptureFace();

            if (res.Code == FaceCaptureResultCode.Success)
                res.Image.Save("c:\\image.bmp");
        }

        private static void _02_DirectFaceCapturer(FaceCheckerParameters parameters)
        {
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
        }
    }
}
