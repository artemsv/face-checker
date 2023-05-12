using System.Drawing;

namespace SmartBase.FaceChecker.Library
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