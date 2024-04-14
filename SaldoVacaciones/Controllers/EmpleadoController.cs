using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Models;
using SaldoVacaciones.Datos;

namespace SaldoVacaciones.Controllers
{
    public class EmpleadoController : Controller
    {
        EmpleadoDatos _EmpleadoDatos = new EmpleadoDatos();
        public IActionResult Listar()
        {
            var oLista = _EmpleadoDatos.Listar();
            return View(oLista);
        }
        /*
        public IActionResult Editar(int idEmpleado)
        {
            var oLista = _EmpleadoDatos.Listar();
            return View(oLista);
        }
        
        public IActionResult Insertar()
        { // Solo devuelve la vista
            return View();
        }
        */
        public IActionResult Insertar(EmpleadoModel oEmpleado)
        { //
            var resultado = _EmpleadoDatos.Insertar(oEmpleado);
            if (resultado)
            {
                return RedirectToAction("Listar");
            }
            else {
                return View(); // sino se queda en el mismo formulario

            }
        }
    }
}
