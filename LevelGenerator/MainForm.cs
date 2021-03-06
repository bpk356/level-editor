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
        SimulationState _simulationState;
        List<Control> _generatedControls;
        bool DifficultyEvaluationCancelRequested;
        IDraggableObject _draggingObject;
        Vector2 _draggingOffset;
        List<SpaceBody> _controlledBodyStates;
        List<List<SpaceBody>> _independentBodyStates;
        bool _showTrajectoryOnMouseMove;
        int _resolution;

        public MainForm()
        {
            GLLoaded = false;
            InitializeComponent();
            _generator = new Random();
            _screenBounds = new Rect(-960 / 2, -640 / 2, 960 / 2, 640 / 2);
            _bestLevel = new Level(1, _generator, _screenBounds);
            UpdateControls();
            DifficultyEvaluationCancelRequested = false;
            _resolution = 4;
        }

        private bool UpdatePicture()
        {
            if (DisplayGLControl.InvokeRequired)
            {
                DisplayGLControl.Invoke(new Func<bool>(UpdatePicture));
            }
            else
            {
                DisplayGLControl.Invalidate();
            }

            return DifficultyEvaluationCancelRequested;
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

                GoalAreaXPositionUpDown.Value = (decimal)_bestLevel.GoalArea.Position.X;
                GoalAreaYPositionUpDown.Value = (decimal)_bestLevel.GoalArea.Position.Y;
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
                currentY = UpdateIndependentBodyControls(currentY);
                UpdateAvoidAreaControls(currentY);

            }
        }

        private void UpdateAvoidAreaControls(int currentY)
        {
            Point location = AvoidAreasLabel.Location;
            location.Y = currentY;
            AvoidAreasLabel.Location = location;

            location = AvoidAreaCountUpDown.Location;
            location.Y = currentY;
            AvoidAreaCountUpDown.Location = location;
            AvoidAreaCountUpDown.Value = (decimal)_bestLevel.AvoidAreas.Count;
            currentY += 35;

            int avoidAreaIndex = 0;
            foreach (var avoidArea in _bestLevel.AvoidAreas)
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
                xPosition.Name = avoidAreaIndex + ",x";
                xPosition.ValueChanged += AvoidArea_ValueChanged;
                xPosition.Value = (decimal)avoidArea.Position.X;
                Controls.Add(xPosition);
                _generatedControls.Add(xPosition);

                NumericUpDown yPosition = new NumericUpDown();
                yPosition.Minimum = (decimal)_screenBounds.MinY;
                yPosition.Maximum = (decimal)_screenBounds.MaxY;
                yPosition.Size = new System.Drawing.Size(60, 20);
                yPosition.Location = new System.Drawing.Point(145, currentY - 3);
                yPosition.Name = avoidAreaIndex + ",y";
                yPosition.ValueChanged += AvoidArea_ValueChanged;
                yPosition.Value = (decimal)avoidArea.Position.Y;
                Controls.Add(yPosition);
                _generatedControls.Add(yPosition);

                NumericUpDown radius = new NumericUpDown();
                radius.Minimum = (decimal)1;
                radius.Maximum = (decimal)1000;
                radius.Size = new System.Drawing.Size(60, 20);
                radius.Location = new System.Drawing.Point(210, currentY - 3);
                radius.Name = avoidAreaIndex + ",radius";
                radius.ValueChanged += AvoidArea_ValueChanged;
                radius.Value = (decimal)avoidArea.Radius;
                Controls.Add(radius);
                _generatedControls.Add(radius);

                currentY += 35;
                avoidAreaIndex++;
            }
        }

        private int UpdateIndependentBodyControls(int currentY)
        {
            int independentBodyI = 0;
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
                xPosition.Name = independentBodyI + ",x";
                xPosition.ValueChanged += IndependentBodyPosition_ValueChanged;
                xPosition.Value = (decimal)independentBody.Position.X;
                Controls.Add(xPosition);
                _generatedControls.Add(xPosition);

                NumericUpDown yPosition = new NumericUpDown();
                yPosition.Minimum = (decimal)_screenBounds.MinY;
                yPosition.Maximum = (decimal)_screenBounds.MaxY;
                yPosition.Size = new System.Drawing.Size(60, 20);
                yPosition.Location = new System.Drawing.Point(145, currentY - 3);
                yPosition.Name = independentBodyI + ",y";
                yPosition.ValueChanged += IndependentBodyPosition_ValueChanged;
                yPosition.Value = (decimal)independentBody.Position.Y;
                Controls.Add(yPosition);
                _generatedControls.Add(yPosition);

                NumericUpDown mass = new NumericUpDown();
                mass.Minimum = (decimal)1;
                mass.Maximum = (decimal)20;
                mass.Size = new System.Drawing.Size(60, 20);
                mass.Location = new System.Drawing.Point(210, currentY - 3);
                mass.Name = independentBodyI + ",mass";
                mass.ValueChanged += IndependentBodyPosition_ValueChanged;
                mass.Value = (decimal)independentBody.Radius;
                Controls.Add(mass);
                _generatedControls.Add(mass);

                CheckBox isStationary = new CheckBox();
                isStationary.Text = "Is Stationary";
                isStationary.Checked = independentBody.IsStationary;
                isStationary.Location = new System.Drawing.Point(275, currentY - 3);
                isStationary.Name = independentBodyI + ",stationary";
                isStationary.CheckedChanged += IndependentBodyIsStationaryCheckBox_CheckedChanged;
                Controls.Add(isStationary);
                _generatedControls.Add(isStationary);

                independentBodyI++;
                currentY += 35;
            }
            return currentY;
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
                    lock (_bestTryDictionary)
                    {
                        foreach (var kvp in _bestTryDictionary)
                        {
                            DrawHelper.SetPixel(kvp.Key, Color.FromArgb(100, kvp.Value ? Color.White : Color.Red), _screenBounds, _resolution);
                        }
                    }
                }

                _bestLevel.Draw(_screenBounds);

                if (_simulationState != null)
                {
                    _simulationState.Draw(_screenBounds);
                }

                if (_controlledBodyStates != null)
                {
                    for (int frameI = 0; frameI < _controlledBodyStates.Count; frameI++)
                    {
                        var controlledBodyState = _controlledBodyStates[frameI];
                        controlledBodyState.Draw(Color.FromArgb(50, Color.SkyBlue), false, _screenBounds);
                        for (int independentBodyI = 0; independentBodyI < _independentBodyStates.Count; independentBodyI++)
                        {
                            var independentBodyState = _independentBodyStates[independentBodyI][frameI];
                            independentBodyState.Draw(Color.FromArgb(50, independentBodyState.IsStationary ? Color.Purple : Color.Blue), false, _screenBounds);
                        }
                    }
                }

                DifficultyLabel.Text = "Difficulty: " + _bestDifficulty + ", Interestingness: " + _bestInterestingness;
            }

            DisplayGLControl.SwapBuffers();
        }

        private void DisplayGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (_simulationState != null)
                {
                    EndSimulating();
                }
                else
                {
                    BeginSimulating(e.X, e.Y);
                }
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                if (_showTrajectoryOnMouseMove)
                {
                    _showTrajectoryOnMouseMove = false;
                }
                else if (!_showTrajectoryOnMouseMove && _controlledBodyStates != null)
                {
                    _controlledBodyStates = null;
                    UpdatePicture();
                }
                else
                {
                    _showTrajectoryOnMouseMove = true;
                }
                /*
                if (_controlledBodyStates == null)
                {
                    _simulationState = new SimulationState(_bestLevel, GLControlMousePositionToWorldCoordinates(e.X, e.Y));
                    List<SpaceBody> controlledBodyStates = new List<SpaceBody>();
                    for (int i = 0; i < SimulationState.SimulationFramesPerSecond * 15; i++)
                    {
                        controlledBodyStates.Add(new SpaceBody(_simulationState.ControlledBody));
                        _simulationState.MoveBodies();
                    }
                    _controlledBodyStates = controlledBodyStates;
                }
                else
                {
                    _controlledBodyStates = null;
                }
                UpdatePicture();*/
            }
        }

        private Vector2 GLControlMousePositionToWorldCoordinates(int x, int y)
        {
            return new Vector2(_screenBounds.MinX + x, _screenBounds.MaxY - 1 - y);
        }

        private void BeginSimulating(int x, int y)
        {
            SimulationTimer.Stop();
            _simulationState = new SimulationState(_bestLevel, GLControlMousePositionToWorldCoordinates(x, y));
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
            if (RecalculateBackgroundWorker.IsBusy)
            {
                DifficultyEvaluationCancelRequested = true;
            }
            else
            {
                DifficultyEvaluationCancelRequested = false;
                RecalculateButton.Text = "Cancel";
                RecalculateButton.Update();
                RecalculateBackgroundWorker.RunWorkerAsync();
            }
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
            _bestLevel.GoalArea.Position.X = (float)GoalAreaXPositionUpDown.Value;
            UpdatePicture();
        }

        private void GoalAreaYPositionUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.GoalArea.Position.Y = (float)GoalAreaYPositionUpDown.Value;
            UpdatePicture();
        }

        private void GoalAreaRadiusUpDown_ValueChanged(object sender, EventArgs e)
        {
            _bestLevel.GoalArea.Radius = (float)GoalAreaRadiusUpDown.Value;
            UpdatePicture();
        }

        private void IndependentBodyIsStationaryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBoxSender = (CheckBox)sender;
            string[] parts = checkBoxSender.Name.Split(',');
            int index = int.Parse(parts[0]);

            _bestLevel.IndependentBodies[index].IsStationary = checkBoxSender.Checked;
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

        private void IndependentBodyCountUpDown_ValueChanged(object sender, EventArgs e)
        {
            int newIndependentBodyCount = (int)IndependentBodyCountUpDown.Value;
            while (_bestLevel.IndependentBodies.Count > newIndependentBodyCount)
            {
                _bestLevel.IndependentBodies.RemoveAt(_bestLevel.IndependentBodies.Count - 1);
            }
            while (_bestLevel.IndependentBodies.Count < newIndependentBodyCount)
            {
                _bestLevel.IndependentBodies.Add(new SpaceBody(_generator, _screenBounds));
            }
            UpdateControls();
        }

        private void AvoidArea_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown upDownSender = (NumericUpDown)sender;
            string[] parts = upDownSender.Name.Split(',');
            int index = int.Parse(parts[0]);
            string axis = parts[1];

            if (axis == "x")
            {
                _bestLevel.AvoidAreas[index].Position.X = (float)upDownSender.Value;
            }
            else if (axis == "y")
            {
                _bestLevel.AvoidAreas[index].Position.Y = (float)upDownSender.Value;
            }
            else if (axis == "radius")
            {
                _bestLevel.AvoidAreas[index].Radius = (int)upDownSender.Value;
            }
            UpdatePicture();
        }

        private void RecalculateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _bestDifficulty = DifficultyEvaluator.EvaluateDifficulty(_bestLevel, _screenBounds, _resolution, out _bestInterestingness, out _bestTryDictionary, UpdatePicture);
            UpdatePicture();
        }

        private void RecalculateBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RecalculateButton.Text = "Recalculate";
        }

        private void DisplayGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }
            Vector2 mousePosition = GLControlMousePositionToWorldCoordinates(e.X, e.Y);
            _draggingObject = null;
            float closestDraggableObjectDistance = 0.0f;
            foreach (var draggable in _bestLevel.GetDraggableObjects())
            {
                if (!draggable.MouseIsOnObject(mousePosition))
                {
                    continue;
                }
                if (draggable.DistanceFromMouse(mousePosition) < closestDraggableObjectDistance || _draggingObject == null)
                {
                    _draggingObject = draggable;
                    closestDraggableObjectDistance = draggable.DistanceFromMouse(mousePosition);
                }
            }
            if (_draggingObject != null)
            {
                _draggingOffset = _draggingObject.OffsetFromMouse(mousePosition);
            }

        }

        private void DisplayGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingObject != null)
            {
                Vector2 mousePosition = GLControlMousePositionToWorldCoordinates(e.X, e.Y);
                Vector2 newObjectPosition = mousePosition + _draggingOffset;
                _draggingObject.UpdatePosition(newObjectPosition);
                UpdateControls();
                UpdatePicture();
                DisplayGLControl.Update();
            }
            else if (_showTrajectoryOnMouseMove)
            {
                SimulationState simulationState = new SimulationState(_bestLevel, GLControlMousePositionToWorldCoordinates(e.X, e.Y));
                List<SpaceBody> controlledBodyStates = new List<SpaceBody>();
                List<List<SpaceBody>> independentBodyStates = new List<List<SpaceBody>>();
                foreach (var independentBody in simulationState.IndependentBodies)
                {
                    independentBodyStates.Add(new List<SpaceBody>());
                }
                for (int i = 0; i < SimulationState.SimulationFramesPerSecond * 15; i++)
                {
                    controlledBodyStates.Add(new SpaceBody(simulationState.ControlledBody));
                    for (int independentBodyI = 0; independentBodyI < simulationState.IndependentBodies.Count; independentBodyI++)
                    {
                        var independentBody = simulationState.IndependentBodies[independentBodyI];
                        independentBodyStates[independentBodyI].Add(new SpaceBody(independentBody));
                    }
                    simulationState.MoveBodies();
                }
                _controlledBodyStates = controlledBodyStates;
                _independentBodyStates = independentBodyStates;
                UpdatePicture();
            }
        }

        private void DisplayGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            _draggingObject = null;
        }

        private void AvoidAreaCountUpDown_ValueChanged(object sender, EventArgs e)
        {
            int newAvoidAreaCount = (int)AvoidAreaCountUpDown.Value;
            while (_bestLevel.AvoidAreas.Count > newAvoidAreaCount)
            {
                _bestLevel.AvoidAreas.RemoveAt(_bestLevel.AvoidAreas.Count - 1);
            }
            while (_bestLevel.AvoidAreas.Count < newAvoidAreaCount)
            {
                _bestLevel.AvoidAreas.Add(new Circle(new Vector2(0, 0), 100));
            }
            UpdateControls();
        }

        private void ResolutionUpDown_ValueChanged(object sender, EventArgs e)
        {
            _resolution = (int)ResolutionUpDown.Value;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (SaveLevelFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _bestLevel.Save(SaveLevelFileDialog.FileName);
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (OpenLevelFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _bestLevel = new Level(OpenLevelFileDialog.FileName);
                UpdatePicture();
                UpdateControls();
            }
        }
    }
}
