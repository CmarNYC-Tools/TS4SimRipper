// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: Loot.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class SituationEnded : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public IconInfo icon_info { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public uint final_score
        {
            get => __pbn__final_score.GetValueOrDefault();
            set => __pbn__final_score = value;
        }
        public bool ShouldSerializefinal_score() => __pbn__final_score != null;
        public void Resetfinal_score() => __pbn__final_score = null;
        private uint? __pbn__final_score;

        [global::ProtoBuf.ProtoMember(4)]
        public SituationLevelUpdate final_level { get; set; }

        [global::ProtoBuf.ProtoMember(5, DataFormat = global::ProtoBuf.DataFormat.FixedSize)]
        public ulong[] sim_ids { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        public ResourceKey audio_sting { get; set; }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
