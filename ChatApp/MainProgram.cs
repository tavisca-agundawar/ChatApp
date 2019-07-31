using System;
using System.Net.Sockets;
using System.Threading;

namespace ChatApp
{
    public class MainProgram
    {
        User user;
        
        public void Run()
        {
            user = User.GetUserDetails();

            var newConnection = new NetworkListener();
            newConnection.NewConnection += new MainProgram().NewIncomingConnection;

            var listenerThread = new Thread(newConnection.StartListening);
            listenerThread.Start();

            Console.WriteLine("To initiate connection press 'Y/y':");
            char choice = Convert.ToChar(Console.Read());
            if ( choice == 'y' || choice == 'Y')
            {
                Console.Clear();
                Console.WriteLine("Enter peer address to connect:");
                string peerAddress = Console.ReadLine();

                NetworkClient networkClient = new NetworkClient();

                networkClient.Connect(ExtractAddress(peerAddress));
            }
        }

        public void NewIncomingConnection(object sender, NewConnectionHandlerArgs e)
        {
            var newConversation = new Conversation();
            newConversation.StartConversationByListener(e._socket,user);
        }

        public string ExtractAddress(string givenAddress)
        {
            var initialSplit = givenAddress.Split('@');

            string returnAddress = initialSplit[initialSplit.Length - 1];
            return returnAddress;
        }
    }
}
