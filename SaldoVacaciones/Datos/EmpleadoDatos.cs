using SaldoVacaciones.Models;
using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NuGet.DependencyResolver;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;


namespace SaldoVacaciones.Datos
{
    public class EmpleadoDatos
    {
        public List<EmpleadoModel> Listar(string codigo)
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
                int num = 0;
                string nombre = "";
                string cedula = "";
                if (string.IsNullOrEmpty(codigo)) { // si filtro esta vacio se lista todo
                    num = 3;
                }
                else if (ContieneNumeros(codigo)) { // si contiene numeros entonces filtra por id dado
                    num = 0;
                    cedula = codigo;
                }
                else if (ContieneLetrasYEspacios(codigo)) { // si contiene solo letras y el espacio entonces filtra por nombre
                    num = 1;
                    nombre = codigo;
                }
                cmd.Parameters.AddWithValue("inCodigo", num);
                cmd.Parameters.AddWithValue("inCedula", cedula);
                cmd.Parameters.AddWithValue("inNombre", nombre); 
                cmd.CommandType = CommandType.StoredProcedure;
                int contador = 0;
                using (var dr = cmd.ExecuteReader()) // este se utiliza cuando se retorna una gran cantidad de datos, por ejemplo la tabla completa
                {
                    // hace una lectura del procedimiento almacenado
                    while (dr.Read())
                    {
                        oLista.Add(new EmpleadoModel()
                        {
                            // tecnicamente hace un select, es por eso que se lee cada registro uno a uno que ya esta ordenado
                            Id = (int)Convert.ToInt64(dr["Id"]),
                            NombrePuesto = dr["NombrePuesto"].ToString(),
                            IdPuesto = (int)Convert.ToInt64(dr["IdPuesto"]),
                            ValorDocumentoIdentidad = dr["ValorDocumentoIdentidad"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            FechaContratacion = (DateTime)dr["FechaContratacion"],
                            SaldoVacaciones = (short)(dr["SaldoVacaciones"]),
                            EsActivo = (bool)(dr["EsActivo"]),
                        });
                    }
                }
            }


            return oLista;
        }

        public bool ContieneNumeros(string cadena)
        {
            // Patrón de expresión regular para verificar si la cadena contiene al menos un dígito
            string patron = @"^\d+$"; ;

            // Creamos un objeto Regex con el patrón especificado
            Regex regex = new Regex(patron);

            // Utilizamos el método IsMatch para verificar si la cadena coincide con el patrón
            return regex.IsMatch(cadena);
        }

        public bool ContieneLetrasYEspacios(string cadena)
        {
            // Patrón de expresión regular para verificar si la cadena contiene solo letras y espacios en blanco
            string patron = @"^[a-zA-Z\s]+$";

            // Creamos un objeto Regex con el patrón especificado
            Regex regex = new Regex(patron);

            // Utilizamos el método IsMatch para verificar si la cadena coincide con el patrón
            return regex.IsMatch(cadena);
        }

        public EmpleadoModel Obtener(string ValorDocumentoIdentidad)
        {
            var oEmpleado = new EmpleadoModel();

            var cn = new Conexion();

            // abre la conexion
            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();
                // el procedure de listar
                SqlCommand cmd = new SqlCommand("dbo.ObtenerEmpleado", conexion);
                cmd.Parameters.AddWithValue("InValorDocumentoIdentidad", ValorDocumentoIdentidad);
                cmd.Parameters.AddWithValue("OutResultCode", 0); // se le coloca un 0 en el outresultcode
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
                    cmd.Parameters.AddWithValue("inNombrePuesto", oEmpleado.NombrePuesto);
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
                    SqlCommand cmd = new SqlCommand("dbo.ModificarEmpleado", conexion);
                    cmd.Parameters.AddWithValue("inId", oEmpleado.Id);
                    cmd.Parameters.AddWithValue("inNombre", oEmpleado.Nombre.Trim()); // se le hace un trim a la hora de insertar
                    cmd.Parameters.AddWithValue("inNombrePuesto", oEmpleado.NombrePuesto);
                    cmd.Parameters.AddWithValue("inValorDocumentoIdentidad", oEmpleado.ValorDocumentoIdentidad);
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
        public bool Eliminar(string ValorDocumentoIdentidad)
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
                    SqlCommand cmd = new SqlCommand("dbo.EliminarEmpleado", conexion);
                    cmd.Parameters.AddWithValue("InValorDocumentoIdentidad", ValorDocumentoIdentidad); // se le hace un trim a la hora de insertar
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
