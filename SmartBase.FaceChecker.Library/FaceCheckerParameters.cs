using System;

namespace SmartBase.FaceChecker
{
    public class FaceCheckerParameters
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Action<string> LogCallback { get; set; }
        public int CloseTimeoutInMs { get; set; }
    }
}
