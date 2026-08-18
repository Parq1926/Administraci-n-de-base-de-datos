using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SistemaDeDonaciones.Models.Api;
using SistemaDeDonaciones.Services;

namespace SistemaDeDonaciones.Controllers
{
    public class RegistroInput
    {
        public string Nombre { get; set; } = "";
        public string? Apellidos { get; set; }
        public string Email { get; set; } = "";
        public string Contrasena { get; set; } = "";
        public string ConfirmarContrasena { get; set; } = "";
    }

    public class CuentaController : Controller
    {
        private readonly UsuarioApiService _usuarioApi;
        private readonly AuthService _authService;

        public CuentaController(UsuarioApiService usuarioApi, AuthService authService)
        {
            _usuarioApi = usuarioApi;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInput modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo.Email) || string.IsNullOrWhiteSpace(modelo.Contrasena))
            {
                TempData["Error"] = "Debe indicar el correo y la contraseña.";
                return RedirectToAction(nameof(Login));
            }

            LoginResponse respuesta;

            try
            {
                respuesta = await _usuarioApi.LoginAsync(modelo);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = "No se pudo validar el inicio de sesión. Detalle: " + ex.Message;
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error inesperado al iniciar sesión. Detalle: " + ex.Message;
                return RedirectToAction(nameof(Login));
            }

            if (!respuesta.Exitoso)
            {
                TempData["Error"] = string.IsNullOrWhiteSpace(respuesta.Mensaje)
                    ? "Correo o contraseña incorrectos."
                    : respuesta.Mensaje;
                return RedirectToAction(nameof(Login));
            }

            await IniciarSesionAsync(respuesta.IdUsuario, modelo.Email, respuesta.TipoUsuario, respuesta.Nombre);

            return RedirigirSegunTipo(respuesta.TipoUsuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroInput modelo)
        {
            if (string.IsNullOrWhiteSpace(modelo.Nombre)
                || string.IsNullOrWhiteSpace(modelo.Email)
                || string.IsNullOrWhiteSpace(modelo.Contrasena))
            {
                TempData["Error"] = "Debe completar nombre, correo y contraseña.";
                return RedirectToAction(nameof(Registro));
            }

            if (modelo.Contrasena != modelo.ConfirmarContrasena)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                return RedirectToAction(nameof(Registro));
            }

            var input = new UsuarioInput
            {
                Nombre = modelo.Nombre,
                Apellidos = modelo.Apellidos,
                Email = modelo.Email,
                Contrasena = modelo.Contrasena,
                TipoUsuario = "Usuario",
                Activo = true
            };

            OperationResponse respuesta;

            try
            {
                respuesta = await _usuarioApi.InsertarAsync(input);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = "No se pudo crear la cuenta. Detalle: " + ex.Message;
                return RedirectToAction(nameof(Registro));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Ocurrió un error inesperado al crear la cuenta. Detalle: " + ex.Message;
                return RedirectToAction(nameof(Registro));
            }

            if (!respuesta.Exito)
            {
                TempData["Error"] = string.IsNullOrWhiteSpace(respuesta.Mensaje)
                    ? "No se pudo crear la cuenta."
                    : respuesta.Mensaje;
                return RedirectToAction(nameof(Registro));
            }

            // Cuenta creada correctamente: se inicia sesión automáticamente.
            await IniciarSesionAsync(respuesta.Id, modelo.Email, "Usuario", modelo.Nombre);

            TempData["Exito"] = "Cuenta creada exitosamente.";
            return RedirectToAction("Index", "Cliente");
        }

        private async Task IniciarSesionAsync(int idUsuario, string email, string tipoUsuario, string nombre)
        {
            var principal = _authService.CrearClaimsPrincipal(idUsuario, email, tipoUsuario, nombre);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }

        // TipoUsuario en la base de datos actual usa los valores "Admin" y "Usuario".
        // Se contemplan también "Empleado"/"Cliente" por si el valor cambia a futuro.
        private IActionResult RedirigirSegunTipo(string? tipoUsuario)
        {
            return tipoUsuario?.ToLower() switch
            {
                "admin" => RedirectToAction("Index", "Empleado"),
                "empleado" => RedirectToAction("Index", "Empleado"),
                "usuario" => RedirectToAction("Index", "Cliente"),
                "cliente" => RedirectToAction("Index", "Cliente"),
                _ => RedirectToAction("Index", "Cliente")
            };
        }
    }
}
