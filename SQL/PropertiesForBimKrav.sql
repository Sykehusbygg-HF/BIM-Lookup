CREATE OR ALTER VIEW PropertyBimKravView AS
SELECT ROW_NUMBER() OVER(ORDER BY PSet.Name ASC) AS TempKey, Property.Oid, Property.Name, Property.Version, Property.Description, Property.Comment, Property.Skisseprosjekt, Property.Forprosjekt, Property.Detaljprosjekt, Property.Arbeidstegning, Property.Overlevering, PSet.Name AS PsetName, PSet.Oid AS PsetOid FROM Property
	JOIN PSetPropertySets_PropertyProperties ON
	PSetPropertySets_PropertyProperties.Properties = Property.Oid
	JOIN PSet ON
	PSetPropertySets_PropertyProperties.PropertySets = PSet.Oid