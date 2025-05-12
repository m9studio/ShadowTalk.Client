namespace M9Studio.ShadowTalk.Client
{
    internal class User
    {
        public int ServerId;
        public ServerInfo server;

        public int Id;
        public string Name;

        //симетричный ключ шифрования, для отправки через сервер
        public string Key;
    }
}
