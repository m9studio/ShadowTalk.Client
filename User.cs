using M9Studio.SecureStream;
using System.Net;

namespace M9Studio.ShadowTalk.Client
{
    internal class User
    {
        public int ServerId;

        public int Id;
        public string Name;

        //симетричный ключ шифрования, для отправки через сервер
        public string Key;

        public string RSA;

        //сессия
        public SecureSession<IPEndPoint> Session;
        public int NewCount = 0;
        public ServerInfo Server;
    }
}
