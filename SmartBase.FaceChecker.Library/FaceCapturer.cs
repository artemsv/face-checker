using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;

namespace SmartBase.FaceChecker.Library
{
    public class FaceCapturer : IDisposable
    {
        private readonly FaceCheckerParameters _parameters;
        private readonly VideoCapture _capture;
        private CascadeClassifier _face_cascade;
        private CascadeClassifier _eyes_cascade;
        private bool _disposedValue;

        public FaceCapturer(FaceCheckerParameters parameters)
        {
            _parameters = parameters;

            _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            _parameters.LogCallback("VideoCapture is created");

            _face_cascade = new CascadeClassifier("./haarcascades/haarcascade_frontalface_default.xml");
            _eyes_cascade = new CascadeClassifier("./haarcascades/haarcascade_eye.xml");
        }

        public Bitmap CapturedImage { get; private set; }
        public int Width => _parameters.Width;
        public int Height => _parameters.Height;

        internal bool GrabFrame()
        {
            using (var frameMat = _capture.RetrieveMat())
            {
                if (!frameMat.Empty())
                {
                    var rects = _face_cascade.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));
                    if (rects.Length > 0)
                    {
                        Cv2.Rectangle(frameMat, rects[0], Scalar.Red);
                        CapturedImage = BitmapConverter.ToBitmap(frameMat);

                        return false;
                    }

                    CapturedImage = BitmapConverter.ToBitmap(frameMat);
                }
                else
                    _parameters.LogCallback("Mat is empty");
            }

            return true;
        }

        internal bool Start()
        {
            _capture.Open(0, VideoCaptureAPIs.ANY);
            _capture.Set(VideoCaptureProperties.FrameWidth, _parameters.Width);
            _capture.Set(VideoCaptureProperties.FrameHeight, _parameters.Height);

            if (!_capture.IsOpened())
            {
                _parameters.LogCallback("Failed to initialize VideoCapture");
                return false;
            }

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _capture.Dispose();
                    _eyes_cascade.Dispose();
                    _face_cascade.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
