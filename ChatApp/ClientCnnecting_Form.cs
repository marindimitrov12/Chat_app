using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using SocketServer.cs;

namespace ChatApp
{
    public partial class ClientCnnecting_Form : Form
    {
        public static string ipAdress;
        ClientSide server;
        public ClientCnnecting_Form()
        {
            InitializeComponent();
            server = new ClientSide(textBox1.Text);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ipAdress = textBox1.Text;

            if (server.CheckIPAdress(ipAdress) == false)
            {
                MessageBox.Show("Invalid ipAdress!!!");
            }
            else
            {
                TcpClient client = new TcpClient();
                try
                {
                    await client.ConnectAsync(IPAddress.Parse(ipAdress), 5000);
                    MessageBox.Show("Connection open,host active");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Connection could not be established due to: \n" + ex.Message);

                }
                finally
                {
                    if (client != null)
                    {
                        if (client.Connected)
                        {
                            client.Close();
                            ClientForm clientgame = new ClientForm();
                            IPAddress new1 = IPAddress.Parse(ipAdress);
                            this.Close();
                            this.Hide();
                            clientgame.ShowDialog();

                        }


                    }



                }
            }
        }
    }
}
