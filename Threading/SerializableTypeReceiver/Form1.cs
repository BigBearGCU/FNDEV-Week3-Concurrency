using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net.Sockets;
using System.Data;
using NetworkLib;
using SerializableTypeDemo;


namespace SerializableTypeReceiver
{

    public partial class Form1 : Form
    {
        // worker thread
        Thread workerThread;
        SynchronizationContext ctx;
        // network stream
        NetworkStream networkStream;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // set UI components
            button1.Enabled = false;
            label2.Text = "Listening for data...";
            textBox1.Text = "";

            // create worker thread instance
            ctx = SynchronizationContext.Current;
            workerThread = new Thread(new ThreadStart(this.WorkerThreadFunction));
            workerThread.Name = "Network Listener thread";
            workerThread.Start();
        }


        // Called on worker thread from btnStartThread_Click
        private void WorkerThreadFunction()
        {
            string result;
            SendOrPostCallback callback = new SendOrPostCallback(AddString);

            SoapFormatter formatter = new SoapFormatter();
            networkStream = Server.Listen();
            Data data = formatter.Deserialize(networkStream) as Data;
            result = String.Format("privateData: {0}\r\npublicData: {1}", data.PrivateData, data.publicData);
            networkStream.Close();

            ctx.Send(callback, result);
        }


        // Called on main thread as callback through SynchonizationContext
        private void AddString(object state)
        {
            textBox1.Text = state as string;
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Doing something else...","Something Else");
        }
    }
}
