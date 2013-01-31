using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

using System.Windows.Forms;

namespace SynchronizationContextDemo
{
    public partial class Form1 : Form
    {
        SynchronizationContext ctx;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ctx = SynchronizationContext.Current;
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();
            comment.Text = "Updated by UI thread";
        }

        private void ThreadProc()
        {
            SendOrPostCallback callback = new SendOrPostCallback(UpdateLabel);
            for(int i=0; i<100; ++i)
            {
                Thread.Sleep(100);
                ctx.Post(callback, string.Format("Updated from worker thread: {0}% completed...", i));   // UI work in UI context
            }

            ctx.Send(callback, "Updated from worker thread: Completed");
        }

        private void UpdateLabel(object state)
        {
            result.Text = state as string;
        }
    }
}
