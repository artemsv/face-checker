using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SmartBase.FaceChecker.Library
{
    public partial class FaceCheckerForm : Form
    {
        private readonly FaceCheckerParameters _parameters;

        internal Bitmap CapturedImage { get; private set; }

        private FaceCaptureHelper _helper;

        public FaceCheckerForm(FaceCheckerParameters parameters)
        {
            InitializeComponent();

            _parameters = parameters;
            _helper = new FaceCaptureHelper(_parameters);
        }

        private void VideoCaptureForm_Load(object sender, EventArgs e)
        {
            Left = _parameters.Left;
            Top = _parameters.Top;

            if (!_helper.Start())
            {
                Close();
                return;
            }

            ClientSize = new System.Drawing.Size(_parameters.Width, _parameters.Height);

            ImageCaptureWorker.RunWorkerCompleted += ImageCaptureWorker_RunWorkerCompleted;
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
            {
                ImageCaptureWorker.CancelAsync();
                _helper.Destroy();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var bgWorker = (BackgroundWorker) sender;

            while (!bgWorker.CancellationPending)
            {
                if (!_helper.HandleNext())
                    break;
                
                bgWorker.ReportProgress(0, _helper.CapturedImage);
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
