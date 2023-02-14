using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace BimLookup.Module.BusinessObjects.DBViews
{

    public partial class PropertyBimKravView
    {
        public PropertyBimKravView(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
