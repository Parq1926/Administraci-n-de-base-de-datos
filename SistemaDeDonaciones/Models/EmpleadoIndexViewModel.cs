using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Models;
public class EmpleadoIndexViewModel
{
    public List<Proyecto> Proyectos { get; set; } = new();
    public List<Donante> Donantes { get; set; } = new();
    public List<Donacion> Donaciones { get; set; } = new();

    public int ProyectosActivos => Proyectos.Count(p => p.Activo != false
        && string.Equals(p.Estado, "Activo", StringComparison.OrdinalIgnoreCase));

    public int DonantesRegistrados => Donantes.Count(d => d.Activo != false);

    //avance porcentual
    public List<Proyecto> ProyectosDestacados =>
        Proyectos.OrderByDescending(p => p.PorcentajeAvance).Take(3).ToList();

    //Últimas donaciones registradas
    public List<Donacion> DonacionesRecientes =>
        Donaciones.OrderByDescending(d => d.FechaDonacion).Take(5).ToList();
}
