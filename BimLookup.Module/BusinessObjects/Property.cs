using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.XtraRichEdit.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BimLookup.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Note_32x32")]
    [Category("BIM Catalog")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("Property")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Property : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Property(Session session)
            : base(session)
        {
        }
        // Automatically fill the UpdatedOn and UpdatedBy columns 
        // when a current user saves a modified Contact item.
        protected override void OnSaving()
        {
            //Håll koll på latest version to be able 
            base.OnSaving();
            if (Versions.Count == 0)
                Latest = true;
            else if (Version >= Versions?.Max(x => x.Version))
                Latest = true;
            else Latest = false;
            if (Latest)
            {
                foreach (var ver in Versions)
                {
                    if (ver.Oid != this.Oid)
                    {
                        ver.Latest = false;
                        ver.Save();
                    }
                }
            }
            if(this.Guid == null || string.IsNullOrEmpty(this.Guid))
            {
                this.Guid = System.Guid.NewGuid().ToString();
            }

        }
        protected override void OnSaved()
        {
            base.OnSaved();
           
        }
        
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private int _MariaDB_ID_Property;
        [XafDisplayName("MariaDB_ID_Property"), ToolTip("MariaDB_ID_Property")]
        [Browsable(false)]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("MariaDB_ID_Property")]//, RuleRequiredField(DefaultContexts.Save)]
        public int MariaDB_ID_Property
        {
            get { return _MariaDB_ID_Property; }
            set { SetPropertyValue(nameof(MariaDB_ID_Property), ref _MariaDB_ID_Property, value); }
        }
        private string _Name;
        [XafDisplayName("Name"), ToolTip("Name")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Name")]//, RuleRequiredField(DefaultContexts.Save)]
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue(nameof(Name), ref _Name, value); }
        }
        [NonPersistent]
        [XafDisplayName("Property Set"), ToolTip("Property Set")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        public string PropertySetDisplayValue
        {
            get { return string.Join(",", this.PropertySets); }
        }
        [NonPersistent]
        [XafDisplayName("Phases"), ToolTip("Phases")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        public string PhasesDisplayValue
        {
            get {
                List<string> dispvalues = new List<string>();
                if (Skisseprosjekt)
                    dispvalues.Add("Skisseprosjekt");
                if (Forprosjekt)
                    dispvalues.Add("Forprosjekt");
                if (Detaljprosjekt)
                    dispvalues.Add("Detaljprosjekt");
                if (Arbeidstegning)
                    dispvalues.Add("Arbeidstegning");
                if (Overlevering)
                    dispvalues.Add("Overlevering");
                return string.Join(",", dispvalues); 
            
            }
        }
        #region Versioning
        private int _Version;
        [XafDisplayName("Version"), ToolTip("Version")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Version")]//, RuleRequiredField(DefaultContexts.Save)]
        [Browsable(false)]
        public int Version
        {
            get { return _Version; }
            set { SetPropertyValue(nameof(Version), ref _Version, value); }
        }
        //[NonPersistent]
        [XafDisplayName("Version"), ToolTip("Version")]
        [PersistentAlias("Version")]
        public int DisplayVersion
        {
            get { return Version; }
        }
        //[PersistentAlias("Version")]
        //[PersistentAlias("iif(Versions.Count = 0, True, iif(Version > Versions[Name = 'Version'].Max(Name),True,False))")]
        //[PersistentAlias("Iif([Versions].Count = 0 Or Version >= [Versions].Max([Version]), 'Senaste', 'Gammal')")]
        //[PersistentAlias("Iif([Versions].Count = 0, 'Senaste', 'Gammal')")]
        //[PersistentAlias("[Versions.Count] = 0")]   // Or [Versions][].Max([Version]) <= [Version], True, False)")]
        //[NonPersistent]
        //[PersistentAlias("([Version1].Count = 0 And [Version2].Count = 0) Or (Version >= [Version1].Max([Version]) And Version >= [Version2].Max([Version]))")]
        //[XafDisplayName("Latest Version"), ToolTip("Latest Version")]
        //[Browsable(false)]
        //public bool LatestVersion
        //{
        //    get {
        //        // LINQ-style criteria syntax typed
        //        //CriteriaOperator opLambda = CriteriaOperator.FromLambda<Property, bool>(x => x.Versions.Count == 0 || x.Versions.Max(y => y.Version) <= x.Version);
          //      return Convert.ToBoolean(EvaluateAlias("LatestVersion"));
        //        //if (Versions.Count == 0)
        //        //    return 1;
        //        //if (Version >= Versions?.Max(x => x.Version))
        //        //    return 1;
        //        //return 0;
           // }
        //}
        private bool _Latest = true;
        [XafDisplayName("Latest"), ToolTip("Latest")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Latest")]//, RuleRequiredField(DefaultContexts.Save)]
#if !DEBUG
        [Browsable(false)]
#endif
        public bool Latest
        {
            get
            {
                return _Latest;
            }
            set { SetPropertyValue(nameof(Latest), ref _Latest, value); }
        }
        [Browsable(false)]
        public bool IncludeInBalancing
        {
            get
            {
                return this.PropertyGroup?.IncludeInBalancing ?? false;
            }
        }

            [Association("Version1-Version2"), Browsable(false)]
        public XPCollection<Property> Version1
        {
            get
            {
                return GetCollection<Property>("Version1");
            }
        }
        [Association("Version1-Version2"), Browsable(false)]
        public XPCollection<Property> Version2
        {
            get
            {
                return GetCollection<Property>("Version2");
            }
        }
        private XPCollection<Property> _Versions;
        public XPCollection<Property> Versions
        {
            get
            {
                if (_Versions == null)
                {
                    _Versions = new XPCollection<Property>(Version1);
                    _Versions.AddRange(Version2);
                    _Versions.CollectionChanged += new XPCollectionChangedEventHandler(_Versions_CollectionChanged);
                }
                if (_Versions.Where(x => x.Oid == this.Oid).FirstOrDefault() == null)
                    _Versions.Add(this);
                return _Versions;
            }
        }
        void _Versions_CollectionChanged(object sender, XPCollectionChangedEventArgs e)
        {
            if (e.CollectionChangedType == XPCollectionChangedType.AfterRemove)
            {
                Version1.Remove((Property)e.ChangedObject);
                Version2.Remove((Property)e.ChangedObject);
            }
            else if (e.CollectionChangedType == XPCollectionChangedType.AfterAdd)
            {
                Version1.Add((Property)e.ChangedObject);
            }
        }
#endregion Versioning
        private IfcPropertyType ifcPropertyType;
        [Association("IfcPropertyType-Properties")]
        public IfcPropertyType IfcPropertyType
        {
            get { return ifcPropertyType; }
            set { SetPropertyValue(nameof(IfcPropertyType), ref ifcPropertyType, value); }
        }

        private RevitPropertyType revitPropertyType;
        [Association("RevitPropertyType-Properties")]
        public RevitPropertyType RevitPropertyType
        {
            get { return revitPropertyType; }
            set { SetPropertyValue(nameof(RevitPropertyType), ref revitPropertyType, value); }
        }
        //private string _TypeInstance;
        //[XafDisplayName("TypeInstance"), ToolTip("TypeInstance")]
        ////[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("TypeInstance")]//, RuleRequiredField(DefaultContexts.Save)]
        //public string TypeInstance
        //{
        //    get { return _TypeInstance; }
        //    set { SetPropertyValue(nameof(TypeInstance), ref _TypeInstance, value); }
        //}
        private TypeInstanceEnum type_Instance;
        [XafDisplayName("Type/Instance")]
        public TypeInstanceEnum Type_Instance
        {
            get { return type_Instance; }
            set { SetPropertyValue(nameof(Type_Instance), ref type_Instance, value); }
        }
        private string _Guid;
        [XafDisplayName("Guid")]
        //[Browsable(false)]
        [VisibleInListView(false)]
        [ModelDefault("AllowEdit", "False")]
        public string Guid
        {
            get { return _Guid; }
            set { SetPropertyValue(nameof(Guid), ref _Guid, value); }
        }
        // ...
        //[DisplayName("Type/Instance")]
        public enum TypeInstanceEnum
        {
            [ImageName("ModelEditor_Actions_ActionDesign")]
            Type,
            [ImageName("ModelEditor_Actions")]
            Instance,
        }
        private PropertyGroup propertyGroup;
        [Association("PropertyGroup-Properties")]
        public PropertyGroup PropertyGroup
        {
            get { return propertyGroup; }
            set { SetPropertyValue(nameof(PropertyGroup), ref propertyGroup, value); }
        }
        private string _Description;
        [XafDisplayName("Description"), ToolTip("Description")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Description")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return _Description; }
            set { SetPropertyValue(nameof(Description), ref _Description, value); }
        }
        private string _Comment;
        [XafDisplayName("Comment"), ToolTip("Comment")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Comment")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue(nameof(Comment), ref _Comment, value); }
        }
        private Owner _owner;
        [Persistent("Owner")]
        [XafDisplayName("Owner"), ToolTip("Owner")]
        [Association("Owner-Properties")]
        public Owner Owner
        {
            get { return _owner; }
            set { SetPropertyValue(nameof(Owner), ref _owner, value); }
        }
        public Property CreateNewVersion(IObjectSpace ObjectSpace)
        {
            Property newversion = ObjectSpace.CreateObject<Property>();
            newversion.Comment = Comment;
            newversion.Description = Description;
            newversion.IfcPropertyType = IfcPropertyType;

            newversion.Type_Instance = Type_Instance;
            newversion.MariaDB_ID_Property = MariaDB_ID_Property;
            newversion.Name = Name;
            newversion.Arbeidstegning = Arbeidstegning;
            newversion.Detaljprosjekt = Detaljprosjekt;
            newversion.Forprosjekt = Forprosjekt;
            newversion.Overlevering = Overlevering;
            newversion.Skisseprosjekt = Skisseprosjekt;
            newversion.PropertyGroup = PropertyGroup;
            newversion.PropertySets.AddRange(PropertySets);
            newversion.RevitCategories.AddRange(RevitCategories);
            newversion.RevitPropertyType = RevitPropertyType;
            newversion.Versions.AddRange(Versions);
            newversion.Versions.Add(this);
            int currentVersion = newversion.Versions.Max(x => x.Version);
            newversion.Version = currentVersion+1;
            return newversion;
        }
        private bool _Skisseprosjekt;
        [XafDisplayName("Skisseprosjekt"), ToolTip("Skisseprosjekt")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Skisseprosjekt")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public bool Skisseprosjekt
        {
            get { return _Skisseprosjekt; }
            set { SetPropertyValue(nameof(Skisseprosjekt), ref _Skisseprosjekt, value); }
        }
        private bool _Forprosjekt;
        [XafDisplayName("Forprosjekt"), ToolTip("Forprosjekt")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Forprosjekt")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public bool Forprosjekt
        {
            get { return _Forprosjekt; }
            set { SetPropertyValue(nameof(Forprosjekt), ref _Forprosjekt, value); }
        }
        private bool _Detaljprosjekt;
        [XafDisplayName("Detaljprosjekt"), ToolTip("Detaljprosjekt")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Detaljprosjekt")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public bool Detaljprosjekt
        {
            get { return _Detaljprosjekt; }
            set { SetPropertyValue(nameof(Detaljprosjekt), ref _Detaljprosjekt, value); }
        }
        private bool _Arbeidstegning;
        [XafDisplayName("Arbeidstegning"), ToolTip("Arbeidstegning")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Arbeidstegning")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public bool Arbeidstegning
        {
            get { return _Arbeidstegning; }
            set { SetPropertyValue(nameof(Arbeidstegning), ref _Arbeidstegning, value); }
        }
        private bool _Overlevering;
        [XafDisplayName("Overlevering"), ToolTip("Overlevering")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Overlevering")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public bool Overlevering
        {
            get { return _Overlevering; }
            set { SetPropertyValue(nameof(Overlevering), ref _Overlevering, value); }
        }
        private BindingList<Phase> _Phases;
        private void EnsureNonPersistentObjectBindingList()
        {
            if (_Phases == null)
            {
                _Phases = new BindingList<Phase>();
            }
        }
        [NonPersistent]
        public BindingList<Phase> Phases
        {
            get
            {
                EnsureNonPersistentObjectBindingList();
                List<Phase> collection = this.Session.Query<Phase>().ToList();
                _Phases.Clear();

                if (Skisseprosjekt)
                {
                    Phase phase = collection.Where(x => x.Name == "Skisseprosjekt").FirstOrDefault();
                    if (phase != null)
                        _Phases.Add(collection.Where(x => x.Name == "Skisseprosjekt").FirstOrDefault());
                }
                if (Forprosjekt)
                {
                    Phase phase = collection.Where(x => x.Name == "Forprosjekt").FirstOrDefault();
                    if (phase != null)
                        _Phases.Add(collection.Where(x => x.Name == "Forprosjekt").FirstOrDefault());
                }
                if (Detaljprosjekt)
                {
                    Phase phase = collection.Where(x => x.Name == "Detaljprosjekt").FirstOrDefault();
                    if (phase != null)
                        _Phases.Add(collection.Where(x => x.Name == "Detaljprosjekt").FirstOrDefault());
                }
                if (Arbeidstegning)
                {
                    Phase phase = collection.Where(x => x.Name == "Arbeidstegning").FirstOrDefault();
                    if (phase != null)
                        _Phases.Add(collection.Where(x => x.Name == "Arbeidstegning").FirstOrDefault());
                };
                if (Overlevering)
                {
                    Phase phase = collection.Where(x => x.Name == "Overlevering").FirstOrDefault();
                    if (phase != null)
                        _Phases.Add(collection.Where(x => x.Name == "Overlevering").FirstOrDefault());
                }

                return _Phases;
            }
        }
        //        List<Phase> _Phases = new List<Phase>();
        //        //EnsureNonPersistentObjectBindingList();
        //        //_Phases.Clear();
        //        List<Phase> collection = this.Session.Query<Phase>().ToList();
        //        if (PhaseSkisseprosjekt)
        //        {
        //            Phase phase = collection.Where(x => x.Name == "Skisseprosjekt").FirstOrDefault();
        //            if(phase != null)
        //                _Phases.Add(collection.Where(x => x.Name == "Skisseprosjekt").FirstOrDefault());
        //        }
        //        if (PhaseForprosjekt)
        //        {
        //            Phase phase = collection.Where(x => x.Name == "Forprosjekt").FirstOrDefault();
        //            if (phase != null)
        //                _Phases.Add(collection.Where(x => x.Name == "Forprosjekt").FirstOrDefault());
        //        }
        //        if (PhaseDetaljprosjekt)
        //        {
        //            Phase phase = collection.Where(x => x.Name == "Detaljprosjekt").FirstOrDefault();
        //            if (phase != null)
        //                _Phases.Add(collection.Where(x => x.Name == "Detaljprosjekt").FirstOrDefault());
        //        }
        //        if (PhaseArbeidstegning)
        //        {
        //            Phase phase = collection.Where(x => x.Name == "Arbeidstegning").FirstOrDefault();
        //            if (phase != null)
        //                _Phases.Add(collection.Where(x => x.Name == "Arbeidstegning").FirstOrDefault());
        //        };
        //        if (PhaseOverlevering)
        //        {
        //            Phase phase = collection.Where(x => x.Name == "Overlevering").FirstOrDefault();
        //            if (phase != null)
        //                _Phases.Add(collection.Where(x => x.Name == "Overlevering").FirstOrDefault());
        //        }
        //        XPCollection<Phase> phases = new XPCollection<Phase>(this.Session, _Phases);
        //        return phases;
        //    }
        //}
        //[NonPersistent]
        //public XPCollection<Phase> Phases2
        //{
        //    get
        //    {
        //        XPCollection < Phase > phases = new XPCollection<Phase>(this.Session);
        //        if (PhaseSkisseprosjekt)
        //            phases.Add(Phase.Skisseprosjekt);
        //        if (PhaseForprosjekt)
        //            phases.Add(Phase.Forprosjekt);
        //        if (PhaseDetaljprosjekt)
        //            phases.Add(Phase.Detaljprosjekt);
        //        if (PhaseArbeidstegning)
        //            phases.Add(Phase.Arbeidstegning);
        //        if (PhaseOverlevering)
        //            phases.Add(Phase.Overlevering);
        //        return phases;
        //    }
        //}
        //[NonPersistent]
        //[XafDisplayName("Skisseprosjekt"), ToolTip("Skisseprosjekt")]
        //public bool Skisseprosjekt
        //{
        //    get
        //    {
        //        if (IsLoading)
        //            return false;
        //        return Phases.Where(x => x.Name == "Skisseprosjekt").Count() > 0;
        //    }
        //    set
        //    {
        //        if (IsLoading)
        //            return;
        //        Phase phase = Phases.Where(x => x.Name == "Skisseprosjekt").FirstOrDefault();
        //        if (value == false)
        //        {
        //            if (phase != null && Phases.Contains(phase))
        //            {
        //                Phases.Remove(phase);
        //            }
        //        }
        //        else
        //        {
        //            if (Phases.Contains(phase))
        //                return;
        //            Phase pasedef = this.Session.Query<Phase>().Where(x => x.Name == "Skisseprosjekt").FirstOrDefault();
        //            if (pasedef == null)
        //            {
        //                pasedef = new Phase(this.Session) { Name = "Skisseprosjekt" };
        //            }
        //            Phases.Add(pasedef);
        //        }
        //    }
        //}
        //[NonPersistent]
        //[XafDisplayName("Forprosjekt"), ToolTip("Forprosjekt")]
        //public bool Forprosjekt
        //{
        //    get
        //    {
        //        if (IsLoading)
        //            return false;
        //        return Phases.Where(x => x.Name == "Forprosjekt").Count() > 0;
        //    }
        //    set
        //    {
        //        if (IsLoading)
        //            return;
        //        Phase phase = Phases.Where(x => x.Name == "Forprosjekt").FirstOrDefault();
        //        if (value == false)
        //        {
        //            if (phase != null)
        //                if (phase != null && Phases.Contains(phase))
        //                {
        //                    Phases.Remove(phase);
        //                }
        //        }
        //        else
        //        {
        //            if (Phases.Contains(phase))
        //                return;
        //            Phase pasedef = this.Session.Query<Phase>().Where(x => x.Name == "Forprosjekt").FirstOrDefault();
        //            if (pasedef == null)
        //            {
        //                pasedef = pasedef = new Phase(this.Session) { Name = "Forprosjekt" };
        //            }
        //            Phases.Add(pasedef);
        //        }

        //    }
        //}
        //[NonPersistent]
        //[XafDisplayName("Detaljprosjekt"), ToolTip("Detaljprosjekt")]
        //public bool Detaljprosjekt
        //{
        //    get
        //    {
        //        if (IsLoading)
        //            return false;
        //        return Phases.Where(x => x.Name == "Detaljprosjekt").Count() > 0;
        //    }
        //    set
        //    {
        //        if (IsLoading)
        //            return;
        //        Phase phase = this.Phases.Where(x => x.Name == "Detaljprosjekt").FirstOrDefault();
        //        if (value == false)
        //        {
        //            if (phase != null)
        //                if (phase != null && Phases.Contains(phase))
        //                {
        //                    Phases.Remove(phase);
        //                }
        //        }
        //        else
        //        {
        //            if (Phases.Contains(phase))
        //                return;
        //            Phase pasedef = this.Session.Query<Phase>().Where(x => x.Name == "Detaljprosjekt").FirstOrDefault();
        //            if (pasedef == null)
        //            {
        //                pasedef = pasedef = new Phase(this.Session) { Name = "Detaljprosjekt" };
        //            }
        //            Phases.Add(pasedef);
        //        }
        //    }
        //}
        //[NonPersistent]
        //[XafDisplayName("Arbeidstegning"), ToolTip("Arbeidstegning")]
        //public bool Arbeidstegning
        //{
        //    get
        //    {
        //        if (IsLoading)
        //            return false;
        //        return Phases.Where(x => x.Name == "Arbeidstegning").Count() > 0;
        //    }
        //    set
        //    {
        //        if (IsLoading)
        //            return;
        //        Phase phase = Phases.Where(x => x.Name == "Arbeidstegning").FirstOrDefault();
        //        if (value == false)
        //        {

        //            if (phase != null)
        //                if (phase != null && Phases.Contains(phase))
        //                {
        //                    Phases.Remove(phase);
        //                }
        //        }
        //        else
        //        {
        //            if (Phases.Contains(phase))
        //                return;
        //            Phase pasedef = this.Session.Query<Phase>().Where(x => x.Name == "Arbeidstegning").FirstOrDefault();
        //            if (pasedef == null)
        //            {
        //                pasedef = pasedef = new Phase(this.Session) { Name = "Arbeidstegning" };
        //            }
        //            Phases.Add(pasedef);
        //        }


        //    }
        //}
        //[NonPersistent]
        //[XafDisplayName("Overlevering"), ToolTip("Overlevering")]
        //public bool Overlevering
        //{
        //    get
        //    {
        //        if (IsLoading)
        //            return false;
        //        return Phases.Where(x => x.Name == "Overlevering").Count() > 0;
        //    }
        //    set
        //    {
        //        if (IsLoading)
        //            return;
        //        Phase phase = Phases.Where(x => x.Name == "Overlevering").FirstOrDefault();
        //        if (value == false)
        //        {
        //            if (phase != null)
        //                if (phase != null && Phases.Contains(phase))
        //                {
        //                    Phases.Remove(phase);
        //                }
        //        }
        //        else
        //        {
        //            if (Phases.Contains(phase))
        //                return;
        //            Phase pasedef = this.Session.Query<Phase>().Where(x => x.Name == "Overlevering").FirstOrDefault();
        //            if (pasedef == null)
        //            {
        //                pasedef = pasedef = new Phase(this.Session) { Name = "Overlevering" };;
        //            }
        //            Phases.Add(pasedef);
        //        }
        //    }
        //}
        [Association("Properties-PSets")]
        [XafDisplayName("Property Sets")]
        public XPCollection<PropertySet> PropertySets
        {
            get { return GetCollection<PropertySet>(nameof(PropertySets)); }
        }
        [Browsable(false)]
        public string[] PropertySetIds
        {
            get
            {
                return this.PropertySets?.Select(x => x?.Oid.ToString()).ToArray();
            }
        }
        [Association("Properties-RevitCategories")]
        [XafDisplayName("Revit Categories")]
        public XPCollection<RevitCategory> RevitCategories
        {
            get
            {
                return GetCollection<RevitCategory>(nameof(RevitCategories));
            }
        }
        [Browsable(false)]
        public List<Discipline> GetDisciplines()
        {
            return this.RevitCategories.SelectMany(x => x.Disciplines).Distinct().ToList();
        }
        [Browsable(false)]
        public bool ContainsDiscipline(Discipline discipline)
        { 
            var x = this.RevitCategories.Where(x => x.Disciplines.Contains(discipline)).FirstOrDefault();
            return  x != null;
        }
        [Browsable(false)]
        [NonPersistent]
        public string[] RevitCategoriesIds
        {
            get
            {
                return this.RevitCategories?.Select(x => x?.Oid.ToString()).ToArray();
            }
        }
        public XPCollection<Project> Projects
        {
            get
            {
                List<Project> result = this.Properties.Select(x => x.Project)?.Distinct()?.ToList();
                XPCollection <Project> projects = new(this.Session, result);
                return projects;
            }
        }
        //[XafDisplayName("Element"), ToolTip("Revit Category")]
        //public string RevitCategoriesReadOnly
        //{
        //    get
        //    {
        //        return string.Join(';',this.RevitCategories.Select(x => x.Name));
        //    }
        //}

        //[Association("Properties-Phases")]
        //[XafDisplayName("Phases")]
        //[Browsable(false)]
        //public XPCollection<Phase> Phases
        //{
        //    get { return GetCollection<Phase>(nameof(Phases)); }
        //}
        //[Browsable(false)]
        //public string[] PhaseIds
        //{
        //    get
        //    {
        //        return this.Phases?.Select(x => x?.Oid.ToString()).ToArray();
        //    }
        //}
        //...
        [Association("Property-PropertyInstances")]
        public XPCollection<PropertyInstance> Properties
        {
            get
            {
                return GetCollection<PropertyInstance>(nameof(Properties));
            }
        }

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}