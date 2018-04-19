using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace PhysSim
{
    public class EditorObject : INodular
    {
        [XmlIgnore]
        public Vector2 Position;

        [XmlElement(ElementName = "Position")]
        public Vector2 _Position
        {
            get
            {
                return new Vector2(Position.X, -Position.Y);
            }
            set
            {
                Position = new Vector2(value.X, -value.Y);
            }
        }

        public EditorObject(Vector2 pos)
        {
            Position = pos;
        }
        public EditorObject() : this(Vector2.Zero) { }

        public virtual RectangleF GetClip(Vector2 origin, double scale)
        {
            throw new NotImplementedException("GetClip not implimented");
        }
        public virtual void Paint(PaintEventArgs e, Vector2 origin, double scale)
        {
            throw new NotImplementedException("Paint not implimented");
        }
    }

    public interface INodular
    {
        RectangleF GetClip(Vector2 origin, double scale);
        void Paint(PaintEventArgs e, Vector2 origin, double scale);
    }
}
