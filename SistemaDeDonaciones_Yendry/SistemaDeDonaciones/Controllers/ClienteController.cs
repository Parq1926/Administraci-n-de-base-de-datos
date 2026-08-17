using Microsoft.AspNetCore.Mvc;

namespace SistemaDeDonaciones.Controllers
{
    public class ClienteController : Controller
    {

        //Datos de ejemplo
        public IActionResult Index()
        {
            ViewData["Title"] = "Hola, Usuario";
            ViewData["Subtitle"] = "Tu actividad como donante";
            return View();
        }

        public IActionResult Donar()
        {
            ViewData["Title"] = "Hacer una donación";
            ViewData["Subtitle"] = "Elige un proyecto y el desglose de tu aporte";
            return View();
        }

        public IActionResult MisDonaciones()
        {
            ViewData["Title"] = "Mis donaciones";
            ViewData["Subtitle"] = "Historial completo de tus aportes";
            return View();
        }

        public IActionResult MiPerfil()
        {
            ViewData["Title"] = "Mi perfil";
            ViewData["Subtitle"] = "Mantenimiento de tus datos personales";
            return View();
        }

        public IActionResult CambiarClave()
        {
            ViewData["Title"] = "Cambiar contraseña";
            ViewData["Subtitle"] = "Actualiza la contraseña de tu cuenta";
            return View();
        }
    }
}
