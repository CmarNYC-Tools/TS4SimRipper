// <auto-generated>
//   This file was generated by a tool; you should avoid making direct changes.
//   Consider using 'partial classes' to extend these types
//   Input: MTXCatalog.proto
// </auto-generated>

#region Designer generated code
#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace EA.Sims4.Network
{

    [global::ProtoBuf.ProtoContract()]
    public partial class Product : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong id { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string offerid { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public ulong[] children { get; set; }

        [global::ProtoBuf.ProtoMember(4)]
        public global::System.Collections.Generic.List<ResourceKey> instances { get; } = new global::System.Collections.Generic.List<ResourceKey>();

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
        public global::System.Collections.Generic.List<Country> countries { get; } = new global::System.Collections.Generic.List<Country>();

        [global::ProtoBuf.ProtoMember(7, IsRequired = true)]
        public bool isBundle { get; set; }

        [global::ProtoBuf.ProtoMember(8)]
        [global::System.ComponentModel.DefaultValue(BundleContainment.LOOSE)]
        public BundleContainment containmentType
        {
            get => __pbn__containmentType ?? BundleContainment.LOOSE;
            set => __pbn__containmentType = value;
        }
        public bool ShouldSerializecontainmentType() => __pbn__containmentType != null;
        public void ResetcontainmentType() => __pbn__containmentType = null;
        private BundleContainment? __pbn__containmentType;

        [global::ProtoBuf.ProtoMember(9)]
        public ulong[] childProductIds { get; set; }

        [global::ProtoBuf.ProtoMember(10)]
        public int bundlePriority
        {
            get => __pbn__bundlePriority.GetValueOrDefault();
            set => __pbn__bundlePriority = value;
        }
        public bool ShouldSerializebundlePriority() => __pbn__bundlePriority != null;
        public void ResetbundlePriority() => __pbn__bundlePriority = null;
        private int? __pbn__bundlePriority;

        [global::ProtoBuf.ProtoMember(11)]
        public uint keyNameHash
        {
            get => __pbn__keyNameHash.GetValueOrDefault();
            set => __pbn__keyNameHash = value;
        }
        public bool ShouldSerializekeyNameHash() => __pbn__keyNameHash != null;
        public void ResetkeyNameHash() => __pbn__keyNameHash = null;
        private uint? __pbn__keyNameHash;

        [global::ProtoBuf.ProtoMember(12)]
        public uint keyDescriptionHash
        {
            get => __pbn__keyDescriptionHash.GetValueOrDefault();
            set => __pbn__keyDescriptionHash = value;
        }
        public bool ShouldSerializekeyDescriptionHash() => __pbn__keyDescriptionHash != null;
        public void ResetkeyDescriptionHash() => __pbn__keyDescriptionHash = null;
        private uint? __pbn__keyDescriptionHash;

        [global::ProtoBuf.ProtoMember(13)]
        public uint keyUpsellDescriptionHash
        {
            get => __pbn__keyUpsellDescriptionHash.GetValueOrDefault();
            set => __pbn__keyUpsellDescriptionHash = value;
        }
        public bool ShouldSerializekeyUpsellDescriptionHash() => __pbn__keyUpsellDescriptionHash != null;
        public void ResetkeyUpsellDescriptionHash() => __pbn__keyUpsellDescriptionHash = null;
        private uint? __pbn__keyUpsellDescriptionHash;

        [global::ProtoBuf.ProtoMember(14)]
        public ulong thumbnailResourceInstanceIdHash
        {
            get => __pbn__thumbnailResourceInstanceIdHash.GetValueOrDefault();
            set => __pbn__thumbnailResourceInstanceIdHash = value;
        }
        public bool ShouldSerializethumbnailResourceInstanceIdHash() => __pbn__thumbnailResourceInstanceIdHash != null;
        public void ResetthumbnailResourceInstanceIdHash() => __pbn__thumbnailResourceInstanceIdHash = null;
        private ulong? __pbn__thumbnailResourceInstanceIdHash;

        [global::ProtoBuf.ProtoMember(15)]
        public bool hiddenInCatalog
        {
            get => __pbn__hiddenInCatalog.GetValueOrDefault();
            set => __pbn__hiddenInCatalog = value;
        }
        public bool ShouldSerializehiddenInCatalog() => __pbn__hiddenInCatalog != null;
        public void ResethiddenInCatalog() => __pbn__hiddenInCatalog = null;
        private bool? __pbn__hiddenInCatalog;

        [global::ProtoBuf.ProtoMember(16)]
        [global::System.ComponentModel.DefaultValue(true)]
        public bool isPurchasable
        {
            get => __pbn__isPurchasable ?? true;
            set => __pbn__isPurchasable = value;
        }
        public bool ShouldSerializeisPurchasable() => __pbn__isPurchasable != null;
        public void ResetisPurchasable() => __pbn__isPurchasable = null;
        private bool? __pbn__isPurchasable;

        [global::ProtoBuf.ProtoMember(17)]
        [global::System.ComponentModel.DefaultValue(400u)]
        public uint celebrationPriority
        {
            get => __pbn__celebrationPriority ?? 400u;
            set => __pbn__celebrationPriority = value;
        }
        public bool ShouldSerializecelebrationPriority() => __pbn__celebrationPriority != null;
        public void ResetcelebrationPriority() => __pbn__celebrationPriority = null;
        private uint? __pbn__celebrationPriority;

        [global::ProtoBuf.ProtoMember(18)]
        [global::System.ComponentModel.DefaultValue(false)]
        public bool localizedImages
        {
            get => __pbn__localizedImages ?? false;
            set => __pbn__localizedImages = value;
        }
        public bool ShouldSerializelocalizedImages() => __pbn__localizedImages != null;
        public void ResetlocalizedImages() => __pbn__localizedImages = null;
        private bool? __pbn__localizedImages;

        [global::ProtoBuf.ProtoMember(19)]
        [global::System.ComponentModel.DefaultValue(false)]
        public bool show_variants
        {
            get => __pbn__show_variants ?? false;
            set => __pbn__show_variants = value;
        }
        public bool ShouldSerializeshow_variants() => __pbn__show_variants != null;
        public void Resetshow_variants() => __pbn__show_variants = null;
        private bool? __pbn__show_variants;

        [global::ProtoBuf.ProtoMember(20)]
        [global::System.ComponentModel.DefaultValue(typeof(ulong), "0")]
        public ulong keyImage
        {
            get => __pbn__keyImage ?? 0ul;
            set => __pbn__keyImage = value;
        }
        public bool ShouldSerializekeyImage() => __pbn__keyImage != null;
        public void ResetkeyImage() => __pbn__keyImage = null;
        private ulong? __pbn__keyImage;

        [global::ProtoBuf.ProtoMember(21)]
        [global::System.ComponentModel.DefaultValue(ProductType.CAS)]
        public ProductType productType
        {
            get => __pbn__productType ?? ProductType.CAS;
            set => __pbn__productType = value;
        }
        public bool ShouldSerializeproductType() => __pbn__productType != null;
        public void ResetproductType() => __pbn__productType = null;
        private ProductType? __pbn__productType;

        [global::ProtoBuf.ProtoMember(22)]
        [global::System.ComponentModel.DefaultValue("")]
        public string productInfoURL
        {
            get => __pbn__productInfoURL ?? "";
            set => __pbn__productInfoURL = value;
        }
        public bool ShouldSerializeproductInfoURL() => __pbn__productInfoURL != null;
        public void ResetproductInfoURL() => __pbn__productInfoURL = null;
        private string __pbn__productInfoURL;

        [global::ProtoBuf.ProtoMember(23)]
        [global::System.ComponentModel.DefaultValue(true)]
        public bool isAvailable
        {
            get => __pbn__isAvailable ?? true;
            set => __pbn__isAvailable = value;
        }
        public bool ShouldSerializeisAvailable() => __pbn__isAvailable != null;
        public void ResetisAvailable() => __pbn__isAvailable = null;
        private bool? __pbn__isAvailable;

        [global::ProtoBuf.ProtoMember(24)]
        [global::System.ComponentModel.DefaultValue(true)]
        public bool showInPackDetail
        {
            get => __pbn__showInPackDetail ?? true;
            set => __pbn__showInPackDetail = value;
        }
        public bool ShouldSerializeshowInPackDetail() => __pbn__showInPackDetail != null;
        public void ResetshowInPackDetail() => __pbn__showInPackDetail = null;
        private bool? __pbn__showInPackDetail;

        [global::ProtoBuf.ProtoMember(25)]
        [global::System.ComponentModel.DefaultValue(-1)]
        public int packId
        {
            get => __pbn__packId ?? -1;
            set => __pbn__packId = value;
        }
        public bool ShouldSerializepackId() => __pbn__packId != null;
        public void ResetpackId() => __pbn__packId = null;
        private int? __pbn__packId;

        [global::ProtoBuf.ProtoContract()]
        public enum BundleContainment
        {
            LOOSE = 0,
            TIGHT = 1,
        }

        [global::ProtoBuf.ProtoContract()]
        public enum ProductType
        {
            CAS = 0,
            BB = 1,
            GAMEPLAY = 2,
            GENERIC_MTX = 3,
            INVALID_MTX = 4,
        }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class OfferList : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<string> offer_id { get; } = new global::System.Collections.Generic.List<string>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Catalog : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public ulong timestamp { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<Product> products { get; } = new global::System.Collections.Generic.List<Product>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Country : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public string countryCode { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public global::System.Collections.Generic.List<Price> prices { get; } = new global::System.Collections.Generic.List<Price>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class Price : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, IsRequired = true)]
        public float price { get; set; }

        [global::ProtoBuf.ProtoMember(2, IsRequired = true)]
        public string currency { get; set; }

        [global::ProtoBuf.ProtoMember(3, IsRequired = true)]
        public string currencyType { get; set; }

        [global::ProtoBuf.ProtoMember(4, IsRequired = true)]
        public string priceType { get; set; }

        [global::ProtoBuf.ProtoMember(5)]
        public ulong startDate
        {
            get => __pbn__startDate.GetValueOrDefault();
            set => __pbn__startDate = value;
        }
        public bool ShouldSerializestartDate() => __pbn__startDate != null;
        public void ResetstartDate() => __pbn__startDate = null;
        private ulong? __pbn__startDate;

        [global::ProtoBuf.ProtoMember(6)]
        public ulong endDate
        {
            get => __pbn__endDate.GetValueOrDefault();
            set => __pbn__endDate = value;
        }
        public bool ShouldSerializeendDate() => __pbn__endDate != null;
        public void ResetendDate() => __pbn__endDate = null;
        private ulong? __pbn__endDate;

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class ViewedEntitlements : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1)]
        public global::System.Collections.Generic.List<EntitlementNotification> viewed_entitlements { get; } = new global::System.Collections.Generic.List<EntitlementNotification>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class EntitlementNotification : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(2)]
        [global::System.ComponentModel.DefaultValue(1u)]
        public uint viewed_state
        {
            get => __pbn__viewed_state ?? 1u;
            set => __pbn__viewed_state = value;
        }
        public bool ShouldSerializeviewed_state() => __pbn__viewed_state != null;
        public void Resetviewed_state() => __pbn__viewed_state = null;
        private uint? __pbn__viewed_state;

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
        public uint trial_viewed_state
        {
            get => __pbn__trial_viewed_state.GetValueOrDefault();
            set => __pbn__trial_viewed_state = value;
        }
        public bool ShouldSerializetrial_viewed_state() => __pbn__trial_viewed_state != null;
        public void Resettrial_viewed_state() => __pbn__trial_viewed_state = null;
        private uint? __pbn__trial_viewed_state;

        [global::ProtoBuf.ProtoMember(5)]
        public ulong entitlement_id_at_trial_expiration_or_conversion
        {
            get => __pbn__entitlement_id_at_trial_expiration_or_conversion.GetValueOrDefault();
            set => __pbn__entitlement_id_at_trial_expiration_or_conversion = value;
        }
        public bool ShouldSerializeentitlement_id_at_trial_expiration_or_conversion() => __pbn__entitlement_id_at_trial_expiration_or_conversion != null;
        public void Resetentitlement_id_at_trial_expiration_or_conversion() => __pbn__entitlement_id_at_trial_expiration_or_conversion = null;
        private ulong? __pbn__entitlement_id_at_trial_expiration_or_conversion;

        [global::ProtoBuf.ProtoContract()]
        public enum ViewedState
        {
            VIEWED_INVALID = 0,
            VIEWED_NEW = 1,
            VIEWED_CELEBRATED = 2,
            VIEWED_USED = 4,
            VIEWED_ALL = 255,
        }

        [global::ProtoBuf.ProtoContract()]
        public enum TrialViewedStateMask
        {
            TRIAL_VIEWED_MASK_EXPIRED_TRIAL_OR_CONVERTED = 1,
            TRIAL_VIEWED_MASK_TRIALS_NOT_SUPPORTED = 2,
        }

        [global::ProtoBuf.ProtoContract()]
        public enum TrialViewedState
        {
            TRIAL_VIEWED_EXPIRED_TRIAL_OR_CONVERTED = 1,
            TRIAL_VIEWED_TRIALS_NOT_SUPPORTED = 2,
        }

    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
#endregion