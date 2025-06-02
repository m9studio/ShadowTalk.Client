using M9Studio.SecureStream;
using System.Net;

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

        //сессия
        public SecureSession<IPEndPoint>? Session = null;
    }
}
