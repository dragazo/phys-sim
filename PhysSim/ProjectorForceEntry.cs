using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysSim
{
    public partial class ProjectorForceEntry : UserControl
    {
        public ProjectorShape Shape
        {
            get
            {
                return (ProjectorShape)ShapeCombo.SelectedItem;
            }
            set
            {
                ShapeCombo.SelectedItem = value;
            }
        }
        public ProjectorFalloff Falloff
        {
            get
            {
                return (ProjectorFalloff)FalloffCombo.SelectedItem;
            }
            set
            {
                FalloffCombo.SelectedItem = value;
            }
        }
        public ProjectorType Type
        {
            get
            {
                return (ProjectorType)TypeCombo.SelectedItem;
            }
            set
            {
                TypeCombo.SelectedItem = value;
            }
        }
        public ProjectorMode Mode
        {
            get
            {
                return (ProjectorMode)ModeCombo.SelectedItem;
            }
            set
            {
                ModeCombo.SelectedItem = value;
            }
        }
        public ProjectorSpeedMode SpeedMode
        {
            get
            {
                return (ProjectorSpeedMode)SpeedModeCombo.SelectedItem;
            }
            set
            {
                SpeedModeCombo.SelectedItem = value;
            }
        }

        public Vector2 Vector
        {
            get
            {
                return VectorPicker.Value;
            }
            set
            {
                VectorPicker.Value = value;
            }
        }
        public Color Color
        {
            get
            {
                return ColorPicker.Value;
            }
            set
            {
                ColorPicker.Value = value;
            }
        }

        public double Radius
        {
            get
            {
                return RadiusBox.Value;
            }
            set
            {
                RadiusBox.Value = value;
            }
        }

        public ProjectorForceEntry()
        {
            InitializeComponent();

            ShapeCombo.Initialize(ProjectorShape.Circle);
            FalloffCombo.Initialize(ProjectorFalloff.Quadratic);
            TypeCombo.Initialize(ProjectorType.Centripetal);
            ModeCombo.Initialize(ProjectorMode.Accelerate);
            SpeedModeCombo.Initialize(ProjectorSpeedMode.Constant);

            Vector = Vector2.Zero;
            Color = Color.AliceBlue;

            Radius = 20d;

            FalloffCombo.SelectedValueChanged += (o, e) => UpdateText();
            TypeCombo.SelectedValueChanged += (o, e) => UpdateText();
            ModeCombo.SelectedValueChanged += (o, e) => UpdateText();
            SpeedModeCombo.SelectedValueChanged += (o, e) => UpdateText();

            UpdateText();
        }

        private void UpdateText()
        {
            VectorLabel.Text = string.Format("{0}{1}{2}{3}",
                Mode == ProjectorMode.Accelerate ? "Acceleration (m/s^2)" : "Force (N)",
                Falloff == ProjectorFalloff.Constant ? "" : " (at 1m)",
                SpeedMode == ProjectorSpeedMode.Constant ? "" : " (at 1m/s)",
                Type == ProjectorType.Linear ? "" : " (magnitude only)");
        }
    }
}
