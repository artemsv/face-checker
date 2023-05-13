using System.Drawing;

namespace SmartBase.FaceChecker
{
    public enum FaceCaptureResultCode
    {
        None,
        Success,
        Timeout,
    }

    public class FaceCaptureResult
    {
        public Bitmap Image { get; set; }
        public FaceCaptureResultCode Code { get; set; }
    }
}