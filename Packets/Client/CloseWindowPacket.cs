using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID;

        public const byte PacketID = 0x0D;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            WindowID = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(WindowID);
            stream.Purge();
        }
    }
}