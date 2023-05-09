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
                LogCallback = Log
            };

            var faceChecker = new Library.FaceCheckerHelper(faceCheckerParameters);
            var image = faceChecker.CaptureFace();

            image.Save("c:\\ffe.bmp");
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
