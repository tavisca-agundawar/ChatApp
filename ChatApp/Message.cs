using System;

namespace ChatApp
{
    public class Message
    {
        public string message { get; set; }
        public DateTime timeStamp { get; }

        public Message(string message)
        {
            this.message = message;
            timeStamp = DateTime.Now;
        }
    }
}
