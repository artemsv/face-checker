using OpenCvSharp;

namespace SmartBase.FaceChecker
{
    public class FaceFeature
    {
        public Rect Face { get; set; }
        public Rect[] Eyes { get; set; }
    }
}
