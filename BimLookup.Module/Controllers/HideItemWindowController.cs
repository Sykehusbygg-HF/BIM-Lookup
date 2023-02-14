using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security.Strategy;
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
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class HideItemWindowController : WindowController
    {
        private ShowNavigationItemController navigationController;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public HideItemWindowController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
        }
        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            navigationController = Frame.GetController<ShowNavigationItemController>();
            if (navigationController != null)
            {
                navigationController.ItemsInitialized += new EventHandler<EventArgs>(HideItemWindowController_ItemsInitialized);
            }
        }
        protected override void OnDeactivated()
        {
            if (navigationController != null)
            {
                navigationController.ItemsInitialized -= new EventHandler<EventArgs>(HideItemWindowController_ItemsInitialized);
                navigationController = null;
            }
            base.OnDeactivated();
        }
        private void HideItemWindowController_ItemsInitialized(object sender, EventArgs e)
        {
#if !DEBUG
            HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "Debug");
#endif
            //SecuritySystemUser currentUser = SecuritySystem.CurrentUser as SecuritySystemUser;
            //if (currentUser != null)
            //{
            //    foreach (SecuritySystemRole role in currentUser.Roles)
            //    {
            //        if (role.Name == "Users")
            //        {
            //            HideItemByCaption(navigationController.ShowNavigationItemAction.Items, "Reports");
            //        }
            //    }
            //}
        }
        private void HideItemByCaption(ChoiceActionItemCollection items, string navigationItemId)
        {
            foreach (ChoiceActionItem item in items)
            {
                if (item.Id == navigationItemId)
                {
                    item.Active["InactiveForUsersRole"] = false;
                    return;
                }
                HideItemByCaption(item.Items, navigationItemId);
            }
        }
    }
}
