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
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BimLookup.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GenericListViewController : ViewController<ListView>
    {
        EventCallback<DevExpress.Blazor.GridRowClickEventArgs> _eventCallback;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public GenericListViewController()
        {
            InitializeComponent();

            //ownerFilterAction = new SingleChoiceAction(this, "OwnerFilter", "Filters");
            //ownerFilterAction.Caption = "Owner";
            //ownerFilterAction.Execute += OwnerFilterAction_Execute;

            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void OwnerFilterAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {

            //throw new NotImplementedException();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            //ownerFilterAction.Items.Clear();
            //foreach (Owner owner in ObjectSpace.GetObjects<Owner>())
            //{
            //    ownerFilterAction.Items.Add(new  ChoiceActionItem(owner.Name, owner.Oid));
            //}


            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            SetGridAppearanceAndBehaviour();
            //https://supportcenter.devexpress.com/ticket/details/t1071749/blazor-how-to-open-a-detailview-on-a-double-click-instead-of-a-single-click-in-listview
            //Set DoubleClickBehaviour for detail View
            SetDoubleClickBehaviour();
            SetBalancingSpecials();


            //CreateComboBoxFilters();

        }
        //private RenderFragment<GridDataColumnFilterRowCellTemplateContext> CreateComboboxEditControl(IEnumerable<object> data, string valueFieldName, string textFieldName) =>
        //    (GridDataColumnFilterRowCellTemplateContext context) => ComboBoxFilterTemplateRenderer.Create(context, data, valueFieldName, textFieldName);
        //private void CreateComboBoxFilters()
        //{
        //https://supportcenter.devexpress.com/ticket/details/t1089390/lookup-filter-for-blazor-list-view-grid
        //    if (View.Editor is DxGridListEditor gridListEditor)
        //    {
        //        var dataGridAdapter = gridListEditor.GetGridAdapter();
        //        foreach (var columnWrapper in gridListEditor.Columns)
        //        {
        //            var modelColumn = gridListEditor.Model.Columns[columnWrapper.PropertyName];
        //            if (modelColumn != null)
        //            {
        //                if (modelColumn.PropertyEditorType == typeof(LookupPropertyEditor))
        //                {
        //                    var fieldName = ((DxGridColumnWrapper)columnWrapper).FieldName;
        //                    var column = ((List<DxGridDataColumnModel>)dataGridAdapter.GridDataColumnModels)
        //                        .Find(x => x.FieldName == fieldName);
        //                    if (column != null)
        //                    {
        //                        string valueFieldName = ((XafMemberInfo)((XafMemberInfo)modelColumn.ModelMember.MemberInfo).MemberTypeInfo.KeyMember).Name;
        //                        string textFieldName = modelColumn.LookupProperty;
        //                        var data = ObjectSpace.GetObjects(((XafMemberInfo)modelColumn.ModelMember.MemberInfo).MemberType)
        //                            .Cast<BaseObject>()
        //                            .Select(x => new
        //                            {
        //                                Id = x.GetMemberValue(valueFieldName),
        //                                Name = x.GetMemberValue(textFieldName),
        //                            })
        //                            .ToList();
        //                        column.FilterRowCellTemplate = CreateComboboxEditControl(data, "Id", "Name");

        //                        //dataGridAdapter.GridModel.CustomizeCellDisplayText = (e) =>
        //                        //{
        //                        //    if (e.FieldName == fieldName)
        //                        //    {
        //                        //        var key = e.Value;
        //                        //        e.DisplayText = data.FirstOrDefault(x => x.Id == key).Name.ToString();
        //                        //    }
        //                        //};
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void SetBalancingSpecials()
        {
            //if(View.Id == "Property_ListView_GroupedByPropertyGroup")
            //{
            //    if
            //}
        }

        private void SetDoubleClickBehaviour()
        {

            //DxGridList
            //https://supportcenter.devexpress.com/ticket/details/t1071749/blazor-how-to-open-a-detailview-on-a-double-click-instead-of-a-single-click-in-listview
            if (View.Editor is DxGridListEditor editor && editor.Control is IDxGridAdapter gridAdapter)
            {
                if (this.View.Id == "Property_ListView_GroupedByPropertyGroup")
                {
                    gridAdapter.GridModel.RowDoubleClick = default;
                    gridAdapter.GridModel.RowClick = default;
                }
                else
                {
                    gridAdapter.GridModel.RowDoubleClick = gridAdapter.GridModel.RowClick;
                    gridAdapter.GridModel.RowClick = default;
                }
                var oldCustomizeElement = gridAdapter.GridModel.CustomizeElement;
                gridAdapter.GridModel.CustomizeElement = (GridCustomizeElementEventArgs args) =>
                {
                    oldCustomizeElement.Invoke(args);
                    if (args.ElementType is GridElementType.DataCell)
                    {


                        args.CssClass = args.CssClass.Replace("xaf-action", "xaf-double-click");

                    }
                };
            }
        }

    

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
        private void SetGridAppearanceAndBehaviour()
        {
            //Thank you for contacting us. To accomplish this task,
            //use the solution from the Data Grid for Blazor -
            //How to implement the Alternate Row Style feature thread.
            //https://supportcenter.devexpress.com/ticket/details/t815085
            //Alternatively, you can use the table - striped CSS class from Bootstrap CSS:
            if (View.Editor is GridListEditor gridListEditor)
            {
                Debug.Print($"{this.TargetObjectType} GridView Should not be used, Use DxGridView");
                gridListEditor.GetDataGridAdapter().DataGridModel.CssClass += " table-striped";
                gridListEditor.GetDataGridAdapter().DataGridModel.SelectAllMode = DevExpress.Blazor.DataGridSelectAllMode.AllPages;
                //gridListEditor.GetDataGridAdapter().DataGridModel.SelectionMode = DevExpress.Blazor.DataGridSelectionMode.MultipleSelectedDataRows;
                //gridListEditor.GetDataGridAdapter().DataGridModel.EditMode = DataGridEditMode.EditForm;
                gridListEditor.GetDataGridAdapter().DataGridModel.ShowFilterRow = true;
                gridListEditor.GetDataGridAdapter().DataGridModel.ColumnResizeMode = DataGridColumnResizeMode.Component;
            }
            else if (View.Editor is DxGridListEditor dxGridListEditor)
            {
                dxGridListEditor.GetGridAdapter().GridModel.CssClass += " table-striped"; //Doesn't work with .net 6.0 and bootstrap 5.0

                var oldCustomizeElement = dxGridListEditor.GetGridAdapter().GridModel.CustomizeElement;
                dxGridListEditor.GetGridAdapter().GridModel.CustomizeElement = (GridCustomizeElementEventArgs args) =>
                {
                    oldCustomizeElement.Invoke(args);
                    if (args.ElementType == GridElementType.DataRow)
                    {
                        if (args.VisibleIndex % 2 == 0)
                            args.Style = "--bs-table-accent-bg: var(--bs-table-striped-bg)";
                    }
                };

                dxGridListEditor.GetGridAdapter().GridModel.SelectionMode = GridSelectionMode.Multiple;
                //dxGridListEditor.GetGridAdapter().GridModel.AllowSelectRowByClick = true;
                dxGridListEditor.GetGridAdapter().GridModel.PageSizeSelectorAllRowsItemVisible = true;
                //dxGridListEditor.GetGridAdapter().GridModel.EditMode = GridEditMode.EditForm;
                dxGridListEditor.GetGridAdapter().GridModel.ShowFilterRow = true;
                dxGridListEditor.GetGridAdapter().GridModel.ShowGroupPanel = true;
                dxGridListEditor.GetGridAdapter().GridModel.ColumnResizeMode = GridColumnResizeMode.ColumnsContainer;

                if (dxGridListEditor.GetGridAdapter().GridModel is DxGridModel gridModel)
                {
                    gridModel.CssClass = "grid-list-editor-customization"; //Look in the css for this :)
                }   
            }
        }
    }
}
