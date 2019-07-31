using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatApp;
using FluentAssertions;
using Xunit;

namespace Test_ChatApp
{
    public class Test_Network_Listener
    {
        [Fact]
        public void network_Listener_Starts_Listening_And_Initializes_Appropriate_Socket()
        {
            //Arrange
            NetworkListener testNetworkListener;
            IPAddress ipAddress;
            Socket expectedSocket;
            IPEndPoint localEndPoint;

            //Act
            testNetworkListener = new NetworkListener();
            ipAddress = IPAddress.Parse("192.168.42.98");
            expectedSocket = new Socket(ipAddress.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);
            localEndPoint = new IPEndPoint(ipAddress, 11111);

            var worker = new Thread(_ => testNetworkListener.StartListening());
            worker.Start();
            expectedSocket.Connect(localEndPoint);
            worker.Join();

            //Assert
            testNetworkListener.clientSocket.RemoteEndPoint.Should().Be(expectedSocket.LocalEndPoint);
        }

        [Fact]
        public void test_Raising_Of_Event_OnNewConnection()
        {
            //Arrange
            NetworkClient networkClient;
            NetworkListener networkListener;
            IPHostEntry host;
            Socket actualSocket;
            Socket eventSocket = null;
            string ipaddress = null;
            string address = null;
            //var testNewConnection;

            //Act
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipaddress = ip.ToString();
                }
            }

            address = ipaddress + ":" + "11111";

            networkListener = new NetworkListener();
            networkListener.NewConnection += TestNewConnection;

            void TestNewConnection(object sender, NewConnectionHandlerArgs e)
            { 
                eventSocket = e._socket;
            }
            networkClient = new NetworkClient();

            var worker = new Thread(_ => networkListener.StartListening());
            worker.Start();
            
            actualSocket = networkClient.Connect(address);
            
            worker.Join();
            //Assert
            eventSocket.RemoteEndPoint.Should().Be(networkListener.clientSocket.LocalEndPoint);
                       
        }

      
    }
}
