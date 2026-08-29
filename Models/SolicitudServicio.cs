using System.ComponentModel.DataAnnotations;

namespace evaluacion20262.Models;

public class SolicitudServicio
{
    public int Id { get; set; }

    [Required]
    public required string Cliente { get; set; }

    [Required]
    public required string Telefono { get; set; }

    [Required]
    public required string Distrito { get; set; }

    [Required]
    public required string TipoServicio { get; set; }

    [Required]
    public required string Descripcion { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}