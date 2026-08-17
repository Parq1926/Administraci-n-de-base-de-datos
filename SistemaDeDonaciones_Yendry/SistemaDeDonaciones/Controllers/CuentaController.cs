using Microsoft.AspNetCore.Mvc;

namespace SistemaDeDonaciones.Controllers
{
    public class CuentaController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }
    }
}
