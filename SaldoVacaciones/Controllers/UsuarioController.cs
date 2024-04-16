using Microsoft.AspNetCore.Mvc;

namespace SaldoVacaciones.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
