using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhysSim
{
    public partial class BodyDialog : Form
    {
        public Universe Universe = null;
        public Body Body = null;

        public Vector2 Velocity
        {
            get
            {
                return VelocityPicker.Value;
            }
            set
            {
                VelocityPicker.Value = value;
            }
        }
        public Vector2 Forces
        {
            set
            {
                ForcesPicker.Value = value;
            }
        }

        public Vector2 Position
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

        public BodyIcon BodyIcon
        {
            get
            {
                return (BodyIcon)IconCombo.SelectedItem;
            }
            set
            {
                IconCombo.SelectedItem = value;
            }
        }

        public bool Significant
        {
            get
            {
                return SignificantCheck.Checked;
            }
            set
            {
                SignificantCheck.Checked = value;
            }
        }
        public bool Collidable
        {
            get
            {
                return CollidableCheck.Checked;
            }
            set
            {
                CollidableCheck.Checked = value;
            }
        }

        public Color Color
        {
            get
            {
                return BackPicker.Value;
            }
            set
            {
                BackPicker.Value = value;
            }
        }
        public Color IconColor
        {
            get
            {
                return IconPicker.Value;
            }
            set
            {
                IconPicker.Value = value;
            }
        }

        public new string Name
        {
            get
            {
                return NameBox.Text;
            }
            set
            {
                NameBox.Text = value;
            }
        }

        public double Mass
        {
            get
            {
                return MassBox.Value;
            }
            set
            {
                MassBox.Value = value;
            }
        }
        public double Charge
        {
            get
            {
                return ChargeBox.Value;
            }
            set
            {
                ChargeBox.Value = value;
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
        public double Distance
        {
            get
            {
                return DistanceBox.Value;
            }
            set
            {
                DistanceBox.Value = value;
            }
        }

        public BodyDialog()
        {
            InitializeComponent();

            foreach (BodyIcon val in Enum.GetValues(typeof(BodyIcon)))
                IconCombo.Items.Add(val);

            BodyIcon = BodyIcon.None;

            MassBox.TextChanged += (o,e) => RecalculateForces();
            ChargeBox.TextChanged += (o, e) => RecalculateForces();
            XBox.TextChanged += (o, e) => RecalculateForces();
            YBox.TextChanged += (o, e) => RecalculateForces();
            VelocityPicker.OnValueChanged += RecalculateForces;
            SignificantCheck.CheckedChanged += (o, e) => RecalculateForces();
        }

        public void RecalculateForces()
        {
            if (Universe != null && Body != null)
                Forces = Universe.GetForces(Body, MassBox.Value, ChargeBox.Value, Position, Velocity, Significant);                
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            DistanceBox.Value = 0d;
        }
    }
}
