using SaldoVacaciones.Models;
using System.Data.SqlClient;
using System.Data;

namespace SaldoVacaciones.Datos
{
    public class PuestoDatos
    {
        public List<PuestoModel> ListarPuesto()
        {
            var oLista = new List<PuestoModel>();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ListarPuestos", conexion);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode

                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        oLista.Add(new PuestoModel()
                        {                            // tecnicamente hace un select, es por eso que se lee cada registro uno a uno que ya esta ordenado
                            Id = (int)Convert.ToInt64(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            SalarioxHora = Convert.ToDecimal(dr["SalarioxHora"])
                        }) ;
                    }
                }
            }
            return oLista;
        }
    }
}
