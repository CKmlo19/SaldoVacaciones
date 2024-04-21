using SaldoVacaciones.Models;
using System.Data.SqlClient;
using System.Data;

namespace SaldoVacaciones.Datos
{
    public class MovimientoDatos
    {
        public List<MovimientoModel> Listar(int idEmpleado)
        {
            var oLista = new List<MovimientoModel>();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ListarMovimiento", conexion);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode

                cmd.Parameters.AddWithValue("inCodigo", idEmpleado);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        DateTime soloFecha;
                        oLista.Add(new MovimientoModel()
                        {
                            // tecnicamente hace un select, es por eso que se lee cada registro uno a uno que ya esta ordenado
                            Id = (int)Convert.ToInt64(dr["Id"]),
                            IdEmpleado = (int)Convert.ToInt64(dr["IdEmpleado"]),
                            IdTipoMovimiento = (int)Convert.ToInt64(dr["IdTipoMovimiento"]),
                            Fecha = ((DateTime)dr["Fecha"]).Date,
                            Monto = (int)Convert.ToInt64(dr["Monto"]),
                            NombreUsuario = dr["NombreUsuario"].ToString(),
                            NombreTipoMovimiento = dr["NombreTipoMovimiento"].ToString(),
                            PostIP = dr["PostInIp"].ToString(),
                            PostTime = (DateTime)dr["PostTime"]
                        }) ;
                    }
                }
            }


            return oLista;
        }
        public bool Insertar(MovimientoModel oMovimiento)
        {
            bool resultado;

            try
            {
                var cn = new Conexion();

                // abre la conexion
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    // el procedure de listar
                    SqlCommand cmd = new SqlCommand("dbo.InsertarMovimiento", conexion);
                    cmd.Parameters.AddWithValue("inIdEmpleado", oMovimiento.IdEmpleado); // se le hace un trim a la hora de insertar
                    cmd.Parameters.AddWithValue("inNombreTipoMovimiento", oMovimiento.NombreTipoMovimiento);
                    cmd.Parameters.AddWithValue("inNombreUsuario", oMovimiento.NombreUsuario);
                    cmd.Parameters.AddWithValue("inMonto", oMovimiento.Monto);
                    cmd.Parameters.AddWithValue("inPostIp", oMovimiento.PostIP);
                    cmd.Parameters.AddWithValue("OutResultCode", 0); // en un inicio se coloca en 0
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    // Registrar el script en la página para que se ejecute en el lado del cliente
                }
                resultado = true;


            }
            catch (Exception e)
            {
                resultado = false;

            }


            return resultado;

        }

    }
}
