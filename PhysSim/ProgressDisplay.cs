using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace PhysSim
{
    public partial class ProgressDisplay : Form
    {
        public BackgroundWorker W;

        private ProgressDisplay(BackgroundWorker w, string text)
        {
            InitializeComponent();
            Progress.MaxValue = 100;
            Progress.Value = 0;

            W = w;

            FormClosing += (o, e) =>
            {
                if (W.IsBusy) W.CancelAsync();
            };

            W.ProgressChanged += (o, e) =>
            {
                Progress.Value = e.ProgressPercentage;
                Text = string.Format("{0} - {1}%", text, e.ProgressPercentage);
            };

            w.RunWorkerCompleted += (o, e) =>
            {
                Close();
            };

            Text = string.Format("{0} - 0%", text);
        }

        public static ProgressDisplay Show(BackgroundWorker w, string text)
        {
            ProgressDisplay d = new ProgressDisplay(w, text);
            d.ShowDialog();

            return d;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (W.IsBusy) W.CancelAsync();
        }
    }
}
