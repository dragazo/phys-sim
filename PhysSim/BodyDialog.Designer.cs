namespace PhysSim
{
    partial class BodyDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.IconCombo = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.SignificantCheck = new System.Windows.Forms.CheckBox();
            this.CollidableCheck = new System.Windows.Forms.CheckBox();
            this.VelocityPicker = new PhysSim.VectorPicker();
            this.ForcesPicker = new PhysSim.VectorPicker();
            this.DistanceBox = new PhysSim.NumberBox();
            this.RadiusBox = new PhysSim.NumberBox();
            this.ChargeBox = new PhysSim.NumberBox();
            this.MassBox = new PhysSim.NumberBox();
            this.YBox = new PhysSim.NumberBox();
            this.XBox = new PhysSim.NumberBox();
            this.IconPicker = new PhysSim.ColorPicker();
            this.BackPicker = new PhysSim.ColorPicker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Mass (Kg)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Charge (C)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Radius (m)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Velocity (m/s)";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(510, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 20);
            this.button1.TabIndex = 11;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(510, 295);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 20);
            this.button2.TabIndex = 12;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(472, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Background Color";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(472, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Icon Color";
            // 
            // IconCombo
            // 
            this.IconCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IconCombo.FormattingEnabled = true;
            this.IconCombo.Location = new System.Drawing.Point(475, 102);
            this.IconCombo.Name = "IconCombo";
            this.IconCombo.Size = new System.Drawing.Size(217, 21);
            this.IconCombo.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(472, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Icon";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(266, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 23;
            this.label9.Text = "X Coordinate (m)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(266, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 24;
            this.label10.Text = "Y Coordinate (m)";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(9, 24);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(211, 20);
            this.NameBox.TabIndex = 29;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 276);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 31;
            this.label11.Text = "Forces (N)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(266, 105);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "Distance Travelled (m)";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(339, 141);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(90, 20);
            this.ClearButton.TabIndex = 34;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // SignificantCheck
            // 
            this.SignificantCheck.AutoSize = true;
            this.SignificantCheck.Location = new System.Drawing.Point(546, 144);
            this.SignificantCheck.Name = "SignificantCheck";
            this.SignificantCheck.Size = new System.Drawing.Size(75, 17);
            this.SignificantCheck.TabIndex = 37;
            this.SignificantCheck.Text = "Significant";
            this.SignificantCheck.UseVisualStyleBackColor = true;
            // 
            // CollidableCheck
            // 
            this.CollidableCheck.AutoSize = true;
            this.CollidableCheck.Location = new System.Drawing.Point(546, 167);
            this.CollidableCheck.Name = "CollidableCheck";
            this.CollidableCheck.Size = new System.Drawing.Size(71, 17);
            this.CollidableCheck.TabIndex = 38;
            this.CollidableCheck.Text = "Collidable";
            this.CollidableCheck.UseVisualStyleBackColor = true;
            // 
            // VelocityPicker
            // 
            this.VelocityPicker.Location = new System.Drawing.Point(6, 196);
            this.VelocityPicker.Name = "VelocityPicker";
            this.VelocityPicker.ReadOnly = false;
            this.VelocityPicker.Size = new System.Drawing.Size(416, 77);
            this.VelocityPicker.TabIndex = 40;
            // 
            // ForcesPicker
            // 
            this.ForcesPicker.Location = new System.Drawing.Point(6, 292);
            this.ForcesPicker.Name = "ForcesPicker";
            this.ForcesPicker.ReadOnly = true;
            this.ForcesPicker.Size = new System.Drawing.Size(416, 77);
            this.ForcesPicker.TabIndex = 39;
            // 
            // DistanceBox
            // 
            this.DistanceBox.Location = new System.Drawing.Point(269, 121);
            this.DistanceBox.Name = "DistanceBox";
            this.DistanceBox.Size = new System.Drawing.Size(160, 20);
            this.DistanceBox.TabIndex = 32;
            this.DistanceBox.Text = "0";
            this.DistanceBox.Value = 0D;
            // 
            // RadiusBox
            // 
            this.RadiusBox.Location = new System.Drawing.Point(9, 63);
            this.RadiusBox.Name = "RadiusBox";
            this.RadiusBox.Size = new System.Drawing.Size(211, 20);
            this.RadiusBox.TabIndex = 28;
            this.RadiusBox.Text = "0";
            this.RadiusBox.Value = 0D;
            // 
            // ChargeBox
            // 
            this.ChargeBox.Location = new System.Drawing.Point(6, 157);
            this.ChargeBox.Name = "ChargeBox";
            this.ChargeBox.Size = new System.Drawing.Size(211, 20);
            this.ChargeBox.TabIndex = 27;
            this.ChargeBox.Text = "0";
            this.ChargeBox.Value = 0D;
            // 
            // MassBox
            // 
            this.MassBox.Location = new System.Drawing.Point(6, 118);
            this.MassBox.Name = "MassBox";
            this.MassBox.Size = new System.Drawing.Size(211, 20);
            this.MassBox.TabIndex = 26;
            this.MassBox.Text = "0";
            this.MassBox.Value = 0D;
            // 
            // YBox
            // 
            this.YBox.Location = new System.Drawing.Point(269, 64);
            this.YBox.Name = "YBox";
            this.YBox.Size = new System.Drawing.Size(160, 20);
            this.YBox.TabIndex = 22;
            this.YBox.Text = "0";
            this.YBox.Value = 0D;
            // 
            // XBox
            // 
            this.XBox.Location = new System.Drawing.Point(269, 26);
            this.XBox.Name = "XBox";
            this.XBox.Size = new System.Drawing.Size(160, 20);
            this.XBox.TabIndex = 21;
            this.XBox.Text = "0";
            this.XBox.Value = 0D;
            // 
            // IconPicker
            // 
            this.IconPicker.BackColor = System.Drawing.Color.White;
            this.IconPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IconPicker.Location = new System.Drawing.Point(475, 63);
            this.IconPicker.Name = "IconPicker";
            this.IconPicker.Size = new System.Drawing.Size(217, 20);
            this.IconPicker.TabIndex = 15;
            this.IconPicker.Value = System.Drawing.Color.White;
            // 
            // BackPicker
            // 
            this.BackPicker.BackColor = System.Drawing.Color.White;
            this.BackPicker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BackPicker.Location = new System.Drawing.Point(475, 24);
            this.BackPicker.Name = "BackPicker";
            this.BackPicker.Size = new System.Drawing.Size(217, 20);
            this.BackPicker.TabIndex = 13;
            this.BackPicker.Value = System.Drawing.Color.White;
            // 
            // BodyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 378);
            this.Controls.Add(this.VelocityPicker);
            this.Controls.Add(this.ForcesPicker);
            this.Controls.Add(this.CollidableCheck);
            this.Controls.Add(this.SignificantCheck);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.DistanceBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.RadiusBox);
            this.Controls.Add(this.ChargeBox);
            this.Controls.Add(this.MassBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.YBox);
            this.Controls.Add(this.XBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.IconCombo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.IconPicker);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BackPicker);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BodyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Body Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox IconCombo;
        private System.Windows.Forms.Label label8;
        private NumberBox XBox;
        private NumberBox YBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button ClearButton;
        private VectorPicker ForcesPicker;
        private VectorPicker VelocityPicker;
        private System.Windows.Forms.CheckBox SignificantCheck;
        private System.Windows.Forms.CheckBox CollidableCheck;
        private ColorPicker IconPicker;
        private ColorPicker BackPicker;
        private NumberBox DistanceBox;
        private System.Windows.Forms.TextBox NameBox;
        private NumberBox RadiusBox;
        private NumberBox ChargeBox;
        private NumberBox MassBox;
    }
}