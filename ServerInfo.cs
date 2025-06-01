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
    }
}