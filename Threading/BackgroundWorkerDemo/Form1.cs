using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BackgroundWorkerDemo
{
    public partial class Form1 : Form
    {
        BackgroundWorker worker = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            worker = new BackgroundWorker();

            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(LongOperation);

            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(OnProgress);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OnCompleted);

            worker.Disposed += new EventHandler(OnDisposed);

            worker.RunWorkerAsync();
        }

        // called on worker thread
        void LongOperation(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker thisWorker = sender as BackgroundWorker;

            for (int x = 0; x < 100; ++x)
            {
                if (thisWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                Thread.Sleep(100);    //lenghty operation
                thisWorker.ReportProgress(x);
            }

            e.Result = 100;
        }

        // called on UI thread
        void OnProgress(object sender, ProgressChangedEventArgs e)
        {
            progress.Text = String.Format("{0}% completed", e.ProgressPercentage);
        }

        // called on UI thread
        void OnCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                result.Text = String.Format("Result = {0}", e.Result);
            }
            worker.Dispose();
            worker = null;
        }

        // called on UI thread
        void OnDisposed(object sender, EventArgs e)
        {
            progress.Text = "Not started";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (worker != null)
            {
                worker.CancelAsync();
                worker.Dispose();
            }
        }
    }
}
