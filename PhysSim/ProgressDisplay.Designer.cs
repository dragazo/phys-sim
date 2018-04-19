namespace PhysSim
{
    partial class ProgressDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Progress = new PhysSim.DotProgress();
            this.SuspendLayout();
            // 
            // Progress
            // 
            this.Progress.CompleteColor = System.Drawing.Color.LightSkyBlue;
            this.Progress.Dots = 20;
            this.Progress.IncompleteColor = System.Drawing.Color.LightGray;
            this.Progress.Location = new System.Drawing.Point(12, 12);
            this.Progress.MaxValue = 100;
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(396, 19);
            this.Progress.Spacing = 0.5F;
            this.Progress.TabIndex = 0;
            this.Progress.Value = 0;
            // 
            // ProgressDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 36);
            this.Controls.Add(this.Progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ProgressDisplay";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressDisplay";
            this.ResumeLayout(false);

        }

        #endregion

        private DotProgress Progress;
    }
}