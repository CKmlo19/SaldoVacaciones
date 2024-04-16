using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Models;
using SaldoVacaciones.Datos;

namespace SaldoVacaciones.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(UsuarioModel _usuario)
        {
            UsuarioDatos usuarioDatos = new UsuarioDatos();

            var usuario = usuarioDatos.ValidarUsuario(_usuario.Username, _usuario.Password);

            if (usuario != null)
            {
                return RedirectToAction("Listar", "Empleado");
            }
            else {
                return View();

            }
        }
    }
}
