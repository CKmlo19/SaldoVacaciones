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

        public IActionResult ListarMovimientos(string ValorDocumentoIdentidad)
        {
            var empleado = _empleadoDatos.Obtener(ValorDocumentoIdentidad);
            ActiveEmpleado e1 = ActiveEmpleado.GetInstance();
            e1.SetEmpleado(empleado);
            var oLista = _movimientoDatos.Listar(empleado.Id);
            return View(oLista);
        }
        public IActionResult InsertarMovimiento(int idEmpleado)
        {
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
            if (!ModelState.IsValid)
            {
                var oListaTipoMovimiento = _tipoMovimientoDatos.ListarTipoMovimiento();
                List<string> tipoMovimientos = new List<string>();
                for (int i = 0; i < oListaTipoMovimiento.Count; i++)
                {
                    tipoMovimientos.Add(oListaTipoMovimiento[i].Nombre);
                }
                ViewBag.TipoMovimientos = new SelectList(tipoMovimientos);

                return View();
            }


            string ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            ActiveUser u1 = ActiveUser.GetInstance();
            ActiveEmpleado e1 = ActiveEmpleado.GetInstance();
            oMovimiento.NombreUsuario = u1.GetUsername();
            oMovimiento.PostIP = ipAddress;
            oMovimiento.IdEmpleado = e1.GetEmpleado().Id;
            var respuesta = _movimientoDatos.Insertar(oMovimiento);



            if (respuesta)
                return RedirectToAction("ListarMovimientos", "Movimiento", new { ValorDocumentoIdentidad = e1.GetEmpleado().ValorDocumentoIdentidad });
            else {
                var oListaTipoMovimiento = _tipoMovimientoDatos.ListarTipoMovimiento();
                List<string> tipoMovimientos = new List<string>();
                for (int i = 0; i < oListaTipoMovimiento.Count; i++)
                {
                    tipoMovimientos.Add(oListaTipoMovimiento[i].Nombre);
                }
                ViewBag.TipoMovimientos = new SelectList(tipoMovimientos);
                return View();
            }
        }
    }

}
