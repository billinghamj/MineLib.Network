using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Status
{
    public struct ResponsePacket : IPacket
    {
        public string Response;

        public const byte PacketID = 0x00;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Response = stream.ReadString();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(Response);
            stream.Purge();
        }
    }
}