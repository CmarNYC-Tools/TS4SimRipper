// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: RestApi.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class RestControlMessage : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public ulong nucleus_id
        {
            get => __pbn__nucleus_id.GetValueOrDefault();
            set => __pbn__nucleus_id = value;
        }
        public bool ShouldSerializenucleus_id() => __pbn__nucleus_id != null;
        public void Resetnucleus_id() => __pbn__nucleus_id = null;
        private ulong? __pbn__nucleus_id;

        [global::ProtoBuf.ProtoMember(2)]
        public ulong @ref
        {
            get => __pbn__ref.GetValueOrDefault();
            set => __pbn__ref = value;
        }
        public bool ShouldSerializeref() => __pbn__ref != null;
        public void Resetref() => __pbn__ref = null;
        private ulong? __pbn__ref;

        [global::ProtoBuf.ProtoMember(3)]
        [global::System.ComponentModel.DefaultValue("")]
        public string persona
        {
            get => __pbn__persona ?? "";
            set => __pbn__persona = value;
        }
        public bool ShouldSerializepersona() => __pbn__persona != null;
        public void Resetpersona() => __pbn__persona = null;
        private string __pbn__persona;

        [global::ProtoBuf.ProtoMember(4)]
        public SocialControlMessage social { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public ExchangeControlMessage exchange { get; set; }

        [global::ProtoBuf.ProtoMember(6)]
        [global::System.ComponentModel.DefaultValue(RestApiErrorMessages.REST_API_ERROR_SUCCESS)]
        public RestApiErrorMessages error_message
        {
            get => __pbn__error_message ?? RestApiErrorMessages.REST_API_ERROR_SUCCESS;
            set => __pbn__error_message = value;
        }
        public bool ShouldSerializeerror_message() => __pbn__error_message != null;
        public void Reseterror_message() => __pbn__error_message = null;
        private RestApiErrorMessages? __pbn__error_message;

        [global::ProtoBuf.ProtoMember(7)]
        [global::System.ComponentModel.DefaultValue("")]
        public string access_token
        {
            get => __pbn__access_token ?? "";
            set => __pbn__access_token = value;
        }
        public bool ShouldSerializeaccess_token() => __pbn__access_token != null;
        public void Resetaccess_token() => __pbn__access_token = null;
        private string __pbn__access_token;

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion
