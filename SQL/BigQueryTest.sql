CREATE OR ALTER VIEW MasterkravProjectView AS
SELECT ROW_NUMBER() OVER(ORDER BY PSet.Name ASC) AS TempKey, PropertyInstance.Oid AS ID_PropertyInstance, Property.Oid AS ID_Property, RevitCategory.Oid AS ID_RevitCategory, PSet.Oid AS ID_PSet , 
		Project.Code AS ProjectCode, Discipline.Code AS DicsiplineCode, PSet.Name AS PSetName , Property.Name AS PropertyName , 
		Property.Type_Instance AS TypeInstance, IfcPropertyType.Name AS IfcPropertyType, RevitPropertyType.Name AS RevitPropertyType, 
		PropertyGroup.Name AS PropertyGroup, RevitCategory.Name AS RevitElement, Property.Guid AS PropertyGuid,
		PropertyInstance.Skisseprosjekt, PropertyInstance.Forprosjekt, PropertyInstance.Detaljprosjekt, PropertyInstance.Arbeidstegning, PropertyInstance.Overlevering
		
	FROM PropertyInstance
	JOIN Property ON
	PropertyInstance.Property = Property.Oid
	
	JOIN PSetPropertySets_PropertyProperties ON
	PSetPropertySets_PropertyProperties.Properties = Property.Oid
	
	JOIN PSet ON
	PSetPropertySets_PropertyProperties.PropertySets = PSet.Oid
	
	JOIN RevitCategoryRevitCategories_PropertyProperties ON
	RevitCategoryRevitCategories_PropertyProperties.Properties = Property.Oid

	JOIN RevitCategory ON
	RevitCategoryRevitCategories_PropertyProperties.RevitCategories = RevitCategory.Oid

	JOIN RevitCategoryRevitCategories_DisciplineDisciplines ON
	RevitCategoryRevitCategories_DisciplineDisciplines.RevitCategories = RevitCategory.Oid

	Join Discipline ON
	RevitCategoryRevitCategories_DisciplineDisciplines.Disciplines = Discipline.Oid

	JOIN Project ON
	PropertyInstance.Project = Project.Oid

	JOIN IfcPropertyType ON
	IfcPropertyType.Oid = Property.IfcPropertyType

	JOIN RevitPropertyType ON
	RevitPropertyType.Oid = Property.RevitPropertyType

	JOIN PropertyGroup ON
	PropertyGroup.Oid = Property.PropertyGroup
