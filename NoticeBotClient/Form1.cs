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

        public Form1()
        {
            InitializeComponent();
            IPHostEntry host = Dns.GetHostByName(Dns.GetHostName());
            string myip = host.AddressList[0].ToString();
            clientSocket.Connect("61.99.150.65", 9999);
            stream = clientSocket.GetStream();
            byte[] buffer = Encoding.Unicode.GetBytes(myip + "$");
            Thread t_handler = new Thread(GetMessage);
            t_handler.IsBackground = true;
            t_handler.Start();
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
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
            if (loglistbox.InvokeRequired)
            {
                loglistbox.BeginInvoke(new MethodInvoker(delegate
                {
                    loglistbox.AppendText(text + Environment.NewLine);
                }));
            }
            else
                loglistbox.AppendText(text + Environment.NewLine);
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            byte[] buffer = Encoding.Unicode.GetBytes(this.textBox2.Text + "|" +this.textBox1.Text+ "$");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

        }
    }
}
