using System;
using System.Net.Sockets;

namespace ChatApp
{
    public class NewConnectionHandlerArgs : EventArgs
    {
        public Socket _socket;

        public NewConnectionHandlerArgs(Socket socket)
        {
            this._socket = socket;
        }

    }
}