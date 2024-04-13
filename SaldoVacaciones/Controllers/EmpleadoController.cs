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
        public IActionResult Editar(int idEmpleado)
        {
            var oLista = _EmpleadoDatos.Listar();
            return View(oLista);
        }
    }
}
