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
    public partial class HostedServerForm : Form
    {
        AsyncSockedserver server;

        public HostedServerForm()
        {
            InitializeComponent();
            server = new AsyncSockedserver();
            Thread newtr = new Thread(() =>
            {
                server.StartListening();
            });
            newtr.IsBackground = true;
            newtr.Start();
            Thread listenforText = new Thread(() =>
            {
                server.StartListenning2();
            });
            listenforText.IsBackground = true;
            listenforText.Start();
            server.RaiseTextsendEvent += HandleTextSend;
            server.RaiseImageSendEvent += HandleImageSend;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox2.Text;
            server.SendText(text);
            textBox1.AppendText("You:"+text);
            textBox1.AppendText(Environment.NewLine);
        }
        public void HandleTextSend(object sender, TextSend e)
        {
            string text = e.Mytext;
            Debug.WriteLine(text);
            textBox1.Invoke(new Action(() => textBox1.AppendText("Client:"+text)));
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
                server.TakeCareofTcpClientEmage(arrImage);
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
