using System.Net;
using System.Net.Sockets;
using System.Threading;
using ChatApp;
using FluentAssertions;
using Xunit;

namespace Test_ChatApp
{
    public class Test_Network_Client
    {
        [Theory]
        [InlineData("192.168.10.123:4444", "192.168.10.123", 4444)]
        [InlineData("172.16.5.214:11111", "172.16.5.214", 11111)]
        [InlineData("172.16.51.21:8888", "172.16.51.21", 8888)]
        public void network_Client_Extracts_Address_And_Port_Number(string address, string expectedIP, int expectedPort)
        {
            //Arrange
            NetworkClient networkClient;

            //Act
            networkClient = new NetworkClient();
            networkClient.ExtractIPandPortNumber(address);

            //Assert
            networkClient._ipaddress.Should().Be(expectedIP);
            networkClient._portNumber.Should().Be(expectedPort);

        }

        [Fact]
        public void network_Client_Successfully_Connects_To_Endpoint()
        {
            //Arrange
            NetworkClient networkClient;
            NetworkListener networkListener;
            IPHostEntry host;
            Socket actualSocket;
            string ipaddress = null;
            string address = null;

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
            var worker = new Thread(_ => networkListener.StartListening());
            worker.Start();

            networkClient = new NetworkClient();
            actualSocket = networkClient.Connect(address);

            worker.Join();

            //Assert
            networkClient._ipaddress.Should().Be(ipaddress);
            networkClient._portNumber.Should().Be(11111);
            actualSocket.RemoteEndPoint.Should().Be(networkListener.clientSocket.LocalEndPoint);

            
        }
    }
}
