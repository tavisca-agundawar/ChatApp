using System;

namespace ChatApp
{
    public class Display
    {
        public void PeerMessage(string message, string userName)
        {
            Console.WriteLine($"                {message}:{userName}");
        }

        public void UserMessage(string sentMessage, string userName)
        {
            Console.WriteLine($"{userName}:{sentMessage}");
        }
    }
}
