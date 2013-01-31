using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ParallelUIStarter
{
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        //not parallel method will block execution
        private void NotParallel()
        {
            for (int i = 0; i < 100; i++)
            {
                doWork(i);

            }
        }

        //do work method
        private void doWork(int item)
        {
            var watch = Stopwatch.StartNew();
            for (int i = 3; i < 30000; i++)
            {

                if (isPrime(i)) ;

            }
            watch.Stop();
            //add items to list
            resultsListBox.Items.Add(String.Format("{0} took {1} milliseconds", item, watch.ElapsedMilliseconds));
        }


        //Check to see if i is a prime number
        private bool isPrime(int i)
        {

            for (int j = 2; j <= (i / 2); j++)
            {

                if ((i % j) == 0)

                    return false;

            }



            return true;

        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            NotParallel();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
