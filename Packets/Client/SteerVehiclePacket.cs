using MineLib.Network.IO;

namespace MineLib.Network.Packets.Client
{
    public struct SteerVehiclePacket : IPacket
    {
        public float Sideways;
        public float Forward;
        public bool Jump;
        public bool Unmount;

        public const byte PacketID = 0x0C;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            Sideways = stream.ReadFloat();
            Forward = stream.ReadFloat();
            Jump = stream.ReadBoolean();
            Unmount = stream.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteFloat(Sideways);
            stream.WriteFloat(Forward);
            stream.WriteBool(Jump);
            stream.WriteBool(Unmount);
            stream.Purge();
        }
    }
}