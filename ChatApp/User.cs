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
