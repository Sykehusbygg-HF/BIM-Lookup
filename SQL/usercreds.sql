SELECT  pri.name As Username
,       pri.type_desc AS [User Type]
,       permit.permission_name AS [Permission]
,       permit.state_desc AS [Permission State]
,       permit.class_desc Class
,       object_name(permit.major_id) AS [Object Name]
FROM    sys.database_principals pri
LEFT JOIN
        sys.database_permissions permit
ON      permit.grantee_principal_id = pri.principal_id
WHERE name = 'azure_db_connect'