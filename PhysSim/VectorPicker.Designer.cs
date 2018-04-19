namespace PhysSim
{
    partial class VectorPicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.XBox = new PhysSim.NumberBox();
            this.YBox = new PhysSim.NumberBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AngleBox = new PhysSim.NumberBox();
            this.label3 = new System.Windows.Forms.Label();
            this.MagnitudeBox = new PhysSim.NumberBox();
            this.label4 = new System.Windows.Forms.Label();
            this.DirectionPicker = new PhysSim.DirectionPicker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X Component";
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(254, 16);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(160, 20);
            this.XBox.TabIndex = 1;
            this.XBox.Value = 0D;
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(254, 55);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(160, 20);
            this.YBox.TabIndex = 3;
            this.YBox.Value = 0D;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y Component";
            // 
            // AngleBox
            // 
            this.AngleBox.Location = new System.Drawing.Point(87, 55);
            this.AngleBox.Name = "AngleBox";
            this.AngleBox.Size = new System.Drawing.Size(160, 20);
            this.AngleBox.TabIndex = 7;
            this.AngleBox.Value = 0D;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Angle";
            // 
            // MagnitudeBox
            // 
            this.MagnitudeBox.Location = new System.Drawing.Point(87, 16);
            this.MagnitudeBox.Name = "MagnitudeBox";
            this.MagnitudeBox.Size = new System.Drawing.Size(160, 20);
            this.MagnitudeBox.TabIndex = 5;
            this.MagnitudeBox.Value = 0D;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Magnitude";
            // 
            // DirectionPicker
            // 
            this.DirectionPicker.Location = new System.Drawing.Point(3, 0);
            this.DirectionPicker.Name = "DirectionPicker";
            this.DirectionPicker.Size = new System.Drawing.Size(75, 75);
            this.DirectionPicker.TabIndex = 8;
            // 
            // VectorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DirectionPicker);
            this.Controls.Add(this.AngleBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MagnitudeBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.YBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.XBox);
            this.Controls.Add(this.label1);
            this.Name = "VectorPicker";
            this.Size = new System.Drawing.Size(416, 77);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private NumberBox XBox;
        private NumberBox YBox;
        private System.Windows.Forms.Label label2;
        private NumberBox AngleBox;
        private System.Windows.Forms.Label label3;
        private NumberBox MagnitudeBox;
        private System.Windows.Forms.Label label4;
        private DirectionPicker DirectionPicker;
    }
}
