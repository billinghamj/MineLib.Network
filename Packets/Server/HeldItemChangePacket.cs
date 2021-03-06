using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct HeldItemChangePacket : IPacket
    {
        public byte Slot;

        public const byte PacketID = 0x09;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Slot = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(Slot);
            stream.Purge();
        }
    }
}