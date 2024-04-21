using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Models;
using SaldoVacaciones.Datos;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;



namespace SaldoVacaciones.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UsuarioModel _usuario)
        {
            UsuarioDatos usuarioDatos = new UsuarioDatos();

            var usuario = usuarioDatos.ValidarUsuario(_usuario.Username, _usuario.Password);

            if (usuario != null)
            {
                var claims = new List<Claim> {  
                    new Claim(ClaimTypes.Name, usuario.Username),
                    new Claim("Username", usuario.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity)); 
                ActiveUser u1 = ActiveUser.GetInstance();
                u1.SetUsername(usuario.Username);
                u1.SetPassword(usuario.Password);
                return RedirectToAction("Listar", "Empleado");
            }
            else {
                return View();

            }
        }
        public async Task<IActionResult> Salir()
        {
            // se elimina la cookie al salir
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Usuario");
        }
    }
}
