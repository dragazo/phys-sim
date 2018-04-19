using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace PhysSim
{
    public sealed partial class Player : Form
    {
        private Universe U = new Universe();
        private List<EditorObject> Selection = new List<EditorObject>();

        private ContextMenuStrip Context = new ContextMenuStrip(),
            BodyContext = new ContextMenuStrip(),
            NodeContext = new ContextMenuStrip(),
            ProjectorContext = new ContextMenuStrip(),
            TeleporterContext = new ContextMenuStrip();

        private EditorObject ContextObject;
        private Point ContextPoint;

        private FileInfo WorkingFile = null;

        private Body Following = null;

        private const string TitleTextFormat =
#if DEBUG
            "Physics Simulator <DEBUG MODE> - {1}{0}";
#else
        "Physics Simulator - {1}{0}";
#endif

        public Player()
        {
            InitializeComponent();

            U.Bodies.Add(new Body() { Position = new Vector2(200, 200) });

            BackColor = Color.White;
            WorkingFile = null;

            #region Context
            Context.Items.Add(new ToolStripMenuItem("Add", null,
                new ToolStripMenuItem("Body", null, (o, e) =>
                {
                    Body b = new Body() { Position = ((Vector2)ContextPoint + U.Origin) / U.Scale };
                    U.Bodies.Add(b);

                    Timeline.Snap(U);

                    Invalidate();
                }),
                new ToolStripMenuItem("Node", null, (o, e) =>
                {
                    Node n = new Node() { Position = ((Vector2)ContextPoint + U.Origin) / U.Scale };
                    U.Boundary.Nodes.Add(n);

                    Timeline.Snap(U);

                    Invalidate();
                }),
                new ToolStripMenuItem("Projector", null, (o, e) =>
                {
                    Projector p = new Projector() { Position = ((Vector2)ContextPoint + U.Origin) / U.Scale };
                    U.Projectors.Add(p);

                    Timeline.Snap(U);

                    Invalidate();
                }),
                new ToolStripMenuItem("Teleporter", null, (o, e) =>
                {
                    Vector2 m = ((Vector2)ContextPoint + U.Origin) / U.Scale;
                    Teleporter t = new Teleporter()
                    {
                        A = new Node(m + new Vector2(0, 100) / U.Scale),
                        B = new Node(m),
                        C = new Node(m + new Vector2(100, 0) / U.Scale),
                        D = new Node(m + new Vector2(100, 100) / U.Scale)
                    };

                    U.Teleporters.Add(t);

                    Timeline.Snap(U);

                    Invalidate();
                })
            ));
            Context.Items.Add("Remove", null, (o, e) =>
            {
                for (int i = 0; i < Selection.Count; i++)
                    U.RemoveObject(Selection[i]);

                Selection.Clear();

                Timeline.Snap(U);

                Invalidate();
            });
            Context.Items.Add("Predict", null, (o, e) =>
            {
                if (!PredictionSetup()) return;

                ProgressDisplay.Show(U.Predict(SimTime, SimSteps, SimIter, p =>
                {
                    if (p == null) return;

                    Prediction = p;
                    Invalidate();
                }), "Predicting Trajectories");
            });
            Context.Items.Add("Discard Prediction", null, (o, e) =>
            {
                Prediction = null;

                Invalidate();
            });
            #endregion

            #region BodyContext
            BodyContext.Items.Add("Edit", null, (o, e) =>
            {
                Body c = (Body)ContextObject;

                BodyDialog d = c.GetEditor();
                d.Universe = U;
                d.Body = c;
                d.RecalculateForces();

                if (d.ShowDialog() == DialogResult.OK)
                {
                    c.Import(d);

                    Timeline.Snap(U);

                    Invalidate();
                }
                d.Dispose();
            });
            BodyContext.Items.Add("Clone", null, (o, e) =>
            {
                Body c = (Body)ContextObject;

                Body b = c.Clone();
                b.Position = ((Vector2)PointToClient(MousePosition) + U.Origin) / U.Scale;

                U.Bodies.Add(b);

                Timeline.Snap(U);

                Invalidate();
            });
            BodyContext.Items.Add("Remove", null, (o, e) =>
            {
                if (Selection.Contains(ContextObject))
                {
                    for (int i = 0; i < Selection.Count; i++)
                        U.RemoveObject(Selection[i]);

                    Selection.Clear();
                }
                else
                {
                    U.RemoveObject(ContextObject);
                    Selection.Remove(ContextObject);
                }

                Timeline.Snap(U);

                Invalidate();
            });
            BodyContext.Items.Add("Relative Prediction", null, (o, e) =>
            {
                if (!PredictionSetup()) return;

                Body c = (Body)ContextObject;

                int pos = 0;
                for (; pos < U.Bodies.Count; pos++)
                    if (U.Bodies[pos] == c) break;

                ProgressDisplay.Show(U.Predict(SimTime, SimSteps, SimIter, p =>
                {
                    if (p == null) return;

                    Vector2 orig = p[pos][0];
                    for (int i = 0; i < p.Length; i++)
                    {
                        if (i == pos) continue;
                        for (int v = 0; v < p[i].Length; v++)
                            p[i][v] = p[i][v] - p[pos][v] + orig;
                    }

                    for (int i = 0; i < p[pos].Length; i++)
                        p[pos][i] = orig;

                    Prediction = p;
                    Invalidate();
                }), "Predicting Relative Trajectories");
            });
            BodyContext.Items.Add("Follow", null, (o, e) =>
            {
                Following = (Body)ContextObject;

                U.Origin = Following.Position * U.Scale - 0.5d * (Vector2)Size;
                Invalidate();
            });
            #endregion

            #region NodeContext
            NodeContext.Items.Add("Edit", null, (o, e) =>
            {
                Node n = (Node)ContextObject;

                NodeDialog d = n.GetEditor();
                if (d.ShowDialog() == DialogResult.OK)
                {
                    n.Import(d);

                    Timeline.Snap(U);

                    Invalidate();
                }
                d.Dispose();
            });
            NodeContext.Items.Add("Remove", null, (o, e) =>
            {
                if (Selection.Contains(ContextObject))
                {
                    for (int i = 0; i < Selection.Count; i++)
                        U.RemoveObject(Selection[i]);

                    Selection.Clear();
                }
                else
                {
                    U.RemoveObject(ContextObject);
                    Selection.Remove(ContextObject);
                }

                Timeline.Snap(U);

                Invalidate();
            });
            NodeContext.Items.Add("Select Wall", null, (o, e) =>
            {
                Node start = (Node)ContextObject;
                List<Node> group = new List<Node>() { start };
                int added;
                do
                {
                    added = 0;
                    foreach (Wall w in U.Boundary.Walls.Where(w => group.Contains(w.A) || group.Contains(w.B)))
                    {
                        if (!group.Contains(w.A))
                        {
                            group.Add(w.A);
                            added++;
                        }
                        if (!group.Contains(w.B))
                        {
                            group.Add(w.B);
                            added++;
                        }
                    }
                } while (added > 0);

                foreach (Node n in group)
                    if (!Selection.Contains(n)) Selection.Add(n);

                Invalidate();
            });
            #endregion

            #region ProjectorContext
            ProjectorContext.Items.Add("Edit", null, (o, e) =>
            {
                Projector c = (Projector)ContextObject;

                ProjectorDialog d = c.GetEditor();

                if (d.ShowDialog() == DialogResult.OK)
                {
                    c.Import(d);

                    Timeline.Snap(U);

                    Invalidate();
                }

                d.Dispose();
            });
            ProjectorContext.Items.Add("Remove", null, (o, e) =>
            {
                if (Selection.Contains(ContextObject))
                {
                    for (int i = 0; i < Selection.Count; i++)
                        U.RemoveObject(Selection[i]);

                    Selection.Clear();
                }
                else
                {
                    U.RemoveObject(ContextObject);
                    Selection.Remove(ContextObject);
                }

                Timeline.Snap(U);

                Invalidate();
            });
            ProjectorContext.Items.Add("Clone", null, (o, e) =>
            {
                Projector c = (Projector)ContextObject;

                Projector b = c.Clone();
                b.Position = ((Vector2)PointToClient(MousePosition) + U.Origin) / U.Scale;

                U.Projectors.Add(b);

                Timeline.Snap(U);

                Invalidate();
            });
            #endregion

            #region TeleporterContext
            TeleporterContext.Items.Add("Edit", null, (o, e) =>
            {
                Teleporter t = U.GetTeleporter((Node)ContextObject);
                if (t == null) return;

                TeleporterDialog d = t.GetEditor();

                if (d.ShowDialog() == DialogResult.OK)
                {
                    t.Import(d);

                    Timeline.Snap(U);

                    Invalidate();
                }

                d.Dispose();
            });
            TeleporterContext.Items.Add("Remove", null, (o, e) =>
            {
                if (Selection.Contains(ContextObject))
                {
                    for (int i = 0; i < Selection.Count; i++)
                        U.RemoveObject(Selection[i]);

                    Selection.Clear();
                }
                else
                {
                    U.RemoveObject(ContextObject);
                    Selection.Remove(ContextObject);
                }

                Timeline.Snap(U);

                Invalidate();
            });
            #endregion

            Timeline.UpToDateChanged += () =>
            {
                Text = string.Format(TitleTextFormat,
                    WorkingFile != null ? WorkingFile.FullName : "Untitled",
                    Timeline.UpToDate ? "" : "*");
            };

            Timeline.Initialize(U);

            Timer();
        }

        private const int CenterMassImageRadius = 10;
        private static readonly Bitmap CenterMassImage;

        static Player()
        {
            CenterMassImage = new Bitmap(CenterMassImageRadius * 2, CenterMassImageRadius * 2);
            Rectangle r = new Rectangle(0, 0, CenterMassImage.Width - 1, CenterMassImage.Height - 1);
            Graphics g = Graphics.FromImage(CenterMassImage);

            Pen p = new Pen(Color.Orange, 1f);
            SolidBrush b = new SolidBrush(Color.Orange);

            g.FillPie(b, r, 0, 90);
            g.FillPie(b, r, 180, 90);
            g.DrawEllipse(p, r);

            p.Dispose();
            b.Dispose();
            g.Dispose();
        }

        private bool _paused = true;
        private bool Paused
        {
            get
            {
                return _paused;
            }
            set
            {
                if (value && _paused) return;

                _paused = value;
                if (value)
                {
                    lock (U.Lock) { }
                    Timeline.Snap(U);
                }
            }
        }

        private Timeline Timeline = new Timeline();

        private const int PhysSteps = 1000;

        private double SimTime = .015d;
        private int SimSteps = 1000;
        private int SimIter = 1000;

        private double TimeScale = 1d;

        private Vector2[][] Prediction = null;

        private bool PredictionSetup()
        {
            SimulationDialog d = new SimulationDialog();

            d.TimeBox.Value = SimTime;
            d.StepBox.Value = SimSteps;
            d.IterBox.Value = SimIter;

            if (d.ShowDialog() != DialogResult.OK)
            {
                d.Dispose();
                return false;
            }

            SimTime = Math.Max(1e-9d, d.TimeBox.Value);
            SimSteps = (int)Math.Max(1d, d.StepBox.Value);
            SimIter = (int)Math.Max(0d, d.IterBox.Value);

            d.Dispose();

            return true;
        }

        private const float HaloInnerOffset = 10f;
        private const float HaloOuterOffset = 15f;

        private const string TapeMeasureFormat = "[{0}m]";
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            U.Paint(e, Prediction);

            e.Graphics.DrawRectangle(SelectionPen, SelectionRect);

            if (MeterStick != null)
            {
                Vector2 delta = MeterStick.End - MeterStick.Start;
                Vector2 center = MeterStick.Start + delta / 2;

                if (delta.Y / delta.X > 0d)
                    center -= new Vector2(0, 2 * MeasureFont.Size);

                e.Graphics.DrawLine(MeasurePen,
                    MeterStick.Start,
                    MeterStick.End);

                e.Graphics.DrawString(
                    string.Format(TapeMeasureFormat, delta.Magnitude / U.Scale),
                    MeasureFont, MeasureBrush, (Point)center);
            }

            if (WallDrawer != null)
            {
                e.Graphics.DrawLine(SelectionPen, WallDrawer.Start, WallDrawer.End);
            }

            if (Selection.Count > 0)
            {
                //selection halos

                Region outer = new Region(), inner = new Region();
                GraphicsPath outerpath = new GraphicsPath(), innerpath = new GraphicsPath();

                outer.MakeEmpty();
                inner.MakeEmpty();

                int bodies = 0;
                for (int i = 0; i < Selection.Count; i++)
                {
                    if (Selection[i] is Body) bodies++;

                    RectangleF clip = Selection[i].GetClip(U.Origin, U.Scale);
                    if (!clip.IntersectsWith(ClientRectangle)) continue;

                    outerpath.AddEllipse(clip.X - HaloOuterOffset, clip.Y - HaloOuterOffset,
                        clip.Width + 2 * HaloOuterOffset, clip.Height + 2 * HaloOuterOffset);
                    innerpath.AddEllipse(clip.X - HaloInnerOffset, clip.Y - HaloInnerOffset,
                        clip.Width + 2 * HaloInnerOffset, clip.Height + 2 * HaloInnerOffset);

                    outer.Union(outerpath);
                    inner.Union(innerpath);

                    outerpath.Reset();
                    innerpath.Reset();
                }

                outer.Exclude(inner);
                outer.Intersect(ClientRectangle);

                e.Graphics.FillRegion(SelectionBrush, outer);

                outerpath.Dispose();
                innerpath.Dispose();

                outer.Dispose();
                inner.Dispose();

                //center of mass indicator

                if (bodies > 0)
                {
                    Vector2 cm = Vector2.Zero;
                    double sm = 0d;
                    for (int i = 0; i < Selection.Count; i++)
                    {
                        if (!(Selection[i] is Body)) continue;
                        Body b = (Body)Selection[i];

                        cm += b.Position * b.Mass;
                        sm += b.Mass;
                    }
                    Point cmp = cm * (U.Scale / sm) - U.Origin;

                    e.Graphics.DrawImage(CenterMassImage, cmp.X - CenterMassImageRadius, cmp.Y - CenterMassImageRadius);
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            double a = e.Delta > 0 ? 1.5d : .6666666666d;

            if (Following == null)
            {
                U.Origin = U.Origin * a + (Vector2)e.Location * a - e.Location;
                U.Scale *= a;
            }
            else
            {
                U.Scale *= a;
                U.Origin = Following.Position * U.Scale - 0.5d * (Vector2)Size;
            }

            if (Paused) Invalidate();
        }

        protected override async void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
                await Pan(e.Location);
            else if (e.Button == MouseButtons.Left)
            {
                Paused = true;
                EditorObject clicked = U.GetClicked(e.Location, Selection);

                if (ModifierKeys == Keys.Shift)
                    await Measure(e.Location);
                else if (clicked != null)
                {
                    if (ModifierKeys == Keys.Alt && clicked.GetType() == typeof(Node) && !U.IsTeleporter((Node)clicked))
                    {
                        await DrawWalls((Node)clicked);

                        Timeline.Snap(U);
                    }
                    else
                    {
                        bool newSelection = (ModifierKeys & Keys.Control) == 0;
                        bool moved = await DragBody(clicked, e.Location);
                        if (!moved)
                        {
                            if (newSelection)
                            {
                                bool omit = Selection.Count == 1 && Selection[0] == clicked;

                                Selection.Clear();
                                if (!omit) Selection.Add(clicked);
                            }
                            else
                                if (Selection.Contains(clicked)) Selection.Remove(clicked); else Selection.Add(clicked);

                            Invalidate();
                        }
                    }
                }
                else await RectSelect(e.Location);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Right)
            {
                Paused = true;

                ContextObject = U.GetClicked(e.Location, Selection);
                ContextPoint = e.Location;

                if (ContextObject != null)
                {
                    if (ContextObject is Body)
                        BodyContext.Show(this, e.Location);
                    else if (ContextObject is Node)
                    {
                        if (ContextObject is Projector)
                        {
                            ProjectorContext.Show(this, e.Location);
                        }
                        else
                        {
                            Teleporter t = U.GetTeleporter((Node)ContextObject);
                            if (t == null) NodeContext.Show(this, e.Location);
                            else TeleporterContext.Show(this, e.Location);
                        }
                    }
                }
                else Context.Show(this, e.Location);
            }
        }

        private SolidBrush SelectionBrush = new SolidBrush(Color.Orange);
        private Pen SelectionPen = new Pen(Color.Orange, 4f)
        {
            StartCap = LineCap.Round,
            EndCap = LineCap.Round,
            DashStyle = DashStyle.DashDot
        };

        private SolidBrush MeasureBrush = new SolidBrush(Color.Orange);
        private Pen MeasurePen = new Pen(Color.Orange, 4f)
        {
            StartCap = LineCap.RoundAnchor,
            EndCap = LineCap.RoundAnchor,
            DashStyle = DashStyle.DashDot
        };
        private Font MeasureFont = new Font(FontFamily.GenericSansSerif, 14f);

        private Rectangle SelectionRect = Rectangle.Empty;
        private TapeMeasure MeterStick = null, WallDrawer = null;

        public async Task DrawWalls(Node start)
        {
            Node last = start;
            WallDrawer = new TapeMeasure();
            WallDrawer.Start = last.Position * U.Scale - U.Origin;

            while (MouseButtons == MouseButtons.Left)
            {
                await Task.Delay(Settings.SleepTime);

                PointF endPoint = WallDrawer.End = PointToClient(MousePosition);

                Node now = U.Boundary.Nodes.OneOrDefault(n => n != last && n.GetClip(U.Origin, U.Scale).Contains(endPoint));
                if (now != null)
                {
                    Wall wall = U.Boundary.Walls.SingleOrDefault(w => w.Contains(last) && w.Contains(now));
                    if (wall == null) U.Boundary.Walls.Add(new Wall(last, now));
                    else U.Boundary.Walls.Remove(wall);

                    last = now;
                    WallDrawer.Start = last.Position * U.Scale - U.Origin;
                }

                Invalidate();
            }

            WallDrawer = null;
            Invalidate();
        }

        public async Task RectSelect(Point start)
        {
            Point end = start;
            while (MouseButtons == MouseButtons.Left)
            {
                await Task.Delay(Settings.SleepTime);

                Point now = PointToClient(MousePosition);
                if (now == end) continue;
                end = now;

                SelectionRect = Utility.BindingRect(start, end);
                Invalidate();
            }

            bool newSelection = (ModifierKeys & Keys.Control) == 0;
            if (newSelection) Selection.Clear();
            foreach (EditorObject obj in U.Objects)
            {
                if (SelectionRect.Contains(obj.Position * U.Scale - U.Origin))
                    if (!newSelection && Selection.Contains(obj)) Selection.Remove(obj); else Selection.Add(obj);
            }

            SelectionRect = Rectangle.Empty;
            Invalidate();
        }

        public async Task Measure(Point start)
        {
            MeterStick = new TapeMeasure();
            MeterStick.Start = start;

            while (MouseButtons == MouseButtons.Left)
            {
                await Task.Delay(Settings.SleepTime);

                Vector2 now = PointToClient(MousePosition);
                if (MeterStick.End == now) continue;

                MeterStick.End = now;
                Invalidate();
            }

            MeterStick = null;
            Invalidate();
        }

        public async Task<bool> DragBody(EditorObject obj, Point dragStart)
        {
            Vector2 start = dragStart;

            bool selection = Selection.Count > 1 && Selection.Contains(obj);
            bool moved = false;

            if (selection)
            {
                Vector2[] orig = new Vector2[Selection.Count];
                for (int i = 0; i < Selection.Count; i++)
                    orig[i] = Selection[i].Position;

                Vector2 prev = start;
                while (MouseButtons == MouseButtons.Left)
                {
                    await Task.Delay(Settings.SleepTime);

                    Vector2 now = PointToClient(MousePosition);
                    if (now != prev)
                    {
                        for (int i = 0; i < Selection.Count; i++)
                            Selection[i].Position = orig[i] + (now - start) / U.Scale;

                        if (!moved) moved = true;
                        prev = now;
                        Invalidate();
                    }
                }

                if (prev != start) Timeline.Snap(U);
            }
            else
            {
                Vector2 orig = obj.Position;

                Vector2 prev = start;
                while (MouseButtons == MouseButtons.Left)
                {
                    await Task.Delay(Settings.SleepTime);

                    Vector2 now = PointToClient(MousePosition);
                    if (now != prev)
                    {
                        obj.Position = orig + (now - start) / U.Scale;

                        if (!moved) moved = true;
                        prev = now;
                        Invalidate();
                    }
                }

                if (prev != start) Timeline.Snap(U);
            }

            return moved;
        }

        public async Task Pan(Point start)
        {
            if (Following != null) Following = null;

            Vector2 orig = U.Origin;
            Vector2 prev = start;

            while (MouseButtons == MouseButtons.Middle)
            {
                await Task.Delay(Settings.SleepTime);

                Vector2 now = PointToClient(MousePosition);
                if (now != prev)
                {
                    U.Origin = orig - now + start;

                    prev = now;
                    if (Paused) Invalidate();
                }
            }
        }

        private async void Timer()
        {
            DateTime then = DateTime.Now;
            DateTime now;

            while (true)
            {
                await Task.Delay(Settings.SleepTime);

                if (!Paused)
                {
                    now = DateTime.Now;
                    double d = (now - then).TotalSeconds;

                    U.StepPhys(TimeScale * d, PhysSteps);

                    then = now;

                    if (Following != null)
                    {
                        if (U.Bodies.Contains(Following))
                            U.Origin = Following.Position * U.Scale - 0.5d * (Vector2)Size;
                        else Following = null;
                    }

                    TickLabel.Text = string.Format("{0}x Time: {1}s", TimeScale, d * TimeScale);
                    Invalidate();
                }
                else
                {
                    then = DateTime.Now;

                    TickLabel.Text = string.Format("{0}x Simulation Paused", TimeScale);
                }
            }
        }

        private void SpeedBar_ValueChanged(object sender, EventArgs e)
        {
            TimeScale = Math.Pow(2d, SpeedBar.Value);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            try
            {
                FileInfo file = Utility.Save(U);
                if (file != null)
                {
                    WorkingFile = file;
                    Timeline.UpToDate = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            try
            {
                if (WorkingFile != null)
                {
                    Utility.Save(U, WorkingFile);
                    Timeline.UpToDate = true;
                }
                else
                    saveAsToolStripMenuItem_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            if (!Timeline.UpToDate)
            {
                ClosingOption r = ClosingSavePrompt();
                if (r == ClosingOption.Cancel) return;
            }

            Universe u;
            FileInfo file = Utility.Load(out u);

            if (file != null && u != null)
            {
                WorkingFile = file;
                U = u;

                Timeline.Initialize(U);

                Selection.Clear();
                Following = null;
                Prediction = null;

                Invalidate();
            }
        }

        private void MenuStrip_MenuActivate(object sender, EventArgs e)
        {
            Paused = true;
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = !Paused;
            if (Paused) Invalidate();
        }

        private void predictToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            if (!PredictionSetup()) return;

            ProgressDisplay.Show(U.Predict(SimTime, SimSteps, SimIter, p =>
            {
                if (p == null) return;

                Prediction = p;
                Invalidate();
            }), "Predicting Trajectories");
        }

        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            if (!PredictionSetup()) return;

            ProgressDisplay.Show(U.Calculate(SimTime, SimSteps, SimIter, u =>
            {
                if (u == null) return;

                U = u;
                Timeline.Snap(U);

                Invalidate();
            }), "Calculating Trajectories");
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            if (!Timeline.UpToDate)
            {
                ClosingOption r = ClosingSavePrompt();
                if (r == ClosingOption.Cancel) return;
            }

            Selection.Clear();
            U = new Universe();

            Following = null;
            Prediction = null;
            WorkingFile = null;

            Timeline.Initialize(U);

            Invalidate();
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            Timeline.Revert(ref U);

            Timeline.Snap(U);

            Invalidate();
        }

        private Universe CopyData = null;
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;

            CopyData = U.Clone(Selection);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;
            if (CopyData == null) return;

            Selection.Clear();
            U.Append(CopyData, t => Selection.Add(t));

            Timeline.Snap(U);
            Invalidate();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;
            if (Timeline.Undo(ref U))
            {
                Selection.Clear();
                Invalidate();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paused = true;
            if (Timeline.Redo(ref U))
            {
                Selection.Clear();
                Invalidate();
            }
        }

        private ClosingOption ClosingSavePrompt()
        {
            ClosingOption r = Utility.ClosingPrompt();
            if (r == ClosingOption.Save)
            {
                try
                {
                    if (WorkingFile != null)
                        Utility.Save(U, WorkingFile);
                    else
                        if (Utility.Save(U) == null) r = ClosingOption.Cancel;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return r;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Paused = true;
            base.OnFormClosing(e);

            if (!Timeline.UpToDate)
            {
                ClosingOption r = ClosingSavePrompt();
                if (r == ClosingOption.Cancel) e.Cancel = true;
            }
        }

        //Diagnostic
        private void TestPerformance()
        {
            Universe u = new Universe();
            u.Bodies.Add(new Body(1d, 0d, 1d, new Vector2(0, 0), new Vector2(5, 0)));
            u.Bodies.Add(new Body(1d, 0d, 1d, new Vector2(5, 5), new Vector2(5, -5)));
            u.Bodies.Add(new Body(1d, 0d, 1d, new Vector2(0, 5), new Vector2(5, 20)));

            int times = 1000;
            int steps = 1000;
            Stopwatch s = Stopwatch.StartNew();
            for (int i = 0; i < times; i++) u.StepPhys(0.015d, steps);

            s.Stop();
            MessageBox.Show(string.Format("{0} ms per step", s.Elapsed.TotalMilliseconds / (times * steps)));
        }
    }
}
