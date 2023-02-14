using BimLookup.Module.BusinessObjects;
using DevExpress.Blazor;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Grid;
using DevExpress.ExpressApp.Blazor.Editors.Grid.Models;
using DevExpress.ExpressApp.Blazor.Editors.Models;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraRichEdit.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using static DevExpress.CodeParser.CodeStyle.Formatting.Rules;

namespace BimLookup.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BalancingViewController : ViewController<ListView>
    {
        SingleChoiceAction ownerFilterAction;
        //SingleChoiceAction phaseFilterAction;
        SingleChoiceAction disciplineFilterAction;

        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public BalancingViewController()
        {
            InitializeComponent();

            this.TargetViewId = "Property_ListView_GroupedByPropertyGroup";
            ownerFilterAction = new SingleChoiceAction(this, "OwnerFilter", "Filters");
            ownerFilterAction.Caption = "Owner";
            ownerFilterAction.ToolTip = "Owner";
            ownerFilterAction.Execute += FilterAction_Execute;

            //phaseFilterAction = new SingleChoiceAction(this, "PhaseFilter", "Filters");
            //phaseFilterAction.Caption = "Phase";
            //phaseFilterAction.ToolTip = "Phase";
            //phaseFilterAction.Execute += FilterAction_Execute;

            disciplineFilterAction = new SingleChoiceAction(this, "DisciplineFilter", "Filters");
            disciplineFilterAction.Caption = "Discipline";
            disciplineFilterAction.ToolTip = "Discipline";
            disciplineFilterAction.Execute += FilterAction_Execute;

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void FilterAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            ListView lv = View as ListView;
            if (lv == null)
                return;
            GridListEditor le = lv.Editor as GridListEditor;
            DxGridListEditor ledx = lv.Editor as DxGridListEditor;
            if (le == null && ledx == null)
                return;

            List<Property> selectedobjects = new List<Property>();
            if (le != null)
            {
                var selectedobjectsX = le.GetSelectedObjects();
                foreach (Property obj in selectedobjectsX)
                    selectedobjects.Add(obj);
            }
            if (ledx != null)
            {
               var selectedobjectsX = ledx.GetSelectedObjects();
                foreach (Property obj in selectedobjectsX)
                    selectedobjects.Add(obj);
            }

            //throw new NotImplementedException();
            object ownerid = ownerFilterAction.SelectedItem.Data;
            //object phaseid = phaseFilterAction.SelectedItem.Data;
            //string Phase = phaseFilterAction.SelectedItem.Caption;
            object disciplineid = disciplineFilterAction.SelectedItem.Data;

            if (ownerid != null)
            {
                Guid id = (Guid)ownerid;
                View.CollectionSource.Criteria["Owner"] = CriteriaOperator.FromLambda<Property>(x => x.Owner.Oid == id);
            }
            else
            {
                View.CollectionSource.Criteria["Owner"] = null;
            }
            if (disciplineid != null)
            {

                //TODO: Make this quicker
                Guid id = (Guid)disciplineid;
                Discipline dic = ObjectSpace.GetObjectByKey<Discipline>(id);
                var cats = ObjectSpace.GetObjects<RevitCategory>().Where(x=>x.Disciplines.Contains(dic)).ToList().Distinct();
                var props = cats.SelectMany(x => x.Properties.Select(y=>y.Oid)).ToList();

                if (dic != null)
                    View.CollectionSource.Criteria["Discipline"] = CriteriaOperator.FromLambda<Property>(x => props.Contains(x.Oid));
            }
            else
            {
                View.CollectionSource.Criteria["Discipline"] = null;
            }

            if (le != null)
                le.SelectObjects(selectedobjects);
            if (ledx != null)
            {
                ledx.SelectObjects(selectedobjects);
            }

        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ownerFilterAction.Items.Clear();
            ownerFilterAction.Items.Add(new ChoiceActionItem("Owner", null));
            foreach (Owner owner in ObjectSpace.GetObjects<Owner>())
            {
                ownerFilterAction.Items.Add(new ChoiceActionItem(owner.Name, owner.Oid));
            }
            ownerFilterAction.SelectedIndex = 0;
            //phaseFilterAction.Items.Clear();
            //phaseFilterAction.Items.Add(new ChoiceActionItem("All", null));
            //foreach (Phase phase in ObjectSpace.GetObjects<Phase>())
            //{
            //    phaseFilterAction.Items.Add(new ChoiceActionItem(phase.Name, phase.Oid));
            //}
            //phaseFilterAction.SelectedIndex = 0;
            disciplineFilterAction.Items.Clear();
            disciplineFilterAction.Items.Add(new ChoiceActionItem("Discipline", null));
            foreach (Discipline dis in ObjectSpace.GetObjects<Discipline>())
            {
                disciplineFilterAction.Items.Add(new ChoiceActionItem(dis.Name, dis.Oid));
            }
            disciplineFilterAction.SelectedIndex = 0;


            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            
            SetBalancingSpecials();
        }
        private void SetBalancingSpecials()
        {
            //if(View.Id == "Property_ListView_GroupedByPropertyGroup")
            //{
            //    if
            //}
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
