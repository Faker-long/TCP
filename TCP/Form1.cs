using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCP
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = null;
        }
        private Socket SockClient;
        private IPEndPoint secerEndPoint;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                secerEndPoint = new IPEndPoint(IPAddress.Parse(textBox1.Text), Convert.ToInt32(textBox2.Text));
                SockClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SockClient.Connect(secerEndPoint);
                richTextBox1.Text += "连接成功！";
                if (Thread == null)
                {
                    Thread = new Thread(RecevieInfo);
                    Thread.IsBackground = true;
                    Thread.Start();
                }
                else
                {
                    Thread.Abort();
                    Thread = null;
                }
            }
            catch
            {
                richTextBox1.Text += "\r\n" + "连接失败！";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string strMsg = textBox3.Text.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(strMsg);
            SockClient.Send(byteArray);
            richTextBox1.Text += "\r\n" + "客户端发送数据:" + strMsg;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SockClient.Close();
            }
            catch { }
            richTextBox1.Text += "\r\n" + "客户端已关闭";
        }
        Thread Thread;
        Thread Thread1;
        private void RecevieInfo()
        {
            while (true)
            {
                try
                {
                    byte[] msgArray = new byte[1024 * 1024];
                    int trueClientMsgLength = SockClient.Receive(msgArray);
                    string strMsg = Encoding.UTF8.GetString(msgArray, 0, trueClientMsgLength);
                    this.Invoke((MethodInvoker)delegate { richTextBox1.Text += "\r\n" + "客户端接收数据:" + strMsg; });
                }
                catch { }
                //  richTextBox1.Text += "\r\n" + "接收数据:" + strMsg;
                //   MessageBox.Show(strMsg);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.ScrollToCaret();
        }
        private Socket SockServer;
        private IPEndPoint ServerEndPoint;
        Socket server;
        private void TH()
        {
            server = SockServer.Accept();
            string clientname = server.RemoteEndPoint.ToString();// SockClient.RemoteEndPoint.ToString();
            this.Invoke((MethodInvoker)delegate { richTextBox1.Text += "\r\n" + "收到客户端连接:" + clientname; });
            while (true)
            {
                try
                {
                    byte[] msgArray = new byte[1024 * 1024];
                    int trueClientMsgLength = server.Receive(msgArray, 0, msgArray.Length, 0);//Receive(msgArray);
                    string strMsg = Encoding.UTF8.GetString(msgArray, 0, trueClientMsgLength);
                    this.Invoke((MethodInvoker)delegate { richTextBox1.Text += "\r\n" + "服务器接收数据:" + strMsg; });
                }
                catch { }
                //  richTextBox1.Text += "\r\n" + "接收数据:" + strMsg;
                //   MessageBox.Show(strMsg);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ServerEndPoint = new IPEndPoint(IPAddress.Parse(textBox4.Text), Convert.ToInt32(textBox5.Text));
                SockServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SockServer.Bind(ServerEndPoint);
                SockServer.Listen(1);
                richTextBox1.Text += "服务器创建成功！";
                if (Thread1 == null)
                {
                    Thread1 = new Thread(TH);
                    Thread1.IsBackground = true;
                    Thread1.Start();
                }
            }
            catch
            {
                richTextBox1.Text += "服务器创建失败！";
            }
        }
        
        private void button6_Click(object sender, EventArgs e)
        {
            Thread1.Abort();
            SockServer.Close();
                
                

          //  catch { }
            richTextBox1.Text += "\r\n" + "服务器已关闭";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string strMsg = textBox6.Text.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(strMsg);
            server.Send(byteArray);
            richTextBox1.Text += "\r\n" + "服务器发送数据:" + strMsg;
        }
    }
}
