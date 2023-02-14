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
    [ImageName("BO_User_32x32")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Owner : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Owner(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
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

        [Association("Owner-Properties")]
        public XPCollection<Property> Properties
        {
            get
            {
                return GetCollection<Property>(nameof(Properties));
            }
        }
        [Association("Owner-PropertyInstances")]
        public XPCollection<PropertyInstance> PropertyInstances
        {
            get
            {
                return GetCollection<PropertyInstance>(nameof(PropertyInstances));
            }
        }
        [Browsable(false)]
        public string[] PropertyIds
        {
            get
            {
                return this.Properties?.Select(x => x?.Oid.ToString()).ToArray();
            }
        }
        [Browsable(false)]
        public string[] PropertyInstanceIds
        {
            get
            {
                return this.PropertyInstances?.Select(x => x?.Oid.ToString()).ToArray();
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}