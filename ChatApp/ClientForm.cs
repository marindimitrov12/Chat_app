using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SocketServer;
using SocketServer.cs;

namespace ChatApp
{
    public partial class ClientForm : Form
    {
        ClientSide client;
        public string ip;
        public ClientForm()
        {
            InitializeComponent();
            ip = ClientCnnecting_Form.ipAdress;
            client = new ClientSide(ip);
            Thread newthread = new Thread(() => { client.ConnectingToServer(); });
            newthread.IsBackground = true;
            newthread.Start();
            Thread listenningThread = new Thread(() => { client.ConnectingToServer2(); });
            listenningThread.IsBackground = true;
            listenningThread.Start();
            client.RaiseTextsendEvent += HandleTextSend;
            client.RaiseImageresEvent += HandleImageSend;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox2.Text;
            client.SendText(text);
            textBox1.AppendText("You:" + text);
            textBox1.AppendText(Environment.NewLine);
        }
        public void HandleTextSend(object sender, TextSend e)
        {
            string text = e.Mytext;
            Debug.WriteLine(text);
            textBox1.Invoke(new Action(() => textBox1.AppendText("Host:"+text)));
            textBox1.Invoke(new Action(() => textBox1.AppendText(Environment.NewLine)));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            
            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Bmp);
                byte[] arrImage = ms.GetBuffer();
                ms.Close();
                client.SendingToServer(arrImage);
            }
        }
        public void HandleImageSend(object sender, ImageSend e)
        {
            MemoryStream m = new MemoryStream(e.MyStream);
            pictureBox1.Image = new Bitmap(m);
            Debug.WriteLine("Image Shown");
        }
    }
}
