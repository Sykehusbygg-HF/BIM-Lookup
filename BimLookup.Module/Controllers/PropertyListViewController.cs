using BimLookup.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Office.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BimLookup.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PropertyListViewController : ViewController
    {
        UINPPhase _phase;
        UINPOwner _owner;
        ArrayList _SelectedProperties = new ArrayList();
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public PropertyListViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //TargetViewType = ViewType.ListView;
            TargetObjectType = typeof(Property);

            #region Version
            PopupWindowShowAction versionPropertyAction = new PopupWindowShowAction(this, "VersionPropertyAction", PredefinedCategory.View)
            {
                Caption = "Version...",
                ConfirmationMessage = "Are you sure you want to create a new Version of this property?",
                ImageName = "Action_Copy"
            };
            versionPropertyAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            versionPropertyAction.TargetObjectsCriteria = "Not IsNewObject(This)";
            versionPropertyAction.CustomizePopupWindowParams += VersionPropertyAction_CustomizePopupWindowParams;
            versionPropertyAction.TargetViewType = ViewType.Any;
            #endregion
            #region ReSave
#if DEBUG
            SimpleAction savePropertiesAction = new SimpleAction(this, "SaveAllProperties", PredefinedCategory.View)
            {
                Caption = "Save Selected Properties",
                ConfirmationMessage = "Are you sure you want to Save Selected Properties?",
                ImageName = "Action_Save"
            };
            savePropertiesAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            savePropertiesAction.TargetObjectsCriteria = "Not IsNewObject(This)";
            savePropertiesAction.Execute += SavePropertiesAction_Execute;
            savePropertiesAction.TargetViewType = ViewType.ListView;
#endif
            #endregion
            #region Test Dialog Controller with selection


            SimpleAction setMultipleObjectsPhasesAction = new SimpleAction(this, "Set Phases...", PredefinedCategory.ObjectsCreation);
            setMultipleObjectsPhasesAction.ToolTip = "Update Phase on multiple objects at once...";
            setMultipleObjectsPhasesAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            setMultipleObjectsPhasesAction.ImageName = "ModelEditor_Categorized";
            setMultipleObjectsPhasesAction.Execute += SetMultipleObjectsPhasesAction_Execute;
            setMultipleObjectsPhasesAction.TargetViewNesting = Nesting.Root;
            setMultipleObjectsPhasesAction.TargetViewType = ViewType.ListView;

            SimpleAction setMultipleObjectsOwnerAction = new SimpleAction(this, "Set Owner...", PredefinedCategory.ObjectsCreation);
            setMultipleObjectsOwnerAction.ToolTip = "Update Owner on multiple objects at once...";
            setMultipleObjectsOwnerAction.SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects;
            setMultipleObjectsOwnerAction.ImageName = "ModelEditor_Categorized";
            setMultipleObjectsOwnerAction.Execute += SetMultipleObjectsOwnerAction_Execute;
            setMultipleObjectsOwnerAction.TargetViewNesting = Nesting.Root;
            setMultipleObjectsOwnerAction.TargetViewType = ViewType.ListView;
            #endregion

        }

        private void SetMultipleObjectsOwnerAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            _SelectedProperties = new ArrayList();
            if ((e.SelectedObjects.Count > 0) && (e.SelectedObjects[0] is IObjectRecord))
            {
                foreach (var selectedObject in e.SelectedObjects)
                {
                    _SelectedProperties.Add((Property)ObjectSpace.GetObject(selectedObject));
                }
            }
            else
            {
                _SelectedProperties = (ArrayList)e.SelectedObjects;
            }
            if (_SelectedProperties.Count == 0)
                return;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(UINPOwner));
            _owner = objectSpace.CreateObject<UINPOwner>();
            Property templateprop = (Property)_SelectedProperties[0];
            _owner.Owner = templateprop.Owner;
            string detailViewId = Application.FindDetailViewId(typeof(UINPOwner));
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, _owner, View);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController selectOwnerDialog = Application.CreateController<DialogController>();
            e.ShowViewParameters.Controllers.Add(selectOwnerDialog);
            selectOwnerDialog.Accepting += SelectOwnerDialog_Accepting;
        }

        private void SelectOwnerDialog_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            if (_owner == null)
                return;

            Owner own = ObjectSpace.GetObject(_owner.Owner);
            foreach (Property prop in _SelectedProperties)
            {
                prop.Owner = own;
            }
            ObjectSpace.CommitChanges();
        }

        protected override void OnViewChanged()
        {
            base.OnViewChanged();
            //Hide Controllers in Speccific View
            this.Active["CustomViewSelector"] = View != null && View.Id != "Property_ListView_GroupedByPropertyGroup";
        }

        private void SetMultipleObjectsPhasesAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            _SelectedProperties = new ArrayList();
            if ((e.SelectedObjects.Count > 0) && (e.SelectedObjects[0] is IObjectRecord))
            {
                foreach (var selectedObject in e.SelectedObjects)
                {
                    _SelectedProperties.Add((Property)ObjectSpace.GetObject(selectedObject));
                }
            }
            else
            {
                _SelectedProperties = (ArrayList)e.SelectedObjects;
            }
            if (_SelectedProperties.Count == 0)
                return;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(UINPPhase));
            _phase = objectSpace.CreateObject<UINPPhase>();
            Property templateprop = (Property)_SelectedProperties[0];
            _phase.Skisseprosjekt = templateprop.Skisseprosjekt;
            _phase.Arbeidstegning = templateprop.Arbeidstegning;
            _phase.Detaljprosjekt = templateprop.Detaljprosjekt;
            _phase.Forprosjekt = templateprop.Forprosjekt;
            _phase.Overlevering = templateprop.Overlevering;

            string detailViewId = Application.FindDetailViewId(typeof(UINPPhase));
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, _phase, View);
            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController selectPhaseDialog = Application.CreateController<DialogController>();
            e.ShowViewParameters.Controllers.Add(selectPhaseDialog);
            selectPhaseDialog.Accepting += SelectPhaseDialog_Accepting;
        }

        private void SelectPhaseDialog_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            if(_phase == null)
                return;
            foreach (Property prop in _SelectedProperties)
            {
                prop.Skisseprosjekt = _phase.Skisseprosjekt;
                prop.Arbeidstegning = _phase.Arbeidstegning;
                prop.Detaljprosjekt = _phase.Detaljprosjekt;
                prop.Forprosjekt = _phase.Forprosjekt;
                prop.Overlevering = _phase.Overlevering;
                //ObjectSpace.SetModified(prop);
            }
            ObjectSpace.CommitChanges();
        }

        private void SavePropertiesAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            foreach (Property prop in View.SelectedObjects)
            {
                //prop.Latest = !prop.Latest;
                ObjectSpace.SetModified(prop);
                //prop.Save();
            }
            ObjectSpace.CommitChanges();
        }

        private void VersionPropertyAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            Property sourceprop = View.CurrentObject as Property;
            
            Property newprop = sourceprop.CreateNewVersion(ObjectSpace);
            ObjectSpace.CommitChanges();

            IObjectSpace newObjectSpace = Application.CreateObjectSpace(View.ObjectTypeInfo.Type);
            Object objectToShow = newObjectSpace.GetObject(newprop);
            if (objectToShow != null)
            {
                DetailView createdView = Application.CreateDetailView(newObjectSpace, objectToShow);
                createdView.ViewEditMode = ViewEditMode.Edit;
                e.View = createdView;
            }
        }

        private void VersionPropertyAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //TODO: Show New Version Detail View
        }

        protected override void OnActivated()
        {
            base.OnActivated();
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
