using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BimLookup.Module.Controllers
{
    /// <summary>
    /// Placeholder för sttings Detail View Controller
    /// </summary>
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SettingsViewController : ViewController
    {
        ActionUrl urlAction1;
        public SettingsViewController()
        {
            InitializeComponent();
            TargetViewId = "AppSettings_DetailView";
            //urlAction = new ActionUrl(this, "Redirect", "RecordEdit");
            //urlAction.OpenInNewWindow = false;
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //this.urlAction = new DevExpress.ExpressApp.Actions.ActionUrl(this.components);
            //this.urlAction.UrlFieldName = "HyperlinkAPI";
            //this.urlAction.UrlFormatString = "{0}";
            //this.urlAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            //this.urlAction.Category = "RecordEdit";


            //this.urlAction1 = new DevExpress.ExpressApp.Actions.ActionUrl(this.components);
            //this.urlAction1.UrlFormatString = "http://www.yahoo.com";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            //ViewShortcut shortcut = new ViewShortcut("DomainObject2_DetailView", ObjectSpace.FindObject<DomainObject2>(new BinaryOperator("Name", "1")).Oid);
            //if(urlAction == null)
            //    urlAction = new ActionUrl(this, "Redirect", "RecordEdit");
            //urlAction.UrlFormatString = "*/api/index.html";
                //Use a solution from the https://www.devexpress.com/Support/Center/Question/Details/T344730 thread to obtain the View's URL.  
            // Perform various tasks depending on the target View.
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
