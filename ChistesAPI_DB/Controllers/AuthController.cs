using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using ChistesAPI_DB.Models;

namespace ChistesAPI_DB.Controllers
{
    public class AuthController : Controller
    {
        // GET: /auth/login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /auth/login
        [HttpPost]
        public IActionResult Login(Usuarios usuario)
        {
            // Aquí implementa la lógica de autenticación
            if (usuario.Name == "admin" && usuario.Password == "admin")
            {
                // Lógica de autenticación exitosa
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Credenciales incorrectas.";
                return View(new Usuarios()); // Crear una instancia de Usuarios para evitar NullReferenceException
            }
        }



        // GET: /auth/logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
