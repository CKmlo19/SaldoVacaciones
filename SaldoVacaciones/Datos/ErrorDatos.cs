using SaldoVacaciones.Models;
using System.Data.SqlClient;
using System.Data;


namespace SaldoVacaciones.Datos
{
    public class ErrorDatos
    {
        public ErrorModel ObtenerError(int codigo)
        {
            var oError = new ErrorModel();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ObtenerError", conexion);
                cmd.Parameters.AddWithValue("InCodigo", codigo);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        oError.Id = (int)Convert.ToInt64(dr["Id"]);
                        oError.Descripcion = dr["Descripcion"].ToString();
                        oError.Codigo = (int)Convert.ToInt64(dr["Codigo"]);
                    }
                }
            }
            return oError;
        }

    }
}
