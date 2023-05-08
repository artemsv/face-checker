using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace SmartBase.FaceChecker.Library
{
    public partial class FaceCheckerForm : Form
    {
        private readonly VideoCapture capture;
        private readonly CascadeClassifier cascadeClassifier;
        private readonly FaceCheckerParameters _parameters;

        internal Bitmap CapturedImage { get; private set; }

        public FaceCheckerForm(FaceCheckerParameters parameters)
        {
            InitializeComponent();

            capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);

            parameters.LogCallback("VideoCapture is created");

            cascadeClassifier = new CascadeClassifier("haarcascades/haarcascade_frontalface_default.xml");
            _parameters = parameters;
        }

        private void VideoCaptureForm_Load(object sender, EventArgs e)
        {
            Left = _parameters.Left;
            Top = _parameters.Top;

            capture.Open(0, VideoCaptureAPIs.ANY);
            capture.Set(3, _parameters.Width); //Set the frame width
            capture.Set(4, _parameters.Height); //Set the frame height


            if (!capture.IsOpened())
            {
                Close();
                return;
            }

            ClientSize = new System.Drawing.Size(capture.FrameWidth, capture.FrameHeight);

            ImageCaptureWorker.RunWorkerCompleted += ImageCaptureWorker_RunWorkerCompleted;
            ImageCaptureWorker.RunWorkerAsync();
        }

        private void ImageCaptureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void VideoCaptureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ImageCaptureWorker.CancelAsync();
            capture.Dispose();
            cascadeClassifier.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker) sender;

            while (!bgWorker.CancellationPending)
            {
                using (var frameMat = capture.RetrieveMat())
                {
                    if (!frameMat.Empty())
                    {
                        var rects = cascadeClassifier.DetectMultiScale(frameMat, 1.1, 5, HaarDetectionTypes.ScaleImage, new OpenCvSharp.Size(30, 30));
                        if (rects.Length > 0)
                        {
                            Cv2.Rectangle(frameMat, rects[0], Scalar.Red);
                            break;
                        }

                        var frameBitmap = BitmapConverter.ToBitmap(frameMat);
                        bgWorker.ReportProgress(0, frameBitmap);
                    }
                    else
                        _parameters.LogCallback("Mat is empty");
                }
                Thread.Sleep(100);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            CapturedImage = (Bitmap)e.UserState;
            CameraPictureBox.Image?.Dispose();
            CameraPictureBox.Image = CapturedImage;
        }
    }
}
