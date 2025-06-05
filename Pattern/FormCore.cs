using M9Studio.SecureStream;
using M9Studio.ShadowTalk.Core;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Net;
using System.Net.Sockets;

namespace M9Studio.ShadowTalk.Client
{
    public partial class FormCore : Form
    {
        //private readonly IPEndPoint address;

        //private IPEndPoint addressServer;
        //private Socket socketServer;
        //private TcpSecureTransportAdapter adapterServer;

        private IPEndPoint addressP2P;
        private Socket socketP2P;
        private SecureChannelManager<IPEndPoint> chanelP2P;
        public FormCore()
        {
            InitializeComponent();
        }




        private SecureSession<IPEndPoint> Login(string ip, int port, string email, string password)
        {
            SecureSession<IPEndPoint> connect = InitConnect(ip, port);
            connect.Send(new PacketClientToServerLogin
            {
                Email = email,
                Password = password
            });
            JObject packet = connect.ReceiveJObject();//либо ошибка, либо сообщения


            return connect;
        }
        private SecureSession<IPEndPoint> Login(ServerInfo serverInfo)
        {
            SecureSession<IPEndPoint> connect = InitConnect(serverInfo.IP, serverInfo.Port);
            var (a, A_hex) = SRPHelper.GenerateA();
            connect.Send(new PacketClientToServerReconectSRP
            {
                Token = serverInfo.token,
                HMAC = serverInfo.hmac,
                A = A_hex,
                Port = addressP2P.Port
            });
            JObject packet = connect.ReceiveJObject();//либо ошибка, либо сообщения
            PacketServerToClientLoginError error = PacketStruct.Parse<PacketServerToClientLoginError>(packet);
            if (error != null)
            {
                //TODO ошибка
                return null;
            }
            JObject packet2 = connect.ReceiveJObject();//инфа о сообщениях

            return connect;
        }
        private SecureSession<IPEndPoint> InitConnect(string ip, int port)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint address = new IPEndPoint(IPAddress.Any, 0);
            socket.Bind(address);
            socket.Listen(10);
            TcpSecureTransportAdapter adapter = new TcpSecureTransportAdapter(socket);
            SecureChannelManager<IPEndPoint> manager = new SecureChannelManager<IPEndPoint>(adapter);
            return manager.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        }
    }
}
