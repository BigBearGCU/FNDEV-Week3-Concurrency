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


namespace SerializableTypeReceiverNoThread
{

    public partial class Form1 : Form
    {
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

            SoapFormatter formatter = new SoapFormatter();
            networkStream = Server.Listen();
            Data data = formatter.Deserialize(networkStream) as Data;
            string result = String.Format("privateData: {0}\r\npublicData: {1}", data.PrivateData, data.publicData);
            networkStream.Close();

            textBox1.Text = result;
            button1.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Doing something else...", "Something Else");
        }
    }
}
