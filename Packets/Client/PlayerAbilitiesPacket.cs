using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public byte Flags;
        private float FlyingSpeed;
        public float WalkingSpeed;

        public const byte PacketID = 0x13;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Flags = stream.ReadByte();
            FlyingSpeed = stream.ReadFloat();
            WalkingSpeed = stream.ReadFloat();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();
        }
    }
}