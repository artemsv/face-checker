using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SmartBase.FaceChecker
{
    public delegate bool ImageCapturedEventHandler(Mat frameMat, List<FaceFeature> features, bool detected);
    public class FaceCapturer : IDisposable
    {
        private readonly FaceCheckerParameters _parameters;
        private readonly VideoCapture _capture;
        private CascadeClassifier _faceCascade;
        private CascadeClassifier _eyesCascade;
        private bool _disposedValue;
        private ImageCapturedEventHandler _imageCapturedEventHandler;

        public Bitmap FaceImage { get; private set; }
        public int Width => _parameters.Width;
        public int Height => _parameters.Height;

        #region Ctors
        public FaceCapturer(FaceCheckerParameters parameters)
        {
            _parameters = parameters;

            _capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            _parameters.LogCallback("VideoCapture is created");

            _faceCascade = new CascadeClassifier("./haarcascades/haarcascade_frontalface_default.xml");
            _eyesCascade = new CascadeClassifier("./haarcascades/haarcascade_eye.xml");
        } 
        #endregion

        #region Public
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Makes an attempt to grab the image and detect the face.
        /// </summary>
        /// <returns>True if the face as captured successfully.</returns>
        public bool CaptureFace()
        {
            var features = new List<FaceFeature>();
            var res = false;

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

                        // we found both eyes
                        res = eyes.Length == 2;
                    }

                    ProcessImageCallback(frameMat, features, ref res);

                    if (_parameters.HighlightFaceAndEyes)
                        DrawFacesAndEyes(features, frameMat);

                    FaceImage = frameMat.ToBitmap();
                }
                else
                    _parameters.LogCallback("Mat is empty");
            }

            return res;
        }


        public event ImageCapturedEventHandler ImageCaptured
        {
            add { _imageCapturedEventHandler += value; }
            remove { _imageCapturedEventHandler -= value; }
        }

        /// <summary>
        /// Initializes video capturing.
        /// </summary>
        public bool Start()
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
        #endregion

        #region NonPublic
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

        private Mat ConvertGrayScale(Mat image)
        {
            var gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);

            return gray;
        }

        private void ProcessImageCallback(Mat frameMat, List<FaceFeature> features, ref bool detected)
        {
            if (_imageCapturedEventHandler != null)
                detected = _imageCapturedEventHandler(frameMat, features, detected);
        }
        #endregion
    }
}
