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
        private readonly FaceCheckerFormParameters _parameters;
        private readonly System.Windows.Forms.Timer _closeTimer;

        internal Bitmap CapturedImage { get; private set; }

        public FaceCheckerForm(FaceCapturer faceCapturer, FaceCheckerFormParameters parameters)
        {
            InitializeComponent();
            _faceCapturer = faceCapturer;
            _parameters = parameters;

            _closeTimer = new System.Windows.Forms.Timer();
            _closeTimer.Interval = parameters.CloseTimeOutInMs;
            _closeTimer.Tick += CloseTimer_Tick;
            _closeTimer.Start();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            //DialogResult = DialogResult.Cancel;
        }

        private void VideoCaptureForm_Load(object sender, EventArgs e)
        {
            Left = _parameters.Left;
            Top = _parameters.Top;

            ClientSize = new Size(_faceCapturer.Width, _faceCapturer.Height);

            ImageCaptureWorker.RunWorkerAsync();
        }

        private void ImageCaptureWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Close();
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
                    ;// DialogResult = DialogResult.OK;

                try
                {
                    CameraPictureBox.Image?.Dispose();
                    CameraPictureBox.Image = _faceCapturer.CapturedImage;
                }
                catch(Exception ex)
                {

                }

                Thread.Sleep(100);
            }
        }
    }
}
