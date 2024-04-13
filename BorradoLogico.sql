USE [EmpleadoCRUD]
GO
/****** Object:  StoredProcedure [dbo].[ObtenerEmpleado]    Script Date: 11/4/2024 11:10:46 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EliminarEmpleado](
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

	UPDATE Empleado
	SET EsActivo = 0 WHERE Nombre = @InNombre

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