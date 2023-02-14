using BimLookup.Module.Classes.Enums;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace BimLookup.Module.BusinessObjects
{
    /// <summary>
    /// Placeholder för Settings
    /// </summary>
    [DefaultClassOptions]
    // ...
    [RuleObjectExists("AnotherSingletonExists", DefaultContexts.Save, "True", InvertResult = true,
        CustomMessageTemplate = "Another Singleton already exists.")]
    [RuleCriteria("CannotDeleteSingleton", DefaultContexts.Delete, "False",
        CustomMessageTemplate = "Cannot delete Singleton.")]
    [ImageName("ModelEditor_Settings")]
    [XafDisplayName("Settings")]
    //[DefaultProperty("Name")]
    public class AppSettings : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public AppSettings(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private ServerStatus _ServerStatus;
        public ServerStatus ServerStatus
        {
            get { return _ServerStatus; }
            set { SetPropertyValue(nameof(ServerStatus), ref _ServerStatus, value); }
        }
        [Browsable(false)]
        public string ServerStatusText
        {
            get
            {
                return this.ServerStatus.ToString();
            }
        }
        private DateTime _ServerStatusDateTime;
        public DateTime ServerStatusDateTime
        {
            get { return _ServerStatusDateTime; }
            set { SetPropertyValue(nameof(ServerStatusDateTime), ref _ServerStatusDateTime, value); }
        }
        public string Compilation
        {
#if DEBUG
            get => "DEBUG";
#elif RELEASE
           get => "RELEASE";
#elif PROD_SYKEHUSBYGG
            get => "PROD_SYKEHUSBYGG";
#elif DEBUG_PROD_SYKEHUSBYGG
            get => "DEBUG_PROD_SYKEHUSBYGG";
#elif DEMO_SYKEHUSBYGG
            get => "DEMO_SYKEHUSBYGG";
#elif DEBUG_DEMO_SYKEHUSBYGG
            get => "DEBUG_DEMO_SYKEHUSBYGG";
#elif SANDBOX_SYKEHUSBYGG
           get => "SANDBOX_SYKEHUSBYGG";
#elif DEBUG_SANDBOX_SYKEHUSBYGG
            get => "DEBUG_SANDBOX_SYKEHUSBYGG";
#elif EASYTEST
           get => "EASYTEST";
#else 
            get => "?";

#endif
        }
        private string _Database;
        [Persistent("Database")]
        [XafDisplayName("Database"), ToolTip("Database")]
        [ReadOnly(true)]
        [Appearance("Settings_Database_Disabled", Enabled = false)]
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Info", LayoutGroupType.SimpleEditorsGroup, 0)]
        public string Database
        {
            get { return _Database; }
            set { SetPropertyValue("Database", ref _Database, value); }
        }
        private string _DataSource;
        [Persistent("DataSource")]
        [XafDisplayName("Data Source"), ToolTip("Data Source")]
        [ReadOnly(true)]
        [Appearance("Settings_DatabaSource_Disabled", Enabled = false)]
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Info", LayoutGroupType.SimpleEditorsGroup, 0)]
        public string DataSource
        {
            get { return _DataSource; }
            set { SetPropertyValue("DataSource", ref _DataSource, value); }
        }
        private string _DBVersion;
        [Persistent("DBVersion")]
        [XafDisplayName("DB Version"), ToolTip("DB Version")]
        [ReadOnly(true)]
        [Appearance("Settings_DBVersion_Disabled", Enabled = false)]
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Info", LayoutGroupType.SimpleEditorsGroup, 0)]
        public string DBVersion
        {
            get { return _DBVersion; }
            set { SetPropertyValue("DBVersion", ref _DBVersion, value); }
        }
        //private string _Version;
        //[Persistent("Version")]
        [XafDisplayName("Version"), ToolTip("Version")]
        [ReadOnly(true)]
        [Appearance("Settings_Version_Disabled", Enabled = false)]
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Info", LayoutGroupType.SimpleEditorsGroup, 0)]        
        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            }
            //set { SetPropertyValue("Version", ref _Version, value); }
        }
        //private string _DevExpress;
        //[Persistent("DevExpress")]
        [XafDisplayName("DevExpress Version"), ToolTip("DevExpress Version")]
        [ReadOnly(true)]
        [Appearance("Settings_DevExpress_Disabled", Enabled = false)]
        //[DetailViewLayoutAttribute(LayoutColumnPosition.Left, "Info", LayoutGroupType.SimpleEditorsGroup, 0)]
        public string DevExpress
        {
            get { return typeof(XafApplication).Assembly.GetName().Version.ToString(); }
        }
        [ModelDefault(nameof(IModelCommonMemberViewItem.PropertyEditorType), "BimLookup.Blazor.Server.Editors.CustomPropertyEditors.CustomStringPropertyEditor")]
        public string SwaggerPath
        {
            get { return $"swagger"; }
        }
        [ModelDefault(nameof(IModelCommonMemberViewItem.PropertyEditorType), "BimLookup.Blazor.Server.Editors.CustomPropertyEditors.CustomStringPropertyEditor")]
        public string BimKrav
        {
            get { return $"bimkrav"; }
        }

    }
}