using System;
using System.Net;
using System.Net.Sockets;

namespace ChatApp
{
    public class NetworkClient
    {
        public string _ipaddress { get; private set; }
        public int _portNumber { get; private set; }
        public Socket Connect(string address)
        {
            //Accepts address in the form
            // ipv4:portNumber
            //eg: 192.168.10.28:8888

            ExtractIPandPortNumber(address);

            IPAddress ipAddress = IPAddress.Parse(_ipaddress);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress,_portNumber);

            Socket socket = new Socket(ipAddress.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            Display.StatusMessage("Attempting Connection.");

            socket.Connect(localEndPoint);

            return socket;
        }

        public void CloseConnection(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        public void ExtractIPandPortNumber(string address)
        {
            var addressParts = address.Split(':');
            _ipaddress = addressParts[0];
            _portNumber = Convert.ToInt32(addressParts[1]);
        }

    }
}
