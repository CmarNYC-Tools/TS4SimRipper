// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: InteractionOps.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class Interactable : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public bool is_interactable { get; set; }

        [global::ProtoBuf.ProtoMember(2, DataFormat = global::ProtoBuf.DataFormat.FixedSize, IsRequired = true)]
        public ulong object_id { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue(typeof(ulong), "0")]
        public ulong interactable_flags
        {
            get => __pbn__interactable_flags ?? 0ul;
            set => __pbn__interactable_flags = value;
        }
        public bool ShouldSerializeinteractable_flags() => __pbn__interactable_flags != null;
        public void Resetinteractable_flags() => __pbn__interactable_flags = null;
        private ulong? __pbn__interactable_flags;

        [global::ProtoBuf.ProtoContract()]
        public enum InteractableFlags
        {
            INTERACTABLE = 1,
            FORSALE = 2,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class PieMenuItem : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public uint id { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public LocalizedString loc_string { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public ulong[] related_skills { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public ulong[] target_ids { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public ResourceKey icon { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        public LocalizedString disabled_text { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public ResourceKey score_icon { get; set; }

        [global::ProtoBuf.ProtoMember(9)]
        public ulong category_key
        {
            get => __pbn__category_key.GetValueOrDefault();
            set => __pbn__category_key = value;
        }
        public bool ShouldSerializecategory_key() => __pbn__category_key != null;
        public void Resetcategory_key() => __pbn__category_key = null;
        private ulong? __pbn__category_key;

        [global::ProtoBuf.ProtoMember(10)]
        public bool is_super
        {
            get => __pbn__is_super.GetValueOrDefault();
            set => __pbn__is_super = value;
        }
        public bool ShouldSerializeis_super() => __pbn__is_super != null;
        public void Resetis_super() => __pbn__is_super = null;
        private bool? __pbn__is_super;

        [global::ProtoBuf.ProtoMember(11)]
        public float score
        {
            get => __pbn__score.GetValueOrDefault();
            set => __pbn__score = value;
        }
        public bool ShouldSerializescore() => __pbn__score != null;
        public void Resetscore() => __pbn__score = null;
        private float? __pbn__score;

        [global::ProtoBuf.ProtoMember(12)]
        public global::System.Collections.Generic.List<ResourceKey> icons { get; } = new global::System.Collections.Generic.List<ResourceKey>();

        [global::ProtoBuf.ProtoMember(13, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong mood
        {
            get => __pbn__mood.GetValueOrDefault();
            set => __pbn__mood = value;
        }
        public bool ShouldSerializemood() => __pbn__mood != null;
        public void Resetmood() => __pbn__mood = null;
        private ulong? __pbn__mood;

        [global::ProtoBuf.ProtoMember(14)]
        public uint mood_intensity
        {
            get => __pbn__mood_intensity.GetValueOrDefault();
            set => __pbn__mood_intensity = value;
        }
        public bool ShouldSerializemood_intensity() => __pbn__mood_intensity != null;
        public void Resetmood_intensity() => __pbn__mood_intensity = null;
        private uint? __pbn__mood_intensity;

        [global::ProtoBuf.ProtoMember(15)]
        public uint pie_menu_priority
        {
            get => __pbn__pie_menu_priority.GetValueOrDefault();
            set => __pbn__pie_menu_priority = value;
        }
        public bool ShouldSerializepie_menu_priority() => __pbn__pie_menu_priority != null;
        public void Resetpie_menu_priority() => __pbn__pie_menu_priority = null;
        private uint? __pbn__pie_menu_priority;

        [global::ProtoBuf.ProtoMember(16)]
        public LocalizedString success_tooltip { get; set; }

        [global::ProtoBuf.ProtoMember(17)]
        public global::System.Collections.Generic.List<IconInfo> icon_infos { get; } = new global::System.Collections.Generic.List<IconInfo>();

        [global::ProtoBuf.ProtoMember(18)]
        public bool display_notification
        {
            get => __pbn__display_notification.GetValueOrDefault();
            set => __pbn__display_notification = value;
        }
        public bool ShouldSerializedisplay_notification() => __pbn__display_notification != null;
        public void Resetdisplay_notification() => __pbn__display_notification = null;
        private bool? __pbn__display_notification;

        [global::ProtoBuf.ProtoMember(19)]
        public ulong affordance_id
        {
            get => __pbn__affordance_id.GetValueOrDefault();
            set => __pbn__affordance_id = value;
        }
        public bool ShouldSerializeaffordance_id() => __pbn__affordance_id != null;
        public void Resetaffordance_id() => __pbn__affordance_id = null;
        private ulong? __pbn__affordance_id;

        [global::ProtoBuf.ProtoMember(20)]
        public bool phone_notification_control_override
        {
            get => __pbn__phone_notification_control_override.GetValueOrDefault();
            set => __pbn__phone_notification_control_override = value;
        }
        public bool ShouldSerializephone_notification_control_override() => __pbn__phone_notification_control_override != null;
        public void Resetphone_notification_control_override() => __pbn__phone_notification_control_override = null;
        private bool? __pbn__phone_notification_control_override;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class PieMenuCreate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong sim { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<PieMenuItem> items { get; } = new global::System.Collections.Generic.List<PieMenuItem>();

        [global::ProtoBuf.ProtoMember(3)]
        public uint client_reference_id
        {
            get => __pbn__client_reference_id.GetValueOrDefault();
            set => __pbn__client_reference_id = value;
        }
        public bool ShouldSerializeclient_reference_id() => __pbn__client_reference_id != null;
        public void Resetclient_reference_id() => __pbn__client_reference_id = null;
        private uint? __pbn__client_reference_id;

        [global::ProtoBuf.ProtoMember(4)]
        public uint server_reference_id
        {
            get => __pbn__server_reference_id.GetValueOrDefault();
            set => __pbn__server_reference_id = value;
        }
        public bool ShouldSerializeserver_reference_id() => __pbn__server_reference_id != null;
        public void Resetserver_reference_id() => __pbn__server_reference_id = null;
        private uint? __pbn__server_reference_id;

        [global::ProtoBuf.ProtoMember(5)]
        public global::System.Collections.Generic.List<LocalizedStringToken> category_tokens { get; } = new global::System.Collections.Generic.List<LocalizedStringToken>();

        [global::ProtoBuf.ProtoMember(6)]
        public LocalizedString disabled_tooltip { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public bool supress_social_front_page
        {
            get => __pbn__supress_social_front_page.GetValueOrDefault();
            set => __pbn__supress_social_front_page = value;
        }
        public bool ShouldSerializesupress_social_front_page() => __pbn__supress_social_front_page != null;
        public void Resetsupress_social_front_page() => __pbn__supress_social_front_page = null;
        private bool? __pbn__supress_social_front_page;

        [global::ProtoBuf.ProtoMember(8)]
        public ulong selected_affordance_id
        {
            get => __pbn__selected_affordance_id.GetValueOrDefault();
            set => __pbn__selected_affordance_id = value;
        }
        public bool ShouldSerializeselected_affordance_id() => __pbn__selected_affordance_id != null;
        public void Resetselected_affordance_id() => __pbn__selected_affordance_id = null;
        private ulong? __pbn__selected_affordance_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TravelMenuCreate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong sim_id { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public ulong selected_lot_id
        {
            get => __pbn__selected_lot_id.GetValueOrDefault();
            set => __pbn__selected_lot_id = value;
        }
        public bool ShouldSerializeselected_lot_id() => __pbn__selected_lot_id != null;
        public void Resetselected_lot_id() => __pbn__selected_lot_id = null;
        private ulong? __pbn__selected_lot_id;

        [global::ProtoBuf.ProtoMember(3)]
        public uint selected_world_id
        {
            get => __pbn__selected_world_id.GetValueOrDefault();
            set => __pbn__selected_world_id = value;
        }
        public bool ShouldSerializeselected_world_id() => __pbn__selected_world_id != null;
        public void Resetselected_world_id() => __pbn__selected_world_id = null;
        private uint? __pbn__selected_world_id;

        [global::ProtoBuf.ProtoMember(4)]
        [global::System.ComponentModel.DefaultValue("")]
        public string selected_lot_name
        {
            get => __pbn__selected_lot_name ?? "";
            set => __pbn__selected_lot_name = value;
        }
        public bool ShouldSerializeselected_lot_name() => __pbn__selected_lot_name != null;
        public void Resetselected_lot_name() => __pbn__selected_lot_name = null;
        private string __pbn__selected_lot_name;

        [global::ProtoBuf.ProtoMember(5)]
        [global::System.ComponentModel.DefaultValue("")]
        public string friend_account
        {
            get => __pbn__friend_account ?? "";
            set => __pbn__friend_account = value;
        }
        public bool ShouldSerializefriend_account() => __pbn__friend_account != null;
        public void Resetfriend_account() => __pbn__friend_account = null;
        private string __pbn__friend_account;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TravelMenuInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] sim_ids { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TravelMenuResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public bool reserved { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TravelInitiate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong zoneId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MoveInMoveOutInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong moving_family_id
        {
            get => __pbn__moving_family_id.GetValueOrDefault();
            set => __pbn__moving_family_id = value;
        }
        public bool ShouldSerializemoving_family_id() => __pbn__moving_family_id != null;
        public void Resetmoving_family_id() => __pbn__moving_family_id = null;
        private ulong? __pbn__moving_family_id;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue(false)]
        public bool is_in_game_evict
        {
            get => __pbn__is_in_game_evict ?? false;
            set => __pbn__is_in_game_evict = value;
        }
        public bool ShouldSerializeis_in_game_evict() => __pbn__is_in_game_evict != null;
        public void Resetis_in_game_evict() => __pbn__is_in_game_evict = null;
        private bool? __pbn__is_in_game_evict;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SellRetailLot : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong retail_zone_id { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class TravelSimsToZone : global::ProtoBuf.IExtensible
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
        public ulong[] sim_ids { get; set; }

        [global::ProtoBuf.ProtoMember(3, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong active_sim_id
        {
            get => __pbn__active_sim_id.GetValueOrDefault();
            set => __pbn__active_sim_id = value;
        }
        public bool ShouldSerializeactive_sim_id() => __pbn__active_sim_id != null;
        public void Resetactive_sim_id() => __pbn__active_sim_id = null;
        private ulong? __pbn__active_sim_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class CASAvailableZonesInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<WorldZonesInfo> zones { get; } = new global::System.Collections.Generic.List<WorldZonesInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class WorldZonesInfo : global::ProtoBuf.IExtensible
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
        public LocalizedString defaultName { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public global::System.Collections.Generic.List<ZoneInfo> zones { get; } = new global::System.Collections.Generic.List<ZoneInfo>();

        [global::ProtoBuf.ProtoMember(4)]
        public uint worldId
        {
            get => __pbn__worldId.GetValueOrDefault();
            set => __pbn__worldId = value;
        }
        public bool ShouldSerializeworldId() => __pbn__worldId != null;
        public void ResetworldId() => __pbn__worldId = null;
        private uint? __pbn__worldId;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ZoneInfo : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong id
        {
            get => __pbn__id.GetValueOrDefault();
            set => __pbn__id = value;
        }
        public bool ShouldSerializeid() => __pbn__id != null;
        public void Resetid() => __pbn__id = null;
        private ulong? __pbn__id;

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string name
        {
            get => __pbn__name ?? "";
            set => __pbn__name = value;
        }
        public bool ShouldSerializename() => __pbn__name != null;
        public void Resetname() => __pbn__name = null;
        private string __pbn__name;

        [global::ProtoBuf.ProtoMember(4)]
        public LocalizedString defaultName { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public uint world_id
        {
            get => __pbn__world_id.GetValueOrDefault();
            set => __pbn__world_id = value;
        }
        public bool ShouldSerializeworld_id() => __pbn__world_id != null;
        public void Resetworld_id() => __pbn__world_id = null;
        private uint? __pbn__world_id;

        [global::ProtoBuf.ProtoMember(7, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong lot_description_id
        {
            get => __pbn__lot_description_id.GetValueOrDefault();
            set => __pbn__lot_description_id = value;
        }
        public bool ShouldSerializelot_description_id() => __pbn__lot_description_id != null;
        public void Resetlot_description_id() => __pbn__lot_description_id = null;
        private ulong? __pbn__lot_description_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class InteractionProgressUpdate : global::ProtoBuf.IExtensible
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
        public LocalizedString name { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public float percent
        {
            get => __pbn__percent.GetValueOrDefault();
            set => __pbn__percent = value;
        }
        public bool ShouldSerializepercent() => __pbn__percent != null;
        public void Resetpercent() => __pbn__percent = null;
        private float? __pbn__percent;

        [global::ProtoBuf.ProtoMember(4)]
        public float rate_change
        {
            get => __pbn__rate_change.GetValueOrDefault();
            set => __pbn__rate_change = value;
        }
        public bool ShouldSerializerate_change() => __pbn__rate_change != null;
        public void Resetrate_change() => __pbn__rate_change = null;
        private float? __pbn__rate_change;

        [global::ProtoBuf.ProtoMember(5)]
        public ulong interaction_id
        {
            get => __pbn__interaction_id.GetValueOrDefault();
            set => __pbn__interaction_id = value;
        }
        public bool ShouldSerializeinteraction_id() => __pbn__interaction_id != null;
        public void Resetinteraction_id() => __pbn__interaction_id = null;
        private ulong? __pbn__interaction_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SimTransferRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong source_household_id
        {
            get => __pbn__source_household_id.GetValueOrDefault();
            set => __pbn__source_household_id = value;
        }
        public bool ShouldSerializesource_household_id() => __pbn__source_household_id != null;
        public void Resetsource_household_id() => __pbn__source_household_id = null;
        private ulong? __pbn__source_household_id;

        [global::ProtoBuf.ProtoMember(2, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong target_household_id
        {
            get => __pbn__target_household_id.GetValueOrDefault();
            set => __pbn__target_household_id = value;
        }
        public bool ShouldSerializetarget_household_id() => __pbn__target_household_id != null;
        public void Resettarget_household_id() => __pbn__target_household_id = null;
        private ulong? __pbn__target_household_id;

        [global::ProtoBuf.ProtoMember(3, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong active_sim_id
        {
            get => __pbn__active_sim_id.GetValueOrDefault();
            set => __pbn__active_sim_id = value;
        }
        public bool ShouldSerializeactive_sim_id() => __pbn__active_sim_id != null;
        public void Resetactive_sim_id() => __pbn__active_sim_id = null;
        private ulong? __pbn__active_sim_id;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class PhoneNotificationUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] interaction_ids { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public ulong sim_id { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
