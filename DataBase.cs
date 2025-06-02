namespace M9Studio.ShadowTalk.Client
{
    internal class DataBase
    {
        public List<ServerInfo> ServerInfo()
        {
            return new List<ServerInfo>();
        }
        public List<Message> Message(int user, int server)
        {
            return new List<Message>();
        }
        public List<User> User()
        {
            return new List<User>();
        }
    }
}
