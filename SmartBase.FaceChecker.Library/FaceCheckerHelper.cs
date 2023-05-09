using System.Drawing;

namespace SmartBase.FaceChecker.Library
{
    public class FaceCheckerHelper
    {
        private readonly FaceCheckerParameters _parameters;
        private FaceCheckerForm _form;

        public FaceCheckerHelper(FaceCheckerParameters parameters)
        {
            _parameters = parameters;
        }

        public Bitmap CaptureFace()
        {
            using (var helper = new FaceCapturer(_parameters))
            {
                _form = new FaceCheckerForm(helper, new FaceCheckerFormParameters
                {
                    Left = _parameters.Left,
                    Top = _parameters.Top,
                });
                _form.ShowDialog();
            }

            return _form.CapturedImage;

            /*
            var capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            if (!capture.IsOpened())
            {
                return;
            }

            using (Window window = new Window("Webcam", WindowFlags.FullScreen))
            {
                SetWindowStyle();

                using (Mat image = new Mat())
                {
                    while (true)
                    {
                        capture.Read(image);
                        if (image.Empty()) break;

                        SetWindowStyle();

                        //window.ShowImage(image);
                        int key = Cv2.WaitKey(30);
                        if (key == 27) break;
                    }
                }
            }*/
        }


        public void Stop()
        {
            _form.Close();
        }
    }
}
