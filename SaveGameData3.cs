using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace TS4SaveGameOldVersion3
{
    [ProtoContract]
    public class SaveGameData
    {
        [ProtoMember(1)]
        public uint guid;
        [ProtoMember(2)]
        public SaveSlotData save_slot;
        [ProtoMember(3)]
        public AccountData account;
        [ProtoMember(4)]
        public NeighborhoodData[] neighborhoods;
        [ProtoMember(5)]
        public HouseholdData[] households;
        [ProtoMember(6)]
        public SimData[] sims;
        [ProtoMember(7)]
        public ZoneData[] zones;
        [ProtoMember(8)]
        public OpenStreetsData[] streets;
        [ProtoMember(9)]
        public TravelGroupData[] travel_groups;
        [ProtoMember(10)]
        public ulong[] uninstalled_region_ids;
        [ProtoMember(11)]
        public ObjectFallbackDataList object_fallbacks;
        [ProtoMember(12)]
        public GameplayDestinationCleanUpData[] destination_clean_up_data;
        [ProtoMember(14)]
        public GameplayData gameplay_data;
        [ProtoMember(15)]
        public MannequinSimData[] mannequins;
        [ProtoMember(16)]
        public SimRelationshipGraphData relgraph;
        [ProtoMember(17, DataFormat = DataFormat.FixedSize)]
        public ulong[] tutorial_tips;
        [ProtoMember(18)]
        public ulong[] sims_removed_from_travel_groups;
        [ProtoMember(19)]
        public PlayerCustomColors custom_colors;
    }

    [ProtoContract]
    public class SaveSlotData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong slot_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] neighboorhoods;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong last_neighborhood;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong[] zones;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong last_zone;
        [ProtoMember(6)]
        public IdList households;
        [ProtoMember(7)]
        public bool is_migrated;
        [ProtoMember(8)]
        public GameplaySaveSlotData gameplay_data;
        [ProtoMember(9)]
        public string slot_name;
        [ProtoMember(10)]
        public ulong timestamp;
        [ProtoMember(11)]
        public ulong active_household_id;
        [ProtoMember(12, DataFormat = DataFormat.FixedSize)]
        public ulong nucleus_id;
        [ProtoMember(13, DataFormat = DataFormat.FixedSize)]
        public ulong s4_guid_seed;
        [ProtoMember(14)]
        public uint compatibility_version;
        [ProtoMember(15)]
        public bool trigger_tutorial_drama_node;
        [ProtoMember(16)]
        public int tutorial_mode;
    }

    [ProtoContract]
    public class IdList
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong[] ids;
    }

    [ProtoContract]
    public class GameplaySaveSlotData
    {
        [ProtoMember(1)]
        public ulong world_game_time;
        [ProtoMember(2)]
        public SituationSeedData travel_situation_seed;
        [ProtoMember(3)]
        public GameplayCameraData camera_data;
        [ProtoMember(4)]
        public bool is_phone_silenced;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong career_choices_seed;
        [ProtoMember(6)]
        public bool enable_autogeneration_same_sex_preference;
        [ProtoMember(7)]
        public PersistableClubService club_service;
        [ProtoMember(8)]
        public PersistableDramaScheduleService drama_schedule_service;
        [ProtoMember(9)]
        public PersistableRelgraphService relgraph_service;
        [ProtoMember(10)]
        public BusinessServiceData business_service_data;
        [ProtoMember(11)]
        public PersistableCallToActionService call_to_action_service;
        [ProtoMember(12, DataFormat = DataFormat.FixedSize)]
        public ulong[] once_only_drama_nodes;
        [ProtoMember(13)]
        public PersistableRelationshipService relationship_service;
        [ProtoMember(14)]
        public PersistableAdoptionService adoption_service;
        [ProtoMember(15)]
        public PersistableHiddenSimService hidden_sim_service;
        [ProtoMember(16)]
        public PersistableSeasonService season_service;
        [ProtoMember(17)]
        public PersistableWeatherService weather_service;
        [ProtoMember(18)]
        public PersistableLotDecorationService lot_decoration_service;
        [ProtoMember(19)]
        public PersistableHolidayService holiday_service;
        [ProtoMember(20)]
        public PersistableStyleService style_service;
        [ProtoMember(21)]
        public PersistableTrendService trend_service;
        [ProtoMember(22)]
        public PersistableRabbitHoleService rabbit_hole_service;
        [ProtoMember(23)]
        public PersistableNarrativeService narrative_service;
        [ProtoMember(24)]
        public PersistableObjectLostAndFound object_lost_and_found;
        [ProtoMember(25)]
        public PersistableLandlordService landlord_service;
        [ProtoMember(26)]
        public PersistableGlobalPolicyService global_policy_service;
        [ProtoMember(27)]
        public PersistableRoommateService roommate_service;
        [ProtoMember(28)]
        public PersistableOrganizationService organization_service;
        [ProtoMember(29)]
        public PersistableCullingService culling_service;
        [ProtoMember(30)]
        public PersistableStoryProgressionService story_progression_service;
        [ProtoMember(31)]
        public PersistableStreetService street_service;
        [ProtoMember(32)]
        public PersistableRegionService region_service;
        [ProtoMember(33)]
        public PersistableLifestyleService lifestyle_service;
    }

    [ProtoContract]
    public class SituationSeedData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong situation_type_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong situation_id;
        [ProtoMember(3)]
        public uint seed_purpose;
        [ProtoMember(4)]
        public bool invite_only;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong host_sim_id;
        [ProtoMember(6)]
        public SituationAssignmentData[] assignments;
        [ProtoMember(7)]
        public bool user_facing;
        [ProtoMember(8)]
        public float duration;
        [ProtoMember(9, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(10)]
        public SituationJobAndRoleState[] jobs_and_role_states;
        [ProtoMember(11)]
        public ulong create_time;
        [ProtoMember(12)]
        public float score;
        [ProtoMember(13)]
        public SituationSimpleSeedlingData simple_data;
        [ProtoMember(14)]
        public SituationComplexSeedlingData complex_data;
        [ProtoMember(15, DataFormat = DataFormat.FixedSize)]
        public ulong filter_requesting_sim_id;
        [ProtoMember(16)]
        public SituationGoalTrackerData goal_tracker_data;
        [ProtoMember(17)]
        public ulong start_time;
        [ProtoMember(18, DataFormat = DataFormat.FixedSize)]
        public ulong active_household_id;
        [ProtoMember(19)]
        public bool scoring_enabled;
        [ProtoMember(20)]
        public bool main_goal_visibility;
        [ProtoMember(21, DataFormat = DataFormat.FixedSize)]
        public ulong linked_sim_id;
    }

    [ProtoContract]
    public class SituationAssignmentData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong job_type_id;
        [ProtoMember(3)]
        public uint purpose;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong role_state_type_id;
        [ProtoMember(5)]
        public uint spawning_option;
        [ProtoMember(6)]
        public uint request_priority;
        [ProtoMember(7)]
        public bool expectation_preference;
        [ProtoMember(8)]
        public bool accept_alternate_sim;
        [ProtoMember(9)]
        public uint common_blacklist_categories;
        [ProtoMember(10)]
        public bool elevated_importance_override;
        [ProtoMember(11)]
        public bool reservation;
    }

    [ProtoContract]
    public class SituationJobAndRoleState
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong job_type_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong role_state_type_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong emotional_loot_actions_type_id;
    }

    [ProtoContract]
    public class SituationSimpleSeedlingData
    {
        [ProtoMember(1)]
        public uint phase_index;
        [ProtoMember(2)]
        public float remaining_phase_time;
    }

    [ProtoContract]
    public class SituationComplexSeedlingData
    {
        [ProtoMember(1)]
        public byte[] situation_custom_data;
        [ProtoMember(2)]
        public byte[] state_custom_data;
    }

    [ProtoContract]
    public class SituationGoalTrackerData
    {
        [ProtoMember(1)]
        public bool has_offered_goals;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong inherited_target_id;
        [ProtoMember(3)]
        public SituationGoalChainData[] chains;
        [ProtoMember(4)]
        public SituationGoalData[] minor_goals;
        [ProtoMember(5)]
        public SituationGoalData main_goal;
        [ProtoMember(6)]
        public CompletedSituationGoalData[] completed_goals;
        [ProtoMember(7)]
        public GoalTrackerType goal_tracker_type;
    }

    [ProtoContract]
    public class SituationGoalChainData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong starting_goal_set_type_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong chosen_goal_set_type_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong chain_id;
        [ProtoMember(4)]
        public int display_position;
    }

    [ProtoContract]
    public class SituationGoalData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong goal_type_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong actor_id;
        [ProtoMember(3)]
        public uint count;
        [ProtoMember(4)]
        public bool completed;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong chain_id;
        [ProtoMember(6)]
        public byte[] custom_data;
        [ProtoMember(7)]
        public bool locked;
        [ProtoMember(8)]
        public ulong completed_time;
        [ProtoMember(9, DataFormat = DataFormat.FixedSize)]
        public ulong target_id;
        [ProtoMember(10, DataFormat = DataFormat.FixedSize)]
        public ulong secondary_target_id;
    }

    [ProtoContract]
    public class CompletedSituationGoalData
    {
        [ProtoMember(1)]
        public SituationGoalData situation_goal;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong chosen_goal_set_type_id;
    }

    public enum GoalTrackerType
    {
        STANDARD_GOAL_TRACKER = 0,
        DYNAMIC_GOAL_TRACKER = 1,
    }

    [ProtoContract]
    public class GameplayCameraData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong target_id;
        [ProtoMember(2)]
        public Vector3 target_position;
        [ProtoMember(3)]
        public Vector3 camera_position;
        [ProtoMember(4)]
        public bool follow_mode;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
    }

    [ProtoContract]
    public class Vector3
    {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;
    }

    [ProtoContract]
    public class PersistableClubService
    {
        [ProtoMember(1)]
        public bool has_seeded_clubs;
        [ProtoMember(2)]
        public uint club_count;
        [ProtoMember(3)]
        public Club[] clubs;
    }

    [ProtoContract]
    public class Club
    {
        [ProtoMember(1)]
        public ulong club_id;
        [ProtoMember(2)]
        public string name;
        [ProtoMember(3)]
        public ResourceKey icon;
        [ProtoMember(4)]
        public string description;
        [ProtoMember(5)]
        public bool invite_only;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong leader;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong[] members;
        [ProtoMember(8)]
        public ResourceKey venue_type;
        [ProtoMember(9)]
        public ClubCriteria[] membership_criteria;
        [ProtoMember(10)]
        public ClubConductRule[] club_rules;
        [ProtoMember(11)]
        public uint associated_color;
        [ProtoMember(12)]
        public ResourceKey club_seed;
        [ProtoMember(13)]
        public BucksData[] bucks_data;
        [ProtoMember(14)]
        public uint associated_style;
        [ProtoMember(15)]
        public MannequinSimData club_uniform_adult_male;
        [ProtoMember(16)]
        public MannequinSimData club_uniform_child_male;
        [ProtoMember(17)]
        public MannequinSimData club_uniform_adult_female;
        [ProtoMember(18)]
        public MannequinSimData club_uniform_child_female;
        [ProtoMember(19)]
        public ClubOutfitSetting outfit_setting;
        [ProtoMember(20)]
        public uint member_cap;
        [ProtoMember(21, DataFormat = DataFormat.FixedSize)]
        public ulong[] recent_members;
        [ProtoMember(22)]
        public ClubHangoutSetting hangout_setting;
        [ProtoMember(23, DataFormat = DataFormat.FixedSize)]
        public ulong hangout_zone_id;
    }

    [ProtoContract]
    public class ResourceKey
    {
        [ProtoMember(1)]
        public uint type;
        [ProtoMember(2)]
        public uint group;
        [ProtoMember(3)]
        public ulong instance;
    }

    [ProtoContract]
    public class ClubCriteria
    {
        [ProtoMember(1)]
        public ClubCriteriaCategory category;
        [ProtoMember(2)]
        public ClubCriteriaInfo[] criteria_infos;
        [ProtoMember(3)]
        public bool multi_select;
        [ProtoMember(4)]
        public uint criteria_id;
    }

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

    [ProtoContract]
    public class ClubCriteriaInfo
    {
        [ProtoMember(1)]
        public LocalizedString name;
        [ProtoMember(2)]
        public ResourceKey icon;
        [ProtoMember(3)]
        public ResourceKey resource_value;
        [ProtoMember(4)]
        public uint enum_value;
        [ProtoMember(5)]
        public ulong resource_id;
        [ProtoMember(6)]
        public LocalizedString tooltip_name;
    }

    [ProtoContract]
    public class LocalizedString
    {
        [ProtoMember(1)]
        public uint hash;
        [ProtoMember(2)]
        public LocalizedStringToken[] tokens;
    }

    [ProtoContract]
    public class LocalizedStringToken
    {
        [ProtoMember(1)]
        public TokenType type;
        [ProtoMember(2)]
        public SocialRichDataType rdl_type;
        [ProtoMember(3)]
        public string first_name;
        [ProtoMember(4)]
        public string last_name;
        [ProtoMember(5)]
        public uint full_name_key;
        [ProtoMember(6)]
        public bool is_female;
        [ProtoMember(7)]
        public ulong sim_id;
        [ProtoMember(8)]
        public LocalizedString text_string;
        [ProtoMember(9)]
        public float number;
        [ProtoMember(10)]
        public ulong persona_id;
        [ProtoMember(11)]
        public ulong account_id;
        [ProtoMember(12)]
        public string persona_string;
        [ProtoMember(13)]
        public ulong zone_id;
        [ProtoMember(14)]
        public uint world_id;
        [ProtoMember(15)]
        public string zone_name;
        [ProtoMember(16)]
        public ulong event_id;
        [ProtoMember(17)]
        public uint event_type_hash;
        [ProtoMember(18)]
        public uint skill_name_hash;
        [ProtoMember(19)]
        public uint skill_level;
        [ProtoMember(20)]
        public ulong skill_guid;
        [ProtoMember(21)]
        public uint trait_name_hash;
        [ProtoMember(22)]
        public ulong trait_guid;
        [ProtoMember(23)]
        public uint bit_name_hash;
        [ProtoMember(24)]
        public ulong bit_guid;
        [ProtoMember(25)]
        public uint catalog_name_key;
        [ProtoMember(26)]
        public uint catalog_description_key;
        [ProtoMember(27)]
        public string custom_name;
        [ProtoMember(28)]
        public string custom_description;
        [ProtoMember(29)]
        public ulong career_uid;
        [ProtoMember(30)]
        public ulong memory_id;
        [ProtoMember(31)]
        public uint memory_string_hash;
        [ProtoMember(32)]
        public string raw_text;
        [ProtoMember(33)]
        public LocalizedDateAndTimeData date_and_time;
        [ProtoMember(34)]
        public SubTokenData[] sim_list;
    }

    public enum TokenType
    {
        INVALID = 0,
        SIM = 1,
        STRING = 2,
        RAW_TEXT = 3,
        NUMBER = 4,
        OBJECT = 5,
        DATE_AND_TIME = 6,
        RICHDATA = 7,
        STRING_LIST = 8,
        SIM_LIST = 9,
    }

    public enum SocialRichDataType
    {
        RDL_PLAINTEXT = 0,
        RDL_PERSONA = 1,
        RDL_SIM = 31,
        RDL_FAMILY = 32,
        RDL_NEIGHBORHOOD = 101,
        RDL_ZONEINSTANCE = 102,
        RDL_LOT = 103,
        RDL_OBJECT = 201,
        RDL_CAS_PART = 202,
        RDL_BLUEPRINT = 203,
        RDL_MTX_CONSUMABLE = 204,
        RDL_MEMORY = 205,
        RDL_ACHIEVEMENT = 1001,
        RDL_SKILL = 1002,
        RDL_TRAIT = 1003,
        RDL_WISH = 1004,
        RDL_EVENT = 1005,
        RDL_REL_BIT = 1006,
        RDL_CAREER = 1007,
    }

    [ProtoContract]
    public class LocalizedDateAndTimeData
    {
        [ProtoMember(1)]
        public uint seconds;
        [ProtoMember(2)]
        public uint minutes;
        [ProtoMember(3)]
        public uint hours;
        [ProtoMember(4)]
        public uint date;
        [ProtoMember(5)]
        public uint month;
        [ProtoMember(6)]
        public uint full_year;
        [ProtoMember(7)]
        public uint date_and_time_format_hash;
    }

    [ProtoContract]
    public class SubTokenData
    {
        [ProtoMember(1)]
        public TokenType type;
        [ProtoMember(2)]
        public string first_name;
        [ProtoMember(3)]
        public string last_name;
        [ProtoMember(4)]
        public uint full_name_key;
        [ProtoMember(5)]
        public bool is_female;
    }

    [ProtoContract]
    public class ClubConductRule
    {
        [ProtoMember(1)]
        public bool encouraged;
        [ProtoMember(2)]
        public ResourceKey interaction_group;
        [ProtoMember(3)]
        public ClubCriteria with_whom;
    }

    [ProtoContract]
    public class BucksData
    {
        [ProtoMember(1)]
        public uint bucks_type;
        [ProtoMember(2)]
        public uint amount;
        [ProtoMember(3)]
        public UnlockedPerk[] unlocked_perks;
    }

    [ProtoContract]
    public class UnlockedPerk
    {
        [ProtoMember(1)]
        public uint perk;
        [ProtoMember(2)]
        public uint unlock_reason;
        [ProtoMember(3)]
        public ulong time_left;
        [ProtoMember(4)]
        public ulong timestamp;
        [ProtoMember(5)]
        public bool currently_unlocked;
    }

    [ProtoContract]
    public class MannequinSimData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong mannequin_id;
        [ProtoMember(2)]
        public uint gender;
        [ProtoMember(3)]
        public uint age;
        [ProtoMember(4)]
        public uint extended_species;
        [ProtoMember(11)]
        public string physique;
        [ProtoMember(12)]
        public byte[] facial_attributes;
        [ProtoMember(13)]
        public ulong skin_tone;
        [ProtoMember(21)]
        public OutfitList outfits;
        [ProtoMember(22)]
        public uint current_outfit_type;
        [ProtoMember(23)]
        public uint current_outfit_index;
        [ProtoMember(24)]
        public uint previous_outfit_type;
        [ProtoMember(25)]
        public uint previous_outfit_index;
        [ProtoMember(31, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(32)]
        public uint world_id;
        [ProtoMember(51)]
        public AnimationStateRequest animation_pose;
        [ProtoMember(52)]
        public float skin_tone_val_shift;
    }

    [ProtoContract]
    public class OutfitList
    {
        [ProtoMember(1)]
        public OutfitData[] outfits;
    }

    [ProtoContract]
    public class OutfitData
    {
        [ProtoMember(1)]
        public ulong outfit_id;
        [ProtoMember(2)]
        public uint category;
        [ProtoMember(5)]
        public IdList parts;
        [ProtoMember(6)]
        public ulong created;
        [ProtoMember(7)]
        public BodyTypesList body_types_list;
        [ProtoMember(9)]
        public bool match_hair_style;
        [ProtoMember(10)]
        public ulong outfit_flags;
        [ProtoMember(11)]
        public ulong outfit_flags_high;
        [ProtoMember(12)]
        public ColorShiftList part_shifts;
    }

    [ProtoContract]
    public class BodyTypesList
    {
        [ProtoMember(1)]
        public uint[] body_types;
    }

    [ProtoContract]
    public class ColorShiftList
    {
        [ProtoMember(1)]
        public ulong[] color_shift;
    }

    [ProtoContract]
    public class AnimationStateRequest
    {
        [ProtoMember(1)]
        public ResourceKey asm;
        [ProtoMember(2)]
        public string state_name;
        [ProtoMember(3)]
        public string actor_name;
    }

    public enum ClubOutfitSetting
    {
        NO_OUTFIT = 0,
        STYLE = 1,
        COLOR = 2,
        OUTFIT_OVERRIDE = 3,
    }

    public enum ClubHangoutSetting
    {
        NO_HANGOUT = 0,
        VENUE = 1,
        LOT = 2,
    }

    [ProtoContract]
    public class PersistableDramaScheduleService
    {
        [ProtoMember(1)]
        public PersistableDramaNode[] drama_nodes;
        [ProtoMember(2)]
        public CooldownDramaNode[] cooldown_nodes;
        [ProtoMember(3)]
        public PersistableDramaNode[] running_nodes;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong[] drama_nodes_on_permanent_cooldown;
        [ProtoMember(5)]
        public ulong[] startup_drama_node_buckets_used;
        [ProtoMember(6)]
        public DramaNodeCooldownGroup[] cooldown_groups;
        [ProtoMember(7)]
        public ulong[] cooldown_groups_on_permanent_cooldown;
    }

    [ProtoContract]
    public class PersistableDramaNode
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong uid;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong node_type;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong receiver_sim_id;
        [ProtoMember(4)]
        public float receiver_score;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong sender_sim_id;
        [ProtoMember(6)]
        public float sender_score;
        [ProtoMember(7)]
        public ulong selected_time;
        [ProtoMember(8)]
        public byte[] custom_data;
        [ProtoMember(9, DataFormat = DataFormat.FixedSize)]
        public ulong picked_sim_id;
        [ProtoMember(10)]
        public ulong club_id;
        [ProtoMember(11)]
        public SituationSeedData stored_situation;
    }

    [ProtoContract]
    public class CooldownDramaNode
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong node_type;
        [ProtoMember(2)]
        public ulong completed_time;
    }

    [ProtoContract]
    public class DramaNodeCooldownGroup
    {
        [ProtoMember(1)]
        public ulong group;
        [ProtoMember(2)]
        public ulong completed_time;
    }

    [ProtoContract]
    public class PersistableRelgraphService
    {
        [ProtoMember(1)]
        public byte[] relgraph_data;
    }

    [ProtoContract]
    public class BusinessServiceData
    {
        [ProtoMember(1)]
        public BusinessTrackerData[] business_tracker_data;
    }

    [ProtoContract]
    public class BusinessTrackerData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong business_type;
        [ProtoMember(3)]
        public BusinessManagerData[] business_manager_data;
        [ProtoMember(4)]
        public float additional_markup_multiplier;
        [ProtoMember(5)]
        public float additional_customer_count;
        [ProtoMember(6)]
        public AdditionalEmployeeSlotData[] additional_employee_slot_data;
    }

    [ProtoContract]
    public class BusinessManagerData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(2)]
        public BusinessSaveData business_data;
    }

    [ProtoContract]
    public class BusinessSaveData
    {
        [ProtoMember(1)]
        public bool is_open;
        [ProtoMember(2)]
        public int funds;
        [ProtoMember(3)]
        public float markup;
        [ProtoMember(4)]
        public EmployeeBusinessData employee_payroll;
        [ProtoMember(5)]
        public ulong open_time;
        [ProtoMember(6)]
        public bool grand_opening;
        [ProtoMember(7)]
        public BusinessFundsCategoryEntry[] funds_category_tracker_data;
        [ProtoMember(8)]
        public int daily_revenue;
        [ProtoMember(9)]
        public uint daily_items_sold;
        [ProtoMember(10)]
        public uint lifetime_customers_served;
        [ProtoMember(11)]
        public RestaurantSaveData restaurant_save_data;
        [ProtoMember(12)]
        public float star_rating_value;
        [ProtoMember(13)]
        public BusinessBuffBucketTotal[] buff_bucket_totals;
        [ProtoMember(14)]
        public CustomerBusinessData[] customer_data;
        [ProtoMember(15)]
        public uint session_customers_served;
        [ProtoMember(17)]
        public ulong last_off_lot_update;
        [ProtoMember(18)]
        public uint buff_bucket_size;
        [ProtoMember(19)]
        public VetClinicSaveData vet_clinic_save_data;
    }

    [ProtoContract]
    public class EmployeeBusinessData
    {
        [ProtoMember(1)]
        public EmployeeData[] employee_data;
        [ProtoMember(2)]
        public int daily_employee_wages;
        [ProtoMember(3)]
        public BusinessUniformData[] employee_uniforms_male;
        [ProtoMember(4)]
        public BusinessUniformData[] employee_uniforms_female;
        [ProtoMember(5)]
        public BusinessDataPayroll[] employee_payroll;
    }

    [ProtoContract]
    public class EmployeeData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong employee_type;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong employee_id;
    }

    [ProtoContract]
    public class BusinessUniformData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong employee_type;
        [ProtoMember(2)]
        public MannequinSimData employee_uniform_data;
    }

    [ProtoContract]
    public class BusinessDataPayroll
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong clock_in_time;
        [ProtoMember(3)]
        public BusinessDataPayrollEntry[] payroll_data;
    }

    [ProtoContract]
    public class BusinessDataPayrollEntry
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong career_level_guid;
        [ProtoMember(2)]
        public float hours_worked;
    }

    [ProtoContract]
    public class BusinessFundsCategoryEntry
    {
        [ProtoMember(1)]
        public uint funds_category;
        [ProtoMember(2)]
        public int amount;
    }

    [ProtoContract]
    public class RestaurantSaveData
    {
        [ProtoMember(1)]
        public uint ingredient_quality_enum;
        [ProtoMember(2)]
        public uint[] profit_per_meal_queue;
        [ProtoMember(3)]
        public uint dining_spot_count;
        [ProtoMember(4)]
        public uint advertising_type;
    }

    [ProtoContract]
    public class BusinessBuffBucketTotal
    {
        [ProtoMember(1)]
        public uint buff_bucket;
        [ProtoMember(2)]
        public float buff_bucket_total;
    }

    [ProtoContract]
    public class CustomerBusinessData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong customer_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] customer_buffs;
        [ProtoMember(3)]
        public BusinessBuffBucketTotal[] buff_bucket_totals;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong last_buff_id;
    }

    [ProtoContract]
    public class VetClinicSaveData
    {
        [ProtoMember(1)]
        public uint advertising_type;
        [ProtoMember(2)]
        public uint quality_type;
        [ProtoMember(3)]
        public uint[] profit_per_treatment_queue;
        [ProtoMember(4)]
        public uint exam_table_count;
    }

    [ProtoContract]
    public class AdditionalEmployeeSlotData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong employee_type;
        [ProtoMember(2)]
        public uint additional_slot_count;
    }

    [ProtoContract]
    public class PersistableCallToActionService
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong[] permanently_disabled_ids;
    }

    [ProtoContract]
    public class PersistableRelationshipService
    {
        [ProtoMember(1)]
        public PersistableServiceRelationship[] relationships;
        [ProtoMember(2)]
        public PersistableServiceRelationship[] object_relationships;
    }

    [ProtoContract]
    public class PersistableServiceRelationship
    {
        [ProtoMember(1)]
        public ulong sim_id_a;
        [ProtoMember(2)]
        public ulong sim_id_b;
        [ProtoMember(3)]
        public PersistableBidirectionalRelationshipData bidirectional_relationship_data;
        [ProtoMember(4)]
        public PersistableUnidirectionalRelationshipData sim_a_relationship_data;
        [ProtoMember(5)]
        public PersistableUnidirectionalRelationshipData sim_b_relationship_data;
        [ProtoMember(6)]
        public ulong last_update_time;
        [ProtoMember(7)]
        public ulong target_object_id;
        [ProtoMember(8)]
        public ulong target_object_manager_id;
        [ProtoMember(9)]
        public ulong target_object_instance_id;
        [ProtoMember(10)]
        public string object_relationship_name;
    }

    [ProtoContract]
    public class PersistableBidirectionalRelationshipData
    {
        [ProtoMember(1)]
        public ulong[] bits;
        [ProtoMember(2)]
        public Timeout[] timeouts;
        [ProtoMember(3)]
        public PersistableRelationshipTrack[] tracks;
        [ProtoMember(4)]
        public PersistableRelationshipBitLock[] relationship_bit_locks;
    }

    [ProtoContract]
    public class Timeout
    {
        [ProtoMember(1)]
        public ulong timeout_bit_id_hash;
        [ProtoMember(2)]
        public float elapsed_time;
    }

    [ProtoContract]
    public class PersistableRelationshipTrack
    {
        [ProtoMember(1)]
        public ulong track_id;
        [ProtoMember(2)]
        public float value;
        [ProtoMember(3)]
        public bool visible;
        [ProtoMember(4)]
        public float ticks_until_decay_begins;
    }

    [ProtoContract]
    public class PersistableRelationshipBitLock
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong relationship_bit_lock_type;
        [ProtoMember(2)]
        public ulong locked_time;
    }

    [ProtoContract]
    public class PersistableUnidirectionalRelationshipData
    {
        [ProtoMember(1)]
        public ulong[] bits;
        [ProtoMember(2)]
        public Timeout[] timeouts;
        [ProtoMember(3)]
        public SimKnowledge knowledge;
        [ProtoMember(4)]
        public ulong[] bit_added_buffs;
        [ProtoMember(5)]
        public PersistableRelationshipBitLock[] relationship_bit_locks;
        [ProtoMember(6)]
        public ulong sentiment_proximity_cooldown_end;
        [ProtoMember(7)]
        public PersistableRelationshipTrack[] tracks;
    }

    [ProtoContract]
    public class SimKnowledge
    {
        [ProtoMember(1)]
        public ulong[] trait_ids;
        [ProtoMember(2)]
        public uint num_traits;
        [ProtoMember(3)]
        public bool knows_career;
        [ProtoMember(4)]
        public ulong[] stats;
        [ProtoMember(5)]
        public bool knows_major;
    }

    [ProtoContract]
    public class PersistableAdoptionService
    {
        [ProtoMember(2)]
        public AdoptableSimData[] adoptable_sim_data;
    }

    [ProtoContract]
    public class AdoptableSimData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong adoptable_sim_id;
        [ProtoMember(2)]
        public ulong creation_time;
    }

    [ProtoContract]
    public class PersistableHiddenSimService
    {
        [ProtoMember(1)]
        public HiddenSimData[] hidden_sim_data;
    }

    [ProtoContract]
    public class HiddenSimData
    {
        [ProtoMember(1)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong away_action;
    }

    [ProtoContract]
    public class PersistableSeasonService
    {
        [ProtoMember(1)]
        public ulong current_season;
        [ProtoMember(2)]
        public ulong season_start_time;
    }

    [ProtoContract]
    public class PersistableWeatherService
    {
        [ProtoMember(1)]
        public RegionWeather[] region_weathers;
    }

    [ProtoContract]
    public class RegionWeather
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong region;
        [ProtoMember(2)]
        public SeasonWeatherInterpolations weather;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong weather_event;
        [ProtoMember(4)]
        public ulong forecast_time_stamp;
        [ProtoMember(5)]
        public ulong next_weather_event_time;
        [ProtoMember(6)]
        public ulong[] forecasts;
        [ProtoMember(7)]
        public ulong override_forecast;
        [ProtoMember(8)]
        public ulong override_forecast_season_stamp;
    }

    [ProtoContract]
    public class SeasonWeatherInterpolations
    {
        [ProtoMember(1)]
        public SeasonWeatherInterpolationMessage[] season_weather_interlops;
    }

    [ProtoContract]
    public class SeasonWeatherInterpolationMessage
    {
        [ProtoMember(1)]
        public SeasonWeatherInterpolatedType message_type;
        [ProtoMember(2)]
        public float start_value;
        [ProtoMember(3)]
        public ulong start_time;
        [ProtoMember(4)]
        public float end_value;
        [ProtoMember(5)]
        public ulong end_time;
    }

    public enum SeasonWeatherInterpolatedType
    {
        SEASON = 0,
        LEAF_ACCUMULATION = 1,
        FLOWER_GROWTH = 2,
        FOLIAGE_REDUCTION = 3,
        FOLIAGE_COLORSHIFT = 4,
        RAINFALL = 1000,
        SNOWFALL = 1001,
        RAIN_ACCUMULATION = 1002,
        SNOW_ACCUMULATION = 1003,
        WINDOW_FROST = 1004,
        WATER_FROZEN = 1005,
        WIND = 1006,
        TEMPERATURE = 1007,
        THUNDER = 1008,
        LIGHTNING = 1009,
        SNOW_FRESHNESS = 1010,
        STORY_ACT = 1011,
        ECO_FOOTPRINT = 1012,
        ACID_RAIN = 1013,
        STARWARS_RESISTANCE = 1014,
        STARWARS_FIRST_ORDER = 1015,
        SNOW_ICINESS = 1016,
        SKYBOX_PARTLY_CLOUDY = 2000,
        SKYBOX_CLEAR = 2001,
        SKYBOX_LIGHTRAINCLOUDS = 2002,
        SKYBOX_DARKRAINCLOUDS = 2003,
        SKYBOX_LIGHTSNOWCLOUDS = 2004,
        SKYBOX_DARKSNOWCLOUDS = 2005,
        SKYBOX_CLOUDY = 2006,
        SKYBOX_HEATWAVE = 2007,
        SKYBOX_STRANGE = 2008,
        SKYBOX_VERYSTRANGE = 2009,
        SKYBOX_INDUSTRIAL = 2010,
    }

    [ProtoContract]
    public class PersistableLotDecorationService
    {
        [ProtoMember(1)]
        public LotDecorationSetting[] lot_decorations;
        [ProtoMember(2)]
        public HolidayDecorationSetting[] holiday_preferences;
        [ProtoMember(3)]
        public WorldDecorationSetting[] world_decorations_set;
    }

    [ProtoContract]
    public class LotDecorationSetting
    {
        [ProtoMember(1)]
        public ulong zone_id;
        [ProtoMember(2)]
        public DecorationState[] decoration_states;
        [ProtoMember(3)]
        public ulong active_decoration_state;
    }

    [ProtoContract]
    public class DecorationState
    {
        [ProtoMember(1)]
        public ulong decoration_type_id;
        [ProtoMember(2)]
        public DecoratedLocation[] decorated_locations;
    }

    [ProtoContract]
    public class DecoratedLocation
    {
        [ProtoMember(1)]
        public uint location;
        [ProtoMember(2)]
        public ulong decoration;
    }

    [ProtoContract]
    public class HolidayDecorationSetting
    {
        [ProtoMember(1)]
        public ulong holiday;
        [ProtoMember(2)]
        public ulong decoration_preset;
    }

    [ProtoContract]
    public class WorldDecorationSetting
    {
        [ProtoMember(1)]
        public ulong world_id;
        [ProtoMember(2)]
        public ulong set_decorations;
    }

    [ProtoContract]
    public class PersistableHolidayService
    {
        [ProtoMember(1)]
        public Holiday[] holidays;
        [ProtoMember(2)]
        public HolidayCalendar[] calendars;
    }

    [ProtoContract]
    public class Holiday
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong holiday_type;
        [ProtoMember(2)]
        public string name;
        [ProtoMember(3)]
        public ResourceKey icon;
        [ProtoMember(4)]
        public bool time_off_for_work;
        [ProtoMember(5)]
        public bool time_off_for_school;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong[] traditions;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong lot_decoration_preset;
    }

    [ProtoContract]
    public class HolidayCalendar
    {
        [ProtoMember(1)]
        public uint season_length;
        [ProtoMember(2)]
        public HolidayTimeData[] holidays;
    }

    [ProtoContract]
    public class HolidayTimeData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong holiday_id;
        [ProtoMember(2)]
        public uint day;
        [ProtoMember(3)]
        public uint season;
    }

    [ProtoContract]
    public class PersistableStyleService
    {
        [ProtoMember(1)]
        public StyleOutfitInfo[] outfit_infos;
    }

    [ProtoContract]
    public class StyleOutfitInfo
    {
        [ProtoMember(1)]
        public MannequinSimData outfit_info_data;
    }

    [ProtoContract]
    public class PersistableTrendService
    {
        [ProtoMember(1)]
        public uint[] current_trend_tags;
        [ProtoMember(2)]
        public ulong next_update_ticks;
    }

    [ProtoContract]
    public class PersistableRabbitHoleService
    {
        [ProtoMember(1)]
        public RabbitHoleData[] rabbit_holes;
    }

    [ProtoContract]
    public class RabbitHoleData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong rabbit_hole_id;
        [ProtoMember(3)]
        public ulong time_remaining;
        [ProtoMember(4)]
        public ulong[] linked_sim_ids;
        [ProtoMember(5)]
        public ulong picked_stat_id;
        [ProtoMember(6)]
        public ulong rabbit_hole_instance_id;
        [ProtoMember(7)]
        public ulong[] linked_rabbit_hole_ids;
        [ProtoMember(8)]
        public ulong phase;
        [ProtoMember(9)]
        public ulong career_uid;
    }

    [ProtoContract]
    public class PersistableNarrativeService
    {
        [ProtoMember(1)]
        public ulong[] active_narratives;
        [ProtoMember(2)]
        public ulong[] completed_narratives;
        [ProtoMember(3)]
        public NarrativeData[] narratives;
    }

    [ProtoContract]
    public class NarrativeData
    {
        [ProtoMember(1)]
        public ulong narrative_id;
        [ProtoMember(2)]
        public bool introduction_shown;
        [ProtoMember(3)]
        public NarrativeProgressionData[] narrative_progression_entries;
    }

    [ProtoContract]
    public class NarrativeProgressionData
    {
        [ProtoMember(1)]
        public uint theevent;
        [ProtoMember(2)]
        public ulong progression;
    }

    [ProtoContract]
    public class PersistableObjectLostAndFound
    {
        [ProtoMember(1)]
        public PersistableObjectLocator[] locators;
        [ProtoMember(2)]
        public ClonedObjectsLocator[] clones_to_delete;
    }

    [ProtoContract]
    public class PersistableObjectLocator
    {
        [ProtoMember(1)]
        public byte[] theobject;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong open_street_id;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(6)]
        public float time_before_lost;
        [ProtoMember(7)]
        public float time_stamp;
        [ProtoMember(8)]
        public bool return_to_individual_sim;
    }

    [ProtoContract]
    public class ClonedObjectsLocator
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong open_street_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong object_id;
    }

    [ProtoContract]
    public class PersistableLandlordService
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong landlord_id;
    }

    [ProtoContract]
    public class PersistableGlobalPolicyService
    {
        [ProtoMember(1)]
        public PersistableGlobalPolicy[] global_policies;
        [ProtoMember(2)]
        public PersistableGlobalPolicyBillReduction[] bill_reductions;
        [ProtoMember(3)]
        public PersistableGlobalUtilityEffect[] utility_effects;
    }

    [ProtoContract]
    public class PersistableGlobalPolicy
    {
        [ProtoMember(1)]
        public uint progress_state;
        [ProtoMember(2)]
        public uint progress_value;
        [ProtoMember(3)]
        public uint decay_days;
        [ProtoMember(4)]
        public ulong snippet;
    }

    [ProtoContract]
    public class PersistableGlobalPolicyBillReduction
    {
        [ProtoMember(1)]
        public uint bill_reduction_reason;
        [ProtoMember(2)]
        public float bill_reduction_amount;
    }

    [ProtoContract]
    public class PersistableGlobalUtilityEffect
    {
        [ProtoMember(1)]
        public ulong global_policy_snippet_guid64;
    }

    [ProtoContract]
    public class PersistableRoommateService
    {
        [ProtoMember(1)]
        public RoommateData[] roommate_datas;
        [ProtoMember(2)]
        public RoommateAdInfo ad_info;
    }

    [ProtoContract]
    public class RoommateData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(4)]
        public RoommateInfo[] roommate_infos;
        [ProtoMember(5)]
        public RoommateBlacklistSimInfo[] blacklist_infos;
        [ProtoMember(7)]
        public ulong[] pending_destroy_decoration_ids;
        [ProtoMember(8, DataFormat = DataFormat.FixedSize)]
        public ulong locked_out_id;
        [ProtoMember(9)]
        public int available_beds;
    }

    [ProtoContract]
    public class RoommateInfo
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong bed_id;
        [ProtoMember(3)]
        public ulong[] decoration_ids;
        [ProtoMember(4)]
        public RoommateLeaveReasonInfo[] leave_reason_infos;
    }

    [ProtoContract]
    public class RoommateLeaveReasonInfo
    {
        [ProtoMember(1)]
        public ulong reason;
        [ProtoMember(2)]
        public ulong total_time;
        [ProtoMember(3)]
        public bool been_warned;
    }

    [ProtoContract]
    public class RoommateBlacklistSimInfo
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong time_left;
    }

    [ProtoContract]
    public class RoommateAdInfo
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(2)]
        public ulong[] pending_interview_alarms;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong[] interviewee_ids;
    }

    [ProtoContract]
    public class PersistableOrganizationService
    {
        [ProtoMember(1)]
        public OrganizationData[] organizations;
        [ProtoMember(2)]
        public OrganizationEventData[] org_festival_events;
        [ProtoMember(3)]
        public OrganizationEventData[] org_venue_events;
        [ProtoMember(4)]
        public ScheduleEventCallbackData[] schedule_cancelled_event_data;
    }

    [ProtoContract]
    public class OrganizationData
    {
        [ProtoMember(1)]
        public ulong organization_id;
        [ProtoMember(2)]
        public OrganizationMemberData[] organization_members;
    }

    [ProtoContract]
    public class OrganizationMemberData
    {
        [ProtoMember(1)]
        public ulong organization_member_id;
    }

    [ProtoContract]
    public class OrganizationEventData
    {
        [ProtoMember(1)]
        public ulong org_uid;
        [ProtoMember(2)]
        public string org_name;
    }

    [ProtoContract]
    public class ScheduleEventCallbackData
    {
        [ProtoMember(1)]
        public ulong org_event_id;
        [ProtoMember(2)]
        public ulong schedule_venue_event_time;
    }

    [ProtoContract]
    public class PersistableCullingService
    {
        [ProtoMember(1)]
        public PersistableCullingServiceInteractingSim[] interacting_sims;
    }

    [ProtoContract]
    public class PersistableCullingServiceInteractingSim
    {
        [ProtoMember(1)]
        public ulong sim_id;
        [ProtoMember(2)]
        public float time_stamp;
    }

    [ProtoContract]
    public class PersistableStoryProgressionService
    {
        [ProtoMember(1)]
        public PersistableUniversityStoryProgressionAction university_action;
    }

    [ProtoContract]
    public class PersistableUniversityStoryProgressionAction
    {
        [ProtoMember(1)]
        public ulong next_update_time;
    }

    [ProtoContract]
    public class PersistableStreetService
    {
        [ProtoMember(1)]
        public PersistableStreetData[] street_data;
    }

    [ProtoContract]
    public class PersistableStreetData
    {
        [ProtoMember(1)]
        public ulong street_id;
        [ProtoMember(2)]
        public PersistableCivicPolicyProviderData civic_provider_data;
        [ProtoMember(3)]
        public PersistableStreetEcoFootprintData street_eco_footprint_data;
    }

    [ProtoContract]
    public class PersistableCivicPolicyProviderData
    {
        [ProtoMember(1)]
        public PersistableCivicPolicyCustomData[] policy_data;
        [ProtoMember(2)]
        public PersistableCommodityTracker commodity_tracker;
        [ProtoMember(3)]
        public PersistableStatisticsTracker statistics_tracker;
        [ProtoMember(4)]
        public PersistableRankedStatisticTracker ranked_statistic_tracker;
        [ProtoMember(5)]
        public ulong[] balloted_policy_ids;
        [ProtoMember(6)]
        public ulong[] up_for_repeal_policy_ids;
    }

    [ProtoContract]
    public class PersistableCivicPolicyCustomData
    {
        [ProtoMember(1)]
        public ulong policy_id;
        [ProtoMember(2)]
        public byte[] custom_data;
    }

    [ProtoContract]
    public class PersistableCommodityTracker
    {
        [ProtoMember(1)]
        public Commodity[] commodities;
        [ProtoMember(2)]
        public ulong time_of_last_save;
    }

    [ProtoContract]
    public class Commodity
    {
        [ProtoMember(1)]
        public ulong name_hash;
        [ProtoMember(2)]
        public float value;
        [ProtoMember(3)]
        public bool apply_buff_on_start_up;
        [ProtoMember(4)]
        public LocalizedString buff_reason;
        [ProtoMember(5)]
        public ulong time_of_last_value_change;
    }

    [ProtoContract]
    public class PersistableStatisticsTracker
    {
        [ProtoMember(1)]
        public Statistic[] statistics;
    }

    [ProtoContract]
    public class Statistic
    {
        [ProtoMember(1)]
        public ulong name_hash;
        [ProtoMember(2)]
        public float value;
    }

    [ProtoContract]
    public class PersistableRankedStatisticTracker
    {
        [ProtoMember(1)]
        public RankedStatistic[] ranked_statistics;
    }

    [ProtoContract]
    public class RankedStatistic
    {
        [ProtoMember(1)]
        public ulong name_hash;
        [ProtoMember(2)]
        public float value;
        [ProtoMember(3)]
        public ulong rank_level;
        [ProtoMember(4)]
        public ulong highest_level;
        [ProtoMember(5)]
        public ulong time_of_last_value_change;
        [ProtoMember(6)]
        public bool initial_loots_awarded;
        [ProtoMember(7)]
        public bool inclusive_rank_threshold;
    }

    [ProtoContract]
    public class PersistableStreetEcoFootprintData
    {
        [ProtoMember(1)]
        public EcoFootprintStateType current_eco_footprint_state;
        [ProtoMember(2)]
        public bool effects_are_simulated;
        [ProtoMember(3)]
        public float convergence;
    }

    public enum EcoFootprintStateType
    {
        GREEN = 0,
        NEUTRAL = 1,
        INDUSTRIAL = 2,
    }

    [ProtoContract]
    public class PersistableRegionService
    {
        [ProtoMember(1)]
        public PersistableRegionData[] region_data;
    }

    [ProtoContract]
    public class PersistableRegionData
    {
        [ProtoMember(1)]
        public ulong region_id;
        [ProtoMember(2)]
        public PersistableCommodityTracker commodity_tracker;
    }

    [ProtoContract]
    public class PersistableLifestyleService
    {
        [ProtoMember(1)]
        public bool has_seen_daily_cap_notification;
        [ProtoMember(2)]
        public bool has_seen_in_progress_notification;
        [ProtoMember(3)]
        public bool has_seen_hidden_notification;
        [ProtoMember(4)]
        public ulong[] hidden_lifestyles;
    }

    [ProtoContract]
    public class AccountData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong nucleus_id;
        [ProtoMember(2)]
        public string persona_name;
        [ProtoMember(3)]
        public string email;
        [ProtoMember(4)]
        public ulong created;
        [ProtoMember(5)]
        public ulong last_login;
        [ProtoMember(8)]
        public EntitlementData[] entitlements;
        [ProtoMember(9)]
        public UiOptionsData ui_options;
        [ProtoMember(10)]
        public string client_version;
        [ProtoMember(12, DataFormat = DataFormat.FixedSize)]
        public ulong save_slot_id;
        [ProtoMember(14)]
        public GameplayAccountData gameplay_account_data;
        [ProtoMember(16)]
        public UiDialogMessage[] game_notification;
        [ProtoMember(17)]
        public string client_version_at_creation;
        [ProtoMember(18)]
        public ulong number_of_saves;
        [ProtoMember(19)]
        public ulong number_of_saves_mods;
        [ProtoMember(20)]
        public ulong number_of_saves_script_mods;
    }

    [ProtoContract]
    public class EntitlementData
    {
        [ProtoMember(1)]
        public ulong transaction_id;
        [ProtoMember(2)]
        public byte[] payload;
        [ProtoMember(3)]
        public ulong initiation_time;
        [ProtoMember(4)]
        public ulong completion_time;
    }

    [ProtoContract]
    public class UiOptionsData
    {
        [ProtoMember(1)]
        public bool is_new_game;
        [ProtoMember(2)]
        public bool was_notification_alert_shown;
    }

    [ProtoContract]
    public class GameplayAccountData
    {
        [ProtoMember(1)]
        public AccountEventDataTracker achievement_data;
        [ProtoMember(2)]
        public GameplayOptions gameplay_options;
        [ProtoMember(8)]
        public UiDialogMessage[] game_notification;
        [ProtoMember(9)]
        public bool cheats_enabled;
        [ProtoMember(10)]
        public bool cheats_ever_enabled;
    }

    [ProtoContract]
    public class AccountEventDataTracker
    {
        [ProtoMember(1)]
        public uint[] milestones_completed;
        [ProtoMember(2)]
        public uint[] objectives_completed;
        [ProtoMember(3)]
        public EventDataObject data;
        [ProtoMember(4)]
        public MilestoneCompletionCount[] milestone_completion_counts;
    }

    [ProtoContract]
    public class EventDataObject
    {
        [ProtoMember(1)]
        public EventData_SituationData[] situation_data;
        [ProtoMember(2)]
        public EventData_Objective_Data[] objective_data;
        [ProtoMember(3)]
        public EventData_EnumToAmount[] simoleon_data;
        [ProtoMember(4)]
        public EventData_EnumToAmount[] time_data;
        [ProtoMember(5)]
        public EventData_NamedData[] interaction_data;
        [ProtoMember(6)]
        public EventData_RelationshipData[] relationship_data;
        [ProtoMember(8)]
        public ulong[] travel_data;
        [ProtoMember(9)]
        public EventData_TagData[] tag_data;
        [ProtoMember(10)]
        public EventData_CareerData[] career_data;
        [ProtoMember(11)]
        public EventData_RelativeStartingData[] relative_start_data;
        [ProtoMember(12)]
        public EventData_ClubBucksEarned club_bucks_data;
        [ProtoMember(13)]
        public EventData_TimeInClubGathering time_in_gatherings;
        [ProtoMember(14)]
        public EventData_EncouragedInteractions encouraged_interactions_started;
        [ProtoMember(15)]
        public EventData_Moods mood_data;
        [ProtoMember(16)]
        public EventData_EnumToAmount[] bucks_data;
    }

    [ProtoContract]
    public class EventData_SituationData
    {
        [ProtoMember(1)]
        public string name;
        [ProtoMember(2)]
        public EventData_SituationEnums[] results;
    }

    [ProtoContract]
    public class EventData_SituationEnums
    {
        [ProtoMember(1)]
        public uint result_enum;
        [ProtoMember(2)]
        public EventData_EnumToAmount[] enum_quality;
    }

    [ProtoContract]
    public class EventData_EnumToAmount
    {
        [ProtoMember(1)]
        public uint theenum;
        [ProtoMember(2)]
        public long amount;
    }

    [ProtoContract]
    public class EventData_Objective_Data
    {
        [ProtoMember(1)]
        public ulong theenum;
        [ProtoMember(2)]
        public uint amount;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong[] ids;
    }

    [ProtoContract]
    public class EventData_NamedData
    {
        [ProtoMember(1)]
        public string name;
        [ProtoMember(2)]
        public EventData_EnumToAmount[] enums;
    }

    [ProtoContract]
    public class EventData_RelationshipData
    {
        [ProtoMember(1)]
        public uint relationship_id;
        [ProtoMember(2)]
        public EventData_EnumToAmount[] enums;
    }

    [ProtoContract]
    public class EventData_TagData
    {
        [ProtoMember(1)]
        public uint tag_enum;
        [ProtoMember(2)]
        public EventData_EnumToAmount[] enums;
    }

    [ProtoContract]
    public class EventData_CareerData
    {
        [ProtoMember(1)]
        public string name;
        [ProtoMember(2)]
        public ulong time;
        [ProtoMember(3)]
        public ulong money;
    }

    [ProtoContract]
    public class EventData_RelativeStartingData
    {
        [ProtoMember(1)]
        public ulong objective_guid64;
        [ProtoMember(2)]
        public ulong[] starting_values;
    }

    [ProtoContract]
    public class EventData_ClubBucksEarned
    {
        [ProtoMember(1)]
        public uint amount;
    }

    [ProtoContract]
    public class EventData_TimeInClubGathering
    {
        [ProtoMember(1)]
        public uint sim_minutes;
    }

    [ProtoContract]
    public class EventData_EncouragedInteractions
    {
        [ProtoMember(1)]
        public uint interactions_started;
    }

    [ProtoContract]
    public class EventData_Moods
    {
        [ProtoMember(1)]
        public MoodData[] mood_data;
    }

    [ProtoContract]
    public class MoodData
    {
        [ProtoMember(1)]
        public ulong mood;
        [ProtoMember(2)]
        public ulong last_time_in_mood;
    }

    [ProtoContract]
    public class MilestoneCompletionCount
    {
        [ProtoMember(1)]
        public ulong milestone_guid;
        [ProtoMember(2)]
        public uint completion_count;
    }

    [ProtoContract]
    public class GameplayOptions
    {
        [ProtoMember(1)]
        public AutonomyLevel autonomy_level;
        [ProtoMember(2)]
        public bool selected_sim_autonomy_enabled;
        [ProtoMember(3)]
        public SimLifeSpan sim_life_span;
        [ProtoMember(4)]
        public bool aging_enabled;
        [ProtoMember(5)]
        public bool lessons_enabled;
        [ProtoMember(6)]
        public bool tutorial_situation_enabled;
        [ProtoMember(7)]
        public bool reset_lessons;
        [ProtoMember(8)]
        public SimAgeEnabled allow_aging;
        [ProtoMember(9)]
        public bool unplayed_aging_enabled;
        [ProtoMember(10)]
        public bool npc_population_enabled;
        [ProtoMember(11)]
        public uint max_player_population;
        [ProtoMember(12)]
        public SeasonLength season_length;
        [ProtoMember(13)]
        public WeatherOption rain_options;
        [ProtoMember(14)]
        public WeatherOption snow_options;
        [ProtoMember(15)]
        public bool temperature_effects_enabled;
        [ProtoMember(16)]
        public SeasonType initial_season;
        [ProtoMember(17)]
        public bool start_all_sims_opted_out_of_fame;
        [ProtoMember(18)]
        public bool npc_civic_voting_enabled;
        [ProtoMember(19)]
        public bool eco_footprint_gameplay_enabled;
        [ProtoMember(20)]
        public bool build_eco_effects_enabled;
        [ProtoMember(21)]
        public bool icy_conditions_enabled;
        [ProtoMember(22)]
        public bool thunder_snow_storms_enabled;
        [ProtoMember(23)]
        public bool lifestyles_effects_enabled;
    }

    public enum AutonomyLevel
    {
        OFF = 0,
        LIMITED = 1,
        MEDIUM = 2,
        FULL = 3,
        UNDEFINED = 4,
    }

    public enum SimLifeSpan
    {
        SHORT = 0,
        NORMAL = 1,
        LONG = 2,
    }

    public enum SimAgeEnabled
    {
        DISABLED = 0,
        ENABLED = 1,
        FOR_ACTIVE_FAMILY = 2,
    }

    public enum SeasonLength
    {
        NORMAL_SEASON = 0,
        LONG_SEASON = 1,
        VERY_LONG_SEASON = 2,
    }

    public enum WeatherOption
    {
        WEATHER_ENABLED = 0,
        DISABLE_STORMS = 1,
        WEATHER_DISABLED = 2,
    }

    public enum SeasonType
    {
        SUMMER = 0,
        FALL = 1,
        WINTER = 2,
        SPRING = 3,
    }

    [ProtoContract]
    public class UiDialogMessage
    {
        [ProtoMember(1)]
        public ulong dialog_id;
        [ProtoMember(2)]
        public UiDialogChoiceMessage[] choices;
        [ProtoMember(3)]
        public LocalizedString text;
        [ProtoMember(4)]
        public float timeout_duration;
        [ProtoMember(5)]
        public ulong owner_id;
        [ProtoMember(6)]
        public ulong target_id;
        [ProtoMember(7)]
        public UiDialogPicker picker_data;
        [ProtoMember(8)]
        public Type dialog_type;
        [ProtoMember(9)]
        public UiDialogTextInputMessage[] text_input;
        [ProtoMember(10)]
        public LocalizedString title;
        [ProtoMember(11)]
        public uint dialog_options;
        [ProtoMember(12)]
        public ResourceKey icon;
        [ProtoMember(13)]
        public IconInfo icon_info;
        [ProtoMember(14)]
        public IconInfo secondary_icon_info;
        [ProtoMember(15)]
        public ulong timestamp;
        [ProtoMember(16)]
        public LocalizedString lot_title;
        [ProtoMember(17)]
        public IconInfo venue_icon;
        [ProtoMember(18)]
        public IconInfo[] icon_infos;
        [ProtoMember(19)]
        public uint dialog_style;
        [ProtoMember(20)]
        public ulong override_sim_icon_id;
        [ProtoMember(21)]
        public UiDialogMultiPicker multi_picker_data;
        [ProtoMember(22)]
        public uint dialog_bg_style;
        [ProtoMember(23)]
        public bool is_special_dialog;
        [ProtoMember(24)]
        public UiDialogInfoInColumns info_in_columns_data;
        [ProtoMember(25)]
        public LocalizedString[] additional_texts;
        [ProtoMember(26)]
        public bool anonymous_target_sim;
        [ProtoMember(27)]
        public string background_audio_event;
    }

    [ProtoContract]
    public class UiDialogChoiceMessage
    {
        [ProtoMember(1)]
        public uint choice_id;
        [ProtoMember(2)]
        public LocalizedString text;
        [ProtoMember(3)]
        public UiDialogChoiceUiRequest ui_request;
        [ProtoMember(4)]
        public UiExchangeArgs exchange_args;
        [ProtoMember(5)]
        public UiTutorialArgs tutorial_args;
        [ProtoMember(6)]
        public UiCommandArgs command_with_args;
        [ProtoMember(7)]
        public LocalizedString subtext;
        [ProtoMember(8)]
        public LocalizedString disabled_text;
    }

    public enum UiDialogChoiceUiRequest
    {
        NO_REQUEST = 0,
        SHOW_LESSONS = 1,
        SHOW_ACHIEVEMENTS = 2,
        SHOW_GALLERY_ITEM = 3,
        SHOW_FAMILY_INVENTORY = 4,
        SHOW_SKILL_PANEL = 5,
        SHOW_SUMMARY_PANEL = 6,
        SHOW_ASPIRATION_PANEL = 7,
        SHOW_ASPIRATION_UI = 8,
        SHOW_EVENT_UI = 9,
        SHOW_CAREER_PANEL = 10,
        SHOW_RELATIONSHIP_PANEL = 11,
        SHOW_SIM_INVENTORY = 12,
        SHOW_REWARD_STORE = 13,
        SHOW_MOTIVE_PANEL = 14,
        SHOW_STATS = 15,
        SHOW_COLLECTIBLES = 16,
        SHOW_CAREER_UI = 17,
        TRANSITION_TO_NEIGHBORHOOD_SAVE = 18,
        TRANSITION_TO_MAIN_MENU_NO_SAVE = 19,
        SHOW_SHARE_PLAYER_PROFILE = 20,
        SHOW_ASPIRATION_SELECTOR = 21,
        SHOW_SHARE_MY_LIBRARY = 22,
        SHOW_NOTEBOOK = 23,
        SEND_COMMAND = 24,
        CAREER_GO_TO_WORK = 25,
        CAREER_WORK_FROM_HOME = 26,
        CAREER_TAKE_PTO = 27,
        CAREER_CALL_IN_SICK = 28,
        SHOW_OCCULT_POWERS_PANEL = 29,
        SHOW_FAME_PERKS_PANEL = 30,
        SHOW_FACTION_REP_PANEL = 31,
    }

    [ProtoContract]
    public class UiExchangeArgs
    {
        [ProtoMember(1)]
        public ulong item_id;
        [ProtoMember(2)]
        public ExchangeItemTypes item_type;
        [ProtoMember(3)]
        public bool is_favorite;
        [ProtoMember(4)]
        public ulong creator_id;
        [ProtoMember(5)]
        public string creator_name;
        [ProtoMember(6)]
        public TrayMetadata item_data;
        [ProtoMember(7)]
        public byte[] feed_id;
        [ProtoMember(8)]
        public SocialFeedItemType feed_type;
        [ProtoMember(9)]
        public uint quantity;
        [ProtoMember(10)]
        public ulong timestamp;
        [ProtoMember(11)]
        public bool is_maxis_curated;
    }

    public enum ExchangeItemTypes
    {
        EXCHANGE_INVALIDTYPE = 0,
        EXCHANGE_HOUSEHOLD = 1,
        EXCHANGE_BLUEPRINT = 2,
        EXCHANGE_ROOM = 3,
        EXCHANGE_ALLTYPES = 4,
        EXCHANGE_ITEMTYPE_MAX = 5,
    }

    [ProtoContract]
    public class TrayMetadata
    {
        [ProtoMember(1)]
        public ulong id;
        [ProtoMember(2)]
        public ExchangeItemTypes type;
        [ProtoMember(3)]
        public byte[] remote_id;
        [ProtoMember(4)]
        public string name;
        [ProtoMember(5)]
        public string description;
        [ProtoMember(6)]
        public ulong creator_id;
        [ProtoMember(7)]
        public string creator_name;
        [ProtoMember(8)]
        public ulong favorites;
        [ProtoMember(9)]
        public ulong downloads;
        [ProtoMember(10)]
        public SpecificData metadata;
        [ProtoMember(11)]
        public ulong item_timestamp;
        [ProtoMember(12)]
        public ulong[] mtx_ids;
        [ProtoMember(13)]
        public byte[] creator_uuid;
        [ProtoMember(14)]
        public ulong modifier_id;
        [ProtoMember(15)]
        public string modifier_name;
        [ProtoMember(16)]
        public uint[] meta_info;
        [ProtoMember(17)]
        public int verify_code;
        [ProtoMember(20)]
        public uint custom_image_count;
        [ProtoMember(21)]
        public uint mannequin_count;
        [ProtoMember(25)]
        public ulong indexed_counter;
        [ProtoMember(26)]
        public ExchangeItemPlatform creator_platform;
        [ProtoMember(27)]
        public ExchangeItemPlatform modifier_platform;
        [ProtoMember(28)]
        public ulong creator_platform_id;
        [ProtoMember(29)]
        public string creator_platform_name;
        [ProtoMember(30)]
        public ulong modifier_platform_id;
        [ProtoMember(31)]
        public string modifier_platform_name;
    }

    [ProtoContract]
    public class SpecificData
    {
        [ProtoMember(1)]
        public TrayBlueprintMetadata bp_metadata;
        [ProtoMember(2)]
        public TrayHouseholdMetadata hh_metadata;
        [ProtoMember(7)]
        public TrayRoomBlueprintMetadata ro_metadata;
        [ProtoMember(3)]
        public bool is_hidden;
        [ProtoMember(4)]
        public bool is_downloadtemp;
        [ProtoMember(5)]
        public bool is_modded_content;
        [ProtoMember(6)]
        public ExtraThumbnailInfo xti;
        [ProtoMember(8)]
        public string description_hashtags;
        [ProtoMember(9)]
        public ulong language_id;
        [ProtoMember(10)]
        public ulong sku_id;
        [ProtoMember(11)]
        public bool is_maxis_content;
        [ProtoMember(12)]
        public uint payloadsize;
        [ProtoMember(13)]
        public bool was_reported;
        [ProtoMember(14)]
        public bool was_reviewed_and_cleared;
        [ProtoMember(15)]
        public bool is_image_modded_content;
        [ProtoMember(16)]
        public ExchangeItemPlatform sd_creator_platform;
        [ProtoMember(17)]
        public ExchangeItemPlatform sd_modifier_platform;
        [ProtoMember(18)]
        public ulong sd_creator_platform_persona_id;
        [ProtoMember(19)]
        public ulong sd_modifier_platform_persona_id;
        [ProtoMember(20)]
        public bool is_cg_item;
        [ProtoMember(21)]
        public bool is_cg_interested;
        [ProtoMember(22)]
        public string cg_name;
        [ProtoMember(1000)]
        public TrayMetadataVersion version_OBSOLETE;
        [ProtoMember(1001)]
        public uint version;
    }

    [ProtoContract]
    public class TrayBlueprintMetadata
    {
        [ProtoMember(1)]
        public ulong venue_type;
        [ProtoMember(2)]
        public uint size_x;
        [ProtoMember(3)]
        public uint size_z;
        [ProtoMember(4)]
        public uint price_level;
        [ProtoMember(5)]
        public uint price_value;
        [ProtoMember(6)]
        public uint num_bedrooms;
        [ProtoMember(7)]
        public uint num_bathrooms;
        [ProtoMember(8)]
        public uint architecture_value;
        [ProtoMember(9)]
        public uint num_thumbnails;
        [ProtoMember(10)]
        public uint front_side;
        [ProtoMember(11)]
        public uint venue_type_stringkey;
        [ProtoMember(12)]
        public uint ground_floor_index;
        [ProtoMember(13)]
        public uint[] optional_rule_satisfied_stringkeys;
        [ProtoMember(14, DataFormat = DataFormat.FixedSize)]
        public ulong[] lot_traits;
        [ProtoMember(15)]
        public uint building_type;
        [ProtoMember(16)]
        public ulong lot_template_id;
        [ProtoMember(17)]
        public UniversityHousingConfiguration university_housing_configuration;
        [ProtoMember(18)]
        public uint tile_count;
    }

    [ProtoContract]
    public class UniversityHousingConfiguration
    {
        [ProtoMember(1)]
        public ulong university_id;
        [ProtoMember(2)]
        public uint gender;
        [ProtoMember(3)]
        public ulong organization_id;
        [ProtoMember(4)]
        public uint roommate_bed_count;
        [ProtoMember(6)]
        public ulong club_id;
    }

    [ProtoContract]
    public class TrayHouseholdMetadata
    {
        [ProtoMember(1)]
        public uint family_size;
        [ProtoMember(2)]
        public TraySimMetadata[] sim_data;
        [ProtoMember(3)]
        public uint pending_babies;
    }

    [ProtoContract]
    public class TraySimMetadata
    {
        [ProtoMember(1)]
        public PersistableTraitTracker trait_tracker;
        [ProtoMember(2)]
        public PersistableGenealogyTracker genealogy_tracker;
        [ProtoMember(3)]
        public string first_name;
        [ProtoMember(4)]
        public string last_name;
        [ProtoMember(5)]
        public ulong id;
        [ProtoMember(6)]
        public uint gender;
        [ProtoMember(7)]
        public ulong aspirationId;
        [ProtoMember(8)]
        public FamilyRelation[] sim_relationships;
        [ProtoMember(9)]
        public uint age;
        [ProtoMember(10)]
        public WebTraitTracker[] web_trait_tracker;
        [ProtoMember(11)]
        public WebAspirationInfo web_aspiration_info;
        [ProtoMember(12)]
        public uint species;
        [ProtoMember(13)]
        public bool is_custom_gender;
        [ProtoMember(14)]
        public uint occult_types;
        [ProtoMember(15)]
        public string breed_name;
        [ProtoMember(16)]
        public uint breed_name_key;
        [ProtoMember(17)]
        public TrayRankedStatMetadata fame;
    }

    [ProtoContract]
    public class PersistableTraitTracker
    {
        [ProtoMember(1)]
        public ulong[] trait_ids;
    }

    [ProtoContract]
    public class PersistableGenealogyTracker
    {
        [ProtoMember(1)]
        public FamilyRelation[] family_relations;
    }

    [ProtoContract]
    public class FamilyRelation
    {
        [ProtoMember(1)]
        public RelationshipIndex relation_type;
        [ProtoMember(2)]
        public ulong sim_id;
    }

    public enum RelationshipIndex
    {
        RELATIONSHIP_MOTHER = 0,
        RELATIONSHIP_FATHER = 1,
        RELATIONSHIP_MOTHERS_MOM = 2,
        RELATIONSHIP_MOTHERS_FATHER = 3,
        RELATIONSHIP_FATHERS_MOM = 4,
        RELATIONSHIP_FATHERS_FATHER = 5,
        RELATIONSHIP_NONE = 6,
        RELATIONSHIP_PARENT = 7,
        RELATIONSHIP_SIBLING = 8,
        RELATIONSHIP_SPOUSE = 9,
        RELATIONSHIP_UNUSED1 = 10,
        RELATIONSHIP_UNUSED2 = 11,
        RELATIONSHIP_DESCENDANT = 12,
        RELATIONSHIP_GRANDPARENT = 13,
        RELATIONSHIP_GRANDCHILD = 14,
        RELATIONSHIP_SIBLINGS_CHILDREN = 15,
        RELATIONSHIP_PARENTS_SIBLING = 16,
        RELATIONSHIP_COUSIN = 17,
    }

    [ProtoContract]
    public class WebTraitTracker
    {
        [ProtoMember(1)]
        public uint name_hash;
        [ProtoMember(2)]
        public string name_string;
        [ProtoMember(3)]
        public uint description_hash;
        [ProtoMember(4)]
        public string description_string;
        [ProtoMember(5)]
        public ResourceKey icon_key;
        [ProtoMember(6)]
        public long trait_type;
        [ProtoMember(7)]
        public uint description_origin_hash;
        [ProtoMember(8)]
        public string description_origin_string;
        [ProtoMember(9)]
        public ResourceKey cas_selected_icon_key;
    }

    [ProtoContract]
    public class WebAspirationInfo
    {
        [ProtoMember(1)]
        public uint display_hash;
        [ProtoMember(2)]
        public string display_string;
        [ProtoMember(3)]
        public uint description_hash;
        [ProtoMember(4)]
        public string description_string;
        [ProtoMember(5)]
        public ResourceKey icon;
        [ProtoMember(6)]
        public ResourceKey icon_high_res;
        [ProtoMember(7)]
        public WebTraitTracker primary_trait;
    }

    [ProtoContract]
    public class TrayRankedStatMetadata
    {
        [ProtoMember(1)]
        public ulong id;
        [ProtoMember(2)]
        public float value;
    }

    [ProtoContract]
    public class TrayRoomBlueprintMetadata
    {
        [ProtoMember(1)]
        public uint room_type;
        [ProtoMember(2)]
        public uint size_x;
        [ProtoMember(3)]
        public uint size_z;
        [ProtoMember(4)]
        public uint price_value;
        [ProtoMember(5)]
        public uint height;
        [ProtoMember(6)]
        public uint price_level;
        [ProtoMember(7)]
        public uint room_type_stringkey;
    }

    [ProtoContract]
    public class ExtraThumbnailInfo
    {
        [ProtoMember(1)]
        public uint[] thumbnail_info;
    }

    public enum ExchangeItemPlatform
    {
        EXCHANGE_PLATFORM_UNKNOWN = 0,
        EXCHANGE_PLATFORM_WINDOWS = 100,
        EXCHANGE_PLATFORM_OSX = 200,
        EXCHANGE_PLATFORM_PS4 = 300,
        EXCHANGE_PLATFORM_XB1 = 400,
    }

    public enum TrayMetadataVersion
    {
        v000 = 0,
        currentVersion = 6900,
    }

    public enum SocialFeedItemType
    {
        SFI_ITEM_DOWNLOADED = 0,
        SFI_ITEM_UPLOADED = 1,
        SFI_ITEM_FAVORITED = 2,
        SFI_ITEM_COMMENTED = 3,
        SFI_ITEM_SHOWCASED = 4,
        SFI_PROFILE_COMMENTED = 5,
        SFI_NEW_FOLLOWERS = 6,
    }

    [ProtoContract]
    public class UiTutorialArgs
    {
        [ProtoMember(1)]
        public ulong tutorial_id;
    }

    [ProtoContract]
    public class UiCommandArgs
    {
        [ProtoMember(1)]
        public string command_name;
        [ProtoMember(2)]
        public RemoteArgs command_remote_args;
    }

    [ProtoContract]
    public class RemoteArgs
    {
        [ProtoMember(1)]
        public Arg[] args;
    }

    [ProtoContract]
    public class Arg
    {
        [ProtoMember(1)]
        public bool thebool;
        [ProtoMember(2)]
        public int int32;
        [ProtoMember(3)]
        public long int64;
        [ProtoMember(4)]
        public uint uint32;
        [ProtoMember(5)]
        public ulong uint64;
        [ProtoMember(6)]
        public float thefloat;
        [ProtoMember(7)]
        public string thestring;
    }

    [ProtoContract]
    public class UiDialogPicker
    {
        [ProtoMember(1)]
        public ObjectPickerType type;
        [ProtoMember(2)]
        public LocalizedString title;
        [ProtoMember(3)]
        public ulong owner_sim_id;
        [ProtoMember(4)]
        public ulong target_sim_id;
        [ProtoMember(6)]
        public RecipePickerData recipe_picker_data;
        [ProtoMember(7)]
        public SimPickerData sim_picker_data;
        [ProtoMember(8)]
        public ObjectPickerData object_picker_data;
        [ProtoMember(10)]
        public OutfitPickerData outfit_picker_data;
        [ProtoMember(11)]
        public uint max_selectable;
        [ProtoMember(12)]
        public PurchasePickerData shop_picker_data;
        [ProtoMember(13)]
        public LotPickerData lot_picker_data;
        [ProtoMember(14)]
        public bool is_sortable;
        [ProtoMember(15)]
        public bool hide_row_description;
        [ProtoMember(16)]
        public bool use_dropdown_filter;
        [ProtoMember(17)]
        public PickerBaseRowData[] row_picker_data;
        [ProtoMember(18)]
        public uint min_selectable;
        [ProtoMember(19)]
        public PickerFilterData[] filter_data;
        [ProtoMember(20)]
        public DropdownPickerData dropdown_picker_data;
        [ProtoMember(21)]
        public DescriptionDisplay description_display;
        [ProtoMember(22)]
        public OddJobPickerData odd_job_picker_data;
    }

    public enum ObjectPickerType
    {
        RECIPE = 1,
        INTERACTION = 2,
        SIM = 3,
        OBJECT = 4,
        PIE_MENU = 5,
        CAREER = 6,
        OUTFIT = 7,
        PURCHASE = 8,
        LOT = 9,
        SIM_CLUB = 10,
        ITEM = 11,
        OBJECT_LARGE = 12,
        DROPDOWN = 13,
        OBJECT_SQUARE = 14,
        ODD_JOBS = 15,
        MISSIONS = 16,
    }

    [ProtoContract]
    public class RecipePickerData
    {
        [ProtoMember(1)]
        public PickerColumn[] column_list;
        [ProtoMember(2)]
        public RecipePickerRowData[] row_data;
        [ProtoMember(3)]
        public ulong skill_id;
        [ProtoMember(4)]
        public uint[] column_sort_list;
        [ProtoMember(5)]
        public bool display_ingredient_check;
        [ProtoMember(6)]
        public bool display_funds;
    }

    [ProtoContract]
    public class PickerColumn
    {
        [ProtoMember(1)]
        public ColumnType type;
        [ProtoMember(2)]
        public string column_data_name;
        [ProtoMember(3)]
        public string column_icon_name;
        [ProtoMember(4)]
        public LocalizedString label;
        [ProtoMember(5)]
        public ResourceKey icon;
        [ProtoMember(6)]
        public LocalizedString tooltip;
        [ProtoMember(7)]
        public float width;
        [ProtoMember(8)]
        public bool sortable;
    }

    public enum ColumnType
    {
        TEXT = 1,
        ICON = 2,
        ICON_AND_TEXT = 3,
    }

    [ProtoContract]
    public class RecipePickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2)]
        public uint price;
        [ProtoMember(3)]
        public uint skill_level;
        [ProtoMember(4)]
        public uint[] linked_option_ids;
        [ProtoMember(5)]
        public ResourceKey event_icon;
        [ProtoMember(6)]
        public bool visible_as_subrow;
        [ProtoMember(7)]
        public RecipeIngredientData[] ingredients;
        [ProtoMember(8)]
        public uint price_with_ingredients;
        [ProtoMember(9, DataFormat = DataFormat.FixedSize)]
        public ulong mtx_id;
        [ProtoMember(10)]
        public LocalizedString serving_display_name;
        [ProtoMember(11)]
        public uint discounted_price;
        [ProtoMember(12)]
        public bool is_discounted;
        [ProtoMember(13)]
        public bool show_full_subrows;
        [ProtoMember(14)]
        public RecipeBucksCostData[] bucks_costs;
        [ProtoMember(15)]
        public int subrow_sort_id;
        [ProtoMember(16)]
        public IconInfo locked_in_cas_icon_info;
    }

    [ProtoContract]
    public class PickerBaseRowData
    {
        [ProtoMember(1)]
        public uint option_id;
        [ProtoMember(2)]
        public bool is_enable;
        [ProtoMember(3)]
        public LocalizedString name;
        [ProtoMember(4)]
        public ResourceKey icon;
        [ProtoMember(5)]
        public LocalizedString description;
        [ProtoMember(6)]
        public IconInfo icon_info;
        [ProtoMember(7)]
        public LocalizedString tooltip;
        [ProtoMember(8)]
        public bool is_selected;
        [ProtoMember(9)]
        public uint[] tag_list;
    }

    [ProtoContract]
    public class IconInfo
    {
        [ProtoMember(1)]
        public LocalizedString name;
        [ProtoMember(2)]
        public ResourceKey icon;
        [ProtoMember(3)]
        public LocalizedString desc;
        [ProtoMember(4)]
        public ManagerObjectId icon_object;
        [ProtoMember(5)]
        public DefinitionGeoPair icon_object_def;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong texture_id;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong object_instance_id;
        [ProtoMember(8, DataFormat = DataFormat.FixedSize)]
        public ulong texture_effect;
        [ProtoMember(9)]
        public uint control_id;
        [ProtoMember(10)]
        public LocalizedString tooltip;
    }

    [ProtoContract]
    public class ManagerObjectId
    {
        [ProtoMember(1)]
        public ManagerIds manager_id;
        [ProtoMember(2)]
        public ulong object_id;
    }

    public enum ManagerIds
    {
        MGR_UNMANAGED = 0,
        MGR_OBJECT = 1,
        MGR_SOCIAL_GROUP = 2,
        MGR_SIM_INFO = 3,
        MGR_CLIENT = 4,
        MGR_SITUATION = 5,
        MGR_PARTY = 6,
        MGR_HOUSEHOLD = 7,
        MGR_SYSTEM = 8,
        MGR_INVENTORY = 9,
        MGR_TRAVEL_GROUP = 10,
    }

    [ProtoContract]
    public class DefinitionGeoPair
    {
        [ProtoMember(1)]
        public ulong definition_id;
        [ProtoMember(2)]
        public uint geo_state_hash;
        [ProtoMember(3)]
        public uint material_hash;
    }

    [ProtoContract]
    public class RecipeIngredientData
    {
        [ProtoMember(1)]
        public LocalizedString ingredient_name;
        [ProtoMember(2)]
        public bool in_inventory;
    }

    [ProtoContract]
    public class RecipeBucksCostData
    {
        [ProtoMember(1)]
        public uint bucks_type;
        [ProtoMember(2)]
        public uint amount;
    }

    [ProtoContract]
    public class SimPickerData
    {
        [ProtoMember(1)]
        public SimPickerRowData[] row_data;
        [ProtoMember(2)]
        public bool should_show_names;
        [ProtoMember(3)]
        public ClubBuildingInfo club_building_info;
        [ProtoMember(11)]
        public uint[] rel_bit_collection_ids;
        [ProtoMember(12)]
        public uint column_count;
    }

    [ProtoContract]
    public class SimPickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2)]
        public ulong sim_id;
        [ProtoMember(3)]
        public bool select_default;
        [ProtoMember(4)]
        public uint[] failed_criteria;
    }

    [ProtoContract]
    public class ClubBuildingInfo
    {
        [ProtoMember(1)]
        public ClubCriteria[] criterias;
        [ProtoMember(2)]
        public LotInfoItem[] available_lots;
    }

    [ProtoContract]
    public class LotInfoItem
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(2)]
        public string name;
        [ProtoMember(3)]
        public uint world_id;
        [ProtoMember(4)]
        public uint lot_template_id;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong lot_description_id;
        [ProtoMember(6)]
        public LocalizedString venue_type_name;
        [ProtoMember(7)]
        public string household_name;
        [ProtoMember(8)]
        public ResourceKey venue_type;
    }

    [ProtoContract]
    public class ObjectPickerData
    {
        [ProtoMember(1)]
        public ObjectPickerRowData[] row_data;
        [ProtoMember(2)]
        public uint num_columns;
    }

    [ProtoContract]
    public class ObjectPickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2)]
        public ulong object_id;
        [ProtoMember(3)]
        public ulong def_id;
        [ProtoMember(4)]
        public uint count;
        [ProtoMember(5)]
        public LocalizedString rarity_text;
        [ProtoMember(6)]
        public bool use_catalog_product_thumbnails;
    }

    [ProtoContract]
    public class OutfitPickerData
    {
        [ProtoMember(1)]
        public OutfitPickerRowData[] row_data;
        [ProtoMember(2)]
        public ObjectPickerThumbnailType thumbnail_type;
        [ProtoMember(3)]
        public uint[] outfit_category_filters;
    }

    [ProtoContract]
    public class OutfitPickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong outfit_sim_id;
        [ProtoMember(3)]
        public uint outfit_category;
        [ProtoMember(4)]
        public uint outfit_index;
    }

    public enum ObjectPickerThumbnailType
    {
        SIM_INFO = 1,
        MANNEQUIN = 2,
    }

    [ProtoContract]
    public class PurchasePickerData
    {
        [ProtoMember(1)]
        public PurchasePickerRowData[] row_data;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong object_id;
        [ProtoMember(3)]
        public bool show_description;
        [ProtoMember(4)]
        public bool mailman_purchase;
        [ProtoMember(5)]
        public PurchasePickerCategory[] categories;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong inventory_object_id;
        [ProtoMember(7)]
        public bool show_cost;
        [ProtoMember(8)]
        public uint max_selectable_in_row;
        [ProtoMember(9)]
        public bool show_description_tooltip;
        [ProtoMember(10)]
        public bool use_dialog_pick_response;
        [ProtoMember(11)]
        public LocalizedString right_custom_text;
    }

    [ProtoContract]
    public class PurchasePickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2)]
        public ulong def_id;
        [ProtoMember(3)]
        public uint num_owned;
        [ProtoMember(4)]
        public uint[] tag_list;
        [ProtoMember(5)]
        public int num_available;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong object_id;
        [ProtoMember(7)]
        public int custom_price;
        [ProtoMember(8)]
        public bool is_discounted;
    }

    [ProtoContract]
    public class PurchasePickerCategory
    {
        [ProtoMember(1)]
        public uint tag_type;
        [ProtoMember(2)]
        public IconInfo icon_info;
        [ProtoMember(3)]
        public LocalizedString description;
    }

    [ProtoContract]
    public class LotPickerData
    {
        [ProtoMember(1)]
        public LotPickerRowData[] row_data;
    }

    [ProtoContract]
    public class LotPickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2)]
        public LotInfoItem lot_info_item;
    }

    [ProtoContract]
    public class PickerFilterData
    {
        [ProtoMember(1)]
        public uint tag_type;
        [ProtoMember(2)]
        public IconInfo icon_info;
        [ProtoMember(3)]
        public LocalizedString description;
    }

    [ProtoContract]
    public class DropdownPickerData
    {
        [ProtoMember(1)]
        public UiDialogDropdownItem default_item;
        [ProtoMember(2)]
        public UiDialogDropdownItem[] items;
        [ProtoMember(3)]
        public LocalizedString invalid_tooltip;
        [ProtoMember(4)]
        public uint selected_item_id;
        [ProtoMember(5)]
        public uint options;
    }

    [ProtoContract]
    public class UiDialogDropdownItem
    {
        [ProtoMember(1)]
        public LocalizedString text;
        [ProtoMember(2)]
        public IconInfo icon_info;
        [ProtoMember(3)]
        public uint id;
    }

    public enum DescriptionDisplay
    {
        DEFAULT = 1,
        NO_DESCRIPTION = 2,
        FULL_DESCRIPTION = 3,
    }

    [ProtoContract]
    public class OddJobPickerData
    {
        [ProtoMember(1)]
        public OddJobPickerRowData[] row_data;
        [ProtoMember(2)]
        public uint star_ranking;
        [ProtoMember(3)]
        public ResourceKey picker_background;
    }

    [ProtoContract]
    public class OddJobPickerRowData
    {
        [ProtoMember(1)]
        public PickerBaseRowData base_data;
        [ProtoMember(2)]
        public ulong customer_id;
        [ProtoMember(3)]
        public LocalizedString customer_description;
        [ProtoMember(4)]
        public LocalizedString tip_title;
        [ProtoMember(5)]
        public IconInfo tip_icon;
        [ProtoMember(6)]
        public IconInfo customer_thumbnail_override;
        [ProtoMember(7)]
        public IconInfo customer_background;
        [ProtoMember(8)]
        public LocalizedString customer_name;
    }

    public enum Type
    {
        DEFAULT = 1,
        OBJECT_PICKER = 2,
        NOTIFICATION = 3,
        OK_CANCEL_ICONS = 4,
        INFO_SETTING = 5,
        ICONS_LABELS = 6,
        MULTI_PICKER = 7,
        INFO_IN_COLUMNS = 8,
        CUSTOMIZE_OBJECT_MULTI_PICKER = 9,
    }

    [ProtoContract]
    public class UiDialogTextInputMessage
    {
        [ProtoMember(1)]
        public string text_input_name;
        [ProtoMember(2)]
        public LocalizedString default_text;
        [ProtoMember(3)]
        public LocalizedString initial_value;
        [ProtoMember(4)]
        public int min_length;
        [ProtoMember(5)]
        public int max_length;
        [ProtoMember(6)]
        public LocalizedString restricted_characters;
        [ProtoMember(10)]
        public LocalizedString input_too_short_tooltip;
        [ProtoMember(11)]
        public LocalizedString title;
        [ProtoMember(12)]
        public int max_value;
        [ProtoMember(13)]
        public LocalizedString input_invalid_max_tooltip;
        [ProtoMember(14)]
        public int min_value;
        [ProtoMember(15)]
        public LocalizedString input_invalid_min_tooltip;
        [ProtoMember(16)]
        public bool check_profanity;
    }

    [ProtoContract]
    public class UiDialogMultiPicker
    {
        [ProtoMember(1)]
        public UiDialogMultiPickerItem[] multi_picker_items;
    }

    [ProtoContract]
    public class UiDialogMultiPickerItem
    {
        [ProtoMember(1)]
        public UiDialogPicker picker_data;
        [ProtoMember(2)]
        public uint picker_id;
        [ProtoMember(3)]
        public LocalizedString disabled_tooltip;
    }

    [ProtoContract]
    public class UiDialogInfoInColumns
    {
        [ProtoMember(1)]
        public LocalizedString[] column_headers;
        [ProtoMember(2)]
        public UiDialogRowData[] rows;
    }

    [ProtoContract]
    public class UiDialogRowData
    {
        [ProtoMember(1)]
        public IconInfo[] column_info;
    }

    [ProtoContract]
    public class NeighborhoodData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong neighborhood_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong owner_id;
        [ProtoMember(3)]
        public string name;
        [ProtoMember(4)]
        public ulong region_id;
        [ProtoMember(5)]
        public LotOwnerInfo[] lots;
        [ProtoMember(6)]
        public uint permissions;
        [ProtoMember(7)]
        public HouseholdAccountPair[] households;
        [ProtoMember(8)]
        public HouseholdAccountPair[] npc_households;
        [ProtoMember(9)]
        public GameplayNeighborhoodData gameplay_data;
        [ProtoMember(10)]
        public string description;
        [ProtoMember(11)]
        public uint bedroom_count;
        [ProtoMember(12)]
        public uint bathroom_count;
        [ProtoMember(13)]
        public StreetInfoData[] street_data;
    }

    [ProtoContract]
    public class LotOwnerInfo
    {
        [ProtoMember(1)]
        public ulong lot_description_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong zone_instance_id;
        [ProtoMember(3)]
        public string lot_name;
        [ProtoMember(4)]
        public HouseholdAccountPair[] lot_owner;
        [ProtoMember(5)]
        public uint lot_template_id;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong venue_key;
        [ProtoMember(8)]
        public bool venue_eligible;
        [ProtoMember(9)]
        public string lot_description;
        [ProtoMember(10)]
        public int venue_tier;
        [ProtoMember(11)]
        public EcoFootprintStateType eco_footprint_state;
        [ProtoMember(12)]
        public SubVenueInfo[] sub_venue_infos;
    }

    [ProtoContract]
    public class HouseholdAccountPair
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong nucleus_id;
        [ProtoMember(2)]
        public string persona_name;
        [ProtoMember(3)]
        public ulong household_id;
        [ProtoMember(4)]
        public string household_name;
        [ProtoMember(5)]
        public bool is_npc;
    }

    [ProtoContract]
    public class SubVenueInfo
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sub_venue_key;
        [ProtoMember(2)]
        public bool sub_venue_eligible;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong[] sub_venue_lot_traits;
    }

    [ProtoContract]
    public class GameplayNeighborhoodData
    {
        [ProtoMember(1)]
        public NpcPopulationState npc_population_state;
    }

    public enum NpcPopulationState
    {
        NOT_STARTED = 0,
        STARTED = 1,
        DIALOG_DISPLAYED = 2,
        COMPLETED = 3,
    }

    [ProtoContract]
    public class StreetInfoData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong world_id;
        [ProtoMember(2)]
        public uint[] map_overlays;
        [ProtoMember(3)]
        public EcoFootprintStateType eco_footprint_state;
        [ProtoMember(4)]
        public float normalized_eco_footprint_state_progress;
        [ProtoMember(5)]
        public float eco_footprint_delta;
    }

    [ProtoContract]
    public class HouseholdData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong account_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(3)]
        public string name;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong home_zone;
        [ProtoMember(5)]
        public ulong money;
        [ProtoMember(6)]
        public ObjectList inventory;
        [ProtoMember(9, DataFormat = DataFormat.FixedSize)]
        public ulong last_played_sim_id;
        [ProtoMember(10)]
        public ulong creation_time;
        [ProtoMember(11)]
        public IdList sims;
        [ProtoMember(12)]
        public IdList owned_lots;
        [ProtoMember(13)]
        public uint instanced_object_count;
        [ProtoMember(15)]
        public uint revision;
        [ProtoMember(16)]
        public GameplayHouseholdData gameplay_data;
        [ProtoMember(17)]
        public ulong[] cas_inventory;
        [ProtoMember(18)]
        public string description;
        [ProtoMember(19)]
        public ulong last_modified_time;
        [ProtoMember(20, DataFormat = DataFormat.FixedSize)]
        public ulong creator_id;
        [ProtoMember(21)]
        public string creator_name;
        [ProtoMember(22)]
        public byte[] creator_uuid;
        [ProtoMember(23, DataFormat = DataFormat.FixedSize)]
        public ulong modifier_id;
        [ProtoMember(24)]
        public string modifier_name;
        [ProtoMember(25)]
        public RewardPartList reward_inventory;
        [ProtoMember(26)]
        public bool hidden;
        [ProtoMember(27)]
        public bool cheats_enabled;
        [ProtoMember(28)]
        public bool needs_welcome_wagon;
        [ProtoMember(29)]
        public ulong premade_household_id;
        [ProtoMember(30)]
        public IdList pending_urnstones;
        [ProtoMember(14)]
        public bool is_unplayed;
        [ProtoMember(31)]
        public bool is_player;
        [ProtoMember(32, DataFormat = DataFormat.FixedSize)]
        public ulong premade_household_template_id;
    }

    [ProtoContract]
    public class ObjectList
    {
        [ProtoMember(1)]
        public ObjectData[] objects;
    }

    [ProtoContract]
    public class ObjectData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong object_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong owner_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong parent_id;
        [ProtoMember(4)]
        public uint slot_id;
        [ProtoMember(5)]
        public LotCoord position;
        [ProtoMember(6)]
        public uint loc_type;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong container_id;
        [ProtoMember(8)]
        public uint type;
        [ProtoMember(9)]
        public int level;
        [ProtoMember(10)]
        public float scale;
        [ProtoMember(11)]
        public uint state_index;
        [ProtoMember(12)]
        public byte[] attributes;
        [ProtoMember(13)]
        public uint cost;
        [ProtoMember(14, DataFormat = DataFormat.FixedSize)]
        public ulong baby_sim_id;
        [ProtoMember(15)]
        public UiObjectMetadata ui_metadata;
        [ProtoMember(16)]
        public bool has_been_depreciated;
        [ProtoMember(17)]
        public bool needs_depreciation;
        [ProtoMember(18)]
        public bool created_from_lot_template;
        [ProtoMember(19)]
        public bool is_new;
        [ProtoMember(20, DataFormat = DataFormat.FixedSize)]
        public ulong texture_id;
        [ProtoMember(21)]
        public uint material_variant;
        [ProtoMember(22)]
        public uint stack_sort_order;
        [ProtoMember(23)]
        public Vector3 light_color;
        [ProtoMember(24)]
        public uint material_state;
        [ProtoMember(25)]
        public uint geometry_state;
        [ProtoMember(26)]
        public uint object_parent_type;
        [ProtoMember(27)]
        public ulong encoded_parent_location;
        [ProtoMember(28)]
        public float light_dimmer_value;
        [ProtoMember(29)]
        public ResourceKey model_override_resource_key;
        [ProtoMember(30)]
        public ulong guid;
        [ProtoMember(31)]
        public ObjectList unique_inventory;
        [ProtoMember(32)]
        public bool needs_post_bb_fixup;
        [ProtoMember(33)]
        public uint buildbuy_use_flags;
        [ProtoMember(34)]
        public uint texture_effect;
        [ProtoMember(35)]
        public Vector3[] multicolor;
        [ProtoMember(36)]
        public int inventory_plex_id;
        [ProtoMember(37)]
        public bool is_new_object;
        [ProtoMember(38)]
        public uint[] persisted_tags;
    }

    [ProtoContract]
    public class LotCoord
    {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;
        [ProtoMember(4)]
        public float rot_x;
        [ProtoMember(5)]
        public float rot_y;
        [ProtoMember(6)]
        public float rot_z;
        [ProtoMember(7)]
        public float rot_w;
    }

    [ProtoContract]
    public class UiObjectMetadata
    {
        [ProtoMember(1)]
        public SparseMessageData sparse_data;
        [ProtoMember(2)]
        public HoverTipStyle hover_tip;
        [ProtoMember(3)]
        public HoverTipStyle debug_hover_tip;
        [ProtoMember(4)]
        public string custom_name;
        [ProtoMember(5)]
        public LocalizedString recipe_name;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong crafter_sim_id;
        [ProtoMember(7)]
        public LocalizedString[] buff_effects;
        [ProtoMember(8)]
        public LocalizedString recipe_description;
        [ProtoMember(9)]
        public uint quality;
        [ProtoMember(10)]
        public uint servings;
        [ProtoMember(11)]
        public ulong spoiled_time;
        [ProtoMember(12)]
        public LocalizedString percentage_left;
        [ProtoMember(13)]
        public LocalizedString style_name;
        [ProtoMember(14)]
        public uint simoleon_value;
        [ProtoMember(15)]
        public ResourceKey main_icon;
        [ProtoMember(16)]
        public ResourceKey[] sub_icons;
        [ProtoMember(17)]
        public LocalizedString quality_description;
        [ProtoMember(18, DataFormat = DataFormat.FixedSize)]
        public uint quality_color;
        [ProtoMember(19)]
        public LocalizedString[] object_info_names;
        [ProtoMember(20)]
        public LocalizedString[] object_info_descriptions;
        [ProtoMember(21)]
        public string inscription;
        [ProtoMember(22)]
        public string custom_description;
        [ProtoMember(23)]
        public LocalizedString header;
        [ProtoMember(24)]
        public LocalizedString subtext;
        [ProtoMember(25)]
        public LocalizedString crafted_by_text;
        [ProtoMember(26)]
        public LocalizedString stolen_from_text;
        [ProtoMember(27)]
        public LocalizedString rarity_text;
        [ProtoMember(28)]
        public LocalizedString simoleon_text;
        [ProtoMember(29)]
        public LocalizedString relic_description;
        [ProtoMember(30)]
        public float evolution_progress;
        [ProtoMember(31)]
        public LocalizedString season_text;
        [ProtoMember(32)]
        public LocalizedString spoiled_time_text;
        [ProtoMember(33)]
        public ulong rel_override_id;
        [ProtoMember(34)]
        public IconInfo[] icon_infos;
        [ProtoMember(35)]
        public LocalizedString simoleon_custom_text;
    }

    [ProtoContract]
    public class SparseMessageData
    {
        [ProtoMember(1)]
        public int[] set_fields;
    }

    public enum HoverTipStyle
    {
        HOVER_TIP_DISABLED = 0,
        HOVER_TIP_DEFAULT = 1,
        HOVER_TIP_CONSUMABLE_CRAFTABLE = 2,
        HOVER_TIP_GARDENING = 3,
        HOVER_TIP_COLLECTION = 4,
        HOVER_TIP_CUSTOM_OBJECT = 5,
        HOVER_TIP_ICON_TITLE_DESCRIPTION = 6,
        HOVER_TIP_OBJECT_RELATIONSHIP = 7,
    }

    [ProtoContract]
    public class GameplayHouseholdData
    {
        [ProtoMember(1)]
        public bool cheats_enabled;
        [ProtoMember(2)]
        public uint owned_object_count;
        [ProtoMember(3)]
        public ServiceNpcRecord[] service_npc_records;
        [ProtoMember(4)]
        public uint[] delinquent_utilities;
        [ProtoMember(5)]
        public bool can_deliver_bill;
        [ProtoMember(6)]
        public long current_payment_owed;
        [ProtoMember(7)]
        public ulong bill_timer;
        [ProtoMember(8)]
        public ulong shutoff_timer;
        [ProtoMember(9)]
        public ulong warning_timer;
        [ProtoMember(10)]
        public AdditionalBillCost[] additional_bill_costs;
        [ProtoMember(11)]
        public CollectionData[] collection_data;
        [ProtoMember(12)]
        public bool put_bill_in_hidden_inventory;
        [ProtoMember(13)]
        public ulong billable_household_value;
        [ProtoMember(14)]
        public SituationEarnedMedals[] highest_earned_situation_medals;
        [ProtoMember(15)]
        public ulong[] build_buy_unlocks;
        [ProtoMember(16)]
        public bool situation_scoring_enabled;
        [ProtoMember(17)]
        public ResourceKeyList build_buy_unlock_list;
        [ProtoMember(18)]
        public RetailData[] retail_data;
        [ProtoMember(19)]
        public BucksData[] bucks_data;
        [ProtoMember(20)]
        public uint additional_employee_slots;
        [ProtoMember(21)]
        public LotUnpaidBillData[] lot_unpaid_bill_data;
        [ProtoMember(22)]
        public ulong home_world_id;
        [ProtoMember(23, DataFormat = DataFormat.FixedSize)]
        public ulong last_played_home_zone_id;
        [ProtoMember(24)]
        public ulong home_zone_move_in_ticks;
        [ProtoMember(25, DataFormat = DataFormat.FixedSize)]
        public ulong[] always_welcomed_sim_ids;
        [ProtoMember(27)]
        public HouseholdMilestoneDataTracker household_milestone_tracker;
        [ProtoMember(28)]
        public MissingPetTrackerData missing_pet_tracker_data;
        [ProtoMember(29)]
        public LaundryData laundry_data;
        [ProtoMember(30)]
        public HolidayTracker holiday_tracker;
        [ProtoMember(31)]
        public Delivery[] deliveries;
        [ProtoMember(32)]
        public ObjectPreferenceTracker object_preference_tracker;
        [ProtoMember(33)]
        public CurrentBillDetail[] current_bill_details;
        [ProtoMember(34)]
        public BillUtilitySettings[] bill_utility_settings;
        [ProtoMember(35)]
        public ulong repo_man_due_time;
        [ProtoMember(36)]
        public SituationNewEntry[] situation_new_entries;
    }

    [ProtoContract]
    public class ServiceNpcRecord
    {
        [ProtoMember(1)]
        public ulong service_type;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] preferred_sim_ids;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong[] fired_sim_ids;
        [ProtoMember(4)]
        public bool hired;
        [ProtoMember(5)]
        public ulong time_last_started_service;
        [ProtoMember(6)]
        public bool recurring;
        [ProtoMember(7)]
        public ulong time_last_finished_service;
        [ProtoMember(8)]
        public ulong user_specified_data_id;
    }

    [ProtoContract]
    public class AdditionalBillCost
    {
        [ProtoMember(1)]
        public uint bill_source;
        [ProtoMember(2)]
        public ulong cost;
    }

    [ProtoContract]
    public class CollectionData
    {
        [ProtoMember(1)]
        public ulong collectible_def_id;
        [ProtoMember(2)]
        public ulong collection_id;
        [ProtoMember(3)]
        public bool thenew;
        [ProtoMember(4)]
        public uint quality;
        [ProtoMember(5)]
        public IconInfo icon_info;
    }

    [ProtoContract]
    public class SituationEarnedMedals
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong situation_id;
        [ProtoMember(2)]
        public uint medal;
    }

    [ProtoContract]
    public class ResourceKeyList
    {
        [ProtoMember(1)]
        public ResourceKey[] resource_keys;
    }

    [ProtoContract]
    public class RetailData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong retail_zone_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] employee_ids;
        [ProtoMember(3)]
        public bool is_open;
        [ProtoMember(4)]
        public float markup;
        [ProtoMember(5)]
        public int funds;
        [ProtoMember(6)]
        public MannequinSimData employee_uniform_data_male;
        [ProtoMember(7)]
        public MannequinSimData employee_uniform_data_female;
        [ProtoMember(8)]
        public RetailDataPayroll[] employee_payroll;
        [ProtoMember(9)]
        public ulong open_time;
        [ProtoMember(10)]
        public bool grand_opening;
        [ProtoMember(11)]
        public RetailFundsCategoryEntry[] funds_category_tracker_data;
        [ProtoMember(12)]
        public int daily_revenue;
        [ProtoMember(13)]
        public int daily_items_sold;
        [ProtoMember(14)]
        public int daily_employee_wages;
    }

    [ProtoContract]
    public class RetailDataPayroll
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong clock_in_time;
        [ProtoMember(3)]
        public RetailDataPayrollEntry[] payroll_data;
    }

    [ProtoContract]
    public class RetailDataPayrollEntry
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong career_level_guid;
        [ProtoMember(2)]
        public double hours_worked;
    }

    [ProtoContract]
    public class RetailFundsCategoryEntry
    {
        [ProtoMember(1)]
        public uint funds_category;
        [ProtoMember(2)]
        public int amount;
    }

    [ProtoContract]
    public class LotUnpaidBillData
    {
        [ProtoMember(1)]
        public uint money_amount;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong situation_id;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong[] sim_ids_on_lot;
    }

    [ProtoContract]
    public class HouseholdMilestoneDataTracker
    {
        [ProtoMember(1)]
        public uint[] milestones_completed;
        [ProtoMember(2)]
        public uint[] objectives_completed;
        [ProtoMember(3)]
        public EventDataObject data;
        [ProtoMember(4)]
        public MilestoneCompletionCount[] milestone_completion_counts;
    }

    [ProtoContract]
    public class MissingPetTrackerData
    {
        [ProtoMember(1)]
        public ulong missing_pet_id;
        [ProtoMember(2)]
        public ulong test_alarm_finishing_time;
        [ProtoMember(3)]
        public ulong return_alarm_finishing_time;
        [ProtoMember(4)]
        public ulong cooldown_alarm_finishing_time;
        [ProtoMember(5)]
        public bool return_pet_on_zone_load;
        [ProtoMember(6)]
        public bool running_away;
        [ProtoMember(7)]
        public MotiveTestResults[] motive_test_results;
        [ProtoMember(8)]
        public bool alert_posted;
    }

    [ProtoContract]
    public class MotiveTestResults
    {
        [ProtoMember(1)]
        public ulong pet_id;
        [ProtoMember(2)]
        public uint[] test_results;
    }

    [ProtoContract]
    public class LaundryData
    {
        [ProtoMember(1)]
        public ulong last_unload_time;
        [ProtoMember(2)]
        public LaundryConditions[] laundry_conditions;
    }

    [ProtoContract]
    public class LaundryConditions
    {
        [ProtoMember(1)]
        public ulong timestamp;
        [ProtoMember(2)]
        public ulong state_value_name_hash;
    }

    [ProtoContract]
    public class HolidayTracker
    {
        [ProtoMember(1)]
        public Holiday[] holidays;
        [ProtoMember(2)]
        public SituationSeedData[] situations;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong situation_holiday_type;
        [ProtoMember(4)]
        public ulong situation_holiday_time;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong cancelled_holiday_type;
        [ProtoMember(6)]
        public ulong cancelled_holiday_time;
    }

    [ProtoContract]
    public class Delivery
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong delivery_tuning_guid;
        [ProtoMember(3)]
        public ulong expected_arrival_time;
    }

    [ProtoContract]
    public class ObjectPreferenceTracker
    {
        [ProtoMember(1)]
        public ZonePreferenceData[] zone_preference_datas;
    }

    [ProtoContract]
    public class ZonePreferenceData
    {
        [ProtoMember(1)]
        public ulong zone_id;
        [ProtoMember(2)]
        public ulong preference_tag;
        [ProtoMember(3)]
        public SimPreference[] sim_preferences;
    }

    [ProtoContract]
    public class SimPreference
    {
        [ProtoMember(1)]
        public ulong sim_id;
        [ProtoMember(2)]
        public ulong object_id;
        [ProtoMember(3)]
        public long subroot_index;
    }

    [ProtoContract]
    public class CurrentBillDetail
    {
        [ProtoMember(1)]
        public ulong utility;
        [ProtoMember(2)]
        public long billable_amount;
        [ProtoMember(5)]
        public CurrentBillStatisticDelta[] statistic_deltas;
        [ProtoMember(6)]
        public float net_consumption;
    }

    [ProtoContract]
    public class CurrentBillStatisticDelta
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong stat_id;
        [ProtoMember(2)]
        public float delta;
    }

    [ProtoContract]
    public class BillUtilitySettings
    {
        [ProtoMember(1)]
        public ulong utility;
        [ProtoMember(2)]
        public uint end_of_bill_action;
    }

    [ProtoContract]
    public class SituationNewEntry
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong situation_id;
        [ProtoMember(2)]
        public bool new_entry;
    }

    [ProtoContract]
    public class RewardPartList
    {
        [ProtoMember(1)]
        public RewardPartData[] reward_parts;
    }

    [ProtoContract]
    public class RewardPartData
    {
        [ProtoMember(1)]
        public ulong part_id;
        [ProtoMember(2)]
        public bool is_new_reward;
    }

    [ProtoContract]
    public class SimData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(3)]
        public uint world_id;
        [ProtoMember(25)]
        public string zone_name;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(5)]
        public string first_name;
        [ProtoMember(6)]
        public string last_name;
        [ProtoMember(7)]
        public uint gender;
        [ProtoMember(8)]
        public uint age;
        [ProtoMember(9)]
        public float voice_pitch;
        [ProtoMember(10)]
        public ulong skin_tone;
        [ProtoMember(11)]
        public uint voice_actor;
        [ProtoMember(12)]
        public string physique;
        [ProtoMember(13)]
        public float age_progress;
        [ProtoMember(15, DataFormat = DataFormat.FixedSize)]
        public ulong significant_other;
        [ProtoMember(17)]
        public byte[] deprecated_attributes;
        [ProtoMember(18)]
        public byte[] facial_attr;
        [ProtoMember(19)]
        public ulong created;
        [ProtoMember(20)]
        public ObjectList inventory;
        [ProtoMember(21)]
        public OutfitList outfits;
        [ProtoMember(22)]
        public string household_name;
        [ProtoMember(23, DataFormat = DataFormat.FixedSize)]
        public ulong nucleus_id;
        [ProtoMember(14)]
        public uint deprecated_money;
        [ProtoMember(27)]
        public ulong money;
        [ProtoMember(28)]
        public GeneticData genetic_data;
        [ProtoMember(29)]
        public uint flags;
        [ProtoMember(30)]
        public PersistableSimInfoAttributes attributes;
        [ProtoMember(31)]
        public uint revision;
        [ProtoMember(32)]
        public LotCoord location;
        [ProtoMember(33)]
        public uint deprecated_change_number;
        [ProtoMember(34)]
        public ulong primary_aspiration;
        [ProtoMember(35)]
        public ulong last_instantiated_time;
        [ProtoMember(36)]
        public ulong additional_bonus_days;
        [ProtoMember(37)]
        public SuperInteractionSaveState interaction_state;
        [ProtoMember(38)]
        public uint current_outfit_type;
        [ProtoMember(39)]
        public uint current_outfit_index;
        [ProtoMember(40)]
        public bool fix_relationship;
        [ProtoMember(41, DataFormat = DataFormat.FixedSize)]
        public ulong current_mood;
        [ProtoMember(42)]
        public uint current_mood_intensity;
        [ProtoMember(43)]
        public ZoneTimeStamp zone_time_stamp;
        [ProtoMember(44)]
        public uint whim_bucks;
        [ProtoMember(45)]
        public uint level;
        [ProtoMember(46)]
        public ulong inventory_value;
        [ProtoMember(47)]
        public GameplaySimData gameplay_data;
        [ProtoMember(48)]
        public float pregnancy_progress;
        [ProtoMember(49)]
        public uint full_name_key;
        [ProtoMember(50)]
        public uint last_inzone_outfit_type;
        [ProtoMember(51)]
        public uint last_inzone_outfit_index;
        [ProtoMember(52)]
        public SimCreationPath sim_creation_path;
        [ProtoMember(53)]
        public float initial_fitness_value;
        [ProtoMember(54)]
        public ulong voice_effect;
        [ProtoMember(55)]
        public uint first_name_key;
        [ProtoMember(56)]
        public uint last_name_key;
        [ProtoMember(57)]
        public uint generation;
        [ProtoMember(58)]
        public uint previous_outfit_type;
        [ProtoMember(59)]
        public uint previous_outfit_index;
        [ProtoMember(60)]
        public uint extended_species;
        [ProtoMember(61)]
        public uint sim_lod;
        [ProtoMember(62)]
        public ulong custom_texture;
        [ProtoMember(63)]
        public PeltLayerDataList pelt_layers;
        [ProtoMember(64)]
        public string breed_name;
        [ProtoMember(65)]
        public uint breed_name_key;
        [ProtoMember(66)]
        public bool age_progress_randomized;
        [ProtoMember(67)]
        public float skin_tone_val_shift;
    }

    [ProtoContract]
    public class GeneticData
    {
        [ProtoMember(1)]
        public byte[] sculpts_and_mods_attr;
        [ProtoMember(2)]
        public string physique;
        [ProtoMember(3)]
        public float voice_pitch;
        [ProtoMember(4)]
        public uint voice_actor;
        [ProtoMember(5)]
        public PartDataList parts_list;
    }

    [ProtoContract]
    public class PartDataList
    {
        [ProtoMember(1)]
        public PartData[] parts;
    }

    [ProtoContract]
    public class PartData
    {
        [ProtoMember(1)]
        public ulong id;
        [ProtoMember(2)]
        public uint body_type;
        [ProtoMember(3)]
        public ulong color_shift;
    }

    [ProtoContract]
    public class PersistableSimInfoAttributes
    {
        [ProtoMember(1)]
        public PersistableRelationshipTracker relationship_tracker;
        [ProtoMember(2)]
        public PersistableCommodityTracker commodity_tracker;
        [ProtoMember(3)]
        public PersistableStatisticsTracker statistics_tracker;
        [ProtoMember(4)]
        public PersistableObjectPreferences object_preferences;
        [ProtoMember(5)]
        public PersistableObjectOwnership object_ownership;
        [ProtoMember(6)]
        public PersistableEventDataTracker event_data_tracker;
        [ProtoMember(7)]
        public PersistableDeathTracker death_tracker;
        [ProtoMember(8)]
        public PersistablePregnancyTracker pregnancy_tracker;
        [ProtoMember(9)]
        public PersistableSimPermissions sim_permissions;
        [ProtoMember(10)]
        public PersistableTraitTracker trait_tracker;
        [ProtoMember(11)]
        public PersistableAdventureTracker adventure_tracker;
        [ProtoMember(12)]
        public PersistableSimCareers sim_careers;
        [ProtoMember(13)]
        public PersistableSkillTracker skill_tracker;
        [ProtoMember(14)]
        public PersistableGenealogyTracker genealogy_tracker;
        [ProtoMember(15)]
        public PersistableUnlockTracker unlock_tracker;
        [ProtoMember(16)]
        public PersistableRoyaltyTracker royalty_tracker;
        [ProtoMember(17)]
        public PersistableOccultTracker occult_tracker;
        [ProtoMember(18)]
        public PersistableNotebookTracker notebook_tracker;
        [ProtoMember(19)]
        public PersistableAppearanceTracker appearance_tracker;
        [ProtoMember(20)]
        public PersistableStoryProgressionTracker story_progression_tracker;
        [ProtoMember(21)]
        public PersistableRankedStatisticTracker ranked_statistic_tracker;
        [ProtoMember(22)]
        public PersistableSicknessTracker sickness_tracker;
        [ProtoMember(23)]
        public PersistableRelicTracker relic_tracker;
        [ProtoMember(24)]
        public PersistableStoredObjectInfoComponent stored_object_info_component;
        [ProtoMember(25)]
        public PersistableStoredAudioComponent stored_audio_component;
        [ProtoMember(26)]
        public PersistableLifestyleBrandTracker lifestyle_brand_tracker;
        [ProtoMember(27)]
        public PersistableSuntanTracker suntan_tracker;
        [ProtoMember(28)]
        public PersistableFamiliarTracker familiar_tracker;
        [ProtoMember(29)]
        public PersistableFavoritesTracker favorites_tracker;
        [ProtoMember(30)]
        public PersistableDegreeTracker degree_tracker;
        [ProtoMember(31)]
        public PersistableOrganizationTracker organization_tracker;
        [ProtoMember(32)]
        public PersistableFixupTracker fixup_tracker;
        [ProtoMember(33)]
        public PersistableTraitStatisticTracker trait_statistic_tracker;
    }

    [ProtoContract]
    public class PersistableRelationshipTracker
    {
        [ProtoMember(1)]
        public PersistableRelationship[] relationships;
    }

    [ProtoContract]
    public class PersistableRelationship
    {
        [ProtoMember(1)]
        public ulong target_id;
        [ProtoMember(2)]
        public float value;
        [ProtoMember(3)]
        public ulong[] bits;
        [ProtoMember(4)]
        public Timeout[] timeouts;
        [ProtoMember(5)]
        public PersistableRelationshipTrack[] tracks;
        [ProtoMember(6)]
        public SimKnowledge knowledge;
        [ProtoMember(7)]
        public BitAddedBuffList[] bit_added_buffs;
        [ProtoMember(8)]
        public ulong last_update_time;
    }

    [ProtoContract]
    public class BitAddedBuffList
    {
        [ProtoMember(1)]
        public ulong bit_id;
        [ProtoMember(2)]
        public ulong[] buff_ids;
    }

    [ProtoContract]
    public class PersistableObjectPreferences
    {
        [ProtoMember(1)]
        public ObjectPreference[] preferences;
    }

    [ProtoContract]
    public class ObjectPreference
    {
        [ProtoMember(1)]
        public uint tag;
        [ProtoMember(2)]
        public ulong object_id;
    }

    [ProtoContract]
    public class PersistableObjectOwnership
    {
        [ProtoMember(1)]
        public ObjectPreference[] owned_object;
    }

    [ProtoContract]
    public class PersistableEventDataTracker
    {
        [ProtoMember(1)]
        public ulong[] milestones_completed;
        [ProtoMember(2)]
        public ulong[] objectives_completed;
        [ProtoMember(3)]
        public EventDataObject data;
        [ProtoMember(4)]
        public ulong[] unlocked_hidden_aspiration_tracks;
        [ProtoMember(5)]
        public TimedAspiration[] timed_aspirations;
        [ProtoMember(6)]
        public MilestoneCompletionCount[] milestone_completion_counts;
        [ProtoMember(7)]
        public AdditionalObjectives[] additional_objectives;
    }

    [ProtoContract]
    public class TimedAspiration
    {
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong aspiration;
        [ProtoMember(6)]
        public ulong end_time;
        [ProtoMember(7)]
        public bool completed;
    }

    [ProtoContract]
    public class AdditionalObjectives
    {
        [ProtoMember(1)]
        public ulong milestone_guid;
        [ProtoMember(2)]
        public ulong[] objective_guids;
    }

    [ProtoContract]
    public class PersistableDeathTracker
    {
        [ProtoMember(1)]
        public uint death_type;
        [ProtoMember(2)]
        public ulong death_time;
    }

    [ProtoContract]
    public class PersistablePregnancyTracker
    {
        [ProtoMember(1)]
        public float deprecated_seed;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] parent_ids;
        [ProtoMember(3)]
        public ulong last_modified;
        [ProtoMember(4)]
        public uint seed;
        [ProtoMember(5)]
        public uint origin;
    }

    [ProtoContract]
    public class PersistableSimPermissions
    {
        [ProtoMember(1)]
        public ulong[] enabled_permissions;
    }

    [ProtoContract]
    public class PersistableAdventureTracker
    {
        [ProtoMember(1)]
        public AdventureMomentPair[] adventures;
        [ProtoMember(2)]
        public AdventureMomentCooldowns[] adventure_cooldowns;
    }

    [ProtoContract]
    public class AdventureMomentPair
    {
        [ProtoMember(1)]
        public ulong adventure_id;
        [ProtoMember(2)]
        public ulong adventure_moment_id;
    }

    [ProtoContract]
    public class AdventureMomentCooldowns
    {
        [ProtoMember(1)]
        public ulong adventure_id;
        [ProtoMember(2)]
        public ulong adventure_moment_id;
        [ProtoMember(3)]
        public ulong adventure_cooldown;
    }

    [ProtoContract]
    public class PersistableSimCareers
    {
        [ProtoMember(1)]
        public PersistableSimCareer[] careers;
        [ProtoMember(2)]
        public CareerHistoryEntry[] career_history;
        [ProtoMember(3)]
        public ulong retirement_career_uid;
        [ProtoMember(4)]
        public string custom_career_name;
        [ProtoMember(5)]
        public string custom_career_description;
    }

    [ProtoContract]
    public class PersistableSimCareer
    {
        [ProtoMember(1)]
        public ulong career_uid;
        [ProtoMember(2)]
        public ulong track_uid;
        [ProtoMember(3)]
        public uint track_level;
        [ProtoMember(4)]
        public uint user_display_level;
        [ProtoMember(5)]
        public bool attended_work;
        [ProtoMember(6)]
        public float base_work_performance;
        [ProtoMember(7)]
        public float positive_work_performance;
        [ProtoMember(8)]
        public float negative_work_performance;
        [ProtoMember(9)]
        public ulong company_name_hash;
        [ProtoMember(10)]
        public ulong current_work_start;
        [ProtoMember(11)]
        public ulong current_work_end;
        [ProtoMember(12)]
        public ulong current_work_duration;
        [ProtoMember(13, DataFormat = DataFormat.FixedSize)]
        public ulong active_situation_id;
        [ProtoMember(14, DataFormat = DataFormat.FixedSize)]
        public ulong career_situation_guid;
        [ProtoMember(15)]
        public bool called_in_sick;
        [ProtoMember(16)]
        public bool pending_promotion;
        [ProtoMember(17)]
        public long join_time;
        [ProtoMember(18)]
        public uint taking_day_off_reason;
        [ProtoMember(19)]
        public uint requested_day_off_reason;
        [ProtoMember(20)]
        public CareerEventManagerData career_event_manager_data;
        [ProtoMember(21)]
        public uint overmax_level;
        [ProtoMember(22)]
        public DetectiveCareerData detective_data;
        [ProtoMember(23)]
        public bool career_session_extended;
        [ProtoMember(24, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(25)]
        public bool has_attended_first_day;
        [ProtoMember(26)]
        public CareerEventCooldown[] career_event_cooldowns;
        [ProtoMember(27)]
        public bool should_restore_career_state;
        [ProtoMember(28)]
        public ulong[] offered_assignments;
        [ProtoMember(29)]
        public ulong[] active_assignments;
        [ProtoMember(30)]
        public float pto_taken;
        [ProtoMember(31)]
        public bool fame_moment_completed;
        [ProtoMember(32)]
        public CareerGigData upcoming_gig;
        [ProtoMember(33)]
        public uint schedule_shift_type;
        [ProtoMember(34)]
        public bool first_gig_completed;
        [ProtoMember(35)]
        public bool seen_scholarship_info;
        [ProtoMember(36)]
        public ulong familiar_rabbit_hole_id;
        [ProtoMember(37)]
        public ulong rabbit_hole_id;
        [ProtoMember(38)]
        public CareerClaimedObjectData[] claimed_object_datas;
    }

    [ProtoContract]
    public class CareerEventManagerData
    {
        [ProtoMember(1)]
        public CareerEventData[] career_events;
        [ProtoMember(2)]
        public SituationSeedData scorable_situation_seed;
    }

    [ProtoContract]
    public class CareerEventData
    {
        [ProtoMember(1)]
        public ulong career_event_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong required_zone_id;
        [ProtoMember(3)]
        public ulong event_situation_id;
        [ProtoMember(4)]
        public uint state;
    }

    [ProtoContract]
    public class DetectiveCareerData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong active_criminal_sim_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong crime_scene_event_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong[] unused_clue_ids;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong[] used_clue_ids;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong case_start_time_in_minutes;
    }

    [ProtoContract]
    public class CareerEventCooldown
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong career_event_id;
        [ProtoMember(2)]
        public int day;
    }

    [ProtoContract]
    public class CareerGigData
    {
        [ProtoMember(1)]
        public ulong gig_type;
        [ProtoMember(2)]
        public ulong gig_time;
        [ProtoMember(3)]
        public ulong[] cast_list;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong customer_sim_id;
        [ProtoMember(5)]
        public bool gig_attended;
        [ProtoMember(6)]
        public MissionObjectiveData[] mission_objective_data;
    }

    [ProtoContract]
    public class MissionObjectiveData
    {
        [ProtoMember(1)]
        public ulong objective_id;
        [ProtoMember(2)]
        public uint state;
        [ProtoMember(3)]
        public MissionCreatedSituations[] created_situations;
    }

    [ProtoContract]
    public class MissionCreatedSituations
    {
        [ProtoMember(1)]
        public ulong zone_id;
        [ProtoMember(2)]
        public ulong[] situation_ids;
    }

    [ProtoContract]
    public class CareerClaimedObjectData
    {
        [ProtoMember(1)]
        public ulong guid;
        [ProtoMember(2)]
        public ulong[] claimed_object_ids;
    }

    [ProtoContract]
    public class CareerHistoryEntry
    {
        [ProtoMember(1)]
        public ulong career_uid;
        [ProtoMember(2)]
        public ulong track_uid;
        [ProtoMember(3)]
        public uint track_level;
        [ProtoMember(4)]
        public uint user_display_level;
        [ProtoMember(5)]
        public ulong time_left;
        [ProtoMember(6)]
        public uint highest_level;
        [ProtoMember(7)]
        public uint overmax_level;
        [ProtoMember(8)]
        public uint daily_pay;
        [ProtoMember(9)]
        public uint days_worked;
        [ProtoMember(10)]
        public uint active_days_worked;
        [ProtoMember(11)]
        public bool player_rewards_deferred;
        [ProtoMember(12)]
        public uint schedule_shift_type;
    }

    [ProtoContract]
    public class PersistableSkillTracker
    {
        [ProtoMember(1)]
        public Skill[] skills;
    }

    [ProtoContract]
    public class Skill
    {
        [ProtoMember(1)]
        public ulong name_hash;
        [ProtoMember(2)]
        public float value;
        [ProtoMember(3)]
        public bool level_0_buffer_full;
        [ProtoMember(4)]
        public ulong time_of_last_value_change;
    }

    [ProtoContract]
    public class PersistableUnlockTracker
    {
        [ProtoMember(1)]
        public UnlockData[] unlock_data_list;
    }

    [ProtoContract]
    public class UnlockData
    {
        [ProtoMember(1)]
        public ulong unlock_instance_id;
        [ProtoMember(2)]
        public ulong unlock_instance_type;
        [ProtoMember(3)]
        public string custom_name;
        [ProtoMember(4)]
        public bool marked_as_new;
    }

    [ProtoContract]
    public class PersistableRoyaltyTracker
    {
        [ProtoMember(1)]
        public Royalty[] royalties;
    }

    [ProtoContract]
    public class Royalty
    {
        [ProtoMember(1)]
        public uint royalty_type;
        [ProtoMember(2)]
        public ulong royalty_guid64;
        [ProtoMember(3)]
        public LocalizedString entry_name;
        [ProtoMember(4)]
        public float multiplier;
        [ProtoMember(5)]
        public uint current_payment;
    }

    [ProtoContract]
    public class PersistableOccultTracker
    {
        [ProtoMember(1)]
        public uint occult_types;
        [ProtoMember(2)]
        public uint current_occult_types;
        [ProtoMember(3)]
        public OccultSimData[] occult_sim_infos;
        [ProtoMember(4)]
        public uint pending_occult_type;
        [ProtoMember(5)]
        public bool occult_form_available;
    }

    [ProtoContract]
    public class OccultSimData
    {
        [ProtoMember(1)]
        public uint occult_type;
        [ProtoMember(11)]
        public string physique;
        [ProtoMember(12)]
        public byte[] facial_attributes;
        [ProtoMember(13)]
        public float voice_pitch;
        [ProtoMember(14)]
        public uint voice_actor;
        [ProtoMember(15)]
        public ulong voice_effect;
        [ProtoMember(16)]
        public ulong skin_tone;
        [ProtoMember(17)]
        public GeneticData genetic_data;
        [ProtoMember(18)]
        public uint flags;
        [ProtoMember(21)]
        public OutfitList outfits;
        [ProtoMember(22)]
        public float skin_tone_val_shift;
    }

    [ProtoContract]
    public class PersistableNotebookTracker
    {
        [ProtoMember(1)]
        public NotebookEntryData[] notebook_entries;
    }

    [ProtoContract]
    public class NotebookEntryData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong object_recipe_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] object_entry_ids;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong tuning_reference_id;
        [ProtoMember(4)]
        public bool new_entry;
        [ProtoMember(5)]
        public NotebookSubEntryData[] object_sub_entries;
    }

    [ProtoContract]
    public class NotebookSubEntryData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sub_entry_id;
        [ProtoMember(2)]
        public bool new_sub_entry;
    }

    [ProtoContract]
    public class PersistableAppearanceTracker
    {
        [ProtoMember(1)]
        public AppearanceTrackerModifier[] appearance_modifiers;
    }

    [ProtoContract]
    public class AppearanceTrackerModifier
    {
        [ProtoMember(1)]
        public ulong guid;
        [ProtoMember(2)]
        public uint seed;
    }

    [ProtoContract]
    public class PersistableStoryProgressionTracker
    {
        [ProtoMember(1)]
        public StoryProgressionAction[] actions;
    }

    [ProtoContract]
    public class StoryProgressionAction
    {
        [ProtoMember(1)]
        public ulong guid;
        [ProtoMember(10)]
        public ulong duration;
        [ProtoMember(11)]
        public ulong start_time;
        [ProtoMember(50)]
        public ulong custom_guid;
        [ProtoMember(51, DataFormat = DataFormat.FixedSize)]
        public ulong custom_id;
    }

    [ProtoContract]
    public class PersistableSicknessTracker
    {
        [ProtoMember(1)]
        public ulong[] previous_sicknesses;
        [ProtoMember(2)]
        public SicknessData current_sickness;
    }

    [ProtoContract]
    public class SicknessData
    {
        [ProtoMember(1)]
        public ulong sickness;
        [ProtoMember(2)]
        public ulong[] symptoms_discovered;
        [ProtoMember(3)]
        public ulong[] exams_performed;
        [ProtoMember(4)]
        public ulong[] treatments_performed;
        [ProtoMember(5)]
        public ulong[] treatments_ruled_out;
        [ProtoMember(6)]
        public bool is_discovered;
    }

    [ProtoContract]
    public class PersistableRelicTracker
    {
        [ProtoMember(1)]
        public ulong[] known_relics;
    }

    [ProtoContract]
    public class PersistableStoredObjectInfoComponent
    {
        [ProtoMember(1)]
        public ulong object_definition_id;
        [ProtoMember(2)]
        public string custom_name;
        [ProtoMember(3)]
        public StateComponentState[] states;
        [ProtoMember(4)]
        public ulong object_id;
        [ProtoMember(5)]
        public StoredObjectData[] stored_object_data;
    }

    [ProtoContract]
    public class StateComponentState
    {
        [ProtoMember(1)]
        public ulong state_name_hash;
        [ProtoMember(2)]
        public ulong value_name_hash;
    }

    [ProtoContract]
    public class StoredObjectData
    {
        [ProtoMember(1)]
        public uint stored_object_type;
        [ProtoMember(2)]
        public ulong object_definition_id;
        [ProtoMember(3)]
        public string custom_name;
        [ProtoMember(4)]
        public StateComponentState[] states;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong object_id;
    }

    [ProtoContract]
    public class PersistableStoredAudioComponent
    {
        [ProtoMember(1)]
        public SoundResource sound_resource;
        [ProtoMember(2)]
        public uint channel_values_int;
    }

    [ProtoContract]
    public class SoundResource
    {
        [ProtoMember(1)]
        public ResourceKey sound;
        [ProtoMember(2)]
        public ResourceKey music_track_snippet;
    }

    [ProtoContract]
    public class PersistableLifestyleBrandTracker
    {
        [ProtoMember(1)]
        public uint product;
        [ProtoMember(2)]
        public uint target_market;
        [ProtoMember(3)]
        public ResourceKey logo;
        [ProtoMember(4)]
        public string brand_name;
        [ProtoMember(5)]
        public ulong days_active;
    }

    [ProtoContract]
    public class PersistableSuntanTracker
    {
        [ProtoMember(1)]
        public uint tan_level;
        [ProtoMember(2)]
        public PartData[] outfit_part_data_list;
    }

    [ProtoContract]
    public class PersistableFamiliarTracker
    {
        [ProtoMember(1)]
        public ulong active_familiar_uid;
        [ProtoMember(2)]
        public FamiliarData[] familiars;
    }

    [ProtoContract]
    public class FamiliarData
    {
        [ProtoMember(1)]
        public ulong familiar_uid;
        [ProtoMember(2)]
        public uint familiar_type;
        [ProtoMember(3)]
        public string familiar_name;
        [ProtoMember(4)]
        public ulong pet_familiar_id;
    }

    [ProtoContract]
    public class PersistableFavoritesTracker
    {
        [ProtoMember(1)]
        public FavoritesData[] favorites;
        [ProtoMember(2)]
        public ItemStackFavoritesData[] stack_favorites;
    }

    [ProtoContract]
    public class FavoritesData
    {
        [ProtoMember(1)]
        public uint favorite_type;
        [ProtoMember(2)]
        public ulong favorite_id;
        [ProtoMember(3)]
        public ulong favorite_def_id;
    }

    [ProtoContract]
    public class ItemStackFavoritesData
    {
        [ProtoMember(1)]
        public ulong key;
        [ProtoMember(2)]
        public ulong custom_key;
        [ProtoMember(3)]
        public uint stack_scheme;
    }

    [ProtoContract]
    public class PersistableDegreeTracker
    {
        [ProtoMember(1)]
        public ulong current_major;
        [ProtoMember(2)]
        public ulong current_university;
        [ProtoMember(3)]
        public long current_credits;
        [ProtoMember(4)]
        public float total_gradepoints;
        [ProtoMember(5)]
        public long total_courses;
        [ProtoMember(6)]
        public ulong[] previous_majors;
        [ProtoMember(7)]
        public ulong[] previous_courses;
        [ProtoMember(8)]
        public CollegeCourseInfo[] current_courses;
        [ProtoMember(9)]
        public AcceptedDegrees[] accepted_degrees;
        [ProtoMember(10)]
        public long enrollment_status;
        [ProtoMember(11)]
        public ulong end_of_term_time;
        [ProtoMember(12)]
        public ulong reenrollment_time;
        [ProtoMember(13)]
        public ulong start_term_alarm_time;
        [ProtoMember(14)]
        public ulong term_started_time;
        [ProtoMember(15)]
        public ulong kickout_reason;
        [ProtoMember(16)]
        public ulong kickout_destination_zone;
        [ProtoMember(17)]
        public ulong diploma_mail_time;
        [ProtoMember(18)]
        public ulong[] active_scholarships;
        [ProtoMember(19)]
        public ulong[] accepted_scholarships;
        [ProtoMember(20)]
        public ulong[] rejected_scholarships;
        [ProtoMember(21)]
        public ScholarshipInfo[] pending_scholarships;
        [ProtoMember(22)]
        public ulong elective_timestamp;
        [ProtoMember(23)]
        public ElectiveCoursesInfo[] elective_courses;
        [ProtoMember(24)]
        public bool show_reenrollment_dialog_on_spin_up;
    }

    [ProtoContract]
    public class CollegeCourseInfo
    {
        [ProtoMember(1)]
        public ulong course_slot;
        [ProtoMember(2)]
        public ulong course_data;
        [ProtoMember(3)]
        public long final_grade;
        [ProtoMember(4)]
        public long known_grade;
        [ProtoMember(5)]
        public long lectures;
        [ProtoMember(6)]
        public bool final_requirement_completed;
        [ProtoMember(7)]
        public long homework_cheated;
        [ProtoMember(8)]
        public float initial_skill;
    }

    [ProtoContract]
    public class AcceptedDegrees
    {
        [ProtoMember(1)]
        public ulong university_id;
        [ProtoMember(2)]
        public ulong[] degree_ids;
    }

    [ProtoContract]
    public class ScholarshipInfo
    {
        [ProtoMember(1)]
        public ulong scholarship_id;
        [ProtoMember(2)]
        public ulong remaining_evaluation_time;
    }

    [ProtoContract]
    public class ElectiveCoursesInfo
    {
        [ProtoMember(1)]
        public ulong university_id;
        [ProtoMember(2)]
        public ulong[] elective_ids;
    }

    [ProtoContract]
    public class PersistableOrganizationTracker
    {
        [ProtoMember(1)]
        public OrganizationStatusInfo[] org_status_map;
        [ProtoMember(2)]
        public OrganizationActiveTasks[] tasks_map;
        [ProtoMember(3)]
        public ulong[] unenrolled_org_ids;
    }

    [ProtoContract]
    public class OrganizationStatusInfo
    {
        [ProtoMember(1)]
        public ulong org_id;
        [ProtoMember(2)]
        public ulong membership_status;
    }

    [ProtoContract]
    public class OrganizationActiveTasks
    {
        [ProtoMember(1)]
        public ulong org_id;
        [ProtoMember(2)]
        public TimedAspiration[] active_tasks;
    }

    [ProtoContract]
    public class PersistableFixupTracker
    {
        [ProtoMember(1)]
        public ulong[] pending_fixups;
    }

    [ProtoContract]
    public class PersistableTraitStatisticTracker
    {
        [ProtoMember(1)]
        public TraitStatistic[] trait_statistics;
        [ProtoMember(2)]
        public ulong time_to_next_periodic_test;
    }

    [ProtoContract]
    public class TraitStatistic
    {
        [ProtoMember(1)]
        public ulong trait_statistic_id;
        [ProtoMember(2)]
        public float value;
        [ProtoMember(3)]
        public uint state;
        [ProtoMember(4)]
        public uint neglect_buff_index;
        [ProtoMember(5)]
        public bool value_added;
        [ProtoMember(6)]
        public float max_daily_cap;
        [ProtoMember(7)]
        public float min_daily_cap;
    }

    [ProtoContract]
    public class SuperInteractionSaveState
    {
        [ProtoMember(1)]
        public InteractionSaveData[] interactions;
        [ProtoMember(2)]
        public TransitioningInteraction transitioning_interaction;
        [ProtoMember(3)]
        public InteractionSaveData[] queued_interactions;
    }

    [ProtoContract]
    public class InteractionSaveData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong interaction;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong target_id;
        [ProtoMember(3)]
        public uint source;
        [ProtoMember(4)]
        public uint priority;
        [ProtoMember(5)]
        public uint target_part_group_index;
        [ProtoMember(6)]
        public ulong start_time;
    }

    [ProtoContract]
    public class TransitioningInteraction
    {
        [ProtoMember(1)]
        public InteractionSaveData base_interaction_data;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong posture_aspect_body;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong posture_carry_left;
        [ProtoMember(4, DataFormat = DataFormat.FixedSize)]
        public ulong posture_carry_right;
    }

    [ProtoContract]
    public class ZoneTimeStamp
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong game_time_expire;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong time_sim_info_was_saved;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong time_sim_was_saved;
    }

    [ProtoContract]
    public class GameplaySimData
    {
        [ProtoMember(1)]
        public WorldLocation location;
        [ProtoMember(2)]
        public ulong inventory_value;
        [ProtoMember(4)]
        public ZoneTimeStamp zone_time_stamp;
        [ProtoMember(5)]
        public ulong additional_bonus_days;
        [ProtoMember(6)]
        public SuperInteractionSaveState interaction_state;
        [ProtoMember(7)]
        public uint whim_bucks;
        [ProtoMember(8, DataFormat = DataFormat.FixedSize)]
        public ulong spawn_point_id;
        [ProtoMember(9)]
        public uint[] spawner_tags;
        [ProtoMember(10)]
        public uint spawn_point_option;
        [ProtoMember(11)]
        public WhimsetTrackerData whim_tracker;
        [ProtoMember(12)]
        public CollectionData[] collection_data;
        [ProtoMember(13)]
        public ulong[] build_buy_unlocks;
        [ProtoMember(14)]
        public AwayActionTrackerData away_action_tracker;
        [ProtoMember(15)]
        public uint serialization_option;
        [ProtoMember(16)]
        public ulong time_alive;
        [ProtoMember(18)]
        public ResourceKeyList build_buy_unlock_list;
        [ProtoMember(19, DataFormat = DataFormat.FixedSize)]
        public ulong old_household_id;
        [ProtoMember(20, DataFormat = DataFormat.FixedSize)]
        public ulong premade_sim_template_id;
        [ProtoMember(21)]
        public SimFavoriteData favorite_data;
        [ProtoMember(22)]
        public BucksData[] bucks_data;
        [ProtoMember(17)]
        public string creation_source_data;
        [ProtoMember(23)]
        public ulong creation_source;
        [ProtoMember(24)]
        public ulong gameplay_options;
        [ProtoMember(25)]
        public ulong[] squad_members;
        [ProtoMember(26)]
        public ulong vehicle_id;
    }

    [ProtoContract]
    public class WorldLocation
    {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;
        [ProtoMember(4)]
        public float rot_x;
        [ProtoMember(5)]
        public float rot_y;
        [ProtoMember(6)]
        public float rot_z;
        [ProtoMember(7)]
        public float rot_w;
        [ProtoMember(8)]
        public int level;
        [ProtoMember(9)]
        public uint surface_id;
    }

    [ProtoContract]
    public class WhimsetTrackerData
    {
        [ProtoMember(4)]
        public WhimData[] active_whims;
    }

    [ProtoContract]
    public class WhimData
    {
        [ProtoMember(1)]
        public ulong whimset_guid;
        [ProtoMember(2)]
        public SituationGoalData goal_data;
        [ProtoMember(3)]
        public ulong duration;
        [ProtoMember(4)]
        public uint index;
    }

    [ProtoContract]
    public class AwayActionTrackerData
    {
        [ProtoMember(1)]
        public AwayActionData away_action;
    }

    [ProtoContract]
    public class AwayActionData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong away_action_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong target_sim_id;
    }

    [ProtoContract]
    public class SimFavoriteData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong[] recipe_ids;
    }

    public enum SimCreationPath
    {
        SIMCREATION_NONE = 0,
        SIMCREATION_INIT = 1,
        SIMCREATION_REENTRY_ADDSIM = 2,
        SIMCREATION_GALLERY = 3,
        SIMCREATION_PRE_MADE = 4,
        SIMCREATION_CLONED = 5,
    }

    [ProtoContract]
    public class PeltLayerDataList
    {
        [ProtoMember(1)]
        public PeltLayerData[] layers;
    }

    [ProtoContract]
    public class PeltLayerData
    {
        [ProtoMember(1)]
        public ulong layer_id;
        [ProtoMember(2)]
        public uint color;
    }

    [ProtoContract]
    public class ZoneData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(2)]
        public string name;
        [ProtoMember(3)]
        public uint world_id;
        [ProtoMember(4)]
        public uint lot_id;
        [ProtoMember(5)]
        public uint lot_template_id;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong nucleus_id;
        [ProtoMember(8)]
        public uint permissions;
        [ProtoMember(10, DataFormat = DataFormat.FixedSize)]
        public ulong neighborhood_id;
        [ProtoMember(11)]
        public GameplayZoneData gameplay_zone_data;
        [ProtoMember(12, DataFormat = DataFormat.FixedSize)]
        public ulong lot_description_id;
        [ProtoMember(13, DataFormat = DataFormat.FixedSize)]
        public ulong front_door_id;
        [ProtoMember(14)]
        public string description;
        [ProtoMember(15, DataFormat = DataFormat.FixedSize)]
        public ulong[] spawn_point_ids;
        [ProtoMember(16)]
        public uint bedroom_count;
        [ProtoMember(17)]
        public uint bathroom_count;
        [ProtoMember(18)]
        public uint active_plex;
        [ProtoMember(19, DataFormat = DataFormat.FixedSize)]
        public ulong master_zone_object_data_id;
        [ProtoMember(20, DataFormat = DataFormat.FixedSize)]
        public ulong[] lot_traits;
        [ProtoMember(21)]
        public uint pending_house_desc_id;
        [ProtoMember(22)]
        public UniversityHousingConfiguration university_housing_configuration;
        [ProtoMember(23)]
        public uint pending_plex_exterior_house_desc_id;
    }

    [ProtoContract]
    public class GameplayZoneData
    {
        [ProtoMember(1)]
        public ulong game_time;
        [ProtoMember(2)]
        public uint clock_speed_mode;
        [ProtoMember(3)]
        public AllSituationData situations_data;
        [ProtoMember(4)]
        public VenueData venue_data;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong active_household_id_on_save;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong lot_owner_household_id_on_save;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong venue_type_id_on_save;
        [ProtoMember(8)]
        public ZoneBedInfoData bed_info_data;
        [ProtoMember(9)]
        public PersistableCommodityTracker commodity_tracker;
        [ProtoMember(10)]
        public PersistableStatisticsTracker statistics_tracker;
        [ProtoMember(11)]
        public PersistableSkillTracker skill_tracker;
        [ProtoMember(12, DataFormat = DataFormat.FixedSize)]
        public ulong active_travel_group_id_on_save;
        [ProtoMember(13)]
        public EnsembleServiceData ensemble_service_data;
        [ProtoMember(14)]
        public PersistableRankedStatisticTracker ranked_statistic_tracker;
        [ProtoMember(15)]
        public int curfew_setting;
        [ProtoMember(16)]
        public HiddenSimServiceData hidden_sim_service_data;
        [ProtoMember(17)]
        public ZoneCurfewData[] curfew_settings;
        [ProtoMember(18)]
        public Statistic[] architectural_statistics;
    }

    [ProtoContract]
    public class AllSituationData
    {
        [ProtoMember(1)]
        public SituationSeedData[] seeds;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong leave_situation_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong leave_now_situation_id;
        [ProtoMember(4)]
        public SituationBlacklistData[] blacklist_data;
    }

    [ProtoContract]
    public class SituationBlacklistData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2)]
        public SituationBlacklistTagData[] tag_data;
    }

    [ProtoContract]
    public class SituationBlacklistTagData
    {
        [ProtoMember(1)]
        public ulong tag;
        [ProtoMember(2)]
        public ulong time;
    }

    [ProtoContract]
    public class VenueData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong background_situation_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong special_event_id;
        [ProtoMember(3)]
        public ZoneDirectorData zone_director;
        [ProtoMember(4)]
        public PersistableCivicPolicyProviderData civic_provider_data;
    }

    [ProtoContract]
    public class ZoneDirectorData
    {
        [ProtoMember(1)]
        public ResourceKey resource_key;
        [ProtoMember(2)]
        public ZoneDirectorCleanupActionData[] cleanup_actions;
        [ProtoMember(3)]
        public ZoneDirectorSituationData[] situations;
        [ProtoMember(4)]
        public byte[] custom_data;
        [ProtoMember(5)]
        public RestaurantZoneDirectorData restaurant_data;
    }

    [ProtoContract]
    public class ZoneDirectorCleanupActionData
    {
        [ProtoMember(1)]
        public uint guid;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] object_ids;
        [ProtoMember(3)]
        public ResourceKey[] resource_keys;
        [ProtoMember(4)]
        public StateComponentState[] states;
        [ProtoMember(5)]
        public Location location;
    }

    [ProtoContract]
    public class Location
    {
        [ProtoMember(1)]
        public Transform transform;
        [ProtoMember(2)]
        public int level;
        [ProtoMember(3)]
        public ulong parent_id;
        [ProtoMember(4)]
        public uint joint_name_hash;
        [ProtoMember(5)]
        public uint slot_hash;
        [ProtoMember(6)]
        public SurfaceId surface_id;
    }

    [ProtoContract]
    public class Transform
    {
        [ProtoMember(1)]
        public Vector3 translation;
        [ProtoMember(2)]
        public Quaternion orientation;
    }

    [ProtoContract]
    public class Quaternion
    {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;
        [ProtoMember(4)]
        public float w;
    }

    [ProtoContract]
    public class SurfaceId
    {
        [ProtoMember(1)]
        public ulong primary_id;
        [ProtoMember(2)]
        public int secondary_id;
        [ProtoMember(3)]
        public uint type;
    }

    [ProtoContract]
    public class ZoneDirectorSituationData
    {
        [ProtoMember(1)]
        public uint situation_list_guid;
        [ProtoMember(2)]
        public ulong[] situation_ids;
    }

    [ProtoContract]
    public class RestaurantZoneDirectorData
    {
        [ProtoMember(1)]
        public TableGroup[] table_group;
        [ProtoMember(2)]
        public int last_daily_special_absolute_day;
        [ProtoMember(3)]
        public SavedDailySpecial[] saved_daily_specials;
        [ProtoMember(4)]
        public GroupOrder[] group_orders;
    }

    [ProtoContract]
    public class TableGroup
    {
        [ProtoMember(1)]
        public ulong table_id;
        [ProtoMember(2)]
        public SimSeat[] sim_seats;
    }

    [ProtoContract]
    public class SimSeat
    {
        [ProtoMember(1)]
        public ulong sim_id;
        [ProtoMember(2)]
        public uint part_index;
    }

    [ProtoContract]
    public class SavedDailySpecial
    {
        [ProtoMember(1)]
        public uint course_tag;
        [ProtoMember(2)]
        public ulong recipe_id;
    }

    [ProtoContract]
    public class GroupOrder
    {
        [ProtoMember(1)]
        public uint order_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong situation_id;
        [ProtoMember(3)]
        public uint order_status;
        [ProtoMember(4)]
        public uint current_bucket;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong[] table_ids;
        [ProtoMember(6)]
        public FoodAndDrink[] sim_orders;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong assigned_waitstaff_id;
        [ProtoMember(8, DataFormat = DataFormat.FixedSize)]
        public ulong assigned_chef_id;
        [ProtoMember(9, DataFormat = DataFormat.FixedSize)]
        public ulong serving_object_id;
        [ProtoMember(10)]
        public bool is_complimentary;
    }

    [ProtoContract]
    public class FoodAndDrink
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong sim_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong food_recipe_id;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong drink_recipe_id;
        [ProtoMember(4)]
        public uint recommendation_state;
    }

    [ProtoContract]
    public class ZoneBedInfoData
    {
        [ProtoMember(1)]
        public uint num_beds;
        [ProtoMember(2)]
        public bool double_bed_exist;
        [ProtoMember(3)]
        public bool kid_bed_exist;
        [ProtoMember(4)]
        public uint alternative_sleeping_spots;
        [ProtoMember(5)]
        public uint university_roommate_beds;
    }

    [ProtoContract]
    public class EnsembleServiceData
    {
        [ProtoMember(1)]
        public EnsembleData[] ensemble_datas;
    }

    [ProtoContract]
    public class EnsembleData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong ensemble_type_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] sim_ids;
    }

    [ProtoContract]
    public class HiddenSimServiceData
    {
        [ProtoMember(1)]
        public HiddenSimData[] hidden_sim_data;
    }

    [ProtoContract]
    public class ZoneCurfewData
    {
        [ProtoMember(1)]
        public int time;
        [ProtoMember(2)]
        public int curfew_type;
    }

    [ProtoContract]
    public class OpenStreetsData
    {
        [ProtoMember(1)]
        public ulong world_id;
        [ProtoMember(2)]
        public ulong nbh_id;
        [ProtoMember(4)]
        public ObjectList objects;
        [ProtoMember(5)]
        public SituationSeedData[] situation_seeds;
        [ProtoMember(6, DataFormat = DataFormat.FixedSize)]
        public ulong active_household_id_on_save;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong active_zone_id_on_save;
        [ProtoMember(8)]
        public ulong sim_time_on_save;
        [ProtoMember(9)]
        public OpenStreetDirectorData open_street_director;
        [ProtoMember(10)]
        public ConditionalLayerServiceData conditional_layer_service;
        [ProtoMember(11)]
        public AmbientServiceData ambient_service;
        [ProtoMember(12)]
        public SituationConditionalLayerData[] situation_conditional_layers;
    }

    [ProtoContract]
    public class OpenStreetDirectorData
    {
        [ProtoMember(1)]
        public ResourceKey resource_key;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] created_objects;
        [ProtoMember(3)]
        public ulong[] situation_ids;
        [ProtoMember(4)]
        public byte[] custom_data;
        [ProtoMember(5, DataFormat = DataFormat.FixedSize)]
        public ulong[] loaded_layers;
        [ProtoMember(6)]
        public ZoneDirectorSituationData[] situations;
        [ProtoMember(7, DataFormat = DataFormat.FixedSize)]
        public ulong[] loaded_layer_guids;
    }

    [ProtoContract]
    public class ConditionalLayerServiceData
    {
        [ProtoMember(1)]
        public ConditionalLayerInfo[] layer_infos;
    }

    [ProtoContract]
    public class ConditionalLayerInfo
    {
        [ProtoMember(1)]
        public ulong layer_hash;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] object_ids;
        [ProtoMember(3)]
        public ulong conditional_layer;
    }

    [ProtoContract]
    public class AmbientServiceData
    {
        [ProtoMember(1)]
        public AmbientSourceData[] sources;
    }

    [ProtoContract]
    public class AmbientSourceData
    {
        [ProtoMember(1)]
        public uint source_type;
        [ProtoMember(2)]
        public ulong[] situation_ids;
        [ProtoMember(3)]
        public ZoneDirectorSituationData[] situations;
    }

    [ProtoContract]
    public class SituationConditionalLayerData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong layer_guid;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] situation_ids;
    }

    [ProtoContract]
    public class TravelGroupData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong travel_group_id;
        [ProtoMember(2)]
        public HouseholdSimIds[] household_sim_ids;
        [ProtoMember(3, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(4)]
        public bool played;
        [ProtoMember(5)]
        public ulong create_time;
        [ProtoMember(6)]
        public ulong end_time;
        [ProtoMember(7)]
        public ObjectPreferenceTracker object_preference_tracker;
    }

    [ProtoContract]
    public class HouseholdSimIds
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong[] sim_ids;
    }

    [ProtoContract]
    public class ObjectFallbackDataList
    {
        [ProtoMember(1)]
        public ObjectFallbackData[] fallbacks;
    }

    [ProtoContract]
    public class ObjectFallbackData
    {
        [ProtoMember(1)]
        public ulong guid;
        [ProtoMember(2)]
        public ulong fallback_guid;
    }

    [ProtoContract]
    public class GameplayDestinationCleanUpData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong household_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong travel_group_id;
        [ProtoMember(3)]
        public ObjectCleanUpData[] object_clean_up_data_list;
    }

    [ProtoContract]
    public class ObjectCleanUpData
    {
        [ProtoMember(1, DataFormat = DataFormat.FixedSize)]
        public ulong zone_id;
        [ProtoMember(2, DataFormat = DataFormat.FixedSize)]
        public ulong world_id;
        [ProtoMember(3)]
        public ObjectData object_data;
    }

    [ProtoContract]
    public class GameplayData
    {
        [ProtoMember(1)]
        public PremadeLotStatus[] premade_lot_status;
        [ProtoMember(2)]
        public uint daily_sim_info_creation_count;
    }

    [ProtoContract]
    public class PremadeLotStatus
    {
        [ProtoMember(1)]
        public ulong lot_id;
        [ProtoMember(2)]
        public bool is_premade;
    }

    [ProtoContract]
    public class SimRelationshipGraphData
    {
        [ProtoMember(1)]
        public SimRelationshipNode[] nodes;
    }

    [ProtoContract]
    public class SimRelationshipNode
    {
        [ProtoMember(1)]
        public ulong node_id;
        [ProtoMember(2)]
        public ulong sim_id;
        [ProtoMember(3)]
        public GenusData genus;
        [ProtoMember(4)]
        public string first_name;
        [ProtoMember(5)]
        public string last_name;
        [ProtoMember(6)]
        public SimRelationshipEdge[] outgoing_edges;
    }

    [ProtoContract]
    public class GenusData
    {
        [ProtoMember(1)]
        public uint gender;
        [ProtoMember(2)]
        public uint age;
        [ProtoMember(3)]
        public uint extended_species;
        [ProtoMember(4)]
        public uint occult_type;
    }

    [ProtoContract]
    public class SimRelationshipEdge
    {
        [ProtoMember(1)]
        public ulong target_node_id;
        [ProtoMember(2)]
        public ulong edge_data;
    }

    [ProtoContract]
    public class PlayerCustomColors
    {
        [ProtoMember(1)]
        public PlayerCustomColorList[] color_mapping;
    }

    [ProtoContract]
    public class PlayerCustomColorList
    {
        [ProtoMember(1)]
        public ResourceKey part_id;
        [ProtoMember(2)]
        public ulong[] color_shifts;
    }

    [ProtoContract]
    public class Modifier
    {
        [ProtoMember(1)]
        public ulong key;
        [ProtoMember(2)]
        public float amount;
    }

    [ProtoContract]
    public class BlobSimFacialCustomizationData
    {
        [ProtoMember(1)]
        public ulong[] sculpts;
        [ProtoMember(2)]
        public Modifier[] face_modifiers;
        [ProtoMember(3)]
        public Modifier[] body_modifiers;
    }
}
