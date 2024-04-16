using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Models;
using SaldoVacaciones.Datos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;

namespace SaldoVacaciones.Controllers
{
    [Authorize]
    public class EmpleadoController : Controller
    {
        EmpleadoDatos _EmpleadoDatos = new EmpleadoDatos();
        PuestoDatos _PuestoDatos = new PuestoDatos();



        public IActionResult Listar()
        {
            var oLista = _EmpleadoDatos.Listar("");
            return View(oLista);
        }
        [HttpPost]
        public IActionResult Listar(string campo)
        {
            var oLista = _EmpleadoDatos.Listar(campo);
            return View(oLista);
        }

        public IActionResult Editar(string ValorDocumentoIdentidad)
        {
            var oEmpleado = _EmpleadoDatos.Obtener(ValorDocumentoIdentidad);
            var oLista = _PuestoDatos.ListarPuesto();
            List<string> puestos = new List<string>();
            for (int i = 0; i < oLista.Count; i++) {
                puestos.Add(oLista[i].Nombre);
            }
            ViewBag.Puestos = new SelectList(puestos);
            return View(oEmpleado);
        }
        [HttpPost]
        public IActionResult Editar(EmpleadoModel oEmpleado)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var resultado = _EmpleadoDatos.Editar(oEmpleado);
            if (resultado)
            {
                return RedirectToAction("Listar");
            }
            else
            {
                return View(); // sino se queda en el mismo formulario

            }
        }

        public IActionResult Insertar()
        { // Solo devuelve la vista
            var oLista = _PuestoDatos.ListarPuesto();
            List<string> puestos = new List<string>();
            for (int i = 0; i < oLista.Count; i++)
            {
                puestos.Add(oLista[i].Nombre);
            }
            ViewBag.Puestos = new SelectList(puestos);
            return View();
        }

        [HttpPost]
        public IActionResult Insertar(EmpleadoModel oEmpleado)
        { //

            if (!ModelState.IsValid) { 
                return View();
            }
            var resultado = _EmpleadoDatos.Insertar(oEmpleado);
            if (resultado)
            {
                return RedirectToAction("Listar");
            }
            else {
                return View(); // sino se queda en el mismo formulario

            }
        }
        public IActionResult Eliminar(string ValorDocumentoIdentidad)
        {
            var oEmpleado = _EmpleadoDatos.Obtener(ValorDocumentoIdentidad);
            return View(oEmpleado);
        }
        [HttpPost]
        public IActionResult Eliminar(EmpleadoModel oEmpleado)
        {
            var resultado = _EmpleadoDatos.Eliminar(oEmpleado.ValorDocumentoIdentidad);
            if (resultado)
            {
                return RedirectToAction("Listar");
            }
            else
            {
                return View(); // sino se queda en el mismo formulario

            }
        }

        public IActionResult Consultar(EmpleadoModel oEmpleado)
        {
            var resultado = _EmpleadoDatos.Eliminar(oEmpleado.ValorDocumentoIdentidad);
            if (resultado)
            {
                return RedirectToAction("Listar");
            }
            else
            {
                return View(); // sino se queda en el mismo formulario

            }
        }

    }
}
