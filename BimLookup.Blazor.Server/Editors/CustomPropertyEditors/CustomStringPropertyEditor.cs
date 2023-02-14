using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using System;
// ...

namespace BimLookup.Blazor.Server.Editors.CustomPropertyEditors
{
    [PropertyEditor(typeof(string), false)]
    public class CustomStringPropertyEditor : BlazorPropertyEditorBase
    {
        public CustomStringPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override IComponentAdapter CreateComponentAdapter() => new InputAdapter(new InputModel());
    }
}
