namespace SmartBase.FaceChecker.Library
{
    partial class FaceCheckerForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows

        private void InitializeComponent()
        {
            this.CameraPictureBox = new System.Windows.Forms.PictureBox();
            this.ImageCaptureWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.CameraPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CameraPictureBox
            // 
            this.CameraPictureBox.BackColor = System.Drawing.Color.Black;
            this.CameraPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CameraPictureBox.Location = new System.Drawing.Point(0, 0);
            this.CameraPictureBox.Name = "CameraPictureBox";
            this.CameraPictureBox.Size = new System.Drawing.Size(800, 488);
            this.CameraPictureBox.TabIndex = 0;
            this.CameraPictureBox.TabStop = false;
            // 
            // ImageCaptureWorker
            // 
            this.ImageCaptureWorker.WorkerReportsProgress = true;
            this.ImageCaptureWorker.WorkerSupportsCancellation = true;
            this.ImageCaptureWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.ImageCaptureWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            // 
            // FaceCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 488);
            this.Controls.Add(this.CameraPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FaceCheckerForm";
            this.Text = "VideoCaptureForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VideoCaptureForm_FormClosing);
            this.Load += new System.EventHandler(this.VideoCaptureForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CameraPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox CameraPictureBox;
        private System.ComponentModel.BackgroundWorker ImageCaptureWorker;
    }
}

