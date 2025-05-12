namespace M9Studio.ShadowTalk.Client
{
    internal class User
    {
        public int Id;
        public string Name;

        //симетричный ключ шифрования, для отправки через сервер
        public string Key;
    }
}
