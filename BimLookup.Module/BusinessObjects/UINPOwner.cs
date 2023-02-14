using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BimLookup.Module.BusinessObjects
{
    [DomainComponent]
    [DisplayName("Select Owner")]
    public class UINPOwner: NonPersistentBaseObject
    {
        Owner owner = null;
        [XafDisplayName("Owner"), ToolTip("Owner")]
        public Owner Owner
        {
            get { return owner; }
            set { SetPropertyValue(ref owner, value); }
        }
    }
}
