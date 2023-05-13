using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SmartBase.FaceChecker
{
    class FaceFeature
    {
        public Rect Face { get; set; }
        public Rect[] Eyes { get; set; }
    }

    public class FaceCapturer : IDisposable
    {
        private readonly FaceCheckerParameters _parameters;
        private readonly VideoCapture _capture;
        private CascadeClassifier _faceCascade;
        private CascadeClassifier _eyesCascade;
        private bool _disposedValue;

        public FaceCapturer(FaceCheckerParameters parameters)
        {
            _parameters = parameters;

            _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            _parameters.LogCallback("VideoCapture is created");

            _faceCascade = new CascadeClassifier("./haarcascades/haarcascade_frontalface_default.xml");
            _eyesCascade = new CascadeClassifier("./haarcascades/haarcascade_eye.xml");
        }

        public Bitmap CapturedImage { get; private set; }
        public int Width => _parameters.Width;
        public int Height => _parameters.Height;

        internal bool GrabFrame()
        {
            var features = new List<FaceFeature>();

            using (var frameMat = _capture.RetrieveMat())
            {
                if (!frameMat.Empty())
                {
                    var grayMat = ConvertGrayScale(frameMat);
                    var faceRects = DetectFace(grayMat);

                    foreach (var faceRect in faceRects)
                    {
                        var faceMat = grayMat[faceRect];

                        var eyes = DetectEyes(faceMat);

                        features.Add(new FaceFeature()
                        {
                            Face = faceRect,
                            Eyes = eyes
                        });
                    }

                    DrawFacesAndEyes(features, frameMat);

                    CapturedImage = frameMat.ToBitmap();
                }
                else
                    _parameters.LogCallback("Mat is empty");
            }

            return true;
        }

        private void DrawFacesAndEyes(IList<FaceFeature> features, Mat image)
        {
            foreach (FaceFeature feature in features)
            {
                Cv2.Rectangle(image, feature.Face, new Scalar(0, 255, 0), thickness: 1);
                var faceRegion = image[feature.Face];

                foreach (var eye in feature.Eyes)
                {
                    Cv2.Rectangle(faceRegion, eye, new Scalar(255, 0, 0), thickness: 1);
                }
            }
        }

        private Rect[] DetectFace(Mat mat)
        {
            return _faceCascade.DetectMultiScale(mat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));
        }

        private Rect[] DetectEyes(Mat mat)
        {
            return _eyesCascade.DetectMultiScale(mat);
        }

        internal bool Start()
        {
            _capture.Open(0, VideoCaptureAPIs.DSHOW);
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
                    _eyesCascade.Dispose();
                    _faceCascade.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private Mat ConvertGrayScale(Mat image)
        {
            var gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            return gray;
        }
    }
}
