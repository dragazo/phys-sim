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
    public partial class VectorPicker : UserControl
    {
        public Vector2 Value
        {
            get
            {
                return new Vector2(XBox.Value, -YBox.Value);
            }
            set
            {
                XBox.Value = value.X;
                YBox.Value = -value.Y;
            }
        }
        public Action OnValueChanged = null;

        private bool _ReadOnly = false;
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                MagnitudeBox.ReadOnly = value;
                AngleBox.ReadOnly = value;

                XBox.ReadOnly = value;
                YBox.ReadOnly = value;

                DirectionPicker.Enabled = !value;

                _ReadOnly = value;
            }
        }

        private bool _Updating = false;
        private bool Updating
        {
            get
            {
                return _Updating;
            }
            set
            {
                _Updating = value;
                if (!value && OnValueChanged != null) OnValueChanged();
            }
        }

        public VectorPicker()
        {
            InitializeComponent();

            DirectionPicker.OnUpdate += () =>
            {
                if (Updating) return; Updating = true;

                double ang = 2d * Math.PI - DirectionPicker.Direction.Theta;

                AngleBox.Value = ang * 180d / Math.PI;
                XBox.Value = MagnitudeBox.Value * Math.Cos(ang);
                YBox.Value = MagnitudeBox.Value * Math.Sin(ang);

                Updating = false;
            };

            MagnitudeBox.TextChanged += (o, e) =>
            {
                if (Updating) return; Updating = true;

                bool reDir = Value == Vector2.Zero;
                double ang = reDir ? 0d : AngleBox.Value * Math.PI / 180d;

                XBox.Value = MagnitudeBox.Value * Math.Cos(ang);
                YBox.Value = MagnitudeBox.Value * Math.Sin(ang);

                if (reDir || Value == Vector2.Zero)
                {
                    Vector2 now = Value;
                    ang = Math.Atan2(now.Y, now.X);
                    DirectionPicker.Direction = Vector2.FromPolar(ang, 1d);
                    AngleBox.Value = ang * 180d / Math.PI;
                }

                Updating = false;
            };

            AngleBox.TextChanged += (o, e) =>
            {
                if (Updating) return; Updating = true;

                double ang = AngleBox.Value * Math.PI / 180d;

                DirectionPicker.Direction = Vector2.FromPolar(-ang, 1d);

                XBox.Value = MagnitudeBox.Value * Math.Cos(ang);
                YBox.Value = MagnitudeBox.Value * Math.Sin(ang);

                Updating = false;
            };

            XBox.TextChanged += (o, e) =>
            {
                if (Updating) return; Updating = true;

                AngleBox.Value = Math.Atan2(YBox.Value, XBox.Value) * 180d / Math.PI;
                DirectionPicker.Direction = Vector2.FromPolar(-AngleBox.Value * Math.PI / 180d, 1d);

                MagnitudeBox.Value = Value.Magnitude;

                Updating = false;
            };

            YBox.TextChanged += (o, e) =>
            {
                if (Updating) return; Updating = true;

                AngleBox.Value = Math.Atan2(YBox.Value, XBox.Value) * 180d / Math.PI;
                DirectionPicker.Direction = Vector2.FromPolar(-AngleBox.Value * Math.PI / 180d, 1d);

                MagnitudeBox.Value = Value.Magnitude;

                Updating = false;
            };
        }
    }
}
