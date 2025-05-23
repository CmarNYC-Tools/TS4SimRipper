// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: Roommates.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Persistence
{

    [global::ProtoBuf.ProtoContract()]
    public partial class RoommateLeaveReasonInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong reason
        {
            get => __pbn__reason.GetValueOrDefault();
            set => __pbn__reason = value;
        }
        public bool ShouldSerializereason() => __pbn__reason != null;
        public void Resetreason() => __pbn__reason = null;
        private ulong? __pbn__reason;

        [global::ProtoBuf.ProtoMember(2)]
        public ulong total_time
        {
            get => __pbn__total_time.GetValueOrDefault();
            set => __pbn__total_time = value;
        }
        public bool ShouldSerializetotal_time() => __pbn__total_time != null;
        public void Resettotal_time() => __pbn__total_time = null;
        private ulong? __pbn__total_time;

        [global::ProtoBuf.ProtoMember(3)]
        public bool been_warned
        {
            get => __pbn__been_warned.GetValueOrDefault();
            set => __pbn__been_warned = value;
        }
        public bool ShouldSerializebeen_warned() => __pbn__been_warned != null;
        public void Resetbeen_warned() => __pbn__been_warned = null;
        private bool? __pbn__been_warned;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RoommateInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong sim_id
        {
            get => __pbn__sim_id.GetValueOrDefault();
            set => __pbn__sim_id = value;
        }
        public bool ShouldSerializesim_id() => __pbn__sim_id != null;
        public void Resetsim_id() => __pbn__sim_id = null;
        private ulong? __pbn__sim_id;

        [global::ProtoBuf.ProtoMember(2)]
        public ulong bed_id
        {
            get => __pbn__bed_id.GetValueOrDefault();
            set => __pbn__bed_id = value;
        }
        public bool ShouldSerializebed_id() => __pbn__bed_id != null;
        public void Resetbed_id() => __pbn__bed_id = null;
        private ulong? __pbn__bed_id;

        [global::ProtoBuf.ProtoMember(3)]
        public ulong[] decoration_ids { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public global::System.Collections.Generic.List<RoommateLeaveReasonInfo> leave_reason_infos { get; } = new global::System.Collections.Generic.List<RoommateLeaveReasonInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RoommateBlacklistSimInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong sim_id
        {
            get => __pbn__sim_id.GetValueOrDefault();
            set => __pbn__sim_id = value;
        }
        public bool ShouldSerializesim_id() => __pbn__sim_id != null;
        public void Resetsim_id() => __pbn__sim_id = null;
        private ulong? __pbn__sim_id;

        [global::ProtoBuf.ProtoMember(2)]
        public ulong time_left
        {
            get => __pbn__time_left.GetValueOrDefault();
            set => __pbn__time_left = value;
        }
        public bool ShouldSerializetime_left() => __pbn__time_left != null;
        public void Resettime_left() => __pbn__time_left = null;
        private ulong? __pbn__time_left;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RoommateAdInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong household_id
        {
            get => __pbn__household_id.GetValueOrDefault();
            set => __pbn__household_id = value;
        }
        public bool ShouldSerializehousehold_id() => __pbn__household_id != null;
        public void Resethousehold_id() => __pbn__household_id = null;
        private ulong? __pbn__household_id;

        [global::ProtoBuf.ProtoMember(2)]
        public ulong[] pending_interview_alarms { get; set; }

        [global::ProtoBuf.ProtoMember(3, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong[] interviewee_ids { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RoommateData : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong zone_id
        {
            get => __pbn__zone_id.GetValueOrDefault();
            set => __pbn__zone_id = value;
        }
        public bool ShouldSerializezone_id() => __pbn__zone_id != null;
        public void Resetzone_id() => __pbn__zone_id = null;
        private ulong? __pbn__zone_id;

        [global::ProtoBuf.ProtoMember(2, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong household_id
        {
            get => __pbn__household_id.GetValueOrDefault();
            set => __pbn__household_id = value;
        }
        public bool ShouldSerializehousehold_id() => __pbn__household_id != null;
        public void Resethousehold_id() => __pbn__household_id = null;
        private ulong? __pbn__household_id;

        [global::ProtoBuf.ProtoMember(4)]
        public global::System.Collections.Generic.List<RoommateInfo> roommate_infos { get; } = new global::System.Collections.Generic.List<RoommateInfo>();

        [global::ProtoBuf.ProtoMember(5)]
        public global::System.Collections.Generic.List<RoommateBlacklistSimInfo> blacklist_infos { get; } = new global::System.Collections.Generic.List<RoommateBlacklistSimInfo>();

        [global::ProtoBuf.ProtoMember(7)]
        public ulong[] pending_destroy_decoration_ids { get; set; }

        [global::ProtoBuf.ProtoMember(8, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong locked_out_id
        {
            get => __pbn__locked_out_id.GetValueOrDefault();
            set => __pbn__locked_out_id = value;
        }
        public bool ShouldSerializelocked_out_id() => __pbn__locked_out_id != null;
        public void Resetlocked_out_id() => __pbn__locked_out_id = null;
        private ulong? __pbn__locked_out_id;

        [global::ProtoBuf.ProtoMember(9)]
        public int available_beds
        {
            get => __pbn__available_beds.GetValueOrDefault();
            set => __pbn__available_beds = value;
        }
        public bool ShouldSerializeavailable_beds() => __pbn__available_beds != null;
        public void Resetavailable_beds() => __pbn__available_beds = null;
        private int? __pbn__available_beds;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class PersistableRoommateService : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<RoommateData> roommate_datas { get; } = new global::System.Collections.Generic.List<RoommateData>();

        [global::ProtoBuf.ProtoMember(2)]
        public RoommateAdInfo ad_info { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
