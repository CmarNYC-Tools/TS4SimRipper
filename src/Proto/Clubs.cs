// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: Clubs.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class ClubCriteria : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ClubCriteriaCategory category { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<ClubCriteriaInfo> criteria_infos { get; } = new global::System.Collections.Generic.List<ClubCriteriaInfo>();

        [global::ProtoBuf.ProtoMember(3)]
        public bool multi_select
        {
            get => __pbn__multi_select.GetValueOrDefault();
            set => __pbn__multi_select = value;
        }
        public bool ShouldSerializemulti_select() => __pbn__multi_select != null;
        public void Resetmulti_select() => __pbn__multi_select = null;
        private bool? __pbn__multi_select;

        [global::ProtoBuf.ProtoMember(4)]
        public uint criteria_id
        {
            get => __pbn__criteria_id.GetValueOrDefault();
            set => __pbn__criteria_id = value;
        }
        public bool ShouldSerializecriteria_id() => __pbn__criteria_id != null;
        public void Resetcriteria_id() => __pbn__criteria_id = null;
        private uint? __pbn__criteria_id;

        [global::ProtoBuf.ProtoContract()]
        public enum ClubCriteriaCategory
        {
            SKILL = 0,
            TRAIT = 1,
            RELATIONSHIP = 2,
            CAREER = 3,
            HOUSEHOLD_VALUE = 4,
            AGE = 5,
            CLUB_MEMBERSHIP = 6,
            FAME_RANK = 7,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClubCriteriaInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public LocalizedString name { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public ResourceKey icon { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public ResourceKey resource_value { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public uint enum_value
        {
            get => __pbn__enum_value.GetValueOrDefault();
            set => __pbn__enum_value = value;
        }
        public bool ShouldSerializeenum_value() => __pbn__enum_value != null;
        public void Resetenum_value() => __pbn__enum_value = null;
        private uint? __pbn__enum_value;

        [global::ProtoBuf.ProtoMember(5)]
        public ulong resource_id
        {
            get => __pbn__resource_id.GetValueOrDefault();
            set => __pbn__resource_id = value;
        }
        public bool ShouldSerializeresource_id() => __pbn__resource_id != null;
        public void Resetresource_id() => __pbn__resource_id = null;
        private ulong? __pbn__resource_id;

        [global::ProtoBuf.ProtoMember(6)]
        public LocalizedString tooltip_name { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClubConductRule : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public bool encouraged { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public ResourceKey interaction_group { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public ClubCriteria with_whom { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClubBuildingInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<ClubCriteria> criterias { get; } = new global::System.Collections.Generic.List<ClubCriteria>();

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<LotInfoItem> available_lots { get; } = new global::System.Collections.Generic.List<LotInfoItem>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ClubInteractionRuleUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ClubInteractionRuleStatus rule_status { get; set; }

        [global::ProtoBuf.ProtoContract()]
        public enum ClubInteractionRuleStatus
        {
            ENCOURAGED = 0,
            DISCOURAGED = 1,
            NO_EFFECT = 2,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ShowClubInfoUI : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong club_id
        {
            get => __pbn__club_id.GetValueOrDefault();
            set => __pbn__club_id = value;
        }
        public bool ShouldSerializeclub_id() => __pbn__club_id != null;
        public void Resetclub_id() => __pbn__club_id = null;
        private ulong? __pbn__club_id;

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion