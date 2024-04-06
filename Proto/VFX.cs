// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: VFX.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class VFXStart : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong object_id
        {
            get => __pbn__object_id.GetValueOrDefault();
            set => __pbn__object_id = value;
        }
        public bool ShouldSerializeobject_id() => __pbn__object_id != null;
        public void Resetobject_id() => __pbn__object_id = null;
        private ulong? __pbn__object_id;

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string effect_name { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public ulong actor_id { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public uint joint_name_hash { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public ulong target_actor_id
        {
            get => __pbn__target_actor_id.GetValueOrDefault();
            set => __pbn__target_actor_id = value;
        }
        public bool ShouldSerializetarget_actor_id() => __pbn__target_actor_id != null;
        public void Resettarget_actor_id() => __pbn__target_actor_id = null;
        private ulong? __pbn__target_actor_id;

        [global::ProtoBuf.ProtoMember(6)]
        public uint target_joint_name_hash
        {
            get => __pbn__target_joint_name_hash.GetValueOrDefault();
            set => __pbn__target_joint_name_hash = value;
        }
        public bool ShouldSerializetarget_joint_name_hash() => __pbn__target_joint_name_hash != null;
        public void Resettarget_joint_name_hash() => __pbn__target_joint_name_hash = null;
        private uint? __pbn__target_joint_name_hash;

        [global::ProtoBuf.ProtoMember(7)]
        [global::System.ComponentModel.DefaultValue(false)]
        public bool mirror_effect
        {
            get => __pbn__mirror_effect ?? false;
            set => __pbn__mirror_effect = value;
        }
        public bool ShouldSerializemirror_effect() => __pbn__mirror_effect != null;
        public void Resetmirror_effect() => __pbn__mirror_effect = null;
        private bool? __pbn__mirror_effect;

        [global::ProtoBuf.ProtoMember(8)]
        [global::System.ComponentModel.DefaultValue(false)]
        public bool auto_on_effect
        {
            get => __pbn__auto_on_effect ?? false;
            set => __pbn__auto_on_effect = value;
        }
        public bool ShouldSerializeauto_on_effect() => __pbn__auto_on_effect != null;
        public void Resetauto_on_effect() => __pbn__auto_on_effect = null;
        private bool? __pbn__auto_on_effect;

        [global::ProtoBuf.ProtoMember(9)]
        [global::System.ComponentModel.DefaultValue(VFXStartTransitionType.SOFT_TRANSITION)]
        public VFXStartTransitionType transition_type
        {
            get => __pbn__transition_type ?? VFXStartTransitionType.SOFT_TRANSITION;
            set => __pbn__transition_type = value;
        }
        public bool ShouldSerializetransition_type() => __pbn__transition_type != null;
        public void Resettransition_type() => __pbn__transition_type = null;
        private VFXStartTransitionType? __pbn__transition_type;

        [global::ProtoBuf.ProtoMember(10)]
        public Transform transform { get; set; }

        [global::ProtoBuf.ProtoMember(11)]
        public Vector3 target_joint_offset { get; set; }

        [global::ProtoBuf.ProtoMember(12)]
        public uint callback_event_id
        {
            get => __pbn__callback_event_id.GetValueOrDefault();
            set => __pbn__callback_event_id = value;
        }
        public bool ShouldSerializecallback_event_id() => __pbn__callback_event_id != null;
        public void Resetcallback_event_id() => __pbn__callback_event_id = null;
        private uint? __pbn__callback_event_id;

        [global::ProtoBuf.ProtoContract()]
        public enum VFXStartTransitionType
        {
            SOFT_TRANSITION = 0,
            HARD_TRANSITION = 1,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class VFXStop : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong object_id { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public ulong actor_id { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue(VFXStopTransitionType.SOFT_TRANSITION)]
        public VFXStopTransitionType transition_type
        {
            get => __pbn__transition_type ?? VFXStopTransitionType.SOFT_TRANSITION;
            set => __pbn__transition_type = value;
        }
        public bool ShouldSerializetransition_type() => __pbn__transition_type != null;
        public void Resettransition_type() => __pbn__transition_type = null;
        private VFXStopTransitionType? __pbn__transition_type;

        [global::ProtoBuf.ProtoContract()]
        public enum VFXStopTransitionType
        {
            SOFT_TRANSITION = 0,
            HARD_TRANSITION = 1,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class VFXSetState : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong object_id { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public ulong actor_id { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public int state_index { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        [global::System.ComponentModel.DefaultValue(VFXTransitionType.HARD_TRANSITION)]
        public VFXTransitionType transition_type
        {
            get => __pbn__transition_type ?? VFXTransitionType.HARD_TRANSITION;
            set => __pbn__transition_type = value;
        }
        public bool ShouldSerializetransition_type() => __pbn__transition_type != null;
        public void Resettransition_type() => __pbn__transition_type = null;
        private VFXTransitionType? __pbn__transition_type;

    }

    [global::ProtoBuf.ProtoContract()]
    public enum VFXTransitionType
    {
        SOFT_TRANSITION = 0,
        HARD_TRANSITION = 1,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion