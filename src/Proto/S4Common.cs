// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: S4Common.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4
{

    [global::ProtoBuf.ProtoContract()]
    public partial class IdList : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong[] ids { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class GameInstanceInfoPB : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong zone_id { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public uint world_id
        {
            get => __pbn__world_id.GetValueOrDefault();
            set => __pbn__world_id = value;
        }
        public bool ShouldSerializeworld_id() => __pbn__world_id != null;
        public void Resetworld_id() => __pbn__world_id = null;
        private uint? __pbn__world_id;

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string neighborhood_name
        {
            get => __pbn__neighborhood_name ?? "";
            set => __pbn__neighborhood_name = value;
        }
        public bool ShouldSerializeneighborhood_name() => __pbn__neighborhood_name != null;
        public void Resetneighborhood_name() => __pbn__neighborhood_name = null;
        private string __pbn__neighborhood_name;

        [global::ProtoBuf.ProtoMember(4)]
        [global::System.ComponentModel.DefaultValue("")]
        public string zone_name
        {
            get => __pbn__zone_name ?? "";
            set => __pbn__zone_name = value;
        }
        public bool ShouldSerializezone_name() => __pbn__zone_name != null;
        public void Resetzone_name() => __pbn__zone_name = null;
        private string __pbn__zone_name;

        [global::ProtoBuf.ProtoMember(5)]
        public ulong zone_session_id
        {
            get => __pbn__zone_session_id.GetValueOrDefault();
            set => __pbn__zone_session_id = value;
        }
        public bool ShouldSerializezone_session_id() => __pbn__zone_session_id != null;
        public void Resetzone_session_id() => __pbn__zone_session_id = null;
        private ulong? __pbn__zone_session_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserEntitlement : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong entitlement_id
        {
            get => __pbn__entitlement_id.GetValueOrDefault();
            set => __pbn__entitlement_id = value;
        }
        public bool ShouldSerializeentitlement_id() => __pbn__entitlement_id != null;
        public void Resetentitlement_id() => __pbn__entitlement_id = null;
        private ulong? __pbn__entitlement_id;

        [global::ProtoBuf.ProtoMember(2)]
        public uint version
        {
            get => __pbn__version.GetValueOrDefault();
            set => __pbn__version = value;
        }
        public bool ShouldSerializeversion() => __pbn__version != null;
        public void Resetversion() => __pbn__version = null;
        private uint? __pbn__version;

        [global::ProtoBuf.ProtoMember(3)]
        public ulong product_id
        {
            get => __pbn__product_id.GetValueOrDefault();
            set => __pbn__product_id = value;
        }
        public bool ShouldSerializeproduct_id() => __pbn__product_id != null;
        public void Resetproduct_id() => __pbn__product_id = null;
        private ulong? __pbn__product_id;

        [global::ProtoBuf.ProtoMember(4)]
        public ulong last_modified_date
        {
            get => __pbn__last_modified_date.GetValueOrDefault();
            set => __pbn__last_modified_date = value;
        }
        public bool ShouldSerializelast_modified_date() => __pbn__last_modified_date != null;
        public void Resetlast_modified_date() => __pbn__last_modified_date = null;
        private ulong? __pbn__last_modified_date;

        [global::ProtoBuf.ProtoMember(5)]
        public ulong product_sku
        {
            get => __pbn__product_sku.GetValueOrDefault();
            set => __pbn__product_sku = value;
        }
        public bool ShouldSerializeproduct_sku() => __pbn__product_sku != null;
        public void Resetproduct_sku() => __pbn__product_sku = null;
        private ulong? __pbn__product_sku;

        [global::ProtoBuf.ProtoMember(6)]
        [global::System.ComponentModel.DefaultValue(0u)]
        public uint view_state
        {
            get => __pbn__view_state ?? 0u;
            set => __pbn__view_state = value;
        }
        public bool ShouldSerializeview_state() => __pbn__view_state != null;
        public void Resetview_state() => __pbn__view_state = null;
        private uint? __pbn__view_state;

        [global::ProtoBuf.ProtoMember(7)]
        [global::System.ComponentModel.DefaultValue(100u)]
        public uint install_state
        {
            get => __pbn__install_state ?? 100u;
            set => __pbn__install_state = value;
        }
        public bool ShouldSerializeinstall_state() => __pbn__install_state != null;
        public void Resetinstall_state() => __pbn__install_state = null;
        private uint? __pbn__install_state;

        [global::ProtoBuf.ProtoMember(9)]
        public ulong terminate_date
        {
            get => __pbn__terminate_date.GetValueOrDefault();
            set => __pbn__terminate_date = value;
        }
        public bool ShouldSerializeterminate_date() => __pbn__terminate_date != null;
        public void Resetterminate_date() => __pbn__terminate_date = null;
        private ulong? __pbn__terminate_date;

        [global::ProtoBuf.ProtoMember(10)]
        public uint trial_state
        {
            get => __pbn__trial_state.GetValueOrDefault();
            set => __pbn__trial_state = value;
        }
        public bool ShouldSerializetrial_state() => __pbn__trial_state != null;
        public void Resettrial_state() => __pbn__trial_state = null;
        private uint? __pbn__trial_state;

        [global::ProtoBuf.ProtoMember(11)]
        public ulong grant_date
        {
            get => __pbn__grant_date.GetValueOrDefault();
            set => __pbn__grant_date = value;
        }
        public bool ShouldSerializegrant_date() => __pbn__grant_date != null;
        public void Resetgrant_date() => __pbn__grant_date = null;
        private ulong? __pbn__grant_date;

        [global::ProtoBuf.ProtoMember(12)]
        public uint trial_view_state
        {
            get => __pbn__trial_view_state.GetValueOrDefault();
            set => __pbn__trial_view_state = value;
        }
        public bool ShouldSerializetrial_view_state() => __pbn__trial_view_state != null;
        public void Resettrial_view_state() => __pbn__trial_view_state = null;
        private uint? __pbn__trial_view_state;

        [global::ProtoBuf.ProtoContract()]
        public enum TrialStateMask
        {
            MASK_TRIAL_ENTITLEMENT = 16,
        }

        [global::ProtoBuf.ProtoContract()]
        public enum TrialState
        {
            TRIAL_STATE_NONE = 0,
            TRIAL_STATE_CONVERTED = 1,
            TRIAL_STATE_TRIAL_ACTIVE = 16,
            TRIAL_STATE_TRIAL_EXPIRED = 17,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserEntitlementMap : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<UserEntitlement> entitlements { get; } = new global::System.Collections.Generic.List<UserEntitlement>();

        [global::ProtoBuf.ProtoMember(2)]
        public ulong last_modified_date
        {
            get => __pbn__last_modified_date.GetValueOrDefault();
            set => __pbn__last_modified_date = value;
        }
        public bool ShouldSerializelast_modified_date() => __pbn__last_modified_date != null;
        public void Resetlast_modified_date() => __pbn__last_modified_date = null;
        private ulong? __pbn__last_modified_date;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class AchievementItem : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public uint id
        {
            get => __pbn__id.GetValueOrDefault();
            set => __pbn__id = value;
        }
        public bool ShouldSerializeid() => __pbn__id != null;
        public void Resetid() => __pbn__id = null;
        private uint? __pbn__id;

        [global::ProtoBuf.ProtoMember(2)]
        public uint progress
        {
            get => __pbn__progress.GetValueOrDefault();
            set => __pbn__progress = value;
        }
        public bool ShouldSerializeprogress() => __pbn__progress != null;
        public void Resetprogress() => __pbn__progress = null;
        private uint? __pbn__progress;

        [global::ProtoBuf.ProtoMember(3)]
        public uint totalpoints
        {
            get => __pbn__totalpoints.GetValueOrDefault();
            set => __pbn__totalpoints = value;
        }
        public bool ShouldSerializetotalpoints() => __pbn__totalpoints != null;
        public void Resettotalpoints() => __pbn__totalpoints = null;
        private uint? __pbn__totalpoints;

        [global::ProtoBuf.ProtoMember(4)]
        public uint repeatcount
        {
            get => __pbn__repeatcount.GetValueOrDefault();
            set => __pbn__repeatcount = value;
        }
        public bool ShouldSerializerepeatcount() => __pbn__repeatcount != null;
        public void Resetrepeatcount() => __pbn__repeatcount = null;
        private uint? __pbn__repeatcount;

        [global::ProtoBuf.ProtoMember(5)]
        [global::System.ComponentModel.DefaultValue("")]
        public string name
        {
            get => __pbn__name ?? "";
            set => __pbn__name = value;
        }
        public bool ShouldSerializename() => __pbn__name != null;
        public void Resetname() => __pbn__name = null;
        private string __pbn__name;

        [global::ProtoBuf.ProtoMember(6)]
        [global::System.ComponentModel.DefaultValue("")]
        public string desc
        {
            get => __pbn__desc ?? "";
            set => __pbn__desc = value;
        }
        public bool ShouldSerializedesc() => __pbn__desc != null;
        public void Resetdesc() => __pbn__desc = null;
        private string __pbn__desc;

        [global::ProtoBuf.ProtoMember(7)]
        [global::System.ComponentModel.DefaultValue("")]
        public string howto
        {
            get => __pbn__howto ?? "";
            set => __pbn__howto = value;
        }
        public bool ShouldSerializehowto() => __pbn__howto != null;
        public void Resethowto() => __pbn__howto = null;
        private string __pbn__howto;

        [global::ProtoBuf.ProtoMember(8)]
        [global::System.ComponentModel.DefaultValue("")]
        public string imageid
        {
            get => __pbn__imageid ?? "";
            set => __pbn__imageid = value;
        }
        public bool ShouldSerializeimageid() => __pbn__imageid != null;
        public void Resetimageid() => __pbn__imageid = null;
        private string __pbn__imageid;

        [global::ProtoBuf.ProtoMember(9)]
        public ulong grantdate
        {
            get => __pbn__grantdate.GetValueOrDefault();
            set => __pbn__grantdate = value;
        }
        public bool ShouldSerializegrantdate() => __pbn__grantdate != null;
        public void Resetgrantdate() => __pbn__grantdate = null;
        private ulong? __pbn__grantdate;

        [global::ProtoBuf.ProtoMember(10)]
        public ulong expiredate
        {
            get => __pbn__expiredate.GetValueOrDefault();
            set => __pbn__expiredate = value;
        }
        public bool ShouldSerializeexpiredate() => __pbn__expiredate != null;
        public void Resetexpiredate() => __pbn__expiredate = null;
        private ulong? __pbn__expiredate;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class AchievementList : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string name
        {
            get => __pbn__name ?? "";
            set => __pbn__name = value;
        }
        public bool ShouldSerializename() => __pbn__name != null;
        public void Resetname() => __pbn__name = null;
        private string __pbn__name;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string gamename
        {
            get => __pbn__gamename ?? "";
            set => __pbn__gamename = value;
        }
        public bool ShouldSerializegamename() => __pbn__gamename != null;
        public void Resetgamename() => __pbn__gamename = null;
        private string __pbn__gamename;

        [global::ProtoBuf.ProtoMember(3)]
        public global::System.Collections.Generic.List<AchievementItem> achievements { get; } = new global::System.Collections.Generic.List<AchievementItem>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class AchievementMsg : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public int resultcode
        {
            get => __pbn__resultcode.GetValueOrDefault();
            set => __pbn__resultcode = value;
        }
        public bool ShouldSerializeresultcode() => __pbn__resultcode != null;
        public void Resetresultcode() => __pbn__resultcode = null;
        private int? __pbn__resultcode;

        [global::ProtoBuf.ProtoMember(2)]
        public uint mode
        {
            get => __pbn__mode.GetValueOrDefault();
            set => __pbn__mode = value;
        }
        public bool ShouldSerializemode() => __pbn__mode != null;
        public void Resetmode() => __pbn__mode = null;
        private uint? __pbn__mode;

        [global::ProtoBuf.ProtoMember(3)]
        public global::System.Collections.Generic.List<AchievementList> lists { get; } = new global::System.Collections.Generic.List<AchievementList>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserShoppingCartItem : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue("")]
        public string entitlement_tag
        {
            get => __pbn__entitlement_tag ?? "";
            set => __pbn__entitlement_tag = value;
        }
        public bool ShouldSerializeentitlement_tag() => __pbn__entitlement_tag != null;
        public void Resetentitlement_tag() => __pbn__entitlement_tag = null;
        private string __pbn__entitlement_tag;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string offer_id
        {
            get => __pbn__offer_id ?? "";
            set => __pbn__offer_id = value;
        }
        public bool ShouldSerializeoffer_id() => __pbn__offer_id != null;
        public void Resetoffer_id() => __pbn__offer_id = null;
        private string __pbn__offer_id;

        [global::ProtoBuf.ProtoMember(3)]
        public uint quantity
        {
            get => __pbn__quantity.GetValueOrDefault();
            set => __pbn__quantity = value;
        }
        public bool ShouldSerializequantity() => __pbn__quantity != null;
        public void Resetquantity() => __pbn__quantity = null;
        private uint? __pbn__quantity;

        [global::ProtoBuf.ProtoMember(4)]
        public double override_price
        {
            get => __pbn__override_price.GetValueOrDefault();
            set => __pbn__override_price = value;
        }
        public bool ShouldSerializeoverride_price() => __pbn__override_price != null;
        public void Resetoverride_price() => __pbn__override_price = null;
        private double? __pbn__override_price;

        [global::ProtoBuf.ProtoMember(5)]
        public double unit_price
        {
            get => __pbn__unit_price.GetValueOrDefault();
            set => __pbn__unit_price = value;
        }
        public bool ShouldSerializeunit_price() => __pbn__unit_price != null;
        public void Resetunit_price() => __pbn__unit_price = null;
        private double? __pbn__unit_price;

        [global::ProtoBuf.ProtoMember(6)]
        public ulong ientitlement_tag
        {
            get => __pbn__ientitlement_tag.GetValueOrDefault();
            set => __pbn__ientitlement_tag = value;
        }
        public bool ShouldSerializeientitlement_tag() => __pbn__ientitlement_tag != null;
        public void Resetientitlement_tag() => __pbn__ientitlement_tag = null;
        private ulong? __pbn__ientitlement_tag;

        [global::ProtoBuf.ProtoMember(7)]
        public ulong entry_id
        {
            get => __pbn__entry_id.GetValueOrDefault();
            set => __pbn__entry_id = value;
        }
        public bool ShouldSerializeentry_id() => __pbn__entry_id != null;
        public void Resetentry_id() => __pbn__entry_id = null;
        private ulong? __pbn__entry_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UserShoppingCart : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<UserShoppingCartItem> items { get; } = new global::System.Collections.Generic.List<UserShoppingCartItem>();

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string last_modified_date
        {
            get => __pbn__last_modified_date ?? "";
            set => __pbn__last_modified_date = value;
        }
        public bool ShouldSerializelast_modified_date() => __pbn__last_modified_date != null;
        public void Resetlast_modified_date() => __pbn__last_modified_date = null;
        private string __pbn__last_modified_date;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Uint64Value : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong value { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Uint64List : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<Uint64Value> values { get; } = new global::System.Collections.Generic.List<Uint64Value>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class BoolValue : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public bool value { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class HouseholdSimIds : global::ProtoBuf.IExtensible
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

        [global::ProtoBuf.ProtoMember(2, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong[] sim_ids { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Schedule : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<ScheduleEntry> schedule_entries { get; } = new global::System.Collections.Generic.List<ScheduleEntry>();

        [global::ProtoBuf.ProtoContract()]
        public partial class ScheduleEntry : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1)]
            public global::System.Collections.Generic.List<ScheduleDay> days { get; } = new global::System.Collections.Generic.List<ScheduleDay>();

            [global::ProtoBuf.ProtoMember(2)]
            public uint start_hour
            {
                get => __pbn__start_hour.GetValueOrDefault();
                set => __pbn__start_hour = value;
            }
            public bool ShouldSerializestart_hour() => __pbn__start_hour != null;
            public void Resetstart_hour() => __pbn__start_hour = null;
            private uint? __pbn__start_hour;

            [global::ProtoBuf.ProtoMember(3)]
            public uint start_minute
            {
                get => __pbn__start_minute.GetValueOrDefault();
                set => __pbn__start_minute = value;
            }
            public bool ShouldSerializestart_minute() => __pbn__start_minute != null;
            public void Resetstart_minute() => __pbn__start_minute = null;
            private uint? __pbn__start_minute;

            [global::ProtoBuf.ProtoMember(4)]
            public float duration
            {
                get => __pbn__duration.GetValueOrDefault();
                set => __pbn__duration = value;
            }
            public bool ShouldSerializeduration() => __pbn__duration != null;
            public void Resetduration() => __pbn__duration = null;
            private float? __pbn__duration;

            [global::ProtoBuf.ProtoMember(5)]
            [global::System.ComponentModel.DefaultValue(ScheduleShiftType.ALL_DAY)]
            public ScheduleShiftType schedule_shift_type
            {
                get => __pbn__schedule_shift_type ?? ScheduleShiftType.ALL_DAY;
                set => __pbn__schedule_shift_type = value;
            }
            public bool ShouldSerializeschedule_shift_type() => __pbn__schedule_shift_type != null;
            public void Resetschedule_shift_type() => __pbn__schedule_shift_type = null;
            private ScheduleShiftType? __pbn__schedule_shift_type;

            [global::ProtoBuf.ProtoContract()]
            public enum ScheduleDay
            {
                SUNDAY = 0,
                MONDAY = 1,
                TUESDAY = 2,
                WEDNESDAY = 3,
                THURSDAY = 4,
                FRIDAY = 5,
                SATURDAY = 6,
            }

            [global::ProtoBuf.ProtoContract()]
            public enum ScheduleShiftType
            {
                ALL_DAY = 0,
                MORNING = 1,
                EVENING = 2,
            }

        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SimPronoun : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        [global::System.ComponentModel.DefaultValue(GrammaticalCase.UNKNOWN)]
        public GrammaticalCase @case
        {
            get => __pbn__case ?? GrammaticalCase.UNKNOWN;
            set => __pbn__case = value;
        }
        public bool ShouldSerializecase() => __pbn__case != null;
        public void Resetcase() => __pbn__case = null;
        private GrammaticalCase? __pbn__case;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue("")]
        public string pronoun
        {
            get => __pbn__pronoun ?? "";
            set => __pbn__pronoun = value;
        }
        public bool ShouldSerializepronoun() => __pbn__pronoun != null;
        public void Resetpronoun() => __pbn__pronoun = null;
        private string __pbn__pronoun;

        [global::ProtoBuf.ProtoContract()]
        public enum GrammaticalCase
        {
            UNKNOWN = 0,
            SUBJECTIVE = 1,
            OBJECTIVE = 2,
            POSSESSIVE_DEPENDENT = 3,
            POSSESSIVE_INDEPENDENT = 4,
            REFLEXIVE = 5,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SimPronounList : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<SimPronoun> pronouns { get; } = new global::System.Collections.Generic.List<SimPronoun>();

    }

    [global::ProtoBuf.ProtoContract()]
    public enum UserState
    {
        userstate_pending = 1,
        userstate_logged_in = 2,
        userstate_logged_out = 3,
        userstate_timedout_out = 4,
        userstate_bad_login = 5,
        connected_to_mtx_server = 100,
        connected_to_exchange_server = 200,
        connected_to_social_server = 300,
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
