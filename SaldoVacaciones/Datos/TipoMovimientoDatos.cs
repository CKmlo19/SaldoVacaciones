using SaldoVacaciones.Models;
using System.Data.SqlClient;
using System.Data;

namespace SaldoVacaciones.Datos
{
    public class TipoMovimientoDatos
    {

        public List<TipoMovimientoModel> ListarTipoMovimiento()
        {
            var oLista = new List<TipoMovimientoModel>();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ListarTipoMovimiento", conexion);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode

                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        oLista.Add(new TipoMovimientoModel()
                        {                            // tecnicamente hace un select, es por eso que se lee cada registro uno a uno que ya esta ordenado
                            Id = (int)Convert.ToInt64(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            TipoAccion = dr["TipoAccion"].ToString()
                        });
                    }
                }
            }
            return oLista;
        }
    }
}
