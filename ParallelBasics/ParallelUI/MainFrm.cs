using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelUI
{
    public partial class MainFrm : Form
    {
        CancellationTokenSource tokenSource;
        CancellationToken token;
        ParallelOptions options;

        public MainFrm()
        {
            InitializeComponent();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
        }



        private void startBtn_Click(object sender, EventArgs e)
        {
            //NotParallel();
            //Parallelised();
            Task.Factory.StartNew(() => { Parallelised(); },token);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }

        private void resultsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Selected "+resultsListBox.Text);
        }

        private void Parallelised()
        {
            options = new ParallelOptions();
            options.CancellationToken = token;
            
            Parallel.For(0, 100,options, i => 
                {
                    try
                    {
                        doWork(i);
                    }
                    catch (OperationCanceledException ex)
                    {
                        //cancelled
                    }
                }
                );
        }

        private void NotParallel()
        {
            for (int i = 0; i < 100; i++)
            {
                doWork(i);

            }
        }

        private void doWork(int item)
        {
            var watch = Stopwatch.StartNew();
            for (int i = 3; i < 30000; i++)
            {

                if (isPrime(i)) ;
                options.CancellationToken.ThrowIfCancellationRequested();

            }
            watch.Stop();
            if (resultsListBox.InvokeRequired)
                resultsListBox.BeginInvoke((Action)delegate() { resultsListBox.Items.Add(String.Format("{0} took {1} milliseconds", item, watch.ElapsedMilliseconds)); });
            else
                resultsListBox.Items.Add(String.Format("{0} took {1} milliseconds", item, watch.ElapsedMilliseconds));

        }



        private bool isPrime(int i)
        {

            for (int j = 2; j <= (i / 2); j++)
            {

                if ((i % j) == 0)

                    return false;

            }



            return true;

        }
    }
}
