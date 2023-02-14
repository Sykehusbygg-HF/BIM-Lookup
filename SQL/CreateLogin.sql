-- =============================================================================================================================
-- Create SQL Login template for Azure SQL Database, Azure Synapse Analytics Database, and Azure Synapse SQL Analytics on-demand
-- =============================================================================================================================
use master
go
CREATE LOGIN azure_db_connect
	WITH PASSWORD = '14Pzdj96LIhENwDScCmY' 
GO

-- =============================================================================================================================
-- Create Azure Active Directory Login template for Azure SQL Database, Azure Synapse Analytics Database, and Azure Synapse SQL Analytics on-demand
-- =============================================================================================================================

-- CREATE LOGIN <Azure Active Directory Principal, sysname, login_name> FROM EXTERNAL PROVIDER