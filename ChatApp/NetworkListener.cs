using System;
using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    public class NetworkListener
    {
        public Socket clientSocket;
        //public int portNumber;
        public void StartListening(int portNumber)
        {

            try
            {
                IPAddress ipAddress = IPAddress.Parse(GetIPv4Address());

                Socket listener = new Socket(ipAddress.AddressFamily,
                             SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portNumber); //11111);

           
                listener.Bind(localEndPoint);

                listener.Listen(10);

                Display.StatusMessage($"Listening for connections on port number {portNumber}.") ;

                clientSocket = listener.Accept();

                Conversation newConversation = new Conversation();

                Display.StatusMessage("Connection aquired. Starting conversation.");

                newConversation.StartConversationByListener(clientSocket, MainProgram.user);

                //NewConnectionHandlerArgs newConnectionEventData = new NewConnectionHandlerArgs(clientSocket);
                //OnNewConnection(newConnectionEventData);
            }
            
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            

        }


        public void StopListening()
        {
            Display.StatusMessage("Stopping Listner.");
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        public static string GetIPv4Address()
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
