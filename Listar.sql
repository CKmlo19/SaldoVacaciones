USE [EmpleadoCRUD]
GO
/****** Object:  StoredProcedure [dbo].[ListarEmpleado]    Script Date: 11/4/2024 04:19:38 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ListarEmpleado](
	@InCodigo INT, -- Codigo para saber si listar de por filtro de nombre, cedula o espacio vacio (todos)
	@InCedula VARCHAR(16),
	@InNombre VARCHAR(64),
	@OutResulTCode INT OUTPUT
	)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
	-- ejemplo de ejecucion del SP

	-- DECLARE @OutResulTCode int
	-- EXECUTE @RC = [dbo].[ListarEmpleado] @OutResulTCode OUTPUT
	--SELECT @OutResulTCode

	-- se hacen declaraciones

	-- se hacen inicializacion
	SET @OutResulTCode=0;

	-- se hacaen validaciones
	-- Data sets se especifican al final y todos juntos
	--SELECT @OutResulTCode AS OutResulTCode;  -- Este codigo se agrega solo si hay problemas para obtener este  valor como parametros


	IF (@InCodigo = 0) -- Por cedula
		BEGIN

		SELECT E.[Id]   -- En interfaces a usuario final no se muestra, ni en apis
		, E.[IdPuesto]
		, E.[ValorDocumentoIdentidad]
		, E.Nombre
		, E. FechaContratacion
		, E.SaldoVacaciones
		, E.EsActivo
		FROM dbo.Empleado E 
		WHERE E.EsActivo = 1 AND E.ValorDocumentoIdentidad LIKE '%' + @InCedula + '%'

		END
	ELSE IF (@InCodigo = 1) -- Por Nombre
		BEGIN

		SELECT E.[Id]   -- En interfaces a usuario final no se muestra, ni en apis
		, E.[IdPuesto]
		, E.[ValorDocumentoIdentidad]
		, E.Nombre
		, E. FechaContratacion
		, E.SaldoVacaciones
		, E.EsActivo
		FROM dbo.Empleado E 
		WHERE E.EsActivo = 1 AND E.Nombre LIKE '%' + @InNombre + '%' -- El like es para saber si tiene dcho string en InNombre
		
		END
	ELSE IF (@InCodigo = 3) -- Por cedula
		BEGIN

		SELECT E.[Id]   -- En interfaces a usuario final no se muestra, ni en apis
		, E.[IdPuesto]
		, E.[ValorDocumentoIdentidad]
		, E.Nombre
		, E. FechaContratacion
		, E.SaldoVacaciones
		, E.EsActivo
		FROM dbo.Empleado E 
		WHERE E.EsActivo = 1
		ORDER BY E.Nombre;
		END

	END TRY
	BEGIN CATCH
		INSERT INTO dbo.DBError	VALUES (
			SUSER_SNAME(),
			ERROR_NUMBER(),
			ERROR_STATE(),
			ERROR_SEVERITY(),
			ERROR_LINE(),
			ERROR_PROCEDURE(),
			ERROR_MESSAGE(),
			GETDATE()
		);

		SET @OutResulTCode=50005  ;  -- Codigo de error standar del profe para informar de un error capturado en el catch

	END CATCH;

	SET NOCOUNT Off;
END;