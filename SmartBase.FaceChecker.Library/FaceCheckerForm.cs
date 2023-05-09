using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SmartBase.FaceChecker.Library
{
    public partial class FaceCheckerForm : Form
    {
        private readonly FaceCapturer _faceCapturer;
        private readonly FaceCheckerFormParameters _faceCheckerFormParameters;

        internal Bitmap CapturedImage { get; private set; }

        public FaceCheckerForm(FaceCapturer faceCapturer, FaceCheckerFormParameters faceCheckerFormParameters)
        {
            InitializeComponent();
            _faceCapturer = faceCapturer;
            _faceCheckerFormParameters = faceCheckerFormParameters;
        }

        private void VideoCaptureForm_Load(object sender, EventArgs e)
        {
            Left = _faceCheckerFormParameters.Left;
            Top = _faceCheckerFormParameters.Top;

            if (!_faceCapturer.Start())
            {
                Close();
                return;
            }

            ClientSize = new Size(_faceCapturer.Width, _faceCapturer.Height);

            ImageCaptureWorker.RunWorkerAsync();
        }

        private void ImageCaptureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void VideoCaptureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            else
                ImageCaptureWorker.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker) sender;

            while (!bgWorker.CancellationPending)
            {
                if (!_faceCapturer.GrabFrame())
                   // break;

                CameraPictureBox.Image?.Dispose();
                CameraPictureBox.Image = _faceCapturer.CapturedImage;
                Thread.Sleep(100);
            }
        }
    }
}
