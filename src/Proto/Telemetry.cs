// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: Telemetry.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class TelemetryAttribute : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public uint name { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public Type type { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public bool boolval
        {
            get => __pbn__boolval.GetValueOrDefault();
            set => __pbn__boolval = value;
        }
        public bool ShouldSerializeboolval() => __pbn__boolval != null;
        public void Resetboolval() => __pbn__boolval = null;
        private bool? __pbn__boolval;

        [global::ProtoBuf.ProtoMember(4)]
        public int int32val
        {
            get => __pbn__int32val.GetValueOrDefault();
            set => __pbn__int32val = value;
        }
        public bool ShouldSerializeint32val() => __pbn__int32val != null;
        public void Resetint32val() => __pbn__int32val = null;
        private int? __pbn__int32val;

        [global::ProtoBuf.ProtoMember(5)]
        public uint uint32val
        {
            get => __pbn__uint32val.GetValueOrDefault();
            set => __pbn__uint32val = value;
        }
        public bool ShouldSerializeuint32val() => __pbn__uint32val != null;
        public void Resetuint32val() => __pbn__uint32val = null;
        private uint? __pbn__uint32val;

        [global::ProtoBuf.ProtoMember(6)]
        public float floatval
        {
            get => __pbn__floatval.GetValueOrDefault();
            set => __pbn__floatval = value;
        }
        public bool ShouldSerializefloatval() => __pbn__floatval != null;
        public void Resetfloatval() => __pbn__floatval = null;
        private float? __pbn__floatval;

        [global::ProtoBuf.ProtoMember(7)]
        [global::System.ComponentModel.DefaultValue("")]
        public string stringval
        {
            get => __pbn__stringval ?? "";
            set => __pbn__stringval = value;
        }
        public bool ShouldSerializestringval() => __pbn__stringval != null;
        public void Resetstringval() => __pbn__stringval = null;
        private string __pbn__stringval;

        [global::ProtoBuf.ProtoMember(8)]
        public ulong uint64val
        {
            get => __pbn__uint64val.GetValueOrDefault();
            set => __pbn__uint64val = value;
        }
        public bool ShouldSerializeuint64val() => __pbn__uint64val != null;
        public void Resetuint64val() => __pbn__uint64val = null;
        private ulong? __pbn__uint64val;

        [global::ProtoBuf.ProtoContract()]
        public enum Type
        {
            NONE = 0,
            BOOL = 1,
            INT32 = 2,
            UINT32 = 3,
            FLOAT = 4,
            STRING = 5,
            UINT64 = 6,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TelemetryMessage : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public uint module { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public uint group { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public uint name { get; set; }

        [global::ProtoBuf.ProtoMember(4, DataFormat = global::ProtoBuf.DataFormat.FixedSize, IsRequired = true)]
        public uint timestamp { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public global::System.Collections.Generic.List<TelemetryAttribute> attrs { get; } = new global::System.Collections.Generic.List<TelemetryAttribute>();

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
