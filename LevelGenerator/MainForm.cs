﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace LevelGenerator
{
    public partial class MainForm : Form
    {
        Level _bestLevel;
        Random _generator;
        Rect _screenBounds;
        double _bestDifficulty;
        double _bestInterestingness;
        Dictionary<Vector2, bool> _bestTryDictionary;
        bool GLLoaded;
        bool RequestRestart;
        SimulationState _simulationState;
        List<Control> _generatedControls;

        public MainForm()
        {
            GLLoaded = false;
            InitializeComponent();
            _generator = new Random();
            RequestRestart = false;
            _screenBounds = new Rect(-960 / 2, -640 / 2, 960 / 2, 640 / 2);
            _bestLevel = new Level(1, _generator, _screenBounds);
            UpdateControls();
        }

        private void UpdatePicture()
        {
            DisplayGLControl.Invalidate();
        }

        private void TunerBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Level newLevel = new Level(1, _generator, _screenBounds);
                double newInterestingness;
                Dictionary<Vector2, bool> tryDictionary;
                double newDifficulty = DifficultyEvaluator.EvaluateDifficulty(newLevel, _screenBounds, 16, out newInterestingness, out tryDictionary);

                if (newDifficulty > .0001 && newInterestingness > _bestInterestingness || RequestRestart)
                {
                    _bestLevel = newLevel;
                    _bestDifficulty = newDifficulty;
                    _bestInterestingness = newInterestingness;
                    _bestTryDictionary = tryDictionary;
                    UpdatePicture();
                    UpdateControls();
                    RequestRestart = false;
                    break;
                }
            }
        }

        private void UpdateControls()
        {
            if (ControlledBodyXPositionUpDown.InvokeRequired)
            {
                ControlledBodyXPositionUpDown.Invoke(new Action(UpdateControls));
            }
            else
            {
                ControlledBodyXPositionUpDown.Value = (decimal)_bestLevel.ControlledBody.Position.X;
                ControlledBodyYPositionUpDown.Value = (decimal)_bestLevel.ControlledBody.Position.Y;
                ControlledBodyMassUpDown.Value = (decimal)_bestLevel.ControlledBody.Radius;

                GoalAreaXPositionUpDown.Value = (decimal)_bestLevel.GoalArea.CenterPoint.X;
                GoalAreaYPositionUpDown.Value = (decimal)_bestLevel.GoalArea.CenterPoint.Y;
                GoalAreaRadiusUpDown.Value = (decimal)_bestLevel.GoalArea.Radius;

                IndependentBodyCountUpDown.Value = (decimal)_bestLevel.IndependentBodies.Count;

                if (_generatedControls != null)
                {
                    foreach (var generatedControl in _generatedControls)
                    {
                        Controls.Remove(generatedControl);
                    }
                }
                _generatedControls = new List<Control>();

                int currentY = IndependentBodyCountUpDown.Location.Y + 30;
                foreach (var independentBody in _bestLevel.IndependentBodies)
                {
                    Label positionLabel = new Label();
                    positionLabel.Text = "Position";
                    positionLabel.AutoSize = true;
                    positionLabel.Location = new System.Drawing.Point(10, currentY);
                    positionLabel.Size = new System.Drawing.Size(102, 13);
                    Controls.Add(positionLabel);
                    _generatedControls.Add(positionLabel);

                    NumericUpDown xPosition = new NumericUpDown();
                    xPosition.Minimum = (decimal)_screenBounds.MinX;
                    xPosition.Maximum = (decimal)_screenBounds.MaxX;
                    xPosition.Size = new System.Drawing.Size(60, 20);
                    xPosition.Location = new System.Drawing.Point(80, currentY - 3);
                    xPosition.Name = "0,x";
                    xPosition.ValueChanged += IndependentBodyPosition_ValueChanged;
                    xPosition.Value = (decimal)independentBody.Position.X;
                    Controls.Add(xPosition);
                    _generatedControls.Add(xPosition);

                    NumericUpDown yPosition = new NumericUpDown();
                    yPosition.Minimum = (decimal)_screenBounds.MinY;
                    yPosition.Maximum = (decimal)_screenBounds.MaxY;
                    yPosition.Size = new System.Drawing.Size(60, 20);
                    yPosition.Location = new System.Drawing.Point(145, currentY - 3);
                    yPosition.Name = "0,y";
                    yPosition.ValueChanged += IndependentBodyPosition_ValueChanged;
                    yPosition.Value = (decimal)independentBody.Position.Y;
                    Controls.Add(yPosition);
                    _generatedControls.Add(yPosition);

                    NumericUpDown mass = new NumericUpDown();
                    mass.Minimum = (decimal)1;
                    mass.Maximum = (decimal)20;
                    mass.Size = new System.Drawing.Size(60, 20);
                    mass.Location = new System.Drawing.Point(210, currentY - 3);
                    mass.Name = "0,mass";
                    mass.ValueChanged += IndependentBodyPosition_ValueChanged;
                    mass.Value = (decimal)independentBody.Radius;
                    Controls.Add(mass);
                    _generatedControls.Add(mass);
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GLLoaded = true;
            SetupViewport();
        }

        private void SetupViewport()
        {
            int width = DisplayGLControl.Width;
            int height = DisplayGLControl.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, 0, height, -1, 1);
            GL.Viewport(0, 0, width, height);
            GL.ClearColor(Color.Black);
        }

        private void DisplayGLControl_Paint(object sender, PaintEventArgs e)
        {
            if (!GLLoaded)
            {
                return;
            }

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_bestLevel != null)
            {
                if (_bestTryDictionary != null)
                {
                    foreach (var kvp in _bestTryDictionary)
                    {
                        DrawHelper.SetPixel(kvp.Key, kvp.Value ? Color.White : Color.Red, _screenBounds);
                    }
                }

                _bestLevel.Draw(_screenBounds);

                if (_simulationState != null)
                {
                    _simulationState.Draw(_screenBounds);
                }

                DifficultyLabel.Text = "Difficulty: " + _bestDifficulty + ", Interestingness: " + _bestInterestingness;
            }

            DisplayGLControl.SwapBuffers();
        }

        private void DisplayGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                BeginSimulating(e.X, e.Y);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                EndSimulating();
            }
        }

        private void BeginSimulating(int x, int y)
        {
            SimulationTimer.Stop();
            _simulationState = new SimulationState(_bestLevel, new Vector2(_screenBounds.MinX + x, _screenBounds.MaxY - 1 - y));
            SimulationTimer.Start();
        }

        private void EndSimulating()
        {
            SimulationTimer.Stop();
            _simulationState = null;
            UpdatePicture();
        }

        private void SimulationTimer_Tick(object sender, EventArgs e)
        {
            _simulationState.MoveBodies();
            UpdatePicture();
        }

        private void RecalculateButton_Click(object sender, EventArgs e)
        {
            RecalculateButton.Enabled = false;
            RecalculateButton.Update();
            _bestDifficulty = DifficultyEvaluator.EvaluateDifficulty(_bestLevel, _screenBounds, 8, out _bestInterestingness, out _bestTryDictionary);
            UpdatePicture();
            RecalculateButton.Enabled = true;
        }

        private void ControlledBodyXPositionUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.ControlledBody.Position.X = (float)ControlledBodyXPositionUpDown.Value;
            UpdatePicture();
        }

        private void ControlledBodyYPositionUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.ControlledBody.Position.Y = (float)ControlledBodyYPositionUpDown.Value;
            UpdatePicture();
        }

        private void ControlledBodyMassUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.ControlledBody.Radius = (int)ControlledBodyMassUpDown.Value;
            UpdatePicture();
        }

        private void GoalAreaXPositionUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.GoalArea.CenterPoint.X = (float)GoalAreaXPositionUpDown.Value;
            UpdatePicture();
        }

        private void GoalAreaYPositionUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.GoalArea.CenterPoint.Y = (float)GoalAreaYPositionUpDown.Value;
            UpdatePicture();
        }

        private void GoalAreaRadiusUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.GoalArea.Radius = (float)GoalAreaRadiusUpDown.Value;
            UpdatePicture();
        }

        private void IndependentBodyPosition_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown upDownSender = (NumericUpDown)sender;
            string[] parts = upDownSender.Name.Split(',');
            int index = int.Parse(parts[0]);
            string axis = parts[1];

            if (axis == "x")
            {
                _bestLevel.IndependentBodies[index].Position.X = (float)upDownSender.Value;
            }
            else if (axis == "y")
            {
                _bestLevel.IndependentBodies[index].Position.Y = (float)upDownSender.Value;
            }
            else if (axis == "mass")
            {
                _bestLevel.IndependentBodies[index].Radius = (int)upDownSender.Value;
            }
            UpdatePicture();
        }
    }
}
