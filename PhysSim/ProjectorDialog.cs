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
    public partial class ProjectorDialog : Form
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

        public ProjectorDialog()
        {
            InitializeComponent();

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(new ToolStripMenuItem("Add", null, (o, e) => AddEntry(new ProjectorForceEntry())));

            EditorFlow.ContextMenuStrip = menu;
        }

        public IEnumerable<ProjectorForceEntry> Entries
        {
            get
            {
                return EditorFlow.Controls.OfType<ProjectorForceEntry>().OrderByDescending(e => e.Radius);
            }
        }

        public void AddEntry(ProjectorForceEntry entry)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add(new ToolStripMenuItem("Remove", null, (o, e) => RemoveEntry(entry)));

            entry.ContextMenuStrip = menu;

            EditorFlow.Controls.Add(entry);
        }
        public void RemoveEntry(ProjectorForceEntry entry)
        {
            EditorFlow.Controls.Remove(entry);
        }

        public void ClearEntries()
        {
            EditorFlow.Controls.Clear();
        }
    }
}
