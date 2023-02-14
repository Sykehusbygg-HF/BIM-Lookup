//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.DC;
//using DevExpress.ExpressApp.Model;
//using DevExpress.Persistent.Base;
//using DevExpress.Xpo;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace BimLookup.Module.BusinessObjects
//{
//    //[NonPersistent]
//    [DomainComponent, DefaultClassOptions]
//    [ModelDefault("Caption", "BIM Lookup")]
//    [ImageName("BO_User")]
//    [XafDisplayName("BIM Lookup")]
//    //[DefaultProperty("Name")]
//    public class PropertyView : DevExpress.ExpressApp.NonPersistentBaseObject, IObjectSpaceLink, INotifyPropertyChanged﻿
//    {
//        public event PropertyChangedEventHandler PropertyChanged;

//        // This method is called by the Set accessor of each property.  
//        // The CallerMemberName attribute that is applied to the optional propertyName  
//        // parameter causes the property name of the caller to be substituted as an argument.  
//        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "Project")
//        {
//            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Workers"));
//            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InventoryTotals"));
//        }
//        private Project _Project = null;
//        [XafDisplayName("Project"), ToolTip("Välj Projekt")]
//        //[EditorAlias("CategoryDropdownPropertyEditor")]
//        public Project Project
//        {
//            get
//            {
//                return _Project;
//            }
//            set
//            {
//                _Project = value;
//                NotifyPropertyChanged();
//            }
//        }
//    }
//}
