using M9Studio.SecureStream;
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




    }
}
