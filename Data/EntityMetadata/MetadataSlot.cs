﻿using fNbt;
using MineLib.Network.IO;

namespace MineLib.Network.Data.EntityMetadata
{
    public class MetadataSlot : MetadataEntry
    {
        public override byte Identifier { get { return 5; } }
        public override string FriendlyName { get { return "slot"; } }

        public ItemStack Value;

        public static implicit operator MetadataSlot(ItemStack value)
        {
            return new MetadataSlot(value);
        }

        public MetadataSlot()
        {
        }

        public MetadataSlot(ItemStack value)
        {
            Value = value;
        }

        public override void FromReader(PacketByteReader reader)
        {
            Value = ItemStack.FromReader(reader);
        }

        public override void WriteTo(ref PacketStream stream, byte index)
        {
            stream.WriteByte(GetKey(index));
            stream.WriteShort(Value.Id);
            if (Value.Id != -1)
            {
                stream.WriteSByte(Value.Count);
                stream.WriteShort(Value.Metadata);
                if (Value.Nbt != null)
                {
                    var file = new NbtFile(Value.Nbt);
                    byte[] data = file.SaveToBuffer(NbtCompression.GZip);
                    stream.WriteShort((short) data.Length);
                    stream.WriteByteArray(data);
                }
                else
                    stream.WriteShort(-1);
            }
        }
    }
}