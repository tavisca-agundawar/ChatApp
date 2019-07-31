using System;
using System.Net.Sockets;
using System.Threading;

namespace ChatApp
{
    public class MainProgram
    {
        public static User user { get; private set; }
        
        public void Run()
        {
            user = User.GetUserDetails();

            var newConnection = new NetworkListener();
            //newConnection.NewConnection += new MainProgram().NewIncomingConnection;

            //Console.WriteLine("Enter port number to begin listening:");
            int portNumber = ExtractPortNumber(ExtractAddress(user.userAddress));

            //Display.StatusMessage($"Listening on {portNumber}");

            var listenerThread = new Thread(_ => newConnection.StartListening(portNumber));

            listenerThread.Start();

            var choice = Display.GetInputFromUser("To initiate connection press 'Y/y':");
            if (choice.ToLowerInvariant().Equals("y"))
            {
                //Console.Clear();

                //listenerThread.Interrupt();
                //listenerThread.Join();
                //newConnection.StopListening();
                listenerThread.Suspend();
                
                string peerAddress;

                peerAddress = Display.GetInputFromUser("Enter peer address to connect:");

                NetworkClient networkClient = new NetworkClient();

                User peer = new User(peerAddress);

                Socket socket = networkClient.Connect(ExtractAddress(peerAddress));

                Conversation newConversation = new Conversation();
                newConversation.StartConversationByClient(socket, user, peer);

            }

            Console.ReadKey(true);
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

            //Display.StatusMessage(returnAddress);

            return returnAddress;
        }

        private int ExtractPortNumber(string address)
        {
            var addressParts = address.Split(':');
            return Convert.ToInt32(addressParts[1]);
        }
    }
}
