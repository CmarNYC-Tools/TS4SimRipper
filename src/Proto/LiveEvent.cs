// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: LiveEvent.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Persistence
{

    [global::ProtoBuf.ProtoContract()]
    public partial class LiveEventProgress : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<LiveEventLogin> loginEvents { get; } = new global::System.Collections.Generic.List<LiveEventLogin>();

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<LiveEventQuest> questEvents { get; } = new global::System.Collections.Generic.List<LiveEventQuest>();

        [global::ProtoBuf.ProtoContract()]
        public partial class LiveEventBase : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public ulong id { get; set; }

            [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
            public LiveEventProgress.LiveEventType type { get; set; }

            [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
            public global::EA.Sims4.Network.LocalizedDateAndTimeData startDate { get; set; }

            [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
            public global::EA.Sims4.Network.LocalizedDateAndTimeData endDate { get; set; }

            [global::ProtoBuf.ProtoMember(5)]
            public bool isEnabled
            {
                get => __pbn__isEnabled.GetValueOrDefault();
                set => __pbn__isEnabled = value;
            }
            public bool ShouldSerializeisEnabled() => __pbn__isEnabled != null;
            public void ResetisEnabled() => __pbn__isEnabled = null;
            private bool? __pbn__isEnabled;

        }

        [global::ProtoBuf.ProtoContract()]
        public partial class LiveEventReward : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public global::EA.Sims4.Network.ResourceKey instance { get; set; }

        }

        [global::ProtoBuf.ProtoContract()]
        public partial class LiveEventLogin : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public LiveEventProgress.LiveEventBase eventData { get; set; }

            [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
            public uint sessionCount { get; set; }

            [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
            public global::EA.Sims4.Network.LocalizedDateAndTimeData lastSessionDate { get; set; }

            [global::ProtoBuf.ProtoMember(4)]
            public global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward> claimedRewards { get; } = new global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward>();

            [global::ProtoBuf.ProtoMember(5)]
            public global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward> unclaimedRewards { get; } = new global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward>();

        }

        [global::ProtoBuf.ProtoContract()]
        public partial class LiveEventQuest : global::ProtoBuf.IExtensible
        {
            private global::ProtoBuf.IExtension __pbn__extensionData;
            global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
                => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

            [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
            public LiveEventProgress.LiveEventBase eventData { get; set; }

            [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
            public global::EA.Sims4.Network.EventDefinition dataFromServer { get; set; }

            [global::ProtoBuf.ProtoMember(3)]
            public global::EA.Sims4.Network.EventProgressResponse.EventProgress progressVerifed { get; set; }

            [global::ProtoBuf.ProtoMember(4)]
            public global::EA.Sims4.Network.EventProgressUpdate progressOffline { get; set; }

            [global::ProtoBuf.ProtoMember(5)]
            public global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward> claimedRewards { get; } = new global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward>();

            [global::ProtoBuf.ProtoMember(6)]
            public global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward> unclaimedRewards { get; } = new global::System.Collections.Generic.List<LiveEventProgress.LiveEventReward>();

        }

        [global::ProtoBuf.ProtoContract()]
        public enum LiveEventType
        {
            UNKNOWN = 0,
            LOGIN = 1,
            QUEST = 2,
        }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, CS8981, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
