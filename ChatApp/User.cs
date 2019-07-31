using System.Net.Sockets;
using System.Text;

namespace ChatApp
{
    public class User
    {
        public string userName { get; set; }
        public string userAddress;

        public User(string userAddress)
        {
            this.userAddress = userAddress;
            userName = ExtractUserName();
        }

        public static User GetUserDetails()
        {
            System.Console.WriteLine("Enter User Address: ");
            string inputAddress = System.Console.ReadLine();
            return new User(inputAddress);
        }

        public static User GetPeerDetails(Socket socket)
        {
            byte[] receivedEncodedUserAdress = new byte[1024];
            int numByte = socket.Receive(receivedEncodedUserAdress);
            string receivedUserAddress = Encoding.ASCII.GetString(receivedEncodedUserAdress, 0, numByte);
            return new User(receivedUserAddress);
        }

        public static void SendUserDetails(Socket socket, string userAsdress)
        {
            byte[] encodedUserAddress = Encoding.ASCII.GetBytes(userAsdress);
            socket.Send(encodedUserAddress);
        }
        private string ExtractUserName()
        {
            //user address must be of form
            //username@192.168.10.150:12345
            //the ip and port may be replaced with desired values
            var addressParts = userAddress.Split('@');
            return addressParts[0].ToString();
        }
    }
}
