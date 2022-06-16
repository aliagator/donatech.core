using System;
using System.ComponentModel.DataAnnotations;
using Donatech.Core.Model;

namespace Donatech.Core.ViewModels
{
	public class NuevaPublicacionViewModel
	{
        [Required(ErrorMessage = "Debe seleccionar un Tipo")]
        public int IdTipo { get; set; }
        public List<TipoProductoDto>? TipoProductoList { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Debe ingresar un Titulo")]
        [MaxLength(30, ErrorMessage = "El Título no puede ser mayor a {0} caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar una Descripción")]
        [MaxLength(300, ErrorMessage = "La Descripción no puede ser mayor a {0} caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;
        public List<string>? EstadoList { get; set; }

        [Display(Name = "Imagen")]
        public byte[]? Imagen { get; set; } = null!;
        public string? ImagenBase64 { get; set; } = null!;
        public string? ImagenMimeType { get; set; } = null!;

        public int IdOferente { get; set; }
        public UsuarioDto? Oferente { get; set; } = null!;
        public int? IdDemandante { get; set; }
        public UsuarioDto? Demandante { get; set; } = null!;
        public System.DateTime FchPublicacion { get; set; }

        public bool? PublicacionRegistrada { get; set; }
    }
}

