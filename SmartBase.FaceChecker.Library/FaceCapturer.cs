using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace SmartBase.FaceChecker.Library
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
            List<FaceFeature> features = new List<FaceFeature>();

            using (var frameMat = _capture.RetrieveMat())
            {
                if (!frameMat.Empty())
                {
                    var gray = ConvertGrayScale(frameMat);
                    var faces = DetectFace(gray);

                    foreach (var rect in faces)
                    {
                        //Get the region of interest where you can find facial features
                        var face_roi = gray[rect];
                        //Detect eyes
                        var eyes = DetectEyes(face_roi);

                        //Record the facial features in a list
                        features.Add(new FaceFeature()
                        {
                            Face = rect,
                            Eyes = eyes
                        });
                    }

                    //Mark the detected feature on the original frame
                    MarkFeatures(features, frameMat);

                    CapturedImage = BitmapConverter.ToBitmap(frameMat);
                }
                else
                    _parameters.LogCallback("Mat is empty");
            }

            return true;
        }

        private void MarkFeatures(IList<FaceFeature> features,  Mat image)
        {
            foreach (FaceFeature feature in features)
            {
                Cv2.Rectangle(image, feature.Face, new Scalar(0, 255, 0), thickness: 1);
                var face_region = image[feature.Face];
                foreach (var eye in feature.Eyes)
                {
                    Cv2.Rectangle(face_region, eye, new Scalar(255, 0, 0), thickness: 1);
                }
            }
        }

        private Rect[] DetectFace(Mat mat)
        {
            return _face_cascade.DetectMultiScale(mat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));
        }

        private Rect[] DetectEyes(Mat mat)
        {
            return _eyes_cascade.DetectMultiScale(mat);
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

        private Mat ConvertGrayScale(Mat image)
        {
            Mat gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            return gray;
        }
    }
}
