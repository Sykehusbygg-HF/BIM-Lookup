using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BimLookup.Module.Classes;

namespace BimLookup.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Note_32x32")]
    [Category("BIM Catalog")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("PropertyInstance")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PropertyInstance : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PropertyInstance(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if(Created == null)
            {
                Created = DateTime.Now;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {

        }
        //TODO: Investigate ExpandObjectMembers [ExpandObjectMembers(ExpandObjectMembers.Always)]
        private Property property;
        [Association("Property-PropertyInstances")]
        [ImmediatePostData]
        public Property Property
        {
            get { return property; }
            set { 
                if(SetPropertyValue(nameof(Property), ref property, value))
                {
                    if (property != null)
                    {
                        this.Skisseprosjekt = property.Skisseprosjekt;
                        this.Arbeidstegning = property.Arbeidstegning;
                        this.Forprosjekt = property.Forprosjekt;
                        this.Detaljprosjekt = property.Detaljprosjekt;
                        this.Overlevering = property.Overlevering;
                        this.Owner = property.Owner;
                        OnPropertyChanged();
                    }
                }
            }
        }
        //public BindingList<Phase> Phases
        //{
        //    get
        //    {
        //        BindingList<Phase> phases = new BindingList<Phase>();
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
        //Task 41, Unnecessary info since Propert Shows the name...
        [Browsable(false)]
        [NonPersistent]
        public string Name
        {
            get { return Property?.Name ?? string.Empty; }
            set { }
        
        }
        [NonPersistent]
        [XafDisplayName("Property Set"), ToolTip("Property Set")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        public string PropertySetDisplayValue
        {
            get {
            
                if(this.Property != null && this.Property?.PropertySets != null) 
                    return string.Join(",", this.Property?.PropertySets); 
                return string.Empty;
            }
        }
        [NonPersistent]
        public int Version
        {
            get { return Property?.Version ?? 0; }
            set { }
        }
        [NonPersistent]
        [Browsable(false)]
        public string Guid
        {
            get { return Property?.Guid; }
            set { }
        }
        [NonPersistent]
        public IfcPropertyType IfcPropertyType
        {
            get { return Property?.IfcPropertyType ?? null; }
            set { }
        }
        public RevitPropertyType RevitPropertyType
        {
            get { return Property?.RevitPropertyType ?? null; }
            set { }
        }
        [Browsable(false)]
        [NonPersistent]
        public string RevitPropertyTypeName
        {
            get { return Property?.RevitPropertyType?.Name ?? String.Empty; }
            set { }
        }

        [XafDisplayName("Type/Instance")]
        [NonPersistent]
        public Property.TypeInstanceEnum Type_Instance
        {
            get { return Property?.Type_Instance ?? Property.TypeInstanceEnum.Instance; }
            set { }
        }
        [Browsable(false)]
        [NonPersistent]
        public string Type_InstanceName
        {
            get { 
                if(Type_Instance == Property.TypeInstanceEnum.Instance)    
                return "Instance";
                return "Type";
            }
            set { }
        }

        public PropertyGroup PropertyGroup
        {
            get { return Property?.PropertyGroup ?? null; }
            set { }
        }
        [Browsable(false)]
        [NonPersistent]
        public string PropertyGroupName
        {
            get { return Property?.PropertyGroup?.Name ?? String.Empty; }
            set { }
        }
        [NonPersistent]
        public string Description
        {
            get { return Property?.Description ?? string.Empty; }
            set { }
        }
        [NonPersistent]
        public string Comment
        {
            get { return Property?.Comment ?? string.Empty; }
            set { }
        }
        private string _ProjectComment;
        [XafDisplayName("Project Comment"), ToolTip("Project Comment")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("ProjectComment")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public string ProjectComment
        {
            get { return _ProjectComment; }
            set { SetPropertyValue(nameof(ProjectComment), ref _ProjectComment, value); }
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
        private bool _PhaseArbeidstegning;
        [XafDisplayName("Arbeidstegning"), ToolTip("Arbeidstegning")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("Arbeidstegning")]//, RuleRequiredField(DefaultContexts.Save)]
        [Size(SizeAttribute.Unlimited)]
        public bool Arbeidstegning
        {
            get { return _PhaseArbeidstegning; }
            set { SetPropertyValue(nameof(Arbeidstegning), ref _PhaseArbeidstegning, value); }
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
        
        private DateTime? _Created;
        //[ReadOnly(true)]
        [XafDisplayName("Created"), ToolTip("Created")]
        [Persistent("Created")]//, RuleRequiredField(DefaultContexts.Save)]
        public DateTime? Created
        {
            get { return _Created; }
            set { SetPropertyValue(nameof(Created), ref _Created, value); }
        }
        //[Association("Properties-PSets")]
        //[XafDisplayName("Property Sets")]
        //public XPCollection<PSet> PropertySets
        //{
        //    get { return GetCollection<PSet>(nameof(PropertySets)); }
        //}

        //...
        private Project project;
        [Association("Project-PropertyInstances")]
        public Project Project
        {
            get { return project; }
            set { SetPropertyValue(nameof(Project), ref project, value); }
        }
        private Owner _owner;
        [XafDisplayName("Owner"), ToolTip("Owner")]
        [Persistent("Owner")]
        [Association("Owner-PropertyInstances")]
        public Owner Owner
        {
            get { return _owner; }
            set { SetPropertyValue(nameof(Owner), ref _owner, value); }
        }
        [Browsable(false)]
        [NonPersistent]
        public string PropertyId
        {
            get
            {
                return this.Property?.Oid.ToString();
            }
            set
            {

            }
        }
        [Browsable(false)]
        [NonPersistent]
        public string OwnerId
        {
            get
            {
                return this.Owner?.Oid.ToString();
            }
            set
            {

            }
        }
        [Browsable(false)]
        public bool InPhase(string phase)
        {
            //TODO: Check if phase is true...
            switch (phase.ToLower())
            {
                case "Skisseprosjekt":
                    return Skisseprosjekt;
                case "Forprosjekt":
                    return Forprosjekt;
                case "Detaljprosjekt":
                    return Detaljprosjekt;
                case "Arbeidstegning":
                    return Arbeidstegning;
                case "Overlevering":
                    return Overlevering;
            }
            return false;
        }
        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static string Serialize(List<PropertyInstance> pis)
        {
            JsonSerializerSettings sett = new JsonSerializerSettings() {  ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None, MaxDepth = 1, MetadataPropertyHandling = MetadataPropertyHandling.Ignore };
            return JsonConvert.SerializeObject(pis, sett);
        }
        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}