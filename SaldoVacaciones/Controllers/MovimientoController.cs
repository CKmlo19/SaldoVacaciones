using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Datos;
using SaldoVacaciones.Models;


namespace SaldoVacaciones.Controllers
{
    public class MovimientoController : Controller
    {
        MovimientoDatos _movimientoDatos = new MovimientoDatos();
        public IActionResult ListarMovimientos(int idEmpleado)
        {
            var oLista = _movimientoDatos.Listar(idEmpleado);
            return View(oLista);
        }
    }
}
