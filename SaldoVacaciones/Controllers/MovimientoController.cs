using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Datos;
using SaldoVacaciones.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace SaldoVacaciones.Controllers
{
    public class MovimientoController : Controller
    {
        MovimientoDatos _movimientoDatos = new MovimientoDatos();
        EmpleadoDatos _empleadoDatos = new EmpleadoDatos();
        TipoMovimientoDatos _tipoMovimientoDatos = new TipoMovimientoDatos();

        public IActionResult ListarMovimientos(EmpleadoModel Empleado)
        {
            ActiveEmpleado e1 = ActiveEmpleado.GetInstance();
            e1.SetEmpleado(Empleado);
            var oLista = _movimientoDatos.Listar(Empleado.Id);
            return View(oLista);
        }
        public IActionResult InsertarMovimiento(int idEmpleado)
        {
            var oListaEmpleados = _empleadoDatos.Listar("3");  // codigo 3 para listar todos los empleados
            var oListaTipoMovimiento = _tipoMovimientoDatos.ListarTipoMovimiento();
            List<string> tipoMovimientos = new List<string>();
            for (int i = 0; i < oListaTipoMovimiento.Count; i++)
            {
                tipoMovimientos.Add(oListaTipoMovimiento[i].Nombre);
            }
            ViewBag.TipoMovimientos = new SelectList(tipoMovimientos);
            return View();
        }
        [HttpPost]
        public IActionResult InsertarMovimiento(MovimientoModel oMovimiento)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            ActiveUser u1 = ActiveUser.GetInstance();
            oMovimiento.NombreUsuario = u1.GetUsername();
            oMovimiento.PostIP = ipAddress;
            var respuesta = _movimientoDatos.Insertar(oMovimiento);
            
            if (respuesta)
                return RedirectToAction("ListarMovimientos", "Movimiento");
            else
                return View();
            
        }
    }

}
