using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;

namespace BimLookup.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class BimLookupModule : ModuleBase {
    public BimLookupModule() {
		// 
		// BimLookupModule
		// 
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
		AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
		RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
#if REPORTS
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
#endif
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
        return new ModuleUpdater[] { updater };
    }
    public override void Setup(XafApplication application) {
        base.Setup(application);
        // Manage various aspects of the application UI and behavior at the module level.
            //https://docs.devexpress.com/eXpressAppFramework/116106/business-model-design-orm/non-persistent-objects/how-to-show-persistent-objects-in-a-non-persistent-objects-view
            //In the event handler, call the PopulateAdditionalObjectSpaces(XafApplication) method to populate the AdditionalObjectSpaces collection with an additional Object Space that can handle the Person type.
            application.ObjectSpaceCreated += Application_ObjectSpaceCreated;

        }
        private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
        {
            CompositeObjectSpace compositeObjectSpace = e.ObjectSpace as CompositeObjectSpace;
            if (compositeObjectSpace != null)
            {
                if (!(compositeObjectSpace.Owner is CompositeObjectSpace))
                {
                    compositeObjectSpace.PopulateAdditionalObjectSpaces((XafApplication)sender);
                }
            }
            BaseObjectSpace os = e.ObjectSpace as BaseObjectSpace;
            if (os != null)
                os.NonPersistentChangesEnabled = true;
        }
    public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
        base.CustomizeTypesInfo(typesInfo);
        CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
    }
}
