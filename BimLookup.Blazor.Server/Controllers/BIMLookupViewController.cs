using BimLookup.Module.BusinessObjects;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
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

namespace BimLookup.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BIMLookupViewController : ViewController
    {
        SimpleAction action;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public BIMLookupViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewId = "Property_ListView_User";
            //action = new SimpleAction(this, "MyAction", "View");
            //action.Execute += action_Execute;

            ParametrizedAction filterActionProject = new ParametrizedAction(this, "Select Project", "Edit", typeof(string));
            filterActionProject.Caption = "Select Project";
            filterActionProject.ImageName = "Action_Search";
            filterActionProject.NullValuePrompt = "Select Project";

            ParametrizedAction filterActionPhase = new ParametrizedAction(this, "Select Phase", "Edit", typeof(string));
            filterActionPhase.Caption = "Select Phase";
            filterActionPhase.ImageName = "Action_Search";
            filterActionPhase.NullValuePrompt = "Select Phase";

            ParametrizedAction filterActionDiscipline = new ParametrizedAction(this, "Select Discipline", "Edit", typeof(string));
            filterActionDiscipline.Caption = "Select Discipline";
            filterActionDiscipline.ImageName = "Action_Search";
            filterActionDiscipline.NullValuePrompt = "Select Discipline";

            filterActionProject.Execute += new ParametrizedActionExecuteEventHandler(filterActionProject_Execute);
            filterActionPhase.Execute += new ParametrizedActionExecuteEventHandler(filterActionPhase_Execute);
            filterActionDiscipline.Execute += new ParametrizedActionExecuteEventHandler(filterActionDiscipline_Execute);
        }

        private void filterActionProject_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var objectType = ((ListView)View).ObjectTypeInfo.Type;
            //var objectType = typeof(Property);
            IObjectSpace objectSpace = Application.CreateObjectSpace(objectType);
            string paramValue = e.ParameterCurrentValue as string;
            if (!string.IsNullOrEmpty(paramValue))
                ((ListView)View).CollectionSource.Criteria["ProjectFilter"] = CriteriaOperator.Parse("[Projects][Contains([Name], ?)]", paramValue);
            else
                ((ListView)View).CollectionSource.SetCriteria("ProjectFilter", null);
        }
        private void filterActionPhase_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var objectType = ((ListView)View).ObjectTypeInfo.Type;
            //var objectType = typeof(Property);
            IObjectSpace objectSpace = Application.CreateObjectSpace(objectType);
            string paramValue = e.ParameterCurrentValue as string;
            if (!string.IsNullOrEmpty(paramValue))
                ((ListView)View).CollectionSource.Criteria["PhaseFilter"] = CriteriaOperator.Parse("[Phases][Contains([Name], ?)]", paramValue);
            else
                ((ListView)View).CollectionSource.SetCriteria("PhaseFilter", null);
        }
        private void filterActionDiscipline_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            var objectType = ((ListView)View).ObjectTypeInfo.Type;
            //var objectType = typeof(Property);
            IObjectSpace objectSpace = Application.CreateObjectSpace(objectType);
            string paramValue = e.ParameterCurrentValue as string;
            if (!string.IsNullOrEmpty(paramValue))
                ((ListView)View).CollectionSource.Criteria["DisciplineFilter"] = CriteriaOperator.Parse("[RevitCategories][[Disciplines][Contains([Name], ?)]]", paramValue);
            else
                ((ListView)View).CollectionSource.SetCriteria("DisciplineFilter", null);
        }

        //private void action_Execute(object sender, SimpleActionExecuteEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

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
