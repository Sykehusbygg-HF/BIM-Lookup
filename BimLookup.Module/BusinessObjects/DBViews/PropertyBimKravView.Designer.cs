//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DevExpress.Persistent.Base;

namespace BimLookup.Module.BusinessObjects.DBViews
{
    [DefaultClassOptions, ImageName("BO_Contact")]
    public partial class PropertyBimKravView : XPLiteObject
    {
        long fTempKey;
        [Key]
        public long TempKey
        {
            get { return fTempKey; }
            set { SetPropertyValue<long>(nameof(TempKey), ref fTempKey, value); }
        }
        Guid fOid;
        public Guid Oid
        {
            get { return fOid; }
            set { SetPropertyValue<Guid>(nameof(Oid), ref fOid, value); }
        }
        string fName;
        public string Name
        {
            get { return fName; }
            set { SetPropertyValue<string>(nameof(Name), ref fName, value); }
        }
        int fVersion;
        public int Version
        {
            get { return fVersion; }
            set { SetPropertyValue<int>(nameof(Version), ref fVersion, value); }
        }
        string fDescription;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return fDescription; }
            set { SetPropertyValue<string>(nameof(Description), ref fDescription, value); }
        }
        string fComment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return fComment; }
            set { SetPropertyValue<string>(nameof(Comment), ref fComment, value); }
        }
        bool fSkisseprosjekt;
        public bool Skisseprosjekt
        {
            get { return fSkisseprosjekt; }
            set { SetPropertyValue<bool>(nameof(Skisseprosjekt), ref fSkisseprosjekt, value); }
        }
        bool fForprosjekt;
        public bool Forprosjekt
        {
            get { return fForprosjekt; }
            set { SetPropertyValue<bool>(nameof(Forprosjekt), ref fForprosjekt, value); }
        }
        bool fDetaljprosjekt;
        public bool Detaljprosjekt
        {
            get { return fDetaljprosjekt; }
            set { SetPropertyValue<bool>(nameof(Detaljprosjekt), ref fDetaljprosjekt, value); }
        }
        bool fArbeidstegning;
        public bool Arbeidstegning
        {
            get { return fArbeidstegning; }
            set { SetPropertyValue<bool>(nameof(Arbeidstegning), ref fArbeidstegning, value); }
        }
        bool fOverlevering;
        public bool Overlevering
        {
            get { return fOverlevering; }
            set { SetPropertyValue<bool>(nameof(Overlevering), ref fOverlevering, value); }
        }
        string fPsetName;
        public string PsetName
        {
            get { return fPsetName; }
            set { SetPropertyValue<string>(nameof(PsetName), ref fPsetName, value); }
        }
        Guid fPsetOid;
        public Guid PsetOid
        {
            get { return fPsetOid; }
            set { SetPropertyValue<Guid>(nameof(PsetOid), ref fPsetOid, value); }
        }
    }

}
