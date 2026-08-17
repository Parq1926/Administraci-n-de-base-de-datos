using Microsoft.AspNetCore.Mvc;
using SistemaDeDonaciones.Models;
using SistemaDeDonaciones.Models.Api;
using SistemaDeDonaciones.Services;

namespace SistemaDeDonaciones.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly FundacionApiService _fundacionApi;
        private readonly ProyectoApiService _proyectoApi;
        private readonly DonanteApiService _donanteApi;
        private readonly DonacionApiService _donacionApi;
        private readonly AlertaApiService _alertaApi;
        private readonly MovimientoApiService _movimientoApi;
        private readonly CodigoDonacionApiService _codigoApi;

        public EmpleadoController(
            FundacionApiService fundacionApi,
            ProyectoApiService proyectoApi,
            DonanteApiService donanteApi,
            DonacionApiService donacionApi,
            AlertaApiService alertaApi,
            MovimientoApiService movimientoApi,
            CodigoDonacionApiService codigoApi)
        {
            _fundacionApi = fundacionApi;
            _proyectoApi = proyectoApi;
            _donanteApi = donanteApi;
            _donacionApi = donacionApi;
            _alertaApi = alertaApi;
            _movimientoApi = movimientoApi;
            _codigoApi = codigoApi;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "¡Bienvenida de nuevo!";
            ViewData["Subtitle"] = "Resumen de la actividad de la plataforma";

            var modelo = new EmpleadoIndexViewModel();

            try
            {
                var proyectosTask = _proyectoApi.ObtenerProyectosAsync();
                var donantesTask = _donanteApi.ObtenerDonantesAsync();
                var donacionesTask = _donacionApi.ObtenerDonacionesAsync();
                await Task.WhenAll(proyectosTask, donantesTask, donacionesTask);

                modelo.Proyectos = proyectosTask.Result;
                modelo.Donantes = donantesTask.Result;
                modelo.Donaciones = donacionesTask.Result;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = "No se pudo cargar el resumen del panel. Detalle: " + ex.Message;
            }

            return View(modelo);
        }

        // ============================================
        // DONANTES
        // ============================================

        [HttpGet]
        public async Task<IActionResult> Donantes(string? buscar, string? tipoDonante, bool? activo)
        {
            ViewData["Title"] = "Donantes";
            ViewData["Subtitle"] = "Mantenimiento de personas y empresas donantes";

            try
            {
                var donantes = await _donanteApi.ObtenerDonantesAsync();

                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    donantes = donantes
                        .Where(d => d.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                                 || (d.Apellidos?.Contains(buscar, StringComparison.OrdinalIgnoreCase) ?? false)
                                 || d.Email.Contains(buscar, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(tipoDonante))
                {
                    donantes = donantes
                        .Where(d => string.Equals(d.TipoDonante, tipoDonante, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (activo.HasValue)
                {
                    donantes = donantes.Where(d => d.Activo == activo).ToList();
                }

                ViewData["Buscar"] = buscar;
                ViewData["TipoDonanteFiltro"] = tipoDonante;
                ViewData["ActivoFiltro"] = activo;

                return View(donantes);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<Donante>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDonante(DonanteInput modelo)
        {
            try
            {
                var respuesta = await _donanteApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Donantes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDonante(DonanteUpdateInput modelo)
        {
            try
            {
                var respuesta = await _donanteApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Donantes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDonante(int idDonante)
        {
            try
            {
                var respuesta = await _donanteApi.EliminarAsync(idDonante);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Donantes));
        }

        // ============================================
        // FUNDACIONES
        // ============================================

        [HttpGet]
        public async Task<IActionResult> Fundaciones(string? buscar, bool? activo)
        {
            ViewData["Title"] = "Fundaciones";
            ViewData["Subtitle"] = "Mantenimiento de fundaciones aliadas";

            try
            {
                var fundaciones = await _fundacionApi.ObtenerFundacionesAsync();

                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    fundaciones = fundaciones
                        .Where(f => f.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                                 || (f.Email?.Contains(buscar, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                if (activo.HasValue)
                {
                    fundaciones = fundaciones.Where(f => f.Activo == activo).ToList();
                }

                ViewData["Buscar"] = buscar;
                ViewData["ActivoFiltro"] = activo;

                return View(fundaciones);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<Fundacion>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearFundacion(FundacionInput modelo)
        {
            try
            {
                var respuesta = await _fundacionApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Fundaciones));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarFundacion(FundacionUpdateInput modelo)
        {
            try
            {
                var respuesta = await _fundacionApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Fundaciones));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarFundacion(int idFundacion)
        {
            try
            {
                var respuesta = await _fundacionApi.EliminarAsync(idFundacion);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Fundaciones));
        }

        // ============================================
        // PROYECTOS
        // ============================================

        [HttpGet]
        public async Task<IActionResult> Proyectos(string? buscar, int? idFundacion, string? estado)
        {
            ViewData["Title"] = "Proyectos";
            ViewData["Subtitle"] = "Mantenimiento de proyectos y metas de recaudación";

            try
            {
                var proyectosTask = _proyectoApi.ObtenerProyectosAsync();
                var fundacionesTask = _fundacionApi.ObtenerFundacionesAsync();
                await Task.WhenAll(proyectosTask, fundacionesTask);

                var proyectos = proyectosTask.Result;

                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    proyectos = proyectos
                        .Where(p => p.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (idFundacion.HasValue)
                {
                    proyectos = proyectos.Where(p => p.IdFundacion == idFundacion).ToList();
                }

                if (!string.IsNullOrWhiteSpace(estado))
                {
                    proyectos = proyectos
                        .Where(p => string.Equals(p.Estado, estado, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                ViewData["Fundaciones"] = fundacionesTask.Result;
                ViewData["Buscar"] = buscar;
                ViewData["IdFundacionFiltro"] = idFundacion;
                ViewData["EstadoFiltro"] = estado;

                return View(proyectos);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                ViewData["Fundaciones"] = new List<Fundacion>();
                return View(new List<Proyecto>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearProyecto(ProyectoInput modelo)
        {
            try
            {
                var respuesta = await _proyectoApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Proyectos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProyecto(ProyectoUpdateInput modelo)
        {
            try
            {
                var respuesta = await _proyectoApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Proyectos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProyecto(int idProyecto)
        {
            try
            {
                var respuesta = await _proyectoApi.EliminarAsync(idProyecto);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Proyectos));
        }

        // ============================================
        // DONACIONES
        // ============================================

        [HttpGet]
        public async Task<IActionResult> Donaciones(string? estado, int? idProyecto, DateTime? desde, DateTime? hasta)
        {
            ViewData["Title"] = "Donaciones";
            ViewData["Subtitle"] = "Registro y mantenimiento de donaciones recibidas";

            try
            {
                var donacionesTask = _donacionApi.ObtenerDonacionesAsync();
                var donantesTask = _donanteApi.ObtenerDonantesAsync();
                var proyectosTask = _proyectoApi.ObtenerProyectosAsync();
                await Task.WhenAll(donacionesTask, donantesTask, proyectosTask);

                var donaciones = donacionesTask.Result
                    .OrderByDescending(d => d.FechaDonacion)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(estado))
                {
                    donaciones = donaciones
                        .Where(d => string.Equals(d.Estado, estado, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (idProyecto.HasValue)
                {
                    donaciones = donaciones.Where(d => d.IdProyecto == idProyecto).ToList();
                }

                if (desde.HasValue)
                {
                    donaciones = donaciones.Where(d => d.FechaDonacion >= desde.Value.Date).ToList();
                }

                if (hasta.HasValue)
                {
                    donaciones = donaciones.Where(d => d.FechaDonacion <= hasta.Value.Date.AddDays(1).AddTicks(-1)).ToList();
                }

                ViewData["Donantes"] = donantesTask.Result;
                ViewData["Proyectos"] = proyectosTask.Result;
                ViewData["EstadoFiltro"] = estado;
                ViewData["IdProyectoFiltro"] = idProyecto;
                ViewData["Desde"] = desde?.ToString("yyyy-MM-dd");
                ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd");

                return View(donaciones);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                ViewData["Donantes"] = new List<Donante>();
                ViewData["Proyectos"] = new List<Proyecto>();
                return View(new List<Donacion>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearDonacion(DonacionInput modelo)
        {
            try
            {
                var respuesta = await _donacionApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Donaciones));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarDonacion(DonacionUpdateInput modelo)
        {
            try
            {
                var respuesta = await _donacionApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Donaciones));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDonacion(int idDonacion)
        {
            try
            {
                var respuesta = await _donacionApi.EliminarAsync(idDonacion);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Donaciones));
        }

        // ============================================
        // ALERTAS - CRUD COMPLETO (CORREGIDO)
        // ============================================

        [HttpGet]
        public async Task<IActionResult> Alertas(string? buscar, bool? leida, string? tipoAlerta)
        {
            ViewData["Title"] = "Alertas";
            ViewData["Subtitle"] = "Notificaciones internas del sistema";

            try
            {
                var alertas = await _alertaApi.ObtenerAlertasAsync();

                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    alertas = alertas
                        .Where(a => a.Titulo.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                                 || (a.Mensaje?.Contains(buscar, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                if (leida.HasValue)
                {
                    alertas = alertas.Where(a => a.Leida == leida).ToList();
                }

                if (!string.IsNullOrWhiteSpace(tipoAlerta))
                {
                    alertas = alertas
                        .Where(a => string.Equals(a.TipoAlerta, tipoAlerta, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                ViewData["Buscar"] = buscar;
                ViewData["LeidaFiltro"] = leida;
                ViewData["TipoAlertaFiltro"] = tipoAlerta;

                return View(alertas);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<Alerta>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearAlerta(InsertarAlertaInput modelo)
        {
            try
            {
                var respuesta = await _alertaApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Alertas));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAlerta(ActualizarAlertaInput modelo)
        {
            try
            {
                var respuesta = await _alertaApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Alertas));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarLeida(int idAlerta)
        {
            try
            {
                var respuesta = await _alertaApi.MarcarLeidaAsync(idAlerta);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Alertas));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarAlerta(int idAlerta)
        {
            try
            {
                var respuesta = await _alertaApi.EliminarAsync(idAlerta);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Alertas));
        }

        // ============================================
        // MOVIMIENTOS - CRUD COMPLETO (CORREGIDO)
        // ============================================

        [HttpGet]
        public async Task<IActionResult> Movimientos(string? tipo, int? idProyecto, DateTime? desde, DateTime? hasta)
        {
            ViewData["Title"] = "Movimientos";
            ViewData["Subtitle"] = "Libro de movimientos de fondos entre proyectos";

            try
            {
                var movimientos = await _movimientoApi.ObtenerMovimientosAsync();

                if (!string.IsNullOrWhiteSpace(tipo))
                {
                    movimientos = movimientos
                        .Where(m => string.Equals(m.TipoMovimiento, tipo, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (idProyecto.HasValue)
                {
                    movimientos = movimientos.Where(m => m.IdProyecto == idProyecto).ToList();
                }

                if (desde.HasValue)
                {
                    movimientos = movimientos.Where(m => m.FechaMovimiento >= desde.Value.Date).ToList();
                }

                if (hasta.HasValue)
                {
                    movimientos = movimientos.Where(m => m.FechaMovimiento <= hasta.Value.Date.AddDays(1).AddTicks(-1)).ToList();
                }

                var proyectos = await _proyectoApi.ObtenerProyectosAsync();
                ViewData["Proyectos"] = proyectos;
                ViewData["TipoFiltro"] = tipo;
                ViewData["IdProyectoFiltro"] = idProyecto;
                ViewData["Desde"] = desde?.ToString("yyyy-MM-dd");
                ViewData["Hasta"] = hasta?.ToString("yyyy-MM-dd");

                return View(movimientos);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                ViewData["Proyectos"] = new List<Proyecto>();
                return View(new List<Movimiento>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearMovimiento(MovimientoInput modelo)
        {
            try
            {
                // ✅ CONVERTIR FECHA A UTC PARA GRAPHQL
                if (modelo.FechaMovimiento.HasValue)
                {
                    modelo.FechaMovimiento = DateTime.SpecifyKind(modelo.FechaMovimiento.Value, DateTimeKind.Utc);
                }
                else
                {
                    modelo.FechaMovimiento = DateTime.UtcNow;
                }

                var respuesta = await _movimientoApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Movimientos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarMovimiento(MovimientoUpdateInput modelo)
        {
            try
            {
                // ✅ CONVERTIR FECHA A UTC PARA GRAPHQL
                if (modelo.FechaMovimiento.HasValue)
                {
                    modelo.FechaMovimiento = DateTime.SpecifyKind(modelo.FechaMovimiento.Value, DateTimeKind.Utc);
                }

                var respuesta = await _movimientoApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Movimientos));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarMovimiento(int idMovimiento)
        {
            try
            {
                var respuesta = await _movimientoApi.EliminarAsync(idMovimiento);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Movimientos));
        }

        // ============================================
        // CÓDIGOS DE DONACIÓN - CRUD COMPLETO
        // ============================================

        [HttpGet]
        public async Task<IActionResult> CodigosDonacion(string? buscar, int? idFundacion, bool? estado)
        {
            ViewData["Title"] = "Códigos de donación";
            ViewData["Subtitle"] = "Clasificación de donaciones por código";

            try
            {
                var codigos = await _codigoApi.ObtenerCodigosAsync();

                if (!string.IsNullOrWhiteSpace(buscar))
                {
                    codigos = codigos
                        .Where(c => c.NombreCodigo.Contains(buscar, StringComparison.OrdinalIgnoreCase)
                                 || (c.Descripcion?.Contains(buscar, StringComparison.OrdinalIgnoreCase) ?? false))
                        .ToList();
                }

                if (idFundacion.HasValue)
                {
                    codigos = codigos.Where(c => c.IdFundacion == idFundacion).ToList();
                }

                if (estado.HasValue)
                {
                    codigos = codigos.Where(c => c.Estado == estado).ToList();
                }

                var fundaciones = await _fundacionApi.ObtenerFundacionesAsync();
                ViewData["Fundaciones"] = fundaciones;
                ViewData["Buscar"] = buscar;
                ViewData["IdFundacionFiltro"] = idFundacion;
                ViewData["EstadoFiltro"] = estado;

                return View(codigos);
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
                ViewData["Fundaciones"] = new List<Fundacion>();
                return View(new List<CodigoDonacion>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCodigoDonacion(CodigoDonacionInput modelo)
        {
            try
            {
                var respuesta = await _codigoApi.InsertarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(CodigosDonacion));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCodigoDonacion(CodigoDonacionUpdateInput modelo)
        {
            try
            {
                var respuesta = await _codigoApi.ActualizarAsync(modelo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(CodigosDonacion));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarCodigoDonacion(int idCodigo)
        {
            try
            {
                var respuesta = await _codigoApi.EliminarAsync(idCodigo);
                TempData[respuesta.Exito ? "Exito" : "Error"] = respuesta.Mensaje;
            }
            catch (GraphQLException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(CodigosDonacion));
        }

        // ============================================
        // VISTAS ESTÁTICAS (sin API)
        // ============================================

        public IActionResult Usuarios()
        {
            ViewData["Title"] = "Usuarios";
            ViewData["Subtitle"] = "Cuentas de empleados y clientes del sistema";
            return View();
        }

        public IActionResult HistorialCodigos()
        {
            ViewData["Title"] = "Historial de códigos";
            ViewData["Subtitle"] = "Bitácora de auditoría";
            return View();
        }
    }
}