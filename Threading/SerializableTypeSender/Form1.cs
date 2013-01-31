using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net.Sockets;
using NetworkLib;
using SerializableTypeDemo;

namespace SerializableTypeSender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Data data = new Data();
            data.PrivateData = textBox1.Text;
            data.publicData = textBox2.Text;

            label3.Text = "Connecting...";

            try
            {
                NetworkStream networkStream = Client.Connect();

                SoapFormatter formatter = new SoapFormatter();
                formatter.Serialize(networkStream, data);
                networkStream.Close();
                label3.Text = "Data sent";
                button1.Enabled = false;
            }
            catch (SocketException se)
            {
                label3.Text = se.Message;
                button1.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            label3.Text = "Data not sent";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            label3.Text = "Data not sent";
        }
    }
}
