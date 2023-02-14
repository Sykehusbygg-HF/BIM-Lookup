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
    [ImageName("BO_Price_Item_32x32")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PropertyGroup : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        // Use CodeRush to create XPO classes and properties with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public PropertyGroup(Session session)
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
        private bool _includeInBalancing;
        [XafDisplayName("Include In Balancing"), ToolTip("Include Properties in this group in Balancing")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        [Persistent("IncludeInBalancing")]//, RuleRequiredField(DefaultContexts.Save)]
        public bool IncludeInBalancing
        {
            get { return _includeInBalancing; }
            set { SetPropertyValue(nameof(IncludeInBalancing), ref _includeInBalancing, value); }
        }
        [Association("PropertyGroup-Properties")]
        public XPCollection<Property> Properties
        {
            get
            {
                return GetCollection<Property>(nameof(Properties));
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

        public override string ToString()
        {
            if (Name == null)
                return string.Empty;
            return Name;
        }
    }
}