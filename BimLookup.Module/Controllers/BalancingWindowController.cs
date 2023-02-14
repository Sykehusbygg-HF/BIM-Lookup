using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Templates;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;


namespace BimLookup.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class BalancingWindowController : WindowController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            Window.TemplateChanged += Window_TemplateChanged;
        }
        private void Window_TemplateChanged(object sender, EventArgs e)
        {
            // Change the dimensions only for the View  
            // with Id set to "PermissionPolicyRole_DetailView".
            if (Window.Template is IPopupWindowTemplateSize size
                && Window.View.Id == "Property_ListView_GroupedByPropertyGroup")
            {
                size.MaxWidth = "100vw";
                size.Width = "1200px";
                size.MaxHeight = "100vh";
                size.Height = "800px";
            }
        }
        protected override void OnDeactivated()
        {
            Window.TemplateChanged -= Window_TemplateChanged;
            base.OnDeactivated();
        }


    }
}
