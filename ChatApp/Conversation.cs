using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChatApp
{
    public class Conversation
    {
        Socket socket;

        User peer;
        User user;

        Display display;
        public void StartConversationByListener(Socket socket, User user)
        {
            bool isInitiatedByListener = true;
            this.socket = socket;
            this.user = user;
            ExchangeUserInformation(isInitiatedByListener);
            string readMessage;
            string sentMessage;
            var sendMessageThread = new Thread(
                () =>
                {
                    sentMessage = ReadMessage();
                    display.UserMessage(sentMessage, user.userName);
                });
            var readMessageThread = new Thread(
                () =>
                {
                    readMessage = ReadMessage();
                    display.PeerMessage(readMessage, peer.userName);
                });

        }

        public void StartConversationByClient(Socket socket, User user)
        {
            bool isInitiatedByListener = false;
            this.socket = socket;
            this.user = user;
            ExchangeUserInformation(isInitiatedByListener);

            string readMessage;
            string sentMessage;
            var sendMessageThread = new Thread(
                () =>
                {
                    sentMessage = ReadMessage();
                    display.UserMessage(sentMessage,user.userName);
                });
            var readMessageThread = new Thread(
                ()=>
                {
                    readMessage = ReadMessage();
                    display.PeerMessage(readMessage,peer.userName);
                });
        }

        private void ExchangeUserInformation(bool isInitiatedByListener)
        {
            //Random random = new Random();
            //int decider = random.Next(100);
            //byte[] encodedDecider = new byte[100];
            //encodedDecider = BitConverter.GetBytes(decider);

            if(isInitiatedByListener)
            {
                byte[] sendUserAddress = Encoding.ASCII.GetBytes(user.userAddress);
                socket.Send(sendUserAddress);

                byte[] receivedEncodedPeerAddress = new byte[1024];
                string receivedPeerAddress = Encoding.ASCII.GetString(receivedEncodedPeerAddress, 0, socket.Receive(receivedEncodedPeerAddress));

                peer = new User(receivedPeerAddress);
            }
            else
            {
                
                byte[] receivedEncodedPeerAddress = new byte[1024];
                string receivedPeerAddress = Encoding.ASCII.GetString(receivedEncodedPeerAddress, 0, socket.Receive(receivedEncodedPeerAddress));
                peer = new User(receivedPeerAddress);

                byte[] sendUserAddress = Encoding.ASCII.GetBytes(user.userAddress);
                socket.Send(sendUserAddress);

            }
            
        }

        public void SendMessage()
        {
            Console.WriteLine($"{user.userName}:");
            byte[] sendEncodedMessage = Encoding.ASCII.GetBytes(Console.ReadLine());
            socket.Send(sendEncodedMessage);
        }

        public string ReadMessage()
        {
            byte[] receiveEncodedMessage = new byte[1024];
            return Encoding.ASCII.GetString(receiveEncodedMessage,0,socket.Receive(receiveEncodedMessage));
            //Console.WriteLine($"{user.userName}:");
        }
    }
}
