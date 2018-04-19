using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PhysSim
{
    public class NumberBox : TextBox
    {
        public double DefaultValue = 0d;

        public double Value
        {
            get
            {
                double d;
                if (double.TryParse(Text, out d)) return d;
                return DefaultValue;
            }
            set
            {
                //Text = value.ToString("0.#################E+0");
                Text = value.ToString("r");
            }
        }

        public NumberBox()
        {
            Value = 0d;
        }
    }

    public class ColorPicker : UserControl
    {
        public Color Value
        {
            get
            {
                return BackColor;
            }
            set
            {
                BackColor = value;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            ColorDialog d = new ColorDialog();
            d.Color = Value;

            if (d.ShowDialog() == DialogResult.OK)
                Value = d.Color;

            d.Dispose();
        }

        public ColorPicker()
        {
            Value = Color.White;

            BorderStyle = BorderStyle.FixedSingle;
        }
    }

    public class DirectionPicker : UserControl
    {
        private Vector2 _direction;
        public Vector2 Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value != Vector2.Zero ? value : Vector2.Right;
                Invalidate();

                if (OnUpdate != null) OnUpdate();
            }
        }

        public Action OnUpdate = null;

        public DirectionPicker()
        {
            Direction = Vector2.Right;

            DoubleBuffered = true;

            NeedlePen.StartCap = LineCap.Round;
            NeedlePen.EndCap = LineCap.ArrowAnchor;

            EnabledChanged += (o, e) => Invalidate();
        }

        protected override async void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (Enabled && e.Button == MouseButtons.Left)
                await Drag();
        }

        private Vector2 Center
        {
            get
            {
                return new Vector2(Size.Width / 2d, Size.Height / 2d);
            }
        }

        private async Task Drag()
        {
            while (MouseButtons == MouseButtons.Left)
            {
                await Task.Delay(Settings.SleepTime);

                Vector2 pos = ((Vector2)PointToClient(MousePosition) - Center);
                if (pos == Vector2.Zero) pos = Vector2.Up;
                else pos = pos.Unit;

                if (pos != Direction) Direction = pos;
            }
        }

        public SolidBrush DialBrush = new SolidBrush(Color.White);
        public SolidBrush DisabledDialBrush = new SolidBrush(Color.LightGray);

        public Pen BorderPen = new Pen(Color.LightGray, 2f);
        public Pen NeedlePen = new Pen(Color.Black, 4f);

        private double NeedleLength = 0.85d;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, Size.Width - 1, Size.Height - 1);

            e.Graphics.FillEllipse(Enabled ? DialBrush : DisabledDialBrush, rect);
            e.Graphics.DrawEllipse(BorderPen, rect);

            e.Graphics.DrawLine(NeedlePen, Center, Center + Direction * (NeedleLength * Width / 2d));
        }
    }

    public class SolidProgress : UserControl
    {
        private int _maxValue = 100;
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                Invalidate();
            }
        }

        private int _value = 0;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Invalidate();
            }
        }

        private SolidBrush WorkBrush = new SolidBrush(Color.LightSkyBlue);

        public Color WorkColor
        {
            get
            {
                return WorkBrush.Color;
            }
            set
            {
                WorkBrush.Color = value;
                Invalidate();
            }
        }

        public SolidProgress()
        {
            BorderStyle = BorderStyle.FixedSingle;

            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int w = (int)Math.Round((float)Width * Value / MaxValue);

            e.Graphics.FillRectangle(WorkBrush,
                0, 0, w, Height);
        }
    }

    public class DotProgress : UserControl
    {
        private int _maxValue = 100;
        public int MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
                Invalidate();
            }
        }

        private int _value = 0;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Invalidate();
            }
        }

        private SolidBrush CompleteBrush = new SolidBrush(Color.LightSkyBlue);
        private SolidBrush IncompleteBrush = new SolidBrush(Color.LightGray);

        public Color CompleteColor
        {
            get
            {
                return CompleteBrush.Color;
            }
            set
            {
                CompleteBrush.Color = value;
                Invalidate();
            }
        }
        public Color IncompleteColor
        {
            get
            {
                return IncompleteBrush.Color;
            }
            set
            {
                IncompleteBrush.Color = value;
                Invalidate();
            }
        }

        public DotProgress()
        {
            DoubleBuffered = true;
            RecalculateDots();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            RecalculateDots();
        }

        private int _dots = 10;
        public int Dots
        {
            get
            {
                return _dots;
            }
            set
            {
                _dots = value;
                RecalculateDots();
                Invalidate();
            }
        }

        private float _spacing = 1f;
        public float Spacing
        {
            get
            {
                return _spacing;
            }
            set
            {
                _spacing = value;
                RecalculateDots();
                Invalidate();
            }
        }

        private Region DotsRegion = null;

        private void RecalculateDots()
        {
            GraphicsPath path = new GraphicsPath();
            Region dots = new Region();
            dots.MakeEmpty();

            float width = Width / (Dots + (Dots - 1) * Spacing);
            for (float d = 0f; d < Width; d += width * (Spacing + 1f))
                path.AddEllipse(d, 0f, width, width);

            dots.Union(path);
            path.Dispose();

            DotsRegion = dots;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            float d = (float)Math.Round((double)Width * Value / MaxValue);

            Region complete = new Region(new RectangleF(0, 0, d, Height));
            Region incomplete = new Region(new RectangleF(d, 0, Width - d, Height));

            complete.Intersect(DotsRegion);
            incomplete.Intersect(DotsRegion);

            e.Graphics.FillRegion(CompleteBrush, complete);
            e.Graphics.FillRegion(IncompleteBrush, incomplete);

            complete.Dispose();
            incomplete.Dispose();
        }
    }
}
