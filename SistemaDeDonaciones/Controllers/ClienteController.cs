using Microsoft.AspNetCore.Mvc;
using SistemaDeDonaciones.Models.Api;
using SistemaDeDonaciones.Services;
using System.Security.Claims;

namespace SistemaDeDonaciones.Controllers
{
    public class ClienteController : Controller
    {
        private readonly UsuarioApiService _usuarioApi;

        public ClienteController(UsuarioApiService usuarioApi)
        {
            _usuarioApi = usuarioApi;
        }

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

        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            ViewData["Title"] = "Mi perfil";
            ViewData["Subtitle"] = "Mantenimiento de tus datos personales";

            var idUsuario = ObtenerIdUsuarioSesion();
            if (idUsuario == null)
            {
                TempData["Error"] = "No se encontró la sesión del usuario.";
                return View(new Usuario());
            }

            try
            {
                var usuarios = await _usuarioApi.ObtenerUsuariosAsync();
                var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);

                if (usuario == null)
                {
                    TempData["Error"] = "No se encontró la información del usuario.";
                    return View(new Usuario());
                }

                return View(usuario);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new Usuario());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(string nombre, string apellidos, string email)
        {
            var idUsuario = ObtenerIdUsuarioSesion();
            if (idUsuario == null)
            {
                TempData["Error"] = "No se encontró la sesión del usuario.";
                return RedirectToAction(nameof(MiPerfil));
            }

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "El nombre y el correo son obligatorios.";
                return RedirectToAction(nameof(MiPerfil));
            }

            var input = new UsuarioUpdateInput
            {
                IdUsuario = idUsuario.Value,
                Nombre = nombre,
                Apellidos = apellidos,
                Email = email
            };

            try
            {
                var respuesta = await _usuarioApi.ActualizarAsync(input);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MiPerfil));
        }

        public IActionResult CambiarClave()
        {
            ViewData["Title"] = "Cambiar contraseña";
            ViewData["Subtitle"] = "Actualiza la contraseña de tu cuenta";
            return View();
        }

        private int? ObtenerIdUsuarioSesion()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }
}
