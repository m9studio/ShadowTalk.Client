using M9Studio.ShadowTalk.Core;

namespace M9Studio.ShadowTalk.Client.Packet
{
    internal class PacketMessageEdit : PacketStruct
    {
        public string UUID;
        public string Text;
    }
}
