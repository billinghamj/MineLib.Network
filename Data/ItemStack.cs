﻿using System;
using System.IO;
using MineLib.Network.IO;
using fNbt;
//using fNbt.Serialization;

namespace MineLib.Network.Data
{
    public struct ItemStack : ICloneable, IEquatable<ItemStack>
    {
        public ItemStack(short id)
            : this()
        {
            _Id = id;
            _Count = 1;
            Metadata = 0;
            Nbt = null;
            Index = 0;
        }

        public ItemStack(short id, sbyte count)
            : this(id)
        {
            Count = count;
        }

        public ItemStack(short id, sbyte count, short metadata)
            : this(id, count)
        {
            Metadata = metadata;
        }

        public ItemStack(short id, sbyte count, short metadata, NbtCompound nbt)
            : this(id, count, metadata)
        {
            Nbt = nbt;
            if (Count == 0)
            {
                Id = -1;
                Metadata = 0;
                Nbt = null;
            }
        }

        public override string ToString()
        {
            if (Empty)
                return "(Empty)";
            string result = "ID: " + Id;
            if (Count != 1) result += "; Count: " + Count;
            if (Metadata != 0) result += "; Metadata: " + Metadata;
            if (Nbt != null) result += Environment.NewLine + Nbt;
            return "(" + result + ")";
        }

        #region Operators

        public static bool operator ==(ItemStack left, ItemStack right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemStack left, ItemStack right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Network

        public static ItemStack FromReader(PacketByteReader reader)
        {
            var slot = EmptyStack;
            slot.Id = reader.ReadShort();
            if (slot.Empty)
                return slot;
            slot.Count = reader.ReadSByte();
            slot.Metadata = reader.ReadShort();
            var length = reader.ReadShort();
            if (length == -1)
                return slot;
            slot.Nbt = new NbtCompound();
            var buffer = reader.ReadByteArray(length);
            var nbt = new NbtFile();
            nbt.LoadFromBuffer(buffer, 0, length, NbtCompression.GZip, null);
            slot.Nbt = nbt.RootTag;
            return slot;
        }

        public void WriteTo(ref PacketStream stream)
        {
            stream.WriteShort(Id);
            if (Empty)
                return;
            stream.WriteSByte(Count);
            stream.WriteShort(Metadata);
            if (Nbt == null)
            {
                stream.WriteShort(-1);
                return;
            }
            var mStream = new MemoryStream();
            var file = new NbtFile(Nbt);
            file.SaveToStream(mStream, NbtCompression.GZip);
            stream.WriteShort((short)mStream.Position);
            //stream.writeVarIntArray(mStream.GetBuffer(), 0, (int)mStream.Position);
        }

        #endregion

        public static ItemStack FromNbt(NbtCompound compound)
        {
            var s = ItemStack.EmptyStack;
            s.Id = compound.Get<NbtShort>("id").Value;
            s.Metadata = compound.Get<NbtShort>("Damage").Value;
            s.Count = (sbyte)compound.Get<NbtByte>("Count").Value;
            s.Index = compound.Get<NbtByte>("Slot").Value;
            if (compound.Get<NbtCompound>("tag") != null)
                s.Nbt = compound.Get<NbtCompound>("tag");
            return s;
        }

        public NbtCompound ToNbt()
        {
            var c = new NbtCompound
            {
                new NbtShort("id", Id),
                new NbtShort("Damage", Metadata),
                new NbtByte("Count", (byte) Count),
                new NbtByte("Slot", (byte) Index)
            };
            if (Nbt != null)
                c.Add(new NbtCompound("tag"));
            return c;
        }

        //[NbtIgnore]
        public bool Empty
        {
            get { return Id == -1; }
        }

        public short Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                if (_Id == -1)
                {
                    _Count = 0;
                    Metadata = 0;
                    Nbt = null;
                }
            }
        }

        public sbyte Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                if (_Count == 0)
                {
                    _Id = -1;
                    Metadata = 0;
                    Nbt = null;
                }
            }
        }

        public short Metadata
        {
            get { return _Metadata; }
            set { _Metadata = value; }
        }

        private short _Id;
        private sbyte _Count;
        private short _Metadata;

        //[IgnoreOnNull]
        public NbtCompound Nbt { get; set; }

        //[NbtIgnore]
        public int Index;

        public object Clone()
        {
            return new ItemStack(Id, Count, Metadata, Nbt);
        }

        //[NbtIgnore]
        public static ItemStack EmptyStack
        {
            get
            {
                return new ItemStack(-1);
            }
        }

        public bool CanMerge(ItemStack other)
        {
            if (Empty || other.Empty)
                return true;
            return _Id == other._Id && _Metadata == other._Metadata && Equals(Nbt, other.Nbt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ItemStack && Equals((ItemStack)obj);
        }

        public bool Equals(ItemStack other)
        {
            return _Id == other._Id && _Count == other._Count && _Metadata == other._Metadata && Index == other.Index && Equals(Nbt, other.Nbt);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _Id.GetHashCode();
                hashCode = (hashCode * 397) ^ _Count.GetHashCode();
                hashCode = (hashCode * 397) ^ _Metadata.GetHashCode();
                hashCode = (hashCode * 397) ^ Index;
                hashCode = (hashCode * 397) ^ (Nbt != null ? Nbt.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
