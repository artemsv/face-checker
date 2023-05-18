using System;

namespace SmartBase.FaceChecker
{
    public class FaceCapturerParameters
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Action<string> LogCallback { get; set; }
        public bool HighlightFaceAndEyes { get; set; }
    }
}
