using M9Studio.SecureStream;
using System.Net;

namespace M9Studio.ShadowTalk.Client
{
    internal class ServerInfo
    {
        public int ServerId;
        public string ServerName;

        public string IP;
        public int Port;


        //для расшифровки сообщений
        public string RSA;

        public string Name;
        public int Id;


        public string K;
        public string token;
        public string hmac;

        public SecureChannelManager<IPEndPoint> ChannelManager;
        public SecureSession<IPEndPoint> Session;
        public Dictionary<int, User> Users = new Dictionary<int, User>();
    }
}