using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoticeBotClient
{



    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream stream = default(NetworkStream);
        string message = string.Empty;

        public Form1()
        {
            InitializeComponent();
            clientSocket.Connect("61.99.150.65", 9999);
            stream = clientSocket.GetStream();


            Thread t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            t_handler.Start();
        }

        private void GetMessage()
        {
            while (true)
            {
                stream = clientSocket.GetStream();
                int BUFFERSIZE = clientSocket.ReceiveBufferSize;
                byte[] buffer = new byte[BUFFERSIZE];
                int bytes = stream.Read(buffer, 0, buffer.Length);

                string message = Encoding.Unicode.GetString(buffer, 0, bytes);
                DisplayText(message);
            }
        }

        private void DisplayText(string text)
        {
            if (richTextBox2.InvokeRequired)
            {
                richTextBox2.BeginInvoke(new MethodInvoker(delegate
                {
                    richTextBox2.AppendText(text + Environment.NewLine);
                }));
            }
            else
                richTextBox2.AppendText(text + Environment.NewLine);
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            byte[] buffer1 = Encoding.Unicode.GetBytes(this.textBox1.Text + "$");
            stream.Write(buffer1, 0, buffer1.Length);
            stream.Flush();

            byte[] buffer = Encoding.Unicode.GetBytes(this.textBox1.Text + "\n" + this.textBox2.Text + "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

        }
    }
}
