USE EmpleadoCRUDWEB; -- Nombre de la base de datos
DECLARE @XMLDoc XML;

-- Lee el archivo XML y lo almacena en la variable @XMLDoc
SELECT @XMLDoc = CAST(BulkColumn AS XML)
FROM OPENROWSET(BULK 'C:\datos.xml', SINGLE_BLOB) AS x;

-- Crea una tabla temporal para almacenar los datos del XML

-- Para los empleados
DECLARE @TempEmpleados TABLE (
    Puesto VARCHAR(64),
    Nombre VARCHAR(64),
    ValorDocumentoIdentidad VARCHAR(16),
    FechaContratacion DATE
);

-- Para el puesto
DECLARE @TempPuesto TABLE(
    Nombre VARCHAR(64),
	SalarioxHora MONEY
);
-- Para los usuarios
DECLARE @TempUsuario TABLE (
	Id INT,
    Nombre VARCHAR(64),
	Password NVARCHAR(64)
);

-- Para TipoEvento
DECLARE @TempTipoEvento TABLE (
	Id INT,
    Nombre VARCHAR(64)
);

-- Para tipoMovimiento
DECLARE @TempTipoMovimiento TABLE(
	Id INT,
    Nombre VARCHAR(64),
	TipoAccion VARCHAR(16)
);

-- Para Error
DECLARE @TempError TABLE(
	Codigo INT,
	Descripcion VARCHAR(64)
);

-- Para Error
DECLARE @TempMovimientos TABLE(
	ValorDocId VARCHAR(16),
    NombreTipoMovimiento VARCHAR(64),
    Fecha DATE,
    Monto INT,
	PostByUser VARCHAR(64),
	PostInIP NVARCHAR(64),
	PostTime DATETIME
);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO @TempPuesto (Nombre, SalarioxHora)
SELECT
    Employee.value('@Nombre', 'VARCHAR(16)'),
    Employee.value('@SalarioxHora', 'MONEY')
FROM @XMLDoc.nodes('/Datos/Puestos/Puesto') AS XTbl(Employee);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO @TempUsuario (Id, Nombre, Password)
SELECT
	Employee.value('@Id', 'INT'),
    Employee.value('@Nombre', 'VARCHAR(64)'),
    Employee.value('@Pass', 'NVARCHAR(64)')
FROM @XMLDoc.nodes('/Datos/Usuarios/usuario') AS XTbl(Employee);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO @TempTipoMovimiento (Id, Nombre, TipoAccion)
SELECT
	Employee.value('@Id', 'INT'),
    Employee.value('@Nombre', 'VARCHAR(64)'),
    Employee.value('@TipoAccion', 'VARCHAR(16)')
FROM @XMLDoc.nodes('/Datos/TiposMovimientos/TipoMovimiento') AS XTbl(Employee);


-- Inserta los datos de las tablas temporales a sus respectivas tablas

INSERT INTO @TempEmpleados (Puesto, Nombre, ValorDocumentoIdentidad, FechaContratacion)
SELECT
    Employee.value('@Puesto', 'VARCHAR(64)'),
    Employee.value('@Nombre', 'VARCHAR(64)'),
    Employee.value('@ValorDocumentoIdentidad', 'VARCHAR(16)'),
    Employee.value('@FechaContratacion', 'DATE')
FROM @XMLDoc.nodes('/Datos/Empleados/empleado') AS XTbl(Employee);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO @TempTipoEvento (Id, Nombre)
SELECT
	Employee.value('@Id', 'INT'),
    Employee.value('@Nombre', 'VARCHAR(64)')
FROM @XMLDoc.nodes('/Datos/TiposEvento/TipoEvento') AS XTbl(Employee);

-- Inserta los datos del XML en la tabla temporal
INSERT INTO @TempError (Codigo, Descripcion)
SELECT
	Employee.value('@Codigo', 'INT'),
    Employee.value('@Descripcion', 'VARCHAR(64)')
FROM @XMLDoc.nodes('/Datos/Error/error') AS XTbl(Employee);

-- Insertar en #TempMovimientos
INSERT INTO @TempMovimientos(ValorDocId, NombreTipoMovimiento, Fecha, Monto, PostByUser,PostInIP, PostTime)
SELECT
    Employee.value('@ValorDocId', 'VARCHAR(16)'),
    Employee.value('@IdTipoMovimiento', 'VARCHAR(64)'),
    Employee.value('@Fecha', 'DATE'),
    Employee.value('@Monto', 'INT'),
	Employee.value('@PostByUser', 'VARCHAR(64)'),
	Employee.value('@PostInIP', 'VARCHAR(64)'),
	Employee.value('@PostTime', 'DATETIME')
FROM @XMLDoc.nodes('/Datos/Movimientos/movimiento') AS XTbl(Employee);

-- Consulta los datos insertados en la tabla temporal
SELECT * FROM @TempEmpleados;
SELECT * FROM @TempPuesto;
SELECT * FROM @TempUsuario;
SELECT * FROM @TempTipoEvento;
SELECT * FROM @TempTipoMovimiento;
SELECT * FROM @TempError;
SELECT * FROM @TempMovimientos;



-- Inserta los datos en la tabla Empleados mapeando los nombres de los puestos a los IDs correspondientes

-- para Puesto
INSERT INTO dbo.Puesto(Nombre, SalarioxHora)
SELECT Nombre, SalarioxHora FROM @TempPuesto;


-- Para empleado
INSERT INTO dbo.Empleado(IdPuesto, Nombre, ValorDocumentoIdentidad, FechaContratacion, SaldoVacaciones, EsActivo)
SELECT
    P.Id,
    T.Nombre,
    T.ValorDocumentoIdentidad,
    T.FechaContratacion,
	0 AS SaldoVacaciones,
	1 AS EsActivo
FROM @TempEmpleados T JOIN Puesto P ON T.Puesto = P.Nombre;

-- Para Usuario
INSERT INTO dbo.Usuario(id, Nombre, Password)
SELECT Id, Nombre, Password FROM @TempUsuario;

-- Para tipoEvento
INSERT INTO dbo.TipoEvento(id, Nombre)
SELECT Id, Nombre FROM @TempTipoEvento;

-- Para tipoMovimiento
INSERT INTO dbo.TipoMovimiento(id, Nombre, TipoAccion)
SELECT Id, Nombre, TipoAccion FROM @TempTipoMovimiento;

-- Para Error
INSERT INTO dbo.Error(Codigo, Descripcion)
SELECT Codigo, Descripcion FROM @TempError;




-- Para Movimiento
INSERT INTO dbo.Movimiento(IdEmpleado, IdTipoMovimiento, Fecha, Monto, NuevoSaldo, IdPostByUser, PostInIp, PostTime)
SELECT
    E.Id,
    M.Id,
	T.Fecha,
	T.Monto,
	(E.SaldoVacaciones + T.Monto) AS NuevoSaldo,
	U.Id,
	T.PostInIP,
	T.PostTime
FROM @TempMovimientos T
JOIN Empleado E ON T.ValorDocId = E.ValorDocumentoIdentidad
JOIN TipoMovimiento M ON T.NombreTipoMovimiento = M.Nombre
JOIN Usuario U ON T.PostByUser = U.Nombre

UPDATE E
SET E.SaldoVacaciones = E.SaldoVacaciones + ISNULL(
    (
        SELECT SUM(P.Monto)
        FROM dbo.Movimiento P
        WHERE P.IdEmpleado = E.Id
    ),
    0
)
FROM dbo.Empleado E;


-- Verificar en las tablas
SELECT * FROM dbo.Empleado;
SELECT * FROM dbo.Puesto;
SELECT * FROM dbo.Usuario;
SELECT * FROM dbo.TipoEvento;
SELECT * FROM dbo.TipoMovimiento;
SELECT * FROM dbo.Error;
SELECT * FROM dbo.Movimiento;

