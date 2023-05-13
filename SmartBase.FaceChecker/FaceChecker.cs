using SmartBase.FaceChecker.Library;
using System.Windows.Forms;

namespace SmartBase.FaceChecker
{
    public class FaceChecker
    {
        private readonly FaceCheckerParameters _parameters;

        public FaceChecker(FaceCheckerParameters parameters)
        {
            _parameters = parameters;
        }

        public FaceCaptureResult CaptureFace()
        {
            var result = new FaceCaptureResult();

            using (var faceCapturer = new FaceCapturer(_parameters))
            {
                if (faceCapturer.Start())
                {
                    var form = CreateForm(faceCapturer);

                    switch (form.ShowDialog())
                    {
                        case DialogResult.OK:
                            result.Code = FaceCaptureResultCode.Success;
                            result.Image = faceCapturer.CapturedImage;
                            break;
                        case DialogResult.Abort:
                            result.Code = FaceCaptureResultCode.Timeout;
                            break;
                    }
                }
            }

            return result;
        }

        private FaceCheckerForm CreateForm(FaceCapturer faceCapturer)
        {
            return new FaceCheckerForm(faceCapturer, new FaceCheckerFormParameters
            {
                Left = _parameters.Left,
                Top = _parameters.Top,
                CloseTimeOutInMs = _parameters.CloseTimeoutInMs
            });
        }
    }
}
