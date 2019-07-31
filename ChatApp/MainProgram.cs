using System;
using System.Net.Sockets;

namespace ChatApp
{
    public class MainProgram
    {
        public static void Main(string[] args)
        {
            var newConnection = new NetworkListener();
            newConnection.NewConnection += new MainProgram().NewConnection;
        }

        public void NewConnection(object sender, NewConnectionHandlerArgs e)
        {
            var newConversation = new Conversation();
            newConversation.StartConversation(e._socket);
        }
    }
}
