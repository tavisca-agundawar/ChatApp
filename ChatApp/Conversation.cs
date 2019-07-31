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
        byte[] receiveEncodedMessage;
        Display display;
        int numBytes;
        byte[] sendEncodedMessage;

        Thread sendMessageThread;
        Thread readMessageThread;

        public Conversation()
        {
            receiveEncodedMessage = new byte[1024];
            display = new Display();
        }
        
        public void StartConversationByListener(Socket socket, User user)
        {

            Display.StatusMessage("Conversation started by Listener.");

            //display = new Display();
            bool isInitiatedByListener = true;
            this.socket = socket;
            this.user = user;

            Display.StatusMessage("Getting peer information.");

            ExchangeUserInformation(isInitiatedByListener);

            //string readMessage;
            //string sentMessage;
            sendMessageThread = new Thread(
                () =>
                {
                    while(true)
                    {
                        SendMessage();
                    }
                    //display.UserMessage(sentMessage, user.userName);
                });

            readMessageThread = new Thread(
                () =>
                {
                    while(true)
                    {
                        ReadMessage();
                    }
                    //display.PeerMessage(readMessage, peer.userName);
                });

            readMessageThread.Start();
            sendMessageThread.Start();

            Display.StatusMessage("Waiting for message input.");

        }

        public void StartConversationByClient(Socket socket, User user, User peer)
        {
            Display.StatusMessage("Conversation started by cliient.");
            //display = new Display();
            bool isInitiatedByListener = false;
            this.peer = peer;
            this.socket = socket;
            this.user = user;

            //byte[] sendUserAddress = Encoding.ASCII.GetBytes(user.userAddress);
            //socket.Send(sendUserAddress);

            ExchangeUserInformation(isInitiatedByListener);
            Display.StatusMessage("Sent user information.");

            //string readMessage;
            //string sentMessage;
            sendMessageThread = new Thread(
                () =>
                {
                    while(true)
                    {
                        SendMessage();
                    }
                    //display.UserMessage(sentMessage, user.userName);
                });

            readMessageThread = new Thread(
                () =>
                {
                    while (true)
                    {
                        ReadMessage();
                    }
                    //display.PeerMessage(readMessage, peer.userName);
                });

            readMessageThread.Start();
            sendMessageThread.Start();
           
            Display.StatusMessage("Waiting for message input.");
        }

        private void ExchangeUserInformation(bool isInitiatedByListener)
        {

            if(isInitiatedByListener)
            {
                byte[] receivedEncodedPeerAddress = new byte[1024];
                string receivedPeerAddress = Encoding.ASCII.GetString(receivedEncodedPeerAddress, 0, this.socket.Receive(receivedEncodedPeerAddress));

                this.peer = new User(receivedPeerAddress);
            }

            else
            {
                byte[] sendUserAddress = Encoding.ASCII.GetBytes(user.userAddress);
                socket.Send(sendUserAddress);
            }

        }

        public void SendMessage()
        {
            string sentMessage = Display.GetInputFromUser($"{user.userName}:");
            //display.UserMessage(sentMessage, user.userName);
            sendEncodedMessage = Encoding.ASCII.GetBytes(sentMessage);
            socket.Send(sendEncodedMessage);
            if (sentMessage.ToLowerInvariant().Equals("quit"))
            {
                EndConversation();
            }
        }

        public void ReadMessage()
        {
            //byte[] receiveEncodedMessage = new byte[1024];
            numBytes = socket.Receive(receiveEncodedMessage);
            string readMessage = Encoding.ASCII.GetString(receiveEncodedMessage,0,numBytes);

            display.PeerMessage(readMessage, peer.userName);

            if(readMessage.ToLowerInvariant().Equals("quit"))
            {
                EndConversation();
            }
            //Console.WriteLine($"{user.userName}:");
        }

        private void EndConversation()
        {
            readMessageThread.Abort();
            sendMessageThread.Abort();
        }
    }
}
