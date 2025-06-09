using M9Studio.SecureStream;
using M9Studio.ShadowTalk.Core;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace M9Studio.ShadowTalk.Client
{
    public class ServerInfo
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

        public string PrivateA;
        public string PublicA;


        private string _token = null;
        public string token
        {
            get
            {
                if (_token == null)
                {
                    long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    _token = $"{Id}:{timestamp}";
                }
                return _token;
            }
        }

        public string hmac => SRPHelper.ComputeHMAC(Convert.FromHexString(K), token);

        private SecureChannelManager<IPEndPoint> _ChannelManager;
        public SecureChannelManager<IPEndPoint> ChannelManager
        {
            get => _ChannelManager;
            set
            {
                _ChannelManager = value;
                if (value == null)
                {
                    foreach (var item in Users)
                    {
                        item.Value.Panel.labelServer.ForeColor = Color.Red;
                        if(item.Value.Form != null)
                        {
                            item.Value.Form.labelServer.ForeColor = Color.Red;
                        }
                    }
                }
                else
                {
                    foreach (var item in Users)
                    {
                        item.Value.Panel.labelServer.ForeColor = Color.Yellow;
                        if (item.Value.Form != null)
                        {
                            item.Value.Form.labelServer.ForeColor = Color.Yellow;
                        }
                    }
                }
            }
        }
        private SecureSession<IPEndPoint> _Session;
        public SecureSession<IPEndPoint> Session
        {
            get => _Session;
            set
            {
                _Session = value;
                if (value == null)
                {
                    foreach (var item in Users)
                    {
                        item.Value.Panel.labelServer.ForeColor = Color.Red;
                        if (item.Value.Form != null)
                        {
                            item.Value.Form.labelServer.ForeColor = Color.Red;
                        }
                    }
                }
                else
                {
                    foreach (var item in Users)
                    {
                        item.Value.Panel.labelServer.ForeColor = Color.Green;
                        if (item.Value.Form != null)
                        {
                            item.Value.Form.labelServer.ForeColor = Color.Green;
                        }
                    }
                }
            }
        }
        public Socket Socket;
        public Dictionary<int, User> Users = new Dictionary<int, User>();
    }
}