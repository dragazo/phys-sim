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
    public partial class NodeDialog : Form
    {
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

        public NodeDialog()
        {
            InitializeComponent();
        }
    }
}
