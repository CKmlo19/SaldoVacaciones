DECLARE @XMLDoc XML;

-- Lee el archivo XML y lo almacena en la variable @XMLDoc
SELECT @XMLDoc = CAST(BulkColumn AS XML)
FROM OPENROWSET(BULK 'C:\datos.xml', SINGLE_BLOB) AS x;

-- Crea una tabla temporal para almacenar los datos del XML
CREATE TABLE #TempPuesto (
    Nombre VARCHAR(64),
	SalarioxHora MONEY
);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO #TempPuesto (Nombre, SalarioxHora)
SELECT
    Employee.value('@Nombre', 'VARCHAR(16)'),
    Employee.value('@SalarioxHora', 'MONEY')
FROM @XMLDoc.nodes('/Datos/Puestos/Puesto') AS XTbl(Employee);

-- Consulta los datos insertados en la tabla temporal
--SELECT * FROM #TempPuesto;
USE EmpleadoCRUD
INSERT INTO dbo.Puesto(Nombre, SalarioxHora)
SELECT Nombre, SalarioxHora FROM #TempPuesto;

SELECT * FROM DBO.Puesto	
DROP TABLE #TempPuesto;

-- Esto es magia negra
DELETE FROM Dbo.Puesto
DBCC CHECKIDENT ('EmpleadoCRUD.dbo.Puesto', RESEED, 0)