using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2.Blazor;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BimLookup.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AccessReportDesignerController : ViewController<DetailView>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public AccessReportDesignerController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            this.TargetViewId = ReportsBlazorModuleV2.ReportDesignerDetailViewName;

        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            View.CustomizeViewItemControl<ReportDesignerViewItem>(this, CustomizeDesignerViewItem);
        }
        private void CustomizeDesignerViewItem(ReportDesignerViewItem designerDetailItem)
        {
            // Access the Report Viewer properties
            string reportName = ((ReportDesignerViewItem.DxReportDesignerAdapter)designerDetailItem.Control).ComponentModel.ReportName;
            // ...
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
