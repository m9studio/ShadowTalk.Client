using M9Studio.ShadowTalk.Core;

namespace M9Studio.ShadowTalk.Client.Packet
{
    public class PacketMessageSend : PacketStruct
    {
        public string UUID;
        public string Text;
    }
}
