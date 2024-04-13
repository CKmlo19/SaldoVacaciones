DECLARE @XMLDoc XML;

-- Lee el archivo XML y lo almacena en la variable @XMLDoc
SELECT @XMLDoc = CAST(BulkColumn AS XML)
FROM OPENROWSET(BULK 'C:\datos.xml', SINGLE_BLOB) AS x;

-- Crea una tabla temporal para almacenar los datos del XML
CREATE TABLE #TempEmpleados (
    Puesto VARCHAR(64),
    Nombre VARCHAR(64),
    ValorDocumentoIdentidad VARCHAR(16),
    FechaContratacion DATE
);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO #TempEmpleados (Puesto, Nombre, ValorDocumentoIdentidad, FechaContratacion)
SELECT
    Employee.value('@Puesto', 'VARCHAR(64)'),
    Employee.value('@Nombre', 'VARCHAR(64)'),
    Employee.value('@ValorDocumentoIdentidad', 'VARCHAR(16)'),
    Employee.value('@FechaContratacion', 'DATE')
FROM @XMLDoc.nodes('/Datos/Empleados/empleado') AS XTbl(Employee);

-- Consulta los datos insertados en la tabla temporal
SELECT * FROM #TempEmpleados;

-- Inserta los datos en la tabla Empleados mapeando los nombres de los puestos a los IDs correspondientes
INSERT INTO dbo.Empleado(IdPuesto, Nombre, ValorDocumentoIdentidad, FechaContratacion, SaldoVacaciones, EsActivo)
SELECT
    P.Id,
    T.Nombre,
    T.ValorDocumentoIdentidad,
    T.FechaContratacion,
	0 AS SaldoVacaciones,
	1 AS EsActivo
FROM #TempEmpleados T JOIN Puesto P ON T.Puesto = P.Nombre;


SELECT * FROM dbo.Empleado;
SELECT * FROM dbo.Puesto;

DROP TABLE #TempEmpleados;