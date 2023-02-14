using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BimLookup.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Workflow_ShowWorkflowInstances_32x32")]
    [Category("BIM Catalog")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    [Persistent("PSet")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PropertySet : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PropertySet(Session session)
            : base(session)
        {
            //using (var uow = new UnitOfWork())
            //{
            //    var properties = uow.Query<Property>().ToList();
            //}
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
        [Association("Properties-PSets")]
        public XPCollection<Property> Properties
        {
            get { return GetCollection<Property>(nameof(Properties)); }
        }
        [Browsable(false)]
        public string[] PropertyIds
        {
            get
            {
                return this.Properties?.Select(x => x?.Oid.ToString()).ToArray();
            }
        }
        public override string ToString()
        {
            return this.Name;
        }
        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}