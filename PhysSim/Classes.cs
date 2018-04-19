using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace PhysSim
{
    public static class Extensions
    {
        public static void Add(this Control.ControlCollection col, params Control[] controls)
        {
            for (int i = 0; i < controls.Length; i++)
                col.Add(controls[i]);
        }

        public static T OneOrDefault<T>(this IEnumerable<T> query)
        {
            T[] items = query.Take(2).ToArray();
            return items.Length == 1 ? items[0] : default(T);
        }
        public static T OneOrDefault<T>(this IEnumerable<T> query, Func<T, bool> condition)
        {
            return query.Where(condition).OneOrDefault();
        }

        public static void Initialize<T>(this ComboBox combo, T initial) where T : IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("Type is not an Enum");
            combo.Items.Clear();
            foreach (T item in Enum.GetValues(typeof(T)))
                combo.Items.Add(item);
            combo.SelectedItem = initial;
        }
    }

    public enum ClosingOption
    {
        Save,
        Discard,
        Cancel
    }

    //core
    public static partial class Utility
    {
        public static bool Confirm()
        {
            return MessageBox.Show("This action cannot be undone", "Confirm operation?", MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public static ClosingOption ClosingPrompt()
        {
            DialogResult r = MessageBox.Show("Data may be permanently lost", "Save before closing?", MessageBoxButtons.YesNoCancel);
            switch (r)
            {
                case DialogResult.Yes:
                    return ClosingOption.Save;
                case DialogResult.No:
                    return ClosingOption.Discard;
                default:
                    return ClosingOption.Cancel;
            }
        }

        public static T CreateControl<T>(Point location, Size size) where T : Control, new()
        {
            T thing = new T();

            thing.Location = location;
            thing.Size = size;

            return thing;
        }

        public static Rectangle BindingRect(params Point[] points)
        {
            if (points.Length < 2) throw new ArgumentException("2 points required to create a rectangle");

            int LX = int.MaxValue;
            int HX = int.MinValue;

            int LY = int.MaxValue;
            int HY = int.MinValue;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X > HX) HX = points[i].X;
                if (points[i].X < LX) LX = points[i].X;

                if (points[i].Y > HY) HY = points[i].Y;
                if (points[i].Y < LY) LY = points[i].Y;
            }

            return new Rectangle(LX, LY, HX - LX, HY - LY);
        }
        public static Rectangle BindingRect(Point a, Point b)
        {
            bool x = a.X < b.X, y = a.Y < b.Y;
            return new Rectangle(x ? a.X : b.X, y ? a.Y : b.Y,
                x ? b.X - a.X : a.X - b.X, y ? b.Y - a.Y : a.Y - b.Y);
        }

        public static RectangleF BindingRect(params PointF[] points)
        {
            if (points.Length < 2) throw new ArgumentException("2 points required to create a rectangle");

            float LX = float.MaxValue;
            float HX = float.MinValue;

            float LY = float.MaxValue;
            float HY = float.MinValue;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X > HX) HX = points[i].X;
                if (points[i].X < LX) LX = points[i].X;

                if (points[i].Y > HY) HY = points[i].Y;
                if (points[i].Y < LY) LY = points[i].Y;
            }

            return new RectangleF(LX, LY, HX - LX, HY - LY);
        }
        public static RectangleF BindingRect(PointF a, PointF b)
        {
            bool x = a.X < b.X, y = a.Y < b.Y;
            return new RectangleF(x ? a.X : b.X, y ? a.Y : b.Y,
                x ? b.X - a.X : a.X - b.X, y ? b.Y - a.Y : a.Y - b.Y);
        }

        public static bool WithinDrawRange(Point point)
        {
            return point.X < Settings.MaxDrawRange && point.X > -Settings.MaxDrawRange
                && point.Y < Settings.MaxDrawRange && point.Y > -Settings.MaxDrawRange;
        }
        public static bool WithinDrawRange(PointF point)
        {
            return point.X < Settings.MaxDrawRange && point.X > -Settings.MaxDrawRange
                && point.Y < Settings.MaxDrawRange && point.Y > -Settings.MaxDrawRange;
        }

        public static bool WithinDrawRange(Rectangle rect)
        {
            return rect.Left > -Settings.MaxDrawRange && rect.Right < Settings.MaxDrawRange
                && rect.Top > -Settings.MaxDrawRange && rect.Bottom < Settings.MaxDrawRange;
        }
        public static bool WithinDrawRange(RectangleF rect)
        {
            return rect.Left > -Settings.MaxDrawRange && rect.Right < Settings.MaxDrawRange
                && rect.Top > -Settings.MaxDrawRange && rect.Bottom < Settings.MaxDrawRange;
        }

        static Utility()
        {
            //IO
            XmlNamespaces.Add(string.Empty, string.Empty);
        }
    }
    //IO
    public static partial class Utility
    {
        private static readonly string[] ColorSplitters = { ", ", ",", " " };
        public static string ColorToString(Color c)
        {
            return string.Format("{0} {1} {2}", c.R, c.G, c.B);
        }
        public static Color ColorFromString(string s)
        {
            string[] strs = s.Split(ColorSplitters, StringSplitOptions.RemoveEmptyEntries);
            int[] ints = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                int res;
                ints[i] = int.TryParse(strs[i], out res) ? res : 0;
            }

            int R, G, B;
            R = ints.Length > 0 ? ints[0] : 0;
            G = ints.Length > 1 ? ints[1] : 0;
            B = ints.Length > 2 ? ints[2] : 0;

            return Color.FromArgb(R, G, B);
        }

        private static readonly XmlSerializer Xml = new XmlSerializer(typeof(Universe));
        private static readonly XmlWriterSettings XmlWriting = new XmlWriterSettings()
        {
            OmitXmlDeclaration = true,
            NewLineHandling = NewLineHandling.Entitize,
            Indent = true,
            Async = true,
            CloseOutput = true
        };
        private static readonly XmlReaderSettings XmlReading = new XmlReaderSettings()
        {
            Async = true,
            CloseInput = true
        };
        private static readonly XmlSerializerNamespaces XmlNamespaces = new XmlSerializerNamespaces();

        public const string XmlExt = ".xpu";

        public const string FileFilter = "Xml Physics Universe (.xpu)|*.xpu";

        public static void Write(Stream stream, Universe universe)
        {
            lock (universe.Lock)
            {
                using (XmlWriter writer = XmlWriter.Create(stream, XmlWriting))
                    Xml.Serialize(writer, universe, XmlNamespaces);
            }
        }

        public static Universe Read(Stream stream)
        {
            Universe universe = null;
            using (XmlReader reader = XmlReader.Create(stream, XmlReading))
                universe = Xml.Deserialize(reader) as Universe;
            if(universe != null) universe.Initialize();
            return universe;
        }

        public static bool Save(Universe universe, FileInfo file)
        {
            try
            {
                FileStream f = file.OpenWrite();
                Write(f, universe);
                f.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Uhoh, looks something went wrong while saving that file:\n\n{0}", ex));
            }
            return false;
        }
        public static FileInfo Save(Universe universe)
        {
            SaveFileDialog d = new SaveFileDialog();
            d.Filter = FileFilter;

            FileInfo file = null;
            if (d.ShowDialog() == DialogResult.OK)
            {
                file = new FileInfo(d.FileName);
                Save(universe, file);
            }
            d.Dispose();

            return file;
        }

        public static Universe Load(FileInfo file)
        {
            Universe universe = null;
            try
            {
                FileStream f = file.OpenRead();
                universe = Read(f);
                f.Close();
            }
            catch (Exception ex)
            {
                universe = null;
                MessageBox.Show(string.Format("Uhoh, looks like that file is corrupted:\n\n{0}", ex));
            }
            return universe;
        }
        public static FileInfo Load(out Universe universe)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = FileFilter;

            FileInfo file = null;
            if (d.ShowDialog() == DialogResult.OK)
            {
                file = new FileInfo(d.FileName);
                universe = Load(file);
            }
            else universe = null;
            d.Dispose();

            return file;
        }
    }

    public static class Settings
    {
        public const double G = 6.6740831e-11d;
        public const double K = -8.9875518e9d;

        public const int SleepTime = 15;
        public const double MinRadius = 5d;

        public const int MaxDrawRange = 1000000;
    }

    #region Universe
    //core
    public sealed partial class Universe
    {
        [XmlIgnore]
        public object Lock = new object();

        public Vector2 Origin = Vector2.Zero;
        public double Scale = 1d;

        public List<Body> Bodies = new List<Body>();
        public Boundary Boundary = new Boundary();
        public List<Projector> Projectors = new List<Projector>();
        public List<Teleporter> Teleporters = new List<Teleporter>();

        public void Initialize()
        {
            Boundary.Initialize();
        }

        public Universe Clone()
        {
            Universe c = new Universe();

            lock (Lock)
            {
                foreach (Body b in Bodies)
                    c.Bodies.Add(b.Clone());

                foreach (Projector p in Projectors)
                    c.Projectors.Add(p.Clone());

                c.Boundary = Boundary.Clone();

                foreach (Teleporter t in Teleporters)
                    c.Teleporters.Add(t.Clone());

                c.Origin = Origin;
                c.Scale = Scale;
            }

            return c;
        }
        public Universe Clone(IEnumerable<EditorObject> objects)
        {
            Universe c = new Universe();

            List<Teleporter> tele = new List<Teleporter>(Teleporters.Count);
            Dictionary<Node, Node> node = new Dictionary<Node, Node>(Boundary.Nodes.Count);
            foreach (EditorObject obj in objects)
            {
                if (obj is Body)
                {
                    Body b = (Body)obj;
                    c.Bodies.Add(b.Clone());
                    continue;
                }
                if (obj is Projector)
                {
                    Projector p = (Projector)obj;
                    c.Projectors.Add(p.Clone());
                    continue;
                }
                if (obj is Node)
                {
                    Node n = (Node)obj;
                    Teleporter t = GetTeleporter(n);
                    if (t == null)
                    {
                        Node n2 = n.Clone();
                        node.Add(n, n2);
                        c.Boundary.Nodes.Add(n2);
                    }
                    else
                    {
                        if (tele.Contains(t)) continue;

                        tele.Add(t);
                        c.Teleporters.Add(t.Clone());
                    }
                }
            }

            foreach (Wall w in Boundary.Walls)
            {
                if (!node.ContainsKey(w.A) || !node.ContainsKey(w.B)) continue;

                c.Boundary.Walls.Add(new Wall(node[w.A], node[w.B]));
            }
            
            return c;
        }

        public void Append(Universe other, Action<EditorObject> OnAdd = null)
        {
            foreach (Body b in other.Bodies)
            {
                Body b2 = b.Clone();
                Bodies.Add(b2);

                if (OnAdd != null) OnAdd(b2);
            }

            foreach (Projector p in other.Projectors)
            {
                Projector p2 = p.Clone();
                Projectors.Add(p2);

                if (OnAdd != null) OnAdd(p2);
            }

            Dictionary<Node, Node> node = new Dictionary<Node, Node>(other.Boundary.Nodes.Count);
            foreach (Node n in other.Boundary.Nodes)
            {
                Node n2 = n.Clone();
                node.Add(n, n2);

                Boundary.Nodes.Add(n2);
                if (OnAdd != null) OnAdd(n2);
            }

            foreach (Wall w in other.Boundary.Walls)
                Boundary.Walls.Add(new Wall(node[w.A], node[w.B]));

            foreach (Teleporter t in other.Teleporters)
            {
                Teleporter t2 = t.Clone();
                Teleporters.Add(t2);

                if (OnAdd != null)
                {
                    OnAdd(t2.A);
                    OnAdd(t2.B);
                    OnAdd(t2.C);
                    OnAdd(t2.D);
                }
            }
        }

        public void RemoveObject(EditorObject obj)
        {
            if (obj is Body)
                Bodies.Remove((Body)obj);
            else if (obj is Node)
            {
                if (obj is Projector)
                    Projectors.Remove((Projector)obj);
                else
                {
                    Node n = (Node)obj;
                    Teleporter t = GetTeleporter(n);
                    if (t != null) Teleporters.Remove(t);
                    else Boundary.RemoveNode(n);
                }
            }
        }

        public Teleporter GetTeleporter(Node n)
        {
            foreach (Teleporter t in Teleporters)
                if (t.Contains(n)) return t;
            return null;
        }
        public bool IsTeleporter(Node n)
        {
            foreach (Teleporter t in Teleporters)
                if (t.Contains(n)) return true;
            return false;
        }

        public IEnumerable<EditorObject> Objects
        {
            get
            {
                for (int i = 0; i < Projectors.Count; i++)
                    yield return Projectors[i];

                for (int i = 0; i < Bodies.Count; i++)
                    yield return Bodies[i];

                for (int i = 0; i < Boundary.Nodes.Count; i++)
                    yield return Boundary.Nodes[i];

                for (int i = 0; i < Teleporters.Count; i++)
                {
                    Teleporter t = Teleporters[i];
                    yield return t.A;
                    yield return t.B;
                    yield return t.C;
                    yield return t.D;
                }
            }
        }
    }
    //simulation
    public partial class Universe
    {
        public void StepPhys(double totalTime, int steps)
        {
            lock (Lock)
            {
                double d = totalTime / steps;
                for (; steps > 0; steps--)
                    Tick(d);
            }
        }
        public void StepPhys(double totalTime, double stepTime)
        {
            lock (Lock)
            {
                for (; totalTime > stepTime; totalTime -= stepTime)
                    Tick(stepTime);
                Tick(totalTime);
            }
        }
        private void Tick(double time)
        {
            #region Body-Body Physics
            for (int i = 0; i < Bodies.Count - 1; i++)
                for (int v = i + 1; v < Bodies.Count; v++)
                {
                    Body a = Bodies[i], b = Bodies[v];

                    Vector2 delta = b.Position - a.Position;
                    Vector2 dir = delta.Unit;

                    if (a.Collidable && b.Collidable && delta.Magnitude < a.Radius + b.Radius)
                    {
                        if (Vector2.Dot(b.Velocity - a.Velocity, delta) < 0d)
                        {
                            Vector2 _dir = Vector2.FromPolar(dir.Theta - Math.PI / 2, 1);

                            double Ath = dir.Theta - a.Velocity.Theta;
                            double Amag = a.Velocity.Magnitude;
                            double Aix = Amag * Math.Sin(Ath);
                            double Aiy = Amag * Math.Cos(Ath);

                            double Bth = dir.Theta - b.Velocity.Theta;
                            double Bmag = b.Velocity.Magnitude;
                            double Bix = Bmag * Math.Sin(Bth);
                            double Biy = Bmag * Math.Cos(Bth);

                            double Yr = (a.Mass * Aiy + b.Mass * Biy) / (a.Mass + b.Mass);

                            double Aay = 2 * Yr - Aiy;
                            double Bay = 2 * Yr - Biy;

                            a.Velocity = dir * Aay + _dir * Aix;
                            b.Velocity = dir * Bay + _dir * Bix;
                        }
                    }
                    else
                    {
                        if (a.Significant || b.Significant)
                        {
                            double sqrMag = delta.SqrMag;
                            double imp = sqrMag > 0d ? time * (Settings.G * a.Mass * b.Mass + Settings.K * a.Charge * b.Charge) / sqrMag : 0d;

                            a.Velocity += dir * (imp / a.Mass);
                            b.Velocity -= dir * (imp / b.Mass);
                        }
                    }
                }
            #endregion

            #region BodyPhysics
            foreach (Body b in Bodies)
            {
                foreach (Projector p in Projectors)
                {
                    Vector2 f = p.GetForce(b.Position, b.Velocity, b.Mass);
                    b.Velocity += f * (time / b.Mass);
                }

                Vector2 d = b.Velocity * time;
                Vector2 nextPos = b.Position + d;

                foreach (Teleporter t in Teleporters)
                    if (t.Collide(b, b.Position, nextPos)) break;

                if (b.Collidable)
                {
                    bool bounced = false;
                    foreach (Wall w in Boundary.Walls)
                    {
                        bounced = w.Collide(b, b.Position, nextPos);
                        if (bounced) break;
                    }

                    if (!bounced)
                    {
                        b.Position += d;
                        b.Distance += d.Magnitude;
                    }
                }
                else
                {
                    b.Position += d;
                    b.Distance += d.Magnitude;
                }
            }
            #endregion
        }

        public Vector2 GetForces(Body body, double mass, double charge, Vector2 position, Vector2 velocity, bool significant)
        {
            Vector2 res = Vector2.Zero;

            foreach (Body other in Bodies)
            {
                if (other == body || (!significant && !other.Significant)) continue;

                Vector2 delta = other.Position - position;
                if (delta == Vector2.Zero) continue;

                double F = (Settings.G * mass * other.Mass + Settings.K * charge * other.Charge) / delta.SqrMag;

                res += delta.Unit * F;
            }

            foreach (Projector p in Projectors)
                res += p.GetForce(position, velocity, mass);

            return res;
        }

        public BackgroundWorker Predict(double time, int steps, int iter, Action<Vector2[][]> OnComplete)
        {
            Vector2[][] locs = new Vector2[Bodies.Count][];

            BackgroundWorker w = new BackgroundWorker();
            w.WorkerSupportsCancellation = true;
            w.WorkerReportsProgress = true;

            w.DoWork += (o, e) =>
            {
                Universe sim = Clone();
                int last = 0;

                for (int i = 0; i < locs.Length; i++)
                {
                    locs[i] = new Vector2[iter + 1];
                    locs[i][0] = sim.Bodies[i].Position;
                }

                for (int i = 1; i < iter + 1; i++)
                {
                    sim.StepPhys(time, steps);

                    for (int x = 0; x < Bodies.Count; x++)
                        locs[x][i] = sim.Bodies[x].Position;

                    if (w.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    int current = (int)Math.Floor(100d * i / iter);
                    if (current != last) w.ReportProgress(last = current);
                }
            };

            if (OnComplete != null)
                w.RunWorkerCompleted += (o, e) => OnComplete(e.Cancelled || e.Error != null ? null : locs);

            w.RunWorkerAsync();

            return w;
        }
        public BackgroundWorker Calculate(double time, int steps, int iter, Action<Universe> OnComplete)
        {
            Universe sim = Clone();

            BackgroundWorker w = new BackgroundWorker();
            w.WorkerSupportsCancellation = true;
            w.WorkerReportsProgress = true;

            w.DoWork += (o, e) =>
            {
                int last = 0;

                for (int i = 1; i < iter + 1; i++)
                {
                    sim.StepPhys(time, steps);

                    if (w.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    int current = (int)Math.Floor(100d * i / iter);
                    if (current != last) w.ReportProgress(last = current);
                }
            };

            if (OnComplete != null)
                w.RunWorkerCompleted += (o, e) => OnComplete(e.Cancelled || e.Error != null ? null : sim);

            w.RunWorkerAsync();

            return w;
        }
    }
    //drawing
    public partial class Universe
    {
        private static readonly Pen OrbitPen = new Pen(Color.Blue);
        public void Paint(PaintEventArgs e, Vector2[][] prediction = null)
        {
            lock (Lock)
            {
                foreach (Projector p in Projectors)
                    p.PaintField(e, Origin, Scale);
            }

            if (prediction != null)
                for (int i = 0; i < prediction.Length; i++)
                    for (int v = 0; v < prediction[i].Length - 1; v++)
                    {
                        PointF A = prediction[i][v] * Scale - Origin;
                        PointF B = prediction[i][v + 1] * Scale - Origin;

                        if (Utility.BindingRect(A, B).IntersectsWith(e.ClipRectangle)
                            && Utility.WithinDrawRange(A) && Utility.WithinDrawRange(B))
                            e.Graphics.DrawLine(OrbitPen, A, B);
                    }

            lock (Lock)
            {
                foreach (Projector p in Projectors)
                    p.Paint(e, Origin, Scale);

                foreach (Body b in Bodies)
                    b.Paint(e, Origin, Scale);

                Boundary.Paint(e, Origin, Scale);

                foreach (Teleporter t in Teleporters)
                    t.Paint(e, Origin, Scale);
            }
        }

        public EditorObject GetClicked(Point p, IEnumerable<EditorObject> priority)
        {
            EditorObject obj = null;
            bool prior = false;

            foreach (EditorObject o in Objects)
            {
                RectangleF clip = o.GetClip(Origin, Scale);
                if (!clip.Contains(p)) continue;

                Vector2 delta = new Vector2(
                    clip.Location.X + clip.Width / 2d - p.X,
                    clip.Location.Y + clip.Height / 2d - p.Y);
                if (delta.Magnitude > clip.Width / 2d) continue;

                bool _priority = priority.Contains(o);
                if(!prior || _priority) obj = o;
                if (_priority) prior = true;
            }

            return obj;
        }
    }
    #endregion

    #region Body
    //core
    public sealed partial class Body : EditorObject
    {
        public string Name = "Unnamed";

        public double Mass, Charge, Radius, Distance = 0d;

        public bool Significant = true, Collidable = true;

        [XmlElement(ElementName = "Velocity")]
        public Vector2 _Velocity
        {
            get
            {
                return new Vector2(Velocity.X, -Velocity.Y);
            }
            set
            {
                Velocity = new Vector2(value.X, -value.Y);
            }
        }

        [XmlIgnore]
        public Vector2 Velocity;

        public Body(double mass, double charge, double radius, Vector2 pos, Vector2 vel)
        {
            Mass = mass;
            Charge = charge;
            Radius = radius;

            Position = pos;
            Velocity = vel;
        }
        public Body() : this(1d, 0d, 5d, Vector2.Zero, Vector2.Zero) { }

        public Body Clone()
        {
            Body b = new Body(Mass, Charge, Radius, Position, Velocity);

            b.Name = Name;

            b.Significant = Significant;
            b.Collidable = Collidable;

            b.Distance = Distance;

            b.Icon = Icon;

            b.Brush = new SolidBrush(Brush.Color);
            b.IconBrush = new SolidBrush(IconBrush.Color);

            return b;
        }
    }
    //drawing
    public partial class Body
    {
        public BodyIcon Icon = BodyIcon.None;

        private SolidBrush Brush = new SolidBrush(Color.Black), IconBrush = new SolidBrush(Color.White);

        [XmlIgnore]
        public Color Color
        {
            get
            {
                return Brush.Color;
            }
            set
            {
                Brush.Color = value;
            }
        }
        [XmlIgnore]
        public Color IconColor
        {
            get
            {
                return IconBrush.Color;
            }
            set
            {
                IconBrush.Color = value;
            }
        }

        [XmlElement(ElementName = "Color")]
        public string _BackColor
        {
            get
            {
                return Utility.ColorToString(Color);
            }
            set
            {
                Color = Utility.ColorFromString(value);
            }
        }
        [XmlElement(ElementName = "IconColor")]
        public string _IconColor
        {
            get
            {
                return Utility.ColorToString(IconColor);
            }
            set
            {
                IconColor = Utility.ColorFromString(value);
            }
        }

        public override RectangleF GetClip(Vector2 orig, double scale)
        {
            double r = Math.Max(Radius * scale, Settings.MinRadius);
            return new RectangleF(
                (float)(Position.X * scale - r - orig.X),
                (float)(Position.Y * scale - r - orig.Y),
                (float)(2 * r),
                (float)(2 * r)
                );
        }

        public override void Paint(PaintEventArgs e, Vector2 orig, double scale)
        {
            RectangleF rect = GetClip(orig, scale);
            if (!rect.IntersectsWith(e.ClipRectangle)) return;

            if (Utility.WithinDrawRange(rect))
                e.Graphics.FillEllipse(Brush, rect);

            #region Icons
            switch (Icon)
            {
                case BodyIcon.Minus:
                    e.Graphics.FillRectangle(IconBrush,
                        rect.X + rect.Size.Width * .15f,
                        rect.Y + rect.Size.Height * .4f,
                        rect.Size.Width * .7f,
                        rect.Size.Height * .2f);
                    break;
                case BodyIcon.Plus:
                    e.Graphics.FillRectangle(IconBrush,
                        rect.X + rect.Size.Width * .15f,
                        rect.Y + rect.Size.Height * .4f,
                        rect.Size.Width * .7f,
                        rect.Size.Height * .2f);
                    e.Graphics.FillRectangle(IconBrush,
                        rect.X + rect.Size.Width * .4f,
                        rect.Y + rect.Size.Height * .15f,
                        rect.Size.Width * .2f,
                        rect.Size.Height * .7f);
                    break;
            }
            #endregion
        }
    }
    //utility
    public partial class Body
    {
        public BodyDialog GetEditor()
        {
            BodyDialog d = new BodyDialog();

            d.Name = Name;
            d.Mass = Mass;
            d.Charge = Charge;
            d.Radius = Radius;

            d.Velocity = Velocity;
            d.Position = Position;

            d.Color = Color;
            d.IconColor = IconColor;

            d.BodyIcon = Icon;

            d.Distance = Distance;

            d.Significant = Significant;
            d.Collidable = Collidable;

            return d;
        }
        public void Import(BodyDialog d)
        {
            Name = d.Name;
            Mass = d.Mass;
            Charge = d.Charge;
            Radius = d.Radius;

            Velocity = d.Velocity;
            Position = d.Position;

            Color = d.Color;
            IconColor = d.IconColor;

            Icon = d.BodyIcon;

            Distance = d.Distance;

            Significant = d.Significant;
            Collidable = d.Collidable;
        }
    }
    #endregion

    public class TapeMeasure
    {
        public Vector2 Start, End;

        public TapeMeasure(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
        }
        public TapeMeasure() : this(Vector2.Zero, Vector2.Zero) { }

        public double Distance
        {
            get
            {
                return (End - Start).Magnitude;
            }
        }
    }

    public sealed class Timeline
    {
        private const int MaxSnapshots = 500;

        private Universe Initial = null;
        private List<Universe> Snapshots = new List<Universe>(MaxSnapshots);
        private int Index = -1;

        public Action UpToDateChanged = null;

        private bool _UpToDate = false;
        public bool UpToDate
        {
            get
            {
                return _UpToDate;
            }
            set
            {
                _UpToDate = value;
                if (UpToDateChanged != null) UpToDateChanged();
            }
        }

        public void Initialize(Universe u)
        {
            Initial = u.Clone();
            Snapshots.Clear();
            Snap(u, true);
            UpToDate = true;
        }

        public void Snap(Universe u, bool background = false)
        {
            for (int i = Snapshots.Count - 1; i > Index; i--)
                Snapshots.RemoveAt(i);

            if (Snapshots.Count == MaxSnapshots)
                Snapshots.RemoveAt(0);

            Snapshots.Add(u.Clone());
            Index = Snapshots.Count - 1;

            if (!background) UpToDate = false;
        }

        private void Load(ref Universe u)
        {
            Universe n = Snapshots[Index].Clone();

            n.Origin = u.Origin;
            n.Scale = u.Scale;

            u = n;

            UpToDate = false;
        }

        public bool Undo(ref Universe u)
        {
            if (Index <= 0) return false;

            Index--;
            Load(ref u);

            return true;
        }

        public bool Redo(ref Universe u)
        {
            if (Index >= Snapshots.Count - 1) return false;

            Index++;
            Load(ref u);

            return true;
        }

        public void Revert(ref Universe u)
        {
            Universe n = Initial.Clone();

            n.Origin = u.Origin;
            n.Scale = u.Scale;

            u = n;

            UpToDate = false;
        }
    }

    public class Node : EditorObject
    {
        private const double Radius = 5d;

        public Node(Vector2 pos)
        {
            Position = pos;
        }
        public Node() : this(Vector2.Zero) { }

        public override RectangleF GetClip(Vector2 origin, double scale)
        {
            return new RectangleF(
                (float)(Position.X * scale - origin.X - Radius),
                (float)(Position.Y * scale - origin.Y - Radius),
                (float)(2 * Radius), (float)(2 * Radius));
        }

        private SolidBrush Brush = new SolidBrush(Color.Orange);
        [XmlIgnore]
        public Color Color
        {
            get
            {
                return Brush.Color;
            }
            set
            {
                Brush.Color = value;
            }
        }

        public override void Paint(PaintEventArgs e, Vector2 origin, double scale)
        {
            RectangleF rect = GetClip(origin, scale);
            if (!rect.IntersectsWith(e.ClipRectangle)) return;

            e.Graphics.FillEllipse(Brush, rect);
        }

        private static uint _id = 0;
        public uint Id = _id++;

        public NodeDialog GetEditor()
        {
            NodeDialog d = new NodeDialog();

            d.Position = Position;

            return d;
        }
        public void Import(NodeDialog d)
        {
            Position = d.Position;
        }

        public Node Clone()
        {
            Node n = new Node(Position);
            n.Color = Color;

            return n;
        }
    }

    public sealed class Wall
    {
        [XmlIgnore]
        public Node A, B;

        [XmlAttribute(AttributeName = "A")]
        public uint _A
        {
            get
            {
                return A.Id;
            }
            set
            {
                if (A == null) A = new Node();
                A.Id = value;
            }
        }

        [XmlAttribute(AttributeName = "B")]
        public uint _B
        {
            get
            {
                return B.Id;
            }
            set
            {
                if (B == null) B = new Node();
                B.Id = value;
            }
        }

        public Wall(Node a, Node b)
        {
            A = a;
            B = b;
        }
        public Wall() : this(null, null) { }

        private static readonly Pen Pen = new Pen(Color.Orange, 4f);
        public void Paint(PaintEventArgs e, Vector2 origin, double scale)
        {
            PointF a, b;
            if (!Utility.WithinDrawRange(a = A.Position * scale - origin)
                || !Utility.WithinDrawRange(b = B.Position * scale - origin)) return;

            e.Graphics.DrawLine(Pen, a, b);
        }

        public bool Contains(Node n)
        {
            return A == n || B == n;
        }

        public bool Collide(Body body, Vector2 posA, Vector2 posB)
        {
            Vector2 dir = B.Position - A.Position;
            double phi = dir.Theta;

            double length = dir.Magnitude;
            dir = dir.Unit;

            Vector2 n_dir = Vector2.FromPolar(phi - Math.PI / 2d, 1d);
            double x = Vector2.Dot(dir, posA - A.Position);

            if (Vector2.Dot(n_dir, posA - A.Position) * Vector2.Dot(n_dir, posB - A.Position) < 0d
               && x > 0d && x < length)
            {
                double Vmag = body.Velocity.Magnitude;
                double theta = (posB - posA).Theta - phi;

                double Vx = Vmag * Math.Cos(theta);
                double Vy = Vmag * Math.Sin(theta);

                body.Velocity = dir * Vx + n_dir * Vy;

                return true;
            }
            return false;
        }
    }

    public sealed class Boundary
    {
        public List<Node> Nodes = new List<Node>();
        public List<Wall> Walls = new List<Wall>();

        public void Paint(PaintEventArgs e, Vector2 origin, double scale)
        {
            for (int i = 0; i < Walls.Count; i++)
                Walls[i].Paint(e, origin, scale);

            for (int i = 0; i < Nodes.Count; i++)
                Nodes[i].Paint(e, origin, scale);
        }

        public Boundary Clone()
        {
            Boundary b = new Boundary();

            Dictionary<Node, Node> d = new Dictionary<Node, Node>(Nodes.Count);

            for (int i = 0; i < Nodes.Count; i++)
            {
                Node n = Nodes[i].Clone();
                b.Nodes.Add(n);

                d[Nodes[i]] = n;
            }

            for (int i = 0; i < Walls.Count; i++)
                b.Walls.Add(new Wall(d[Walls[i].A], d[Walls[i].B]));

            return b;
        }

        public bool RemoveNode(Node n)
        {
            if (!Nodes.Remove(n)) return false;

            for (int i = Walls.Count - 1; i >= 0; i--)
                if (Walls[i].Contains(n)) Walls.RemoveAt(i);

            return true;
        }

        public void Initialize()
        {
            for (int i = 0; i < Walls.Count; i++)
            {
                bool a = false, b = false;
                for (int v = 0; v < Nodes.Count; v++)
                {
                    if (!a && Walls[i].A.Id == Nodes[v].Id)
                    {
                        a = true;
                        Walls[i].A = Nodes[v];
                        continue;
                    }
                    if (!b && Walls[i].B.Id == Nodes[v].Id)
                    {
                        b = true;
                        Walls[i].B = Nodes[v];
                        continue;
                    }
                }
                if (!a || !b) throw new FormatException("File contains corrupt Boundary data");
            }
        }
    }

    public enum ProjectorShape
    {
        Circle,
        Square
    }
    public enum ProjectorMode
    {
        Accelerate,
        Force
    }
    public enum ProjectorFalloff
    {
        Constant,
        Quadratic
    }
    public enum ProjectorType
    {
        Linear,
        Centrifugal,
        Centripetal,
        Acceleration,
        Deceleration
    }
    public enum ProjectorSpeedMode
    {
        Constant,
        Quadratic
    }

    public sealed class ProjectorForce
    {
        public ProjectorShape Shape;
        public double Radius;

        public ProjectorMode Mode;
        public ProjectorType Type;
        public ProjectorFalloff Falloff;
        public ProjectorSpeedMode SpeedMode;

        [XmlIgnore]
        public Vector2 Vector;
        [XmlElement(ElementName = "Vector")]
        public Vector2 _Vector
        {
            get
            {
                return new Vector2(Vector.X, -Vector.Y);
            }
            set
            {
                Vector = new Vector2(value.X, -value.Y);
            }
        }

        public ProjectorForce(ProjectorShape shape, double radius,
            ProjectorMode mode, ProjectorType type, ProjectorFalloff falloff, ProjectorSpeedMode speedMode, Vector2 vector)
        {
            Shape = shape;
            Radius = radius;
            Mode = mode;
            Type = type;
            Falloff = falloff;
            SpeedMode = speedMode;
            Vector = vector;
        }
        public ProjectorForce() : this(ProjectorShape.Circle, 20d,
            ProjectorMode.Accelerate, ProjectorType.Centrifugal, ProjectorFalloff.Quadratic, ProjectorSpeedMode.Constant, Vector2.Zero)
        { }

        public bool WithinField(Projector parent, Vector2 position)
        {
            Vector2 dif = position - parent.Position;
            if (Shape == ProjectorShape.Square)
                return Math.Abs(dif.X) < Radius && Math.Abs(dif.Y) < Radius;
            else
                return dif.Magnitude < Radius;
        }

        public Vector2 GetForce(Projector parent, Vector2 position, Vector2 velocity, double mass)
        {
            Vector2 dir = parent.Position - position;
            double mult = Projector.GetMultiplier(Falloff, SpeedMode, dir.Magnitude, velocity.Magnitude);

            switch (Type)
            {
                case ProjectorType.Linear:
                    return Mode == ProjectorMode.Accelerate ?
                        (mass * mult) * Vector :
                        mult * Vector;
                case ProjectorType.Centrifugal:
                    if (dir == Vector2.Zero) return Vector2.Zero;
                    return Mode == ProjectorMode.Accelerate ?
                        -(mass * mult * Vector.Magnitude) * dir.Unit :
                        -(mult * Vector.Magnitude) * dir.Unit;
                case ProjectorType.Centripetal:
                    if (dir == Vector2.Zero) return Vector2.Zero;
                    return Mode == ProjectorMode.Accelerate ?
                        (mass * mult * Vector.Magnitude) * dir.Unit :
                        (mult * Vector.Magnitude) * dir.Unit;
                case ProjectorType.Acceleration:
                    if (velocity == Vector2.Zero) return Vector2.Zero;
                    return Mode == ProjectorMode.Accelerate ?
                        (mass * mult * Vector.Magnitude) * velocity.Unit :
                        (mult * Vector.Magnitude) * velocity.Unit;
                case ProjectorType.Deceleration:
                    if (velocity == Vector2.Zero) return Vector2.Zero;
                    return Mode == ProjectorMode.Accelerate ?
                        -(mass * mult * Vector.Magnitude) * velocity.Unit :
                        -(mult * Vector.Magnitude) * velocity.Unit;
                default: throw new ArgumentException("Unknown force type");
            }
        }

        private Pen Pen = new Pen(Color.AliceBlue, 4f);
        [XmlIgnore]
        public Color Color
        {
            get
            {
                return Pen.Color;
            }
            set
            {
                Pen.Color = value;
            }
        }
        [XmlElement(ElementName = "Color")]
        public string _Color
        {
            get
            {
                return Utility.ColorToString(Pen.Color);
            }
            set
            {
                Pen.Color = Utility.ColorFromString(value);
            }
        }

        public void Paint(PaintEventArgs e, Projector parent, Vector2 origin, double scale)
        {
            RectangleF field = new RectangleF(
                    (float)(parent.Position.X * scale - origin.X - Radius * scale),
                    (float)(parent.Position.Y * scale - origin.Y - Radius * scale),
                    (float)(2d * Radius * scale),
                    (float)(2d * Radius * scale));

            if (Utility.WithinDrawRange(field))
                if (Shape == ProjectorShape.Square)
                    e.Graphics.DrawRectangle(Pen, Rectangle.Round(field));
                else e.Graphics.DrawEllipse(Pen, field);
        }

        public ProjectorForce Clone()
        {
            ProjectorForce f = new ProjectorForce(Shape, Radius, Mode, Type, Falloff, SpeedMode, Vector);
            f.Color = Color;

            return f;
        }

        public ProjectorForceEntry GetEditor()
        {
            ProjectorForceEntry d = new ProjectorForceEntry();

            d.Radius = Radius;

            d.Shape = Shape;
            d.Mode = Mode;
            d.Type = Type;
            d.Falloff = Falloff;
            d.SpeedMode = SpeedMode;

            d.Vector = Vector;
            d.Color = Color;

            return d;
        }

        public void Import(ProjectorForceEntry d)
        {
            Radius = d.Radius;

            Shape = d.Shape;
            Mode = d.Mode;
            Type = d.Type;
            Falloff = d.Falloff;
            SpeedMode = d.SpeedMode;

            Vector = d.Vector;
            Color = d.Color;
        }
    }

    public sealed class Projector : Node
    {
        private const double Radius = 5d;
        public static double GetMultiplier(ProjectorFalloff falloff, ProjectorSpeedMode speedMode, double distance, double speed)
        {
            if (falloff == ProjectorFalloff.Constant)
            {
                if (speedMode == ProjectorSpeedMode.Constant) return 1d;
                else if (speedMode == ProjectorSpeedMode.Quadratic) return speed * speed;
            }
            else if (falloff == ProjectorFalloff.Quadratic)
            {
                if (speedMode == ProjectorSpeedMode.Constant) return distance > 0d ? 1d / (distance * distance) : 0d;
                else if (speedMode == ProjectorSpeedMode.Quadratic) return distance > 0d ? (speed * speed) / (distance * distance) : 0d;
            }

            throw new ArgumentException("Unknown force settings");
        }

        public List<ProjectorForce> Forces = new List<ProjectorForce>();

        public Projector(Vector2 pos) : base(pos) { }
        public Projector() : base() { }

        public Vector2 GetForce(Vector2 position, Vector2 velocity, double mass)
        {
            Vector2 res = Vector2.Zero;
            foreach (ProjectorForce f in Forces)
                if (f.WithinField(this, position)) res += f.GetForce(this, position, velocity, mass);

            return res;
        }

        [XmlElement(ElementName = "Color")]
        public string _Color
        {
            get
            {
                return Utility.ColorToString(Color);
            }
            set
            {
                Color = Utility.ColorFromString(value);
            }
        }

        public void PaintField(PaintEventArgs e, Vector2 origin, double scale)
        {
            foreach (ProjectorForce f in Forces)
                f.Paint(e, this, origin, scale);
        }

        public new Projector Clone()
        {
            Projector p = new Projector(Position);
            foreach (ProjectorForce f in Forces) p.Forces.Add(f.Clone());

            p.Color = Color;

            return p;
        }

        public new ProjectorDialog GetEditor()
        {
            ProjectorDialog d = new ProjectorDialog();

            d.Position = Position;
            d.Color = Color;

            foreach (ProjectorForce f in Forces)
                d.AddEntry(f.GetEditor());

            return d;
        }

        public void Import(ProjectorDialog d)
        {
            Position = d.Position;
            Color = d.Color;

            Forces.Clear();
            foreach (ProjectorForceEntry e in d.Entries)
            {
                ProjectorForce f = new ProjectorForce();
                f.Import(e);
                Forces.Add(f);
            }
        }
    }

    public sealed class Teleporter
    {
        private Node __A, __B, __C, __D;

        [XmlIgnore]
        public Node A
        {
            get
            {
                return __A;
            }
            set
            {
                __A = value;
                if (__A != null) __A.Color = Color;
            }
        }
        [XmlIgnore]
        public Node B
        {
            get
            {
                return __B;
            }
            set
            {
                __B = value;
                if (__B != null) __B.Color = Color;
            }
        }
        [XmlIgnore]
        public Node C
        {
            get
            {
                return __C;
            }
            set
            {
                __C = value;
                if (__C != null) __C.Color = Color;
            }
        }
        [XmlIgnore]
        public Node D
        {
            get
            {
                return __D;
            }
            set
            {
                __D = value;
                if (__D != null) __D.Color = Color;
            }
        }

        [XmlElement(ElementName = "A")]
        public Vector2 _A
        {
            get
            {
                return A.Position;
            }
            set
            {
                if (A == null) A = new Node(value);
                else A.Position = value;
            }
        }
        [XmlElement(ElementName = "B")]
        public Vector2 _B
        {
            get
            {
                return B.Position;
            }
            set
            {
                if (B == null) B = new Node(value);
                else B.Position = value;
            }
        }
        [XmlElement(ElementName = "C")]
        public Vector2 _C
        {
            get
            {
                return C.Position;
            }
            set
            {
                if (C == null) C = new Node(value);
                else C.Position = value;
            }
        }
        [XmlElement(ElementName = "D")]
        public Vector2 _D
        {
            get
            {
                return D.Position;
            }
            set
            {
                if (D == null) D = new Node(value);
                else D.Position = value;
            }
        }

        public Vector2 RightAB
        {
            get
            {
                if (A.Position == B.Position) return Vector2.Zero;
                return (B.Position - A.Position).Unit;
            }
        }
        public Vector2 ForwardAB
        {
            get
            {
                if (A.Position == B.Position) return Vector2.Zero;
                return Vector2.FromPolar(RightAB.Theta - Math.PI / 2d, 1d);
            }
        }
        public double LengthAB
        {
            get
            {
                return (B.Position - A.Position).Magnitude;
            }
        }

        public Vector2 RightCD
        {
            get
            {
                if (C.Position == D.Position) return Vector2.Zero;
                return (D.Position - C.Position).Unit;
            }
        }
        public Vector2 ForwardCD
        {
            get
            {
                if (C.Position == D.Position) return Vector2.Zero;
                return Vector2.FromPolar(RightCD.Theta - Math.PI / 2d, 1d);
            }
        }
        public double LengthCD
        {
            get
            {
                return (D.Position - C.Position).Magnitude;
            }
        }

        public Teleporter(Node a, Node b, Node c, Node d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }
        public Teleporter() : this(null, null, null, null) { }

        private Pen WallPen = new Pen(Color.Blue, 4f), ArrowPen = new Pen(Color.Blue, 4f) { EndCap = LineCap.ArrowAnchor };
        [XmlIgnore]
        public Color Color
        {
            get
            {
                return WallPen.Color;
            }
            set
            {
                WallPen.Color = value;
                ArrowPen.Color = value;

                if (A != null) A.Color = value;
                if (B != null) B.Color = value;
                if (C != null) C.Color = value;
                if (D != null) D.Color = value;
            }
        }
        [XmlElement(ElementName = "Color")]
        public string _Color
        {
            get
            {
                return Utility.ColorToString(Color);
            }
            set
            {
                Color = Utility.ColorFromString(value);
            }
        }

        private const double ArrowStart = 5d, ArrowEnd = 25d;
        public void Paint(PaintEventArgs e, Vector2 origin, double scale)
        {
            PointF a = A.Position * scale - origin;
            PointF b = B.Position * scale - origin;
            if (Utility.WithinDrawRange(a) && Utility.WithinDrawRange(b))
            {
                e.Graphics.DrawLine(WallPen, a, b);
                A.Paint(e, origin, scale);
                B.Paint(e, origin, scale);

                Vector2 m = (A.Position + B.Position) / 2d;
                Vector2 f = ForwardAB;

                PointF x = m * scale - origin + f * ArrowStart;
                PointF y = m * scale - origin + f * ArrowEnd;

                e.Graphics.DrawLine(ArrowPen, x, y);
            }

            PointF c = C.Position * scale - origin;
            PointF d = D.Position * scale - origin;
            if (Utility.WithinDrawRange(c) && Utility.WithinDrawRange(d))
            {
                e.Graphics.DrawLine(WallPen, c, d);
                C.Paint(e, origin, scale);
                D.Paint(e, origin, scale);

                Vector2 m = (C.Position + D.Position) / 2d;
                Vector2 f = ForwardCD;

                PointF x = m * scale - origin + f * ArrowStart;
                PointF y = m * scale - origin + f * ArrowEnd;

                e.Graphics.DrawLine(ArrowPen, x, y);
            }
        }

        public bool Contains(Node n)
        {
            return A == n || B == n || C == n || D == n;
        }

        private bool CollideAB(Body body, Vector2 posA, Vector2 posB)
        {
            Vector2 r = RightAB;
            Vector2 f = ForwardAB;
            double length = LengthAB;

            double x = Vector2.Dot(r, posA - A.Position);

            if (Vector2.Dot(f, posA - A.Position) * Vector2.Dot(f, posB - A.Position) < 0d
               && x > 0d && x < length)
            {
                double Vmag = body.Velocity.Magnitude;
                double theta = (posB - posA).Theta - r.Theta;

                double Vx = -Vmag * Math.Cos(theta);
                double Vy = Vmag * Math.Sin(theta);

                Vector2 r2 = RightCD;
                Vector2 f2 = ForwardCD;
                double length2 = LengthCD;

                x *= length > 0 && length2 > 0 ? length2 / length : 0d;

                body.Position = D.Position - x * r2;
                body.Velocity = r2 * Vx + f2 * Vy;

                return true;
            }
            return false;
        }

        private bool CollideCD(Body body, Vector2 posA, Vector2 posB)
        {
            Vector2 r = RightCD;
            Vector2 f = ForwardCD;
            double length = LengthCD;

            double x = Vector2.Dot(r, posA - C.Position);

            if (Vector2.Dot(f, posA - C.Position) * Vector2.Dot(f, posB - C.Position) < 0d
               && x > 0d && x < length)
            {
                double Vmag = body.Velocity.Magnitude;
                double theta = (posB - posA).Theta - r.Theta;

                double Vx = -Vmag * Math.Cos(theta);
                double Vy = Vmag * Math.Sin(theta);

                Vector2 r2 = RightAB;
                Vector2 f2 = ForwardAB;
                double length2 = LengthAB;

                x *= length > 0 && length2 > 0 ? length2 / length : 0d;

                body.Position = B.Position - x * r2;
                body.Velocity = r2 * Vx + f2 * Vy;

                return true;
            }
            return false;
        }

        public bool Collide(Body body, Vector2 posA, Vector2 posB)
        {
            return CollideAB(body, posA, posB) || CollideCD(body, posA, posB);
        }

        public Teleporter Clone()
        {
            Teleporter t = new Teleporter();

            t.A = A.Clone();
            t.B = B.Clone();
            t.C = C.Clone();
            t.D = D.Clone();

            t.Color = Color;

            return t;
        }

        public TeleporterDialog GetEditor()
        {
            TeleporterDialog d = new TeleporterDialog();

            d.A = A.Position;
            d.B = B.Position;
            d.C = C.Position;
            d.D = D.Position;

            d.Color = Color;

            return d;
        }

        public void Import(TeleporterDialog d)
        {
            A.Position = d.A;
            B.Position = d.B;
            C.Position = d.C;
            D.Position = d.D;

            Color = d.Color;
        }
    }
}