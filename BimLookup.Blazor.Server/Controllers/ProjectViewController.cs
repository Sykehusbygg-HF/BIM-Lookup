using BimLookup.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Grid;
using DevExpress.ExpressApp.Blazor.Editors.Models;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BimLookup.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ProjectViewController : ViewController
    {
        ArrayList _SelectedProperties = new ArrayList();
        ArrayList _SelectedProjects = new ArrayList();
        Project _project;

        IObjectSpace _balancingObjectSpace;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ProjectViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //TargetViewType = ViewType.DetailView;
            TargetObjectType = typeof(Project);

            ListView lv = this.View as ListView;


            #region Dialog Controller with selection
            SimpleAction setBalaneringAction = new SimpleAction(this, "Add Properties...", PredefinedCategory.ObjectsCreation);
            setBalaneringAction.ToolTip = "Balance the project properties to the project by selecting properties...";
            setBalaneringAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            setBalaneringAction.ImageName = "Action_Filter";
            setBalaneringAction.Execute += SetBalaneringAction_Execute;
            setBalaneringAction.TargetViewNesting = Nesting.Root;
            #endregion
            #region Copy Project
            SimpleAction setFromProjectAction = new SimpleAction(this, "Import...", PredefinedCategory.ObjectsCreation);
            setFromProjectAction.ToolTip = "Balance the project by copying the property settings from another project ...";
            setFromProjectAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            setFromProjectAction.ImageName = "Action_Filter";
            setFromProjectAction.Execute += SetFromProjectAction_Execute;
            setFromProjectAction.TargetViewNesting = Nesting.Root;
            #endregion

        }
        protected override void OnViewChanged()
        {
            base.OnViewChanged();
            //Hide Controllers in Speccific View
            this.Active["CustomViewSelector"] = View != null && View.Id != "Project_ListView_Select";
        }

        private void SetFromProjectAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            _project = (Project)e.CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Project));

            //List All or Some
            string listViewId = "Project_ListView_Select"; ;// Application.FindListViewId(typeof(Property));
            if (Application.FindModelView(listViewId) == null)
            {
                listViewId = Application.FindListViewId(typeof(Project));
            }
            CollectionSourceBase csbase = Application.CreateCollectionSource(objectSpace, typeof(Project), listViewId);
            e.ShowViewParameters.CreatedView = Application.CreateListView(
              listViewId,
              csbase,
              true);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController selectProjectDialog = Application.CreateController<DialogController>();
            e.ShowViewParameters.Controllers.Add(selectProjectDialog);
            e.ShowViewParameters.CreatedView.ControlsCreated += SelectProjectDialog_ControlsCreated;
            selectProjectDialog.Accepting += SelectProjectDialog_Accepting;
        }

        private void SelectProjectDialog_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            _SelectedProjects = new ArrayList();
            if ((e.AcceptActionArgs.SelectedObjects.Count > 0) && (e.AcceptActionArgs.SelectedObjects[0] is IObjectRecord))
            {
                foreach (var selectedObject in e.AcceptActionArgs.SelectedObjects)
                {
                    _SelectedProjects.Add((Project)ObjectSpace.GetObject(selectedObject));
                }
            }
            else
            {
                _SelectedProjects = (ArrayList)e.AcceptActionArgs.SelectedObjects;
            }
            if (_SelectedProjects.Count == 0)
                return;
            foreach (Project proj in _SelectedProjects)
            {
                foreach (PropertyInstance pi in proj.Properties)
                {
                    if (pi.Property == null)
                        continue;
                    //If property allready included
                    PropertyInstance pinst = _project.Properties.Where(x => x.Property != null && x.Property.Oid == pi.Property.Oid).FirstOrDefault();
                    if(pinst == null)
                    {
                        pinst = ObjectSpace.CreateObject<PropertyInstance>();
                        Property propco = ObjectSpace.GetObject<Property>(pi.Property);
                        pinst.Property = propco;
                        _project.Properties.Add(pinst);
                    }
                    Owner owner = ObjectSpace.GetObject<Owner>(pi.Owner);
                    //Copy if overridden props
                    pinst.Arbeidstegning = pi.Arbeidstegning;
                    pinst.Detaljprosjekt = pi.Detaljprosjekt;
                    pinst.Skisseprosjekt = pi.Skisseprosjekt;
                    pinst.Overlevering = pi.Overlevering;
                    pinst.Forprosjekt = pi.Forprosjekt;
                    pinst.Owner = owner;
                }
                ObjectSpace.CommitChanges();
            }
        }

        private void SelectProjectDialog_ControlsCreated(object sender, EventArgs e)
        {
            ////Preselect existing props
            //ListView lv = sender as ListView;
            //if (lv == null)
            //    return;
            //GridListEditor le = lv.Editor as GridListEditor;
            //if (le == null)
            //    return;
        }

        private void SetBalaneringAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            _project = (Project)e.CurrentObject;
            _balancingObjectSpace = Application.CreateObjectSpace(typeof(Property));

            //List All or Some
            string listViewId = "Property_ListView_GroupedByPropertyGroup";// Application.FindListViewId(typeof(Property));
            if (Application.FindModelView(listViewId) == null)
            {
                listViewId = Application.FindListViewId(typeof(Property));
            }
            CollectionSourceBase csbase = Application.CreateCollectionSource(_balancingObjectSpace, typeof(Property), listViewId);

            //Just show from selected sets
            CriteriaOperator crit = null;
            //crit = CriteriaOperator.FromLambda<Property>(x => x != null && x.PropertyGroup != null && x.PropertyGroup.IncludeInBalancing == true);
            if (_project.Existing)
            {
                crit = CriteriaOperator.FromLambda<Property>(x => x.IncludeInBalancing == true);
            }
            else //Just show latest version
            {
                crit = CriteriaOperator.FromLambda<Property>(x => x.IncludeInBalancing == true && x.Latest);
            }
            //Only works for grid view not DxGridView!!!           
            csbase.SetCriteria("criteriaJustFromSpeccificPropertyGroups", crit.LegacyToString());
            e.ShowViewParameters.CreatedView = Application.CreateListView(
               listViewId,
               csbase,
               true);

            e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
            DialogController selectParametersDialog = Application.CreateController<DialogController>();
            e.ShowViewParameters.Controllers.Add(selectParametersDialog);
            e.ShowViewParameters.CreatedView.ControlsCreated += CreatedView_ControlsCreated;
            selectParametersDialog.Accepting += SelectParametersDialog_Accepting;

        }

        private void CreatedView_ControlsCreated(object sender, EventArgs e)
        {
            //Preselect existing props
            ListView lv = sender as ListView;
            if (lv == null)
                return;
            GridListEditor le = lv.Editor as GridListEditor;
            DxGridListEditor ledx = lv.Editor as DxGridListEditor;
            if (le == null && ledx == null)
                return;

            List<Property> props = _project.Properties.Select(x => x.Property).ToList();

            var list = props.Select(x => x.Oid).ToList();

            if (le != null)
                le.SelectObjects(props);
            if (ledx != null)
            {
                if (_balancingObjectSpace != null)
                {
                    var props2 = _balancingObjectSpace.GetObjects<Property>().Where(x => list.Contains(x.Oid)).ToList();
                    ledx.SetSelectedObjects(props2);
                }
            }
        }

        private void Ledx_DataSourceChanged(object sender, EventArgs e)
        {
            
        }

        private void SelectParametersDialog_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            _SelectedProperties = new ArrayList();
            if ((e.AcceptActionArgs.SelectedObjects.Count > 0) && (e.AcceptActionArgs.SelectedObjects[0] is IObjectRecord))
            {
                foreach (var selectedObject in e.AcceptActionArgs.SelectedObjects)
                {
                    _SelectedProperties.Add((Property)ObjectSpace.GetObject(selectedObject));
                }
            }
            else
            {
                _SelectedProperties = (ArrayList)e.AcceptActionArgs.SelectedObjects;
            }
            if (_SelectedProperties.Count == 0)
                return;
            foreach (Property prop in _SelectedProperties)
            {
                //Does the property allready exist on the project?
                PropertyInstance pi = _project.Properties.Where(x => x.Property.Oid == prop.Oid).FirstOrDefault();
                if (pi != null)
                    continue;
                pi = ObjectSpace.CreateObject<PropertyInstance>();
                Property propco = ObjectSpace.GetObject<Property>(prop);
                pi.Property = propco;
                _project.Properties.Add(pi);
            }
            //Remove all old?
            List<Property> props = _SelectedProperties.Cast<Property>().ToList();
            for (int i = _project.Properties.Count - 1; i >= 0; i--)
            {
                PropertyInstance pi = _project.Properties[i];
                // some code
                if (props.Where(x => x.Oid == pi.Property.Oid).FirstOrDefault() == null)
                {
                    _project.Properties.Remove(pi);
                    pi.Delete();
                }
            }
            ObjectSpace.CommitChanges();
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
