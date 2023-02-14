using BimLookup.Module.BusinessObjects;
using BimLookup.Module.Controllers.MariaDB;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace BimLookup.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MariaDBImportViewController : ViewController
    {
        SimpleAction action;
        SimpleAction cleardatabase;
        string myConnectionString = "server=sql-bimkrav-prod.mariadb.database.azure.com;database=bim;uid=Hakan_COWI@sql-bimkrav-prod;pwd=123sykehusbygg123;SSL Mode=Required";
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public MariaDBImportViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
#if DEBUG
            action = new SimpleAction(this, "Import MariaDB", "View");
            action.ConfirmationMessage = "Are you sure you want to import from MariaDB to the current database?";
            action.Execute += action_Execute;
            cleardatabase = new SimpleAction(this, "Clear Database", "View");
            cleardatabase.ConfirmationMessage = "Are you sure you want to clear the database?";
            cleardatabase.Execute += Cleardatabase_Execute;
#endif
        }

        private void Cleardatabase_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            XPObjectSpace xPObjectSpace = this.ObjectSpace as XPObjectSpace;
            xPObjectSpace.Session.ClearDatabase();
        }

        private string ConvertDBValueToString(object value)
        {
            if (value == System.DBNull.Value)
                return string.Empty;
            else if (value is byte[])
            {
                return System.Text.Encoding.UTF8.GetString((byte[])value);
            }
            else

                return value.ToString();
        }
        private void action_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(myConnectionString);
            try
            {
                connection.Open();
                IObjectSpace objectSpace = this.ObjectSpace;
                int i = 0;

                #region objects
                i = GetProperties(connection, objectSpace, i);
                objectSpace.CommitChanges();
                i = VersionProperties(objectSpace, i);
                i = GetPropertySets(connection, objectSpace);
                //i = GetPhaseOld(connection, objectSpace);
                //Revit Categories
                i = GetCategories(connection, objectSpace);
                //Revit Projects
                i = GetProjects(connection, objectSpace);
                //Disciplines
                i = GetDisciplines(connection, objectSpace);
                objectSpace.CommitChanges();
                #endregion
                #region Junctions
                //Pset to property
                i = JunctionPsetProperty(connection, objectSpace);
                //Skriv om, skall tydligen tas från tabellen property_fase_flagg_junction istället...
                //MySqlDataAdapter adapterFaseProp = new MySqlDataAdapter("SELECT * FROM fase_property_junction", connection);
                i = JunctionPropertyPhaseFlag(connection, objectSpace);
                objectSpace.CommitChanges();
                i = JunctionPropertyProject(connection, objectSpace);
                objectSpace.CommitChanges();
                i = JunctionRevitCategoryProperty(connection, objectSpace);
                objectSpace.CommitChanges();
                i = JunctionRevitCategoryDiscipline(connection, objectSpace);
                objectSpace.CommitChanges();
                #endregion
                i = JunctionRevitCategoryIFCCategory(connection, objectSpace);
                objectSpace.CommitChanges();


            }
            catch (Exception ex)
            {
                Debug.Print("Not Ok");
            }
        }

        //private int GetPhaseOld(MySqlConnection connection, IObjectSpace objectSpace)
        //{
        //    int i;
        //    MySqlDataAdapter adapterFase = new MySqlDataAdapter("SELECT * FROM tblfase", connection);
        //    Debug.Print("tblfase");
        //    DataTable dataFase = new DataTable();
        //    adapterFase.Fill(dataFase);
        //    i = 0;
        //    foreach (DataRow row in dataFase.Rows)
        //    {
        //        i++;
        //        int id = (int)row["ID_Fase"];
        //        Phase phase = objectSpace.GetObjectsQuery<Phase>().Where(c => c.MariaDB_ID_Property == id).FirstOrDefault();
        //        if (phase == null)
        //        {
        //            phase = objectSpace.CreateObject<Phase>();
        //        }
        //        phase.MariaDB_ID_Property = id;
        //        phase.Name = ConvertDBValueToString(row["FaseNavn"]);
        //        if (i % 100 == 0 && i != 0)
        //            Debug.Print(i.ToString());
        //    }

        //    return i;
        //}

        private static int JunctionRevitCategoryProperty(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterRevitProp = new MySqlDataAdapter("SELECT * FROM element_property_junction", connection);
            Debug.Print("element_property_junction");
            DataTable dataRevitProp = new DataTable();
            adapterRevitProp.Fill(dataRevitProp);
            i = 0;
            foreach (DataRow row in dataRevitProp.Rows)
            {
                i++;
                int irev = (int)row["ID_Element"];
                int iprop = (int)row["ID_Property"];
                RevitCategory category = objectSpace.GetObjectsQuery<RevitCategory>().Where(c => c.MariaDB_ID_Property == irev).FirstOrDefault();
                Property prop = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == iprop).FirstOrDefault();
                if (category != null && prop != null)
                {
                    if (prop.RevitCategories.Where(x => x.Oid == category.Oid).FirstOrDefault() == null)
                        prop.RevitCategories.Add(category);
                }
                else
                {
                    Debug.Print($"JunctionRevitCategoryProperty: Property {iprop} and or Category {irev} is missing");
                }
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }


        private static int JunctionRevitCategoryDiscipline(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterRevitProp = new MySqlDataAdapter("SELECT * FROM dicipline_element_junction", connection);
            Debug.Print("dicipline_element_junction");
            DataTable dataRevitProp = new DataTable();
            adapterRevitProp.Fill(dataRevitProp);
            i = 0;
            foreach (DataRow row in dataRevitProp.Rows)
            {
                i++;
                int idiscipline = (int)row["disciplineID"];
                int irev = (int)row["elementID"];
                RevitCategory category = objectSpace.GetObjectsQuery<RevitCategory>().Where(c => c.MariaDB_ID_Property == irev).FirstOrDefault();
                Discipline discipline = objectSpace.GetObjectsQuery<Discipline>().Where(c => c.MariaDB_ID_Property == idiscipline).FirstOrDefault();
                if (category != null && discipline != null)
                {
                    if (category.Disciplines.Where(x => x.Oid == category.Oid).FirstOrDefault() == null)
                        category.Disciplines.Add(discipline);
                }
                else
                {
                    Debug.Print($"JunctionRevitCategoryDiscipline: Discipline {idiscipline} and or Category {irev} is missing");
                }
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }
        private static int JunctionRevitCategoryIFCCategory(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterRevitProp = new MySqlDataAdapter("SELECT * FROM element_ifcentitet_junction", connection);
            Debug.Print("element_ifcentitet_junction");
            DataTable dataRevitProp = new DataTable();
            adapterRevitProp.Fill(dataRevitProp);
            i = 0;
            foreach (DataRow row in dataRevitProp.Rows)
            {
                i++;
                string revitName = (string)row["RevitElementName"];
                string ifcName = (string)row["IFCEntityName"];
                RevitCategory category = objectSpace.GetObjectsQuery<RevitCategory>().Where(c => c.Name == revitName).FirstOrDefault();
                if (category != null)
                {
                    category.IFCName = ifcName;
                }
                else
                {
                    Debug.Print($"JunctionRevitCategoryIFCCategory: Category {revitName} is missing");
                }
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }

        private static int JunctionPropertyProject(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterProjectProp = new MySqlDataAdapter("SELECT * FROM property_project_junction", connection);
            Debug.Print("property_project_junction");
            DataTable dataProjectProp = new DataTable();
            adapterProjectProp.Fill(dataProjectProp);
            i = 0;
            foreach (DataRow row in dataProjectProp.Rows)
            {
                i++;
                int iproject = (int)row["ID_Project"];
                int iprop = (int)row["ID_Property"];
                Project project = objectSpace.GetObjectsQuery<Project>().Where(c => c.MariaDB_ID_Property == iproject).FirstOrDefault();
                Property prop = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == iprop).FirstOrDefault();
                if (project != null && prop != null)
                {
                    //Create a new prop instance with the default settings...
                    PropertyInstance pi = project.Properties.Where(x => x.Property.Oid == prop.Oid).FirstOrDefault();
                    if (pi == null)
                    {
                        pi = objectSpace.CreateObject<PropertyInstance>();
                        pi.Property = prop;
                        project.Properties.Add(pi);
                    }
                    //Add projetspeciffic properties!
                }
                else
                {
                        Debug.Print($"JunctionPropertyProject: Property {iprop} and or Project {iproject} is missing");
                }
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }

        private static int JunctionPropertyPhaseFlag(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterFaseProp = new MySqlDataAdapter("SELECT * FROM property_fase_flagg_junction", connection);

            Debug.Print("property_fase_flagg_junction");
            DataTable dataFaseProp = new DataTable();
            adapterFaseProp.Fill(dataFaseProp);
            i = 0;
            foreach (DataRow row in dataFaseProp.Rows)
            {
                i++;
                SByte iskisse = (SByte)row["Skisseprosjekt"];
                SByte ifor = (SByte)row["Forprosjekt"];
                SByte idetalj = (SByte)row["Detaljprosjekt"];
                SByte iarbeid = (SByte)row["Arbeidstegning"];
                SByte iover = (SByte)row["Overlevering"];

                int iprop = (int)row["ID_Property"];
                Property prop = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == iprop).FirstOrDefault();
                if (prop == null)
                {
                    //Debug.Assert(false, $"Fase junction: Property {iprop} is missing");
                    Debug.Print($"Fase junction: Property {iprop} is missing");

                }
                else
                {
                    if (iskisse == 1)
                        prop.Skisseprosjekt = true;
                    if (ifor == 1)
                        prop.Forprosjekt = true;
                    if (idetalj == 1)
                        prop.Detaljprosjekt = true;
                    if (iarbeid == 1)
                        prop.Arbeidstegning = true;
                    if (iover == 1)
                        prop.Overlevering = true;
                }
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }

        private static int JunctionPsetProperty(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapter_pset_property_junction = new MySqlDataAdapter("SELECT * FROM pset_property_junction", connection);
            Debug.Print("pset_property_junction");
            DataTable dataadapter_pset_property_junction = new DataTable();
            adapter_pset_property_junction.Fill(dataadapter_pset_property_junction);
            i = 0;
            foreach (DataRow row in dataadapter_pset_property_junction.Rows)
            {
                i++;
                int ipset = (int)row["ID_Pset"];
                int iprop = (int)row["ID_Property"];
                PropertySet pset = objectSpace.GetObjectsQuery<PropertySet>().Where(c => c.MariaDB_ID_Property == ipset).FirstOrDefault();
                Property prop = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == iprop).FirstOrDefault();
                if (pset != null && prop != null)
                {
                    if (prop.PropertySets.Where(x => x.Oid == pset.Oid).FirstOrDefault() == null)
                        prop.PropertySets.Add(pset);
                    else
                    {
                        Debug.Print($"JunctionPsetProperty: Property {iprop} and or Pset {ipset} is missing");
                    }
                }
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
                
            }

            return i;
        }

        private int GetDisciplines(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterDp = new MySqlDataAdapter("SELECT * FROM tbldiscipline", connection);
            DataTable dataDp = new DataTable();
            adapterDp.Fill(dataDp);
            Debug.Print("tbldiscipline");
            i = 0;
            foreach (DataRow row in dataDp.Rows)
            {
                i++;
                int id = (int)row["ID_Discipline"];
                Discipline discipline = objectSpace.GetObjectsQuery<Discipline>().Where(c => c.MariaDB_ID_Property == id).FirstOrDefault();
                if (discipline == null)
                {
                    discipline = objectSpace.CreateObject<Discipline>();
                }
                discipline.MariaDB_ID_Property = id;

                discipline.Name = ConvertDBValueToString(row["DisciplineName"]);
                discipline.Code = ConvertDBValueToString(row["DisciplineCode"]);
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }

        private int GetProjects(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterProject = new MySqlDataAdapter("SELECT * FROM tblproject", connection);
            DataTable dataProject = new DataTable();
            adapterProject.Fill(dataProject);
            Debug.Print("tblproject");
            i = 0;
            foreach (DataRow row in dataProject.Rows)
            {
                i++;
                int id = (int)row["ID_Project"];
                Project project = objectSpace.GetObjectsQuery<Project>().Where(c => c.MariaDB_ID_Property == id).FirstOrDefault();
                if (project == null)
                {
                    project = objectSpace.CreateObject<Project>();
                }
                project.MariaDB_ID_Property = id;

                project.Name = ConvertDBValueToString(row["ProjectName"]);
                project.Code = ConvertDBValueToString(row["ProjectCode"]);
                project.HealthCompany = ConvertDBValueToString(row["Helseforetak"]);
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }

        private int GetCategories(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterRevitType = new MySqlDataAdapter("SELECT * FROM tblrevitelements", connection);
            Debug.Print("tblrevitelements");
            DataTable dataRevitType = new DataTable();
            adapterRevitType.Fill(dataRevitType);
            i = 0;
            foreach (DataRow row in dataRevitType.Rows)
            {
                i++;
                int id = (int)row["ID_Element"];
                RevitCategory revitCategory = objectSpace.GetObjectsQuery<RevitCategory>().Where(c => c.MariaDB_ID_Property == id).FirstOrDefault();
                if (revitCategory == null)
                {
                    revitCategory = objectSpace.CreateObject<RevitCategory>();
                }
                revitCategory.MariaDB_ID_Property = id;
                revitCategory.Name = ConvertDBValueToString(row["RevitElement"]);
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }

        private int GetPropertySets(MySqlConnection connection, IObjectSpace objectSpace)
        {
            int i;
            MySqlDataAdapter adapterPset = new MySqlDataAdapter("SELECT * FROM tblpset", connection);
            Debug.Print("tblpset");
            DataTable dataPset = new DataTable();
            adapterPset.Fill(dataPset);
            i = 0;
            foreach (DataRow row in dataPset.Rows)
            {
                i++;
                int id = (int)row["ID_Pset"];
                PropertySet pset = objectSpace.GetObjectsQuery<PropertySet>().Where(c => c.MariaDB_ID_Property == id).FirstOrDefault();
                if (pset == null)
                {
                    pset = objectSpace.CreateObject<PropertySet>();
                }
                pset.MariaDB_ID_Property = id;
                pset.Name = ConvertDBValueToString(row["PsetName"]);
                pset.Description = ConvertDBValueToString(row["PsetDescription"]);
                var origin = row["PsetOrigin"];
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
        }
        private int VersionProperties(IObjectSpace objectSpace, int i)
        {
            var list = PreVersionedProperties.GetProVersions();
            foreach(var item in list)
            {
                int id0 = item.Key;
                int id1 = item.Value;
                Property prop0 = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == id0).FirstOrDefault();
                Property prop1 = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == id1).FirstOrDefault();
                if (prop0 == null || prop1 == null)
                {
                    Debug.Assert(true, $"{id0}:{prop0},{id1}{prop1}");
                }
                prop1.Version = prop0.Version + 1;
                prop1.Versions.Add(prop0);
                i++;
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }
            return i;
        }
        private int GetProperties(MySqlConnection connection, IObjectSpace objectSpace, int i)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM tblproperty", connection);
            DataTable data = new DataTable();
            adapter.Fill(data);
            Debug.Print("tblproperty");
            foreach (DataRow row in data.Rows)
            {
                i++;
                int id = (int)row["ID_Property"];
                Property prop = objectSpace.GetObjectsQuery<Property>().Where(c => c.MariaDB_ID_Property == id).FirstOrDefault();
                if (prop == null)
                {
                    prop = objectSpace.CreateObject<Property>();
                }
                prop.MariaDB_ID_Property = id;
                prop.Name = ConvertDBValueToString(row["PropertyName"]);
                
                string ifcproptype = ConvertDBValueToString(row["IfcPropertyType"]);
                IfcPropertyType ifc = objectSpace.GetObjectsQuery<IfcPropertyType>().Where(c => c.Name == ifcproptype).FirstOrDefault();
                if (ifc == null)
                {
                    ifc = objectSpace.CreateObject<IfcPropertyType>();
                    ifc.Name = ifcproptype;
                    objectSpace.CommitChanges();
                }
                prop.IfcPropertyType = ifc;

                string revproptype = ConvertDBValueToString(row["RevitPropertyType"]);
                RevitPropertyType revpt = objectSpace.GetObjectsQuery<RevitPropertyType>().Where(c => c.Name == revproptype).FirstOrDefault();
                if (revpt == null)
                {
                    revpt = objectSpace.CreateObject<RevitPropertyType>();
                    revpt.Name = revproptype;
                    objectSpace.CommitChanges();
                }
                prop.RevitPropertyType = revpt;

                string ti = ConvertDBValueToString(row["TypeInstans"]);
                if(ti == null)
                {
                    Debug.WriteLine($"{prop.Name}: TypeInstans = null");
                }
                else if(ti.ToLower() == "instans" || ti.ToLower() == "instance")
                {
                    prop.Type_Instance = Property.TypeInstanceEnum.Instance; 
                }
                else if (ti.ToLower() == "typ" || ti.ToLower() == "type")
                {
                    prop.Type_Instance = Property.TypeInstanceEnum.Type;
                }
                else
                {
                    Debug.WriteLine($"{prop.Name}: TypeInstans = {ti}");
                    prop.Type_Instance = Property.TypeInstanceEnum.Instance;
                }

                string spg = ConvertDBValueToString(row["PropertyGroup"]);
                PropertyGroup pg = objectSpace.GetObjectsQuery<PropertyGroup>().Where(c => c.Name == spg).FirstOrDefault();
                if (pg == null)
                {
                    pg = objectSpace.CreateObject<PropertyGroup>();
                    pg.Name = spg;
                    objectSpace.CommitChanges();
                }
                prop.PropertyGroup = pg;


                prop.Description = ConvertDBValueToString(row["PropertyDescription"]);
                prop.Comment = ConvertDBValueToString(row["PropertyComment"]);
                var from = row["Kommer fra 2B"];
                var initfrom = row["Initiert av"];
                prop.Guid = ConvertDBValueToString(row["PropertyGUID"]);
                if (i % 100 == 0 && i != 0)
                    Debug.Print(i.ToString());
            }

            return i;
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
