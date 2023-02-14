using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using BimLookup.Module.BusinessObjects;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.AccessControl;

namespace BimLookup.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater
{
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion)
    {
    }
    public override void UpdateDatabaseAfterUpdateSchema()
    {
        base.UpdateDatabaseAfterUpdateSchema();
        AppSettings settings = null;
        if (ObjectSpace.GetObjectsCount(typeof(AppSettings), null) == 0)
        {
            settings = ObjectSpace.CreateObject<AppSettings>();
        }
        if (ObjectSpace.GetObjectsCount(typeof(AppSettings), null) == 1)
        {
            settings = ObjectSpace.GetObjects<AppSettings>().FirstOrDefault();
        }
        else
        {
            var settingslist = ObjectSpace.GetObjects<AppSettings>().ToArray();
            foreach (object o in settingslist)
            {
                ObjectSpace.Delete(o);
            }
            settings = ObjectSpace.CreateObject<AppSettings>();
        }
        ObjectSpace.CommitChanges();
        if (settings != null)
        {
            //In the simplest case, you can create a new IObjectSpace object, cast it to XPObjectSpace, and then access its Session.Connection property.
            XPObjectSpace os = ObjectSpace as XPObjectSpace;
            System.Data.SqlClient.SqlConnection sql = os?.Session?.Connection as System.Data.SqlClient.SqlConnection;
            if (sql != null)
            {
                settings.DataSource = sql.DataSource;
                settings.Database = sql.Database;
                settings.DBVersion = sql.ServerVersion;
                string site = sql.Site?.Name;
                string connectionstring = sql.ConnectionString;
                //settings.DevExpress = typeof(XafApplication).Assembly.GetName().Version.ToString();
                //System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                //settings.Version = executingAssembly.GetName().Version.ToString();

            }
        }
        //string name = "MyName";
        //DomainObject1 theObject = ObjectSpace.FirstOrDefault<DomainObject1>(u => u.Name == name);
        //if(theObject == null) {
        //    theObject = ObjectSpace.CreateObject<DomainObject1>();
        //    theObject.Name = name;
        //}
        //Settings
        if (ObjectSpace.GetObjectsCount(typeof(AppSettings), null) == 0)
        {
            ObjectSpace.CreateObject<AppSettings>();
        }
        ApplicationUser sampleUser = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "User");
        if (sampleUser == null)
        {
            sampleUser = ObjectSpace.CreateObject<ApplicationUser>();
            sampleUser.UserName = "User";
            // Set a password if the standard authentication type is used
            sampleUser.SetPassword("");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)sampleUser).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(sampleUser));
        }
        PermissionPolicyRole defaultRole = CreateDefaultRole();
        sampleUser.Roles.Add(defaultRole);

        ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "Admin");
        if (userAdmin == null)
        {
            userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
            userAdmin.UserName = "Admin";
            // Set a password if the standard authentication type is used
            userAdmin.SetPassword("Admin");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
        }
        ApplicationUser userAPI = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == "APIServiceUser");
        if (userAPI == null)
        {
            userAPI = ObjectSpace.CreateObject<ApplicationUser>();
            userAPI.UserName = "APIServiceUser";
            // Set a password if the standard authentication type is used
            userAPI.SetPassword("hA7T0sZluP6d56Sh1IW5");

            // The UserLoginInfo object requires a user object Id (Oid).
            // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
            ObjectSpace.CommitChanges(); //This line persists created object(s).
            ((ISecurityUserWithLoginInfo)userAPI).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAPI));
        }
        // If a role with the Administrators name doesn't exist in the database, create this role
        PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
        if (adminRole == null)
        {
            adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.Name = "Administrators";
        }
        adminRole.IsAdministrative = true;
        userAdmin.Roles.Add(adminRole);
        userAPI.Roles.Add(adminRole);

        //Add Obligatory Items
        EnsureDefaultPhases();
        ObjectSpace.CommitChanges(); //This line persists created object(s).
    }
    public override void UpdateDatabaseBeforeUpdateSchema()
    {
        base.UpdateDatabaseBeforeUpdateSchema();
        XPObjectSpace xPObjectSpace = this.ObjectSpace as XPObjectSpace;
#if CLEARDATABASE
            //XPObjectSpace xPObjectSpace = this.ObjectSpace as XPObjectSpace;
            xPObjectSpace.Session.ClearDatabase();
#endif
        CreateDatabaseViews(xPObjectSpace);
        //if (CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0"))
        //{
        //    //RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
        //}
    }
    private PermissionPolicyRole CreateDefaultRole()
    {
        PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
        if (defaultRole == null)
        {
            defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
            defaultRole.Name = "Default";

            defaultRole.AddObjectPermissionFromLambda<ApplicationUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddMemberPermissionFromLambda<ApplicationUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
            defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
        }
        return defaultRole;
    }
    private void EnsureDefaultPhases()
    {
        List<string> phases = new List<string>() { "Skisseprosjekt", "Forprosjekt", "Detaljprosjekt", "Arbeidstegning", "Overlevering" };

        foreach (string sphase in phases)
        {
            Phase phase = ObjectSpace.FirstOrDefault<Phase>(p => p.Name == sphase);
            if (phase == null)
            {
                phase = ObjectSpace.CreateObject<Phase>();
                phase.Name = sphase;
            }
        }
    }
    //TODO: Move to before database update...
    //https://supportcenter.devexpress.com/ticket/details/t1126256/use-database-view-as-persistent-object-disable-creation-of-table
    private void CreateDatabaseViews(IObjectSpace objectSpace)
    {
        try
        {

            DevExpress.Xpo.Session session = ((XPObjectSpace)ObjectSpace).Session;
            if (session == null)
                return;


            //DONE: This did not work, it creates a view for a speciffic owner....
            //1. Move to On Update
            //2. Make sure view is db view
            //PropertyBimKravView
            string sqlTableName = "dbo.PropertyBimKravView";
            string sqlPropertiesForBimKrav = $"CREATE OR ALTER VIEW {sqlTableName} AS SELECT ROW_NUMBER() OVER(ORDER BY PSet.Name ASC) AS TempKey, Property.Oid, Property.Name, Property.Version, Property.Description, Property.Comment, Property.Skisseprosjekt, Property.Forprosjekt, Property.Detaljprosjekt, Property.Arbeidstegning, Property.Overlevering, PSet.Name AS PsetName, PSet.Oid AS PsetOid FROM Property JOIN PSetPropertySets_PropertyProperties ON PSetPropertySets_PropertyProperties.Properties = Property.Oid JOIN PSet ON PSetPropertySets_PropertyProperties.PropertySets = PSet.Oid";
            string sqlCheckExists = $"IF OBJECT_ID('{sqlTableName}', 'U') IS NOT NULL SELECT '1' ELSE SELECT '0'";
            string sqlDrop = $"DROP TABLE {sqlTableName};";
            var data = session.ExecuteQuery(sqlCheckExists);
            //string res = data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString();
            if (data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString() == "1")
            {
                //It exists as a table... drop it to be able to create view
                int irowsdrop = session.ExecuteNonQuery(sqlDrop);
            }
            sqlTableName = "PropertyBimKravView";
            sqlDrop = $"DROP TABLE {sqlTableName};";
            data = session.ExecuteQuery(sqlCheckExists);
            //string res = data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString();
            if (data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString() == "1")
            {
                //It exists as a table... drop it to be able to create view
                int irowsdrop = session.ExecuteNonQuery(sqlDrop);
            }

            int irows = session.ExecuteNonQuery(sqlPropertiesForBimKrav);

            sqlTableName = "dbo.MasterkravProjectView";
            string sqlMasterkravProjectView = 
                $"CREATE OR ALTER VIEW {sqlTableName} AS SELECT ROW_NUMBER() OVER(ORDER BY PSet.Name ASC) AS TempKey, PropertyInstance.Oid AS ID_PropertyInstance, PropertyInstance.Created, PropertyInstance.ProjectComment AS ProjectComment, Property.Oid AS ID_Property, Property.Comment AS Comment, Property.Description as Description, RevitCategory.Oid AS ID_RevitCategory, RevitCategory.Name AS RevitCategory, RevitCategory.IFCCategoryName AS IfcCategory , PSet.Oid AS ID_PSet , Project.Code AS ProjectCode, Discipline.Code AS DicsiplineCode, PSet.Name AS PSetName , Property.Name AS PropertyName , Property.Type_Instance AS TypeInstance, IfcPropertyType.Name AS IfcPropertyType, RevitPropertyType.Name AS RevitPropertyType, PropertyGroup.Name AS PropertyGroup, RevitCategory.Name AS RevitElement, Property.Guid AS PropertyGuid,PropertyInstance.Skisseprosjekt, PropertyInstance.Forprosjekt, PropertyInstance.Detaljprosjekt, PropertyInstance.Arbeidstegning, PropertyInstance.Overlevering FROM PropertyInstance " +
                $"JOIN Property ON PropertyInstance.Property = Property.Oid " +
                $"JOIN PSetPropertySets_PropertyProperties ON PSetPropertySets_PropertyProperties.Properties = Property.Oid " +
                $"JOIN PSet ON PSetPropertySets_PropertyProperties.PropertySets = PSet.Oid " +
                $"JOIN RevitCategoryRevitCategories_PropertyProperties ON RevitCategoryRevitCategories_PropertyProperties.Properties = Property.Oid " +
                $"JOIN RevitCategory ON RevitCategoryRevitCategories_PropertyProperties.RevitCategories = RevitCategory.Oid " +
                $"JOIN RevitCategoryRevitCategories_DisciplineDisciplines ON RevitCategoryRevitCategories_DisciplineDisciplines.RevitCategories = RevitCategory.Oid " +
                $"Join Discipline ON RevitCategoryRevitCategories_DisciplineDisciplines.Disciplines = Discipline.Oid " +
                $"JOIN Project ON PropertyInstance.Project = Project.Oid " +
                $"JOIN IfcPropertyType ON IfcPropertyType.Oid = Property.IfcPropertyType " +
                $"JOIN RevitPropertyType ON RevitPropertyType.Oid = Property.RevitPropertyType " +
                $"JOIN PropertyGroup ON PropertyGroup.Oid = Property.PropertyGroup";

            sqlCheckExists = $"IF OBJECT_ID('{sqlTableName}', 'U') IS NOT NULL SELECT '1' ELSE SELECT '0'";
            sqlDrop = $"DROP TABLE {sqlTableName};";
            data = session.ExecuteQuery(sqlCheckExists);
            //string res = data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString();
            if (data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString() == "1")
            {
                //It exists as a table... drop it to be able to create view
                int irowsdrop = session.ExecuteNonQuery(sqlDrop);
            }
            sqlTableName = "MasterkravProjectView";
            sqlDrop = $"DROP TABLE {sqlTableName};";
            data = session.ExecuteQuery(sqlCheckExists);
            //string res = data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString();
            if (data.ResultSet.FirstOrDefault().Rows.FirstOrDefault().Values.FirstOrDefault().ToString() == "1")
            {
                //It exists as a table... drop it to be able to create view
                int irowsdrop = session.ExecuteNonQuery(sqlDrop);
            }
            int irows2 = session.ExecuteNonQuery(sqlMasterkravProjectView);


        }
        catch (Exception ex)
        {
            Debug.Print($"Unable to CreateDatabaseViews: {ex.Message}");
        }
        //unitOfWork.ExecuteNonQuery(sqlPropertiesForBimKrav);
    }
}
