using System.Security.Cryptography;
using MineLib.Network.IO;

namespace MineLib.Network.Packets.Server.Login
{
    public struct EncryptionRequestPacket : IPacket
    {
        public string ServerId;
        public byte[] PublicKey;
        public byte[] SharedKey;
        public byte[] VerificationToken;

        public const byte PacketID = 0x01;
        public byte Id { get { return PacketID; } }

        public void ReadPacket(PacketByteReader stream)
        {
            ServerId = stream.ReadString();
            var pkLength = stream.ReadShort();
            PublicKey = stream.ReadByteArray(pkLength);
            var vtLength = stream.ReadShort();
            VerificationToken = stream.ReadByteArray(vtLength);

            SharedKey = new byte[16];

            var random = RandomNumberGenerator.Create();
            random.GetBytes(SharedKey);
        }

        public void WritePacket(ref PacketStream stream)
        {
            stream.WriteVarInt(Id);
            stream.WriteString(ServerId);
            stream.WriteShort((short)PublicKey.Length);
            stream.WriteByteArray(PublicKey);
            stream.WriteShort((short)VerificationToken.Length);
            stream.WriteByteArray(VerificationToken);
            stream.Purge();
        }
    }
}