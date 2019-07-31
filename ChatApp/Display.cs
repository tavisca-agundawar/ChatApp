using System;

namespace ChatApp
{
    public class Display
    {
        public void PeerMessage(string message, string userName)
        {
            Console.WriteLine($"                {userName}:{message}");
        }

        public void UserMessage(string sentMessage, string userName)
        {
            Console.WriteLine($"{userName}:{sentMessage}");
        }

        public static void StatusMessage(string message)
        {
            Console.WriteLine("Status Update: " + message);
        }

        public static string GetInputFromUser(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }
    }
}
