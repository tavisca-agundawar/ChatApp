using System;
using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    public class NetworkListener
    {
        public Socket clientSocket;
        public void StartListening()
        {
            IPAddress ipAddress = IPAddress.Parse(GetIPv4Address());

            Socket listener = new Socket(ipAddress.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11111);

            try
            {
                listener.Bind(localEndPoint);

                listener.Listen(10);

                Console.WriteLine("Waiting for connection ... ");

                clientSocket = listener.Accept();

                NewConnectionHandlerArgs newConnectionEventData = new NewConnectionHandlerArgs(clientSocket);
                OnNewConnection(newConnectionEventData);
            }
            
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            

        }


        public void StopListening()
        {
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        private static string GetIPv4Address()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        public delegate void NewConnectionHandler(object target, NewConnectionHandlerArgs e);

        public event NewConnectionHandler NewConnection;

        protected virtual void OnNewConnection(NewConnectionHandlerArgs e)
        {
            NewConnection?.Invoke(this, e);
        }

    }
}
