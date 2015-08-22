using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace webSocketServer
{

    public partial class frmWebSocketServer : Form
    {
        private StateObject mStateObject = new StateObject();
        private Thread mWebSocketServer;

        public frmWebSocketServer()
        {
            InitializeComponent();
        }

        private void frmWebSocketServer_Load(object sender, EventArgs e)
        {
            AsynchronousSocketListener.stateObject = mStateObject;
            mWebSocketServer = new Thread(AsynchronousSocketListener.StartListening);
            mWebSocketServer.Start();
        }


        private void sendToClient(string valori)
        {

            if (mStateObject.workSocket != null)
            {
                byte[] array1 = new byte[2];

                array1[0] = (byte)129;
                array1[1] = (byte)valori.Length;

                byte[] array2 = System.Text.Encoding.ASCII.GetBytes(valori);

                byte[] message = new byte[array1.Length + array2.Length];

                Array.Copy(array1, message, array1.Length);
                Array.Copy(array2, 0, message, 2, array2.Length);

                AsynchronousSocketListener.Send(mStateObject.workSocket, message);

            }

        }

        private void panImg_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                sendToClient("Z;120;;");
            else
                sendToClient("Z;-120;;");
        }

        private void panImg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = (Convert.ToInt16(txtXres.Text) * e.Location.X) / panImg.Width;
                int y = (Convert.ToInt16(txtYres.Text) * e.Location.Y) / panImg.Height;
                sendToClient("M;" + x.ToString("000") + ";" + y.ToString("000") + ";");

            }

        }

        private void panImg_MouseUp(object sender, MouseEventArgs e)
        {
            sendToClient("U;;;");
        }

        private void panImg_MouseDown(object sender, MouseEventArgs e)
        {

            int x = (Convert.ToInt16(txtXres.Text) * e.Location.X) / panImg.Width;
            int y = (Convert.ToInt16(txtYres.Text) * e.Location.Y) / panImg.Height;
            sendToClient("D;" + x.ToString("000") + ";" + y.ToString("000") + ";");

        }

        private void panImg_MouseEnter(object sender, EventArgs e)
        {
            panImg.Focus();
        }

    }


    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb;
    }

    public class AsynchronousSocketListener
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static StateObject stateObject;

        public AsynchronousSocketListener()
        {
        }

        public static void StartListening()
        {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            // The DNS name of the computer
            // running the listener is "host.contoso.com".
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8181);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            //StateObject state = new StateObject();
            //state.workSocket = handler;

            stateObject.sb = new StringBuilder();
            stateObject.workSocket = handler;

            //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
            //    new AsyncCallback(ReadCallback), state);

            handler.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), stateObject.workSocket);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            //StateObject state = (StateObject)ar.AsyncState;
            Socket handler = stateObject.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                stateObject.sb.Append(Encoding.ASCII.GetString(
                    stateObject.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = stateObject.sb.ToString();
                if (content.IndexOf("GET") > -1)
                {

                    byte[] objData = System.Text.Encoding.ASCII.GetBytes("HTTP/1.1 101 Switching Protocols" + Environment.NewLine
                                                    + "Connection: Upgrade" + Environment.NewLine
                                                    + "Upgrade: websocket" + Environment.NewLine
                        + "WebSocket-Origin: http://localhost:8080" + Environment.NewLine
                        + "WebSocket-Location: ws://localhost:8181/websession" + Environment.NewLine
                                                    + "Sec-WebSocket-Accept: " + Convert.ToBase64String(
                                                        SHA1.Create().ComputeHash(
                                                            System.Text.Encoding.ASCII.GetBytes(
                                                                new Regex("Sec-WebSocket-Key: (.*)").Match(content).Groups[1].Value.Trim() + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"
                                                            )
                                                        )
                                                    ) + Environment.NewLine
                                                    + Environment.NewLine);

                    //content = System.Text.Encoding.ASCII.GetString(objData);

                    // Echo the data back to the client.
                    Send(handler, objData);
                }
                else
                {

                    // Not all data received. Get more.
                    handler.BeginReceive(stateObject.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), stateObject.workSocket);

                }
            }
        }

        public static void Send(Socket handler, byte[] byteData)
        {
            // Convert the string data to byte data using ASCII encoding.
            //byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }


}
