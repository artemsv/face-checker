using System;

namespace SmartBase.FaceChecker.Library
{
    public class FaceCheckerParameters
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Action<string> LogCallback { get; set; }
    }
}
