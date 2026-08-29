using System.ComponentModel.DataAnnotations;

namespace evaluacion20262.Models;

public class SolicitudServicio
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El cliente es obligatorio.")]
    public required string Cliente { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Display(Name = "Teléfono")]
    public required string Telefono { get; set; }

    [Required(ErrorMessage = "El distrito es obligatorio.")]
    public required string Distrito { get; set; }

    [Required(ErrorMessage = "El tipo de servicio es obligatorio.")]
    [Display(Name = "Tipo de servicio")]
    public required string TipoServicio { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    public required string Descripcion { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}