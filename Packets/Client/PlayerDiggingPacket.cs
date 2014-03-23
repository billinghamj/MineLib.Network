using MineLib.Network.IO;
using MineLib.Network.Enums;


namespace MineLib.Network.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status;
        public int X;
        public byte Y;
        public int Z;
        public byte Face;

        public const byte PacketID = 0x07;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Status = (BlockStatus)stream.ReadByte();
            X = stream.ReadInt();
            Y = stream.ReadByte();
            Z = stream.ReadInt();
            Face = stream.ReadByte();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte((byte)Status);
            stream.WriteInt(X);
            stream.WriteByte(Y);
            stream.WriteInt(Z);
            stream.WriteByte(Face);
            stream.Purge();
        }
    }
}