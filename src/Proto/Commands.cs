// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: Commands.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Commands
{

    [global::ProtoBuf.ProtoContract()]
    public partial class RemoteArgs : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<Arg> args { get; } = new global::System.Collections.Generic.List<Arg>();

        [global::ProtoBuf.ProtoContract()]
        public partial class Arg : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1)]
            public bool @bool
            {
                get => __pbn__bool.GetValueOrDefault();
                set => __pbn__bool = value;
            }
            public bool ShouldSerializebool() => __pbn__bool != null;
            public void Resetbool() => __pbn__bool = null;
            private bool? __pbn__bool;

            [global::ProtoBuf.ProtoMember(2)]
            public int int32
            {
                get => __pbn__int32.GetValueOrDefault();
                set => __pbn__int32 = value;
            }
            public bool ShouldSerializeint32() => __pbn__int32 != null;
            public void Resetint32() => __pbn__int32 = null;
            private int? __pbn__int32;

            [global::ProtoBuf.ProtoMember(3)]
            public long int64
            {
                get => __pbn__int64.GetValueOrDefault();
                set => __pbn__int64 = value;
            }
            public bool ShouldSerializeint64() => __pbn__int64 != null;
            public void Resetint64() => __pbn__int64 = null;
            private long? __pbn__int64;

            [global::ProtoBuf.ProtoMember(4)]
            public uint uint32
            {
                get => __pbn__uint32.GetValueOrDefault();
                set => __pbn__uint32 = value;
            }
            public bool ShouldSerializeuint32() => __pbn__uint32 != null;
            public void Resetuint32() => __pbn__uint32 = null;
            private uint? __pbn__uint32;

            [global::ProtoBuf.ProtoMember(5)]
            public ulong uint64
            {
                get => __pbn__uint64.GetValueOrDefault();
                set => __pbn__uint64 = value;
            }
            public bool ShouldSerializeuint64() => __pbn__uint64 != null;
            public void Resetuint64() => __pbn__uint64 = null;
            private ulong? __pbn__uint64;

            [global::ProtoBuf.ProtoMember(6)]
            public float @float
            {
                get => __pbn__float.GetValueOrDefault();
                set => __pbn__float = value;
            }
            public bool ShouldSerializefloat() => __pbn__float != null;
            public void Resetfloat() => __pbn__float = null;
            private float? __pbn__float;

            [global::ProtoBuf.ProtoMember(7)]
            [global::System.ComponentModel.DefaultValue("")]
            public string @string
            {
                get => __pbn__string ?? "";
                set => __pbn__string = value;
            }
            public bool ShouldSerializestring() => __pbn__string != null;
            public void Resetstring() => __pbn__string = null;
            private string __pbn__string;

        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RemoteUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<Command> commands { get; } = new global::System.Collections.Generic.List<Command>();

        [global::ProtoBuf.ProtoContract()]
        public partial class Command : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public string name { get; set; }

            [global::ProtoBuf.ProtoMember(2)]
            [global::System.ComponentModel.DefaultValue("")]
            public string desc
            {
                get => __pbn__desc ?? "";
                set => __pbn__desc = value;
            }
            public bool ShouldSerializedesc() => __pbn__desc != null;
            public void Resetdesc() => __pbn__desc = null;
            private string __pbn__desc;

            [global::ProtoBuf.ProtoMember(3)]
            [global::System.ComponentModel.DefaultValue("")]
            public string usage
            {
                get => __pbn__usage ?? "";
                set => __pbn__usage = value;
            }
            public bool ShouldSerializeusage() => __pbn__usage != null;
            public void Resetusage() => __pbn__usage = null;
            private string __pbn__usage;

        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CommandResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public bool resultcode { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string resultString
        {
            get => __pbn__resultString ?? "";
            set => __pbn__resultString = value;
        }
        public bool ShouldSerializeresultString() => __pbn__resultString != null;
        public void ResetresultString() => __pbn__resultString = null;
        private string __pbn__resultString;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class KeyValResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<KeyVal> keyvals { get; } = new global::System.Collections.Generic.List<KeyVal>();

        [global::ProtoBuf.ProtoContract()]
        public partial class KeyVal : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public string key { get; set; }

            [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
            public string value { get; set; }

        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SimTravelCommand : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong destZoneID { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string selectedSims { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
