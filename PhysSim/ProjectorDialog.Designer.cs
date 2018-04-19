namespace PhysSim
{
    partial class ProjectorDialog
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
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.EditorFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ColorPicker = new PhysSim.ColorPicker();
            this.YBox = new PhysSim.NumberBox();
            this.XBox = new PhysSim.NumberBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "X Coordinate (m)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(186, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y Coordinate (m)";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(534, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 20);
            this.button1.TabIndex = 12;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(637, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 20);
            this.button2.TabIndex = 13;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(357, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Color";
            // 
            // EditorFlow
            // 
            this.EditorFlow.AutoScroll = true;
            this.EditorFlow.Location = new System.Drawing.Point(12, 64);
            this.EditorFlow.Name = "EditorFlow";
            this.EditorFlow.Size = new System.Drawing.Size(991, 482);
            this.EditorFlow.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Forces";
            // 
            // ColorPicker
            // 
            this.ColorPicker.BackColor = System.Drawing.Color.White;
            this.ColorPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ColorPicker.Location = new System.Drawing.Point(360, 25);
            this.ColorPicker.Name = "ColorPicker";
            this.ColorPicker.Size = new System.Drawing.Size(168, 20);
            this.ColorPicker.TabIndex = 16;
            this.ColorPicker.Value = System.Drawing.Color.White;
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(186, 25);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(168, 20);
            this.YBox.TabIndex = 4;
            this.YBox.Text = "0";
            this.YBox.Value = 0D;
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(12, 25);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(168, 20);
            this.XBox.TabIndex = 2;
            this.XBox.Text = "0";
            this.XBox.Value = 0D;
            // 
            // ProjectorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 558);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EditorFlow);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ColorPicker);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.YBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.XBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProjectorDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Projector Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label8;
        public ColorPicker ColorPicker;
        private NumberBox XBox;
        private NumberBox YBox;
        private System.Windows.Forms.FlowLayoutPanel EditorFlow;
        private System.Windows.Forms.Label label1;
    }
}