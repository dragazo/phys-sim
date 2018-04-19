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
    public partial class TeleporterDialog : Form
    {
        public Vector2 A
        {
            get
            {
                return new Vector2(AXBox.Value, -AYBox.Value);
            }
            set
            {
                AXBox.Value = value.X;
                AYBox.Value = -value.Y;
            }
        }
        public Vector2 B
        {
            get
            {
                return new Vector2(BXBox.Value, -BYBox.Value);
            }
            set
            {
                BXBox.Value = value.X;
                BYBox.Value = -value.Y;
            }
        }
        public Vector2 C
        {
            get
            {
                return new Vector2(CXBox.Value, -CYBox.Value);
            }
            set
            {
                CXBox.Value = value.X;
                CYBox.Value = -value.Y;
            }
        }
        public Vector2 D
        {
            get
            {
                return new Vector2(DXBox.Value, -DYBox.Value);
            }
            set
            {
                DXBox.Value = value.X;
                DYBox.Value = -value.Y;
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

        public TeleporterDialog()
        {
            InitializeComponent();
        }
    }
}
