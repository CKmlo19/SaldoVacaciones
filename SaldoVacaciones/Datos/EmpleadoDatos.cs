using SaldoVacaciones.Models;
using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NuGet.DependencyResolver;


namespace SaldoVacaciones.Datos
{
    public class EmpleadoDatos
    {
        public List<EmpleadoModel> Listar()
        {
            var oLista = new List<EmpleadoModel>();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ListarEmpleado", conexion);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode
                
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("inCodigo", 3);
                cmd.Parameters.AddWithValue("inCedula", "123456789");
                cmd.Parameters.AddWithValue("inNombre", "PEPE");
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        oLista.Add(new EmpleadoModel()
                        {
                            // tecnicamente hace un select, es por eso que se lee cada registro uno a uno que ya esta ordenado
                            Id = (int)Convert.ToInt64(dr["Id"]),
                            IdPuesto = (int)Convert.ToInt64(dr["Id"]),
                            ValorDocumentoIdentidad = dr["ValorDocumentoIdentidad"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            FechaContratacion =(DateTime)dr["FechaContratacion"],
                            SaldoVacaciones = (short)(dr["SaldoVacaciones"]),
                            EsActivo = (bool)(dr["EsActivo"]),
                        });
                    }
                }
            }
            return oLista;
        }
        /*
        public int Insertar(EmpleadoModel oEmpleado)
        {
            int resultado;

            try
            {


                var cn = new Conexion();

                // abre la conexion
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    // el procedure de listar
                    SqlCommand cmd = new SqlCommand("dbo.InsertarEmpleado", conexion);
                    cmd.Parameters.AddWithValue("inNombre", oEmpleado.Nombre.Trim()); // se le hace un trim a la hora de insertar
                    cmd.Parameters.AddWithValue("inSalario", oEmpleado.Salario);
                    cmd.Parameters.AddWithValue("OutResultCode", 0); // en un inicio se coloca en 0
                    cmd.CommandType = CommandType.StoredProcedure;
                    resultado = Convert.ToInt32(cmd.ExecuteScalar()); // Lo ejecuta y retorna un valor
                    Console.WriteLine(resultado);
                    // Registrar el script en la página para que se ejecute en el lado del cliente
                }
            }
            catch (Exception e)
            {
                string error = e.Message;
                resultado = 50006;

            }
            return resultado;
        }
        */

        public EmpleadoModel Obtener(string nombre)
        {
            var oEmpleado = new EmpleadoModel();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ObtenerEmpleado", conexion);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode
                cmd.Parameters.AddWithValue("inNombre", nombre);
                cmd.CommandType = CommandType.StoredProcedure;
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        oEmpleado.Id = (int)Convert.ToInt64(dr["Id"]);
                        oEmpleado.IdPuesto = (int)Convert.ToInt64(dr["Id"]);
                        oEmpleado.ValorDocumentoIdentidad = dr["ValorDocumentoIdentidad"].ToString();
                        oEmpleado.Nombre = dr["Nombre"].ToString();
                        oEmpleado.FechaContratacion = (DateTime)dr["FechaContratacion"];
                        oEmpleado.SaldoVacaciones = (short)(dr["SaldoVacaciones"]);
                        oEmpleado.EsActivo = (bool)(dr["EsActivo"]);
                    }
                }
            }
            return oEmpleado;
        }
        public bool Insertar(EmpleadoModel oEmpleado) {
            bool resultado;

            try
            {
                var cn = new Conexion();

                // abre la conexion
                using (var conexion = new SqlConnection(cn.getCadenaSQL()))
                {
                    conexion.Open();
                    // el procedure de listar
                    SqlCommand cmd = new SqlCommand("dbo.InsertarEmpleado", conexion);
                    cmd.Parameters.AddWithValue("inNombre", oEmpleado.Nombre.Trim()); // se le hace un trim a la hora de insertar
                    cmd.Parameters.AddWithValue("inValorDocumentoIdentidad", oEmpleado.ValorDocumentoIdentidad);
                    cmd.Parameters.AddWithValue("inIdPuesto", oEmpleado.IdPuesto);
                    cmd.Parameters.AddWithValue("OutResultCode", 0); // en un inicio se coloca en 0
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    // Registrar el script en la página para que se ejecute en el lado del cliente
                }
                resultado = true;


            }
            catch(Exception e) {
                resultado = false;

            }


            return resultado;
        
        }

        public bool Editar(EmpleadoModel oEmpleado)
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
                    SqlCommand cmd = new SqlCommand("dbo.InsertarEmpleado", conexion);
                    cmd.Parameters.AddWithValue("inNombre", oEmpleado.Nombre.Trim()); // se le hace un trim a la hora de insertar
                    cmd.Parameters.AddWithValue("inValorDocumentoIdentidad", oEmpleado.ValorDocumentoIdentidad);
                    cmd.Parameters.AddWithValue("inIdPuesto", oEmpleado.IdPuesto);
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
        public bool Eliminar(EmpleadoModel oEmpleado)
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
                    SqlCommand cmd = new SqlCommand("dbo.InsertarEmpleado", conexion);
                    cmd.Parameters.AddWithValue("inNombre", oEmpleado.Nombre.Trim()); // se le hace un trim a la hora de insertar
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
