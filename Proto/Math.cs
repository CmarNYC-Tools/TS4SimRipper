// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: Math.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class Vector2 : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public float x { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public float y { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Vector3 : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public float x { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public float y { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public float z { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Quaternion : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public float x { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public float y { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public float z { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public float w { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Transform : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public Vector3 translation { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public Quaternion orientation { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class LinearCurve : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<CurvePoint> points { get; } = new global::System.Collections.Generic.List<CurvePoint>();

        [global::ProtoBuf.ProtoContract()]
        public partial class CurvePoint : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public float x { get; set; }

            [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
            public float y { get; set; }

        }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion