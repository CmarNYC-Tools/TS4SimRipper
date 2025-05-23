// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: WeatherSeasons.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class SeasonWeatherInterpolationMessage : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public SeasonWeatherInterpolatedType message_type { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public float start_value { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public ulong start_time { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public float end_value { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public ulong end_time { get; set; }

        [global::ProtoBuf.ProtoContract()]
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

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class SeasonWeatherInterpolations : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<SeasonWeatherInterpolationMessage> season_weather_interlops { get; } = new global::System.Collections.Generic.List<SeasonWeatherInterpolationMessage>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class RegionWeather : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, DataFormat = global::ProtoBuf.DataFormat.FixedSize, IsRequired = true)]
        public ulong region { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public SeasonWeatherInterpolations weather { get; set; }

        [global::ProtoBuf.ProtoMember(3, DataFormat = global::ProtoBuf.DataFormat.FixedSize, IsRequired = true)]
        public ulong weather_event { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public ulong forecast_time_stamp { get; set; }

        [global::ProtoBuf.ProtoMember(5, IsRequired = true)]
        public ulong next_weather_event_time { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        public ulong[] forecasts { get; set; }

        [global::ProtoBuf.ProtoMember(7)]
        public ulong override_forecast
        {
            get => __pbn__override_forecast.GetValueOrDefault();
            set => __pbn__override_forecast = value;
        }
        public bool ShouldSerializeoverride_forecast() => __pbn__override_forecast != null;
        public void Resetoverride_forecast() => __pbn__override_forecast = null;
        private ulong? __pbn__override_forecast;

        [global::ProtoBuf.ProtoMember(8)]
        public ulong override_forecast_season_stamp
        {
            get => __pbn__override_forecast_season_stamp.GetValueOrDefault();
            set => __pbn__override_forecast_season_stamp = value;
        }
        public bool ShouldSerializeoverride_forecast_season_stamp() => __pbn__override_forecast_season_stamp != null;
        public void Resetoverride_forecast_season_stamp() => __pbn__override_forecast_season_stamp = null;
        private ulong? __pbn__override_forecast_season_stamp;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class PersistableWeatherService : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<RegionWeather> region_weathers { get; } = new global::System.Collections.Generic.List<RegionWeather>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UiWeatherUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public long[] weather_type_enums { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UiWeatherForecastUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong[] forecast_instance_ids { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MoonPhaseUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public MoonPhase current_moon_phase { get; set; } = MoonPhase.DEFAULT_NO_MOON;

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue(true)]
        public bool skip_environment_changes
        {
            get => __pbn__skip_environment_changes ?? true;
            set => __pbn__skip_environment_changes = value;
        }
        public bool ShouldSerializeskip_environment_changes() => __pbn__skip_environment_changes != null;
        public void Resetskip_environment_changes() => __pbn__skip_environment_changes = null;
        private bool? __pbn__skip_environment_changes;

        [global::ProtoBuf.ProtoContract()]
        public enum MoonPhase
        {
            DEFAULT_NO_MOON = -1,
            NEW_MOON = 0,
            WAXING_CRESCENT = 1,
            FIRST_QUARTER = 2,
            WAXING_GIBBOUS = 3,
            FULL_MOON = 4,
            WANING_GIBBOUS = 5,
            THIRD_QUARTER = 6,
            WANING_CRESCENT = 7,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class UiLunarEffectTooltipUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public uint current_moon_phase { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public LocalizedString tooltip_text { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class MoonForecastUpdate : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<MoonPhase> forecast_moon_phases { get; } = new global::System.Collections.Generic.List<MoonPhase>();

        [global::ProtoBuf.ProtoContract()]
        public enum MoonPhase
        {
            DEFAULT_NO_MOON = -1,
            NEW_MOON = 0,
            WAXING_CRESCENT = 1,
            FIRST_QUARTER = 2,
            WAXING_GIBBOUS = 3,
            FULL_MOON = 4,
            WANING_GIBBOUS = 5,
            THIRD_QUARTER = 6,
            WANING_CRESCENT = 7,
        }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
