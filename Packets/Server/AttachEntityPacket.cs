using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server
{
    public struct AttachEntityPacket : IPacket
    {
        public int EntityID;
        public int VehicleID;
        public bool Leash;

        public const byte PacketID = 0x1B;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            EntityID = stream.ReadInt();
            VehicleID = stream.ReadInt();
            Leash = stream.ReadBoolean();
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteInt(EntityID);
            stream.WriteInt(VehicleID);
            stream.WriteBool(Leash);
            stream.Purge();
        }
    }
}