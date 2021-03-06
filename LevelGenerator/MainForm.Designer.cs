﻿namespace LevelGenerator
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.DifficultyLabel = new System.Windows.Forms.Label();
            this.DisplayGLControl = new OpenTK.GLControl();
            this.SimulationTimer = new System.Windows.Forms.Timer(this.components);
            this.RecalculateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ControlledBodyXPositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.ControlledBodyYPositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.ControlledBodyMassUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.GoalAreaRadiusUpDown = new System.Windows.Forms.NumericUpDown();
            this.GoalAreaXPositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.GoalAreaYPositionUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.IndependentBodyCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.AvoidAreasLabel = new System.Windows.Forms.Label();
            this.AvoidAreaCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.RecalculateBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.ResolutionUpDown = new System.Windows.Forms.NumericUpDown();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SaveLevelFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.LoadButton = new System.Windows.Forms.Button();
            this.OpenLevelFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ControlledBodyXPositionUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlledBodyYPositionUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlledBodyMassUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoalAreaRadiusUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoalAreaXPositionUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoalAreaYPositionUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndependentBodyCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvoidAreaCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // DifficultyLabel
            // 
            this.DifficultyLabel.AutoSize = true;
            this.DifficultyLabel.Location = new System.Drawing.Point(146, 13);
            this.DifficultyLabel.Name = "DifficultyLabel";
            this.DifficultyLabel.Size = new System.Drawing.Size(77, 13);
            this.DifficultyLabel.TabIndex = 2;
            this.DifficultyLabel.Text = "Difficulty: 1000";
            // 
            // DisplayGLControl
            // 
            this.DisplayGLControl.BackColor = System.Drawing.Color.Black;
            this.DisplayGLControl.Location = new System.Drawing.Point(389, 8);
            this.DisplayGLControl.Name = "DisplayGLControl";
            this.DisplayGLControl.Size = new System.Drawing.Size(960, 640);
            this.DisplayGLControl.TabIndex = 3;
            this.DisplayGLControl.VSync = false;
            this.DisplayGLControl.Paint += new System.Windows.Forms.PaintEventHandler(this.DisplayGLControl_Paint);
            this.DisplayGLControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.DisplayGLControl_MouseClick);
            this.DisplayGLControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DisplayGLControl_MouseDown);
            this.DisplayGLControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DisplayGLControl_MouseMove);
            this.DisplayGLControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DisplayGLControl_MouseUp);
            // 
            // SimulationTimer
            // 
            this.SimulationTimer.Interval = 33;
            this.SimulationTimer.Tick += new System.EventHandler(this.SimulationTimer_Tick);
            // 
            // RecalculateButton
            // 
            this.RecalculateButton.Location = new System.Drawing.Point(12, 8);
            this.RecalculateButton.Name = "RecalculateButton";
            this.RecalculateButton.Size = new System.Drawing.Size(75, 23);
            this.RecalculateButton.TabIndex = 8;
            this.RecalculateButton.Text = "Recalculate";
            this.RecalculateButton.UseVisualStyleBackColor = true;
            this.RecalculateButton.Click += new System.EventHandler(this.RecalculateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Controlled Body";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Position";
            // 
            // ControlledBodyXPositionUpDown
            // 
            this.ControlledBodyXPositionUpDown.Location = new System.Drawing.Point(62, 85);
            this.ControlledBodyXPositionUpDown.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.ControlledBodyXPositionUpDown.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            -2147483648});
            this.ControlledBodyXPositionUpDown.Name = "ControlledBodyXPositionUpDown";
            this.ControlledBodyXPositionUpDown.Size = new System.Drawing.Size(60, 20);
            this.ControlledBodyXPositionUpDown.TabIndex = 12;
            this.ControlledBodyXPositionUpDown.ValueChanged += new System.EventHandler(this.ControlledBodyXPositionUpDown_ValueChanged);
            // 
            // ControlledBodyYPositionUpDown
            // 
            this.ControlledBodyYPositionUpDown.Location = new System.Drawing.Point(128, 85);
            this.ControlledBodyYPositionUpDown.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.ControlledBodyYPositionUpDown.Minimum = new decimal(new int[] {
            320,
            0,
            0,
            -2147483648});
            this.ControlledBodyYPositionUpDown.Name = "ControlledBodyYPositionUpDown";
            this.ControlledBodyYPositionUpDown.Size = new System.Drawing.Size(60, 20);
            this.ControlledBodyYPositionUpDown.TabIndex = 13;
            this.ControlledBodyYPositionUpDown.ValueChanged += new System.EventHandler(this.ControlledBodyYPositionUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Mass";
            // 
            // ControlledBodyMassUpDown
            // 
            this.ControlledBodyMassUpDown.Location = new System.Drawing.Point(62, 111);
            this.ControlledBodyMassUpDown.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.ControlledBodyMassUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ControlledBodyMassUpDown.Name = "ControlledBodyMassUpDown";
            this.ControlledBodyMassUpDown.Size = new System.Drawing.Size(60, 20);
            this.ControlledBodyMassUpDown.TabIndex = 15;
            this.ControlledBodyMassUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ControlledBodyMassUpDown.ValueChanged += new System.EventHandler(this.ControlledBodyMassUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Goal Area";
            // 
            // GoalAreaRadiusUpDown
            // 
            this.GoalAreaRadiusUpDown.Location = new System.Drawing.Point(147, 154);
            this.GoalAreaRadiusUpDown.Maximum = new decimal(new int[] {
            960,
            0,
            0,
            0});
            this.GoalAreaRadiusUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GoalAreaRadiusUpDown.Name = "GoalAreaRadiusUpDown";
            this.GoalAreaRadiusUpDown.Size = new System.Drawing.Size(60, 20);
            this.GoalAreaRadiusUpDown.TabIndex = 19;
            this.GoalAreaRadiusUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.GoalAreaRadiusUpDown.ValueChanged += new System.EventHandler(this.GoalAreaRadiusUpDown_ValueChanged);
            // 
            // GoalAreaXPositionUpDown
            // 
            this.GoalAreaXPositionUpDown.Location = new System.Drawing.Point(15, 154);
            this.GoalAreaXPositionUpDown.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.GoalAreaXPositionUpDown.Minimum = new decimal(new int[] {
            480,
            0,
            0,
            -2147483648});
            this.GoalAreaXPositionUpDown.Name = "GoalAreaXPositionUpDown";
            this.GoalAreaXPositionUpDown.Size = new System.Drawing.Size(60, 20);
            this.GoalAreaXPositionUpDown.TabIndex = 20;
            this.GoalAreaXPositionUpDown.ValueChanged += new System.EventHandler(this.GoalAreaXPositionUpDown_ValueChanged);
            // 
            // GoalAreaYPositionUpDown
            // 
            this.GoalAreaYPositionUpDown.Location = new System.Drawing.Point(81, 154);
            this.GoalAreaYPositionUpDown.Maximum = new decimal(new int[] {
            320,
            0,
            0,
            0});
            this.GoalAreaYPositionUpDown.Minimum = new decimal(new int[] {
            320,
            0,
            0,
            -2147483648});
            this.GoalAreaYPositionUpDown.Name = "GoalAreaYPositionUpDown";
            this.GoalAreaYPositionUpDown.Size = new System.Drawing.Size(60, 20);
            this.GoalAreaYPositionUpDown.TabIndex = 21;
            this.GoalAreaYPositionUpDown.ValueChanged += new System.EventHandler(this.GoalAreaYPositionUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 22;
            this.label5.Text = "Independent Bodies";
            // 
            // IndependentBodyCountUpDown
            // 
            this.IndependentBodyCountUpDown.Location = new System.Drawing.Point(128, 188);
            this.IndependentBodyCountUpDown.Name = "IndependentBodyCountUpDown";
            this.IndependentBodyCountUpDown.Size = new System.Drawing.Size(60, 20);
            this.IndependentBodyCountUpDown.TabIndex = 23;
            this.IndependentBodyCountUpDown.ValueChanged += new System.EventHandler(this.IndependentBodyCountUpDown_ValueChanged);
            // 
            // AvoidAreasLabel
            // 
            this.AvoidAreasLabel.AutoSize = true;
            this.AvoidAreasLabel.Location = new System.Drawing.Point(50, 247);
            this.AvoidAreasLabel.Name = "AvoidAreasLabel";
            this.AvoidAreasLabel.Size = new System.Drawing.Size(64, 13);
            this.AvoidAreasLabel.TabIndex = 24;
            this.AvoidAreasLabel.Text = "Avoid Areas";
            // 
            // AvoidAreaCountUpDown
            // 
            this.AvoidAreaCountUpDown.Location = new System.Drawing.Point(128, 245);
            this.AvoidAreaCountUpDown.Name = "AvoidAreaCountUpDown";
            this.AvoidAreaCountUpDown.Size = new System.Drawing.Size(60, 20);
            this.AvoidAreaCountUpDown.TabIndex = 25;
            this.AvoidAreaCountUpDown.ValueChanged += new System.EventHandler(this.AvoidAreaCountUpDown_ValueChanged);
            // 
            // RecalculateBackgroundWorker
            // 
            this.RecalculateBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RecalculateBackgroundWorker_DoWork);
            this.RecalculateBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RecalculateBackgroundWorker_RunWorkerCompleted);
            // 
            // ResolutionUpDown
            // 
            this.ResolutionUpDown.Location = new System.Drawing.Point(94, 10);
            this.ResolutionUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ResolutionUpDown.Name = "ResolutionUpDown";
            this.ResolutionUpDown.Size = new System.Drawing.Size(46, 20);
            this.ResolutionUpDown.TabIndex = 26;
            this.ResolutionUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.ResolutionUpDown.ValueChanged += new System.EventHandler(this.ResolutionUpDown_ValueChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(94, 37);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 27;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveLevelFileDialog
            // 
            this.SaveLevelFileDialog.Filter = "Level Files|*.lvl|All files|*.*";
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(12, 37);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 28;
            this.LoadButton.Text = "Open";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // OpenLevelFileDialog
            // 
            this.OpenLevelFileDialog.Filter = "Level Files|*.lvl|All files|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 758);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ResolutionUpDown);
            this.Controls.Add(this.AvoidAreaCountUpDown);
            this.Controls.Add(this.AvoidAreasLabel);
            this.Controls.Add(this.IndependentBodyCountUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.GoalAreaYPositionUpDown);
            this.Controls.Add(this.GoalAreaXPositionUpDown);
            this.Controls.Add(this.GoalAreaRadiusUpDown);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ControlledBodyMassUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ControlledBodyYPositionUpDown);
            this.Controls.Add(this.ControlledBodyXPositionUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RecalculateButton);
            this.Controls.Add(this.DisplayGLControl);
            this.Controls.Add(this.DifficultyLabel);
            this.Name = "MainForm";
            this.Text = "Level Generator";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ControlledBodyXPositionUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlledBodyYPositionUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControlledBodyMassUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoalAreaRadiusUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoalAreaXPositionUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GoalAreaYPositionUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IndependentBodyCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AvoidAreaCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResolutionUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DifficultyLabel;
        private OpenTK.GLControl DisplayGLControl;
        private System.Windows.Forms.Timer SimulationTimer;
        private System.Windows.Forms.Button RecalculateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ControlledBodyXPositionUpDown;
        private System.Windows.Forms.NumericUpDown ControlledBodyYPositionUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown ControlledBodyMassUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown GoalAreaRadiusUpDown;
        private System.Windows.Forms.NumericUpDown GoalAreaXPositionUpDown;
        private System.Windows.Forms.NumericUpDown GoalAreaYPositionUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown IndependentBodyCountUpDown;
        private System.Windows.Forms.Label AvoidAreasLabel;
        private System.Windows.Forms.NumericUpDown AvoidAreaCountUpDown;
        private System.ComponentModel.BackgroundWorker RecalculateBackgroundWorker;
        private System.Windows.Forms.NumericUpDown ResolutionUpDown;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.SaveFileDialog SaveLevelFileDialog;
        private System.Windows.Forms.Button LoadButton;
        private System.Windows.Forms.OpenFileDialog OpenLevelFileDialog;
    }
}

