using Microsoft.AspNetCore.Mvc;
using SaldoVacaciones.Models;
using SaldoVacaciones.Datos;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace SaldoVacaciones.Controllers
{

    public class UsuarioController : Controller
    {
        ErrorDatos _ErrorDatos = new ErrorDatos();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UsuarioModel _usuario)
        {
            UsuarioDatos usuarioDatos = new UsuarioDatos();

            int resultado = usuarioDatos.ValidarUsuario(_usuario.Username, _usuario.Password);
            if (resultado == 0) {
               
                var usuario = usuarioDatos.RetornarUsuario(_usuario.Username, _usuario.Password);


                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario.Username),
                    new Claim("Username", usuario.Username)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    ActiveUser u1 = ActiveUser.GetInstance();
                    u1.SetUsername(usuario.Username);
                    u1.SetPassword(usuario.Password);
                    u1.SetIP(HttpContext.Connection.RemoteIpAddress.ToString());
                    return RedirectToAction("Listar", "Empleado");
                
            }

            //var usuario = usuarioDatos.ValidarUsuario(_usuario.Username, _usuario.Password);

            /*
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
                u1.SetIP(HttpContext.Connection.RemoteIpAddress.ToString());
                return RedirectToAction("Listar", "Empleado");
            }
            */
            else {
                var oError = _ErrorDatos.ObtenerError(resultado);
                TempData["ErrorMessage"] = "Error: " + oError.Codigo + " Descripcion: " + oError.Descripcion;
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
