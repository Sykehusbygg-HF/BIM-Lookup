using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Google.Protobuf.WellKnownTypes;
using DevExpress.Persistent.Base;

namespace BimLookup.Module.BusinessObjects
{
    [DomainComponent]
    [DisplayName("Select Phases")]
    public class UINPPhase : NonPersistentBaseObject
    {
        public UINPPhase()
        {

        }
        bool skisseprojekt = false;
        [XafDisplayName("Skisseprosjekt"), ToolTip("Skisseprosjekt")]
        public bool Skisseprosjekt
        {
            get { return skisseprojekt; }
            set { SetPropertyValue(ref skisseprojekt, value); }
        }
        bool forprosjekt = false;
        [XafDisplayName("Forprosjekt"), ToolTip("Forprosjekt")]
        public bool Forprosjekt
        {
            get { return forprosjekt; }
            set { SetPropertyValue(ref forprosjekt, value); }
        }

        bool detaljprosjekt = false;
        [XafDisplayName("Detaljprosjekt"), ToolTip("Detaljprosjekt")]
        public bool Detaljprosjekt
        {
            get { return detaljprosjekt; }
            set { SetPropertyValue(ref detaljprosjekt, value); }
        }
        bool arbeidstegning = false;
        [XafDisplayName("Arbeidstegning"), ToolTip("Arbeidstegning")]
        public bool Arbeidstegning
        {
            get { return arbeidstegning; }
            set { SetPropertyValue(ref arbeidstegning, value); }
        }
        bool overlevering = false;
        [XafDisplayName("Overlevering"), ToolTip("Overlevering")]
        public bool Overlevering
        {
            get { return overlevering; }
            set { SetPropertyValue(ref overlevering, value); }
        }

    }
}
