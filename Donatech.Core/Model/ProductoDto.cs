using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Donatech.Core.Model
{
    /// <summary>
    /// Clase DTO de la tabla Producto de la base de datos
    /// </summary>
    [Serializable]
    public class ProductoDto
    {
        public int Id { get; set; }
        public int IdTipo { get; set; }
        public TipoProductoDto? TipoProducto { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string ImagenBase64 { get; set; } = string.Empty;
        public byte[] Imagen { get; set; } = null!;
        public string ImagenMimeType { get; set; } = string.Empty;
        public int IdOferente { get; set; }
        public UsuarioDto? Oferente { get; set; } = null!;
        public int? IdDemandante { get; set; }
        public UsuarioDto? Demandante { get; set; } = null!;
        public System.DateTime FchPublicacion { get; set; }
        public Nullable<System.DateTime> FchFinalizacion { get; set; }
        public bool Enabled { get; set; }
        public string UrlContacto { get; set; } = string.Empty;
        public int Index { get; set; }
        public string? CardDeckHeaderHtml { get; set; } = null!;
        public string? CardDeckFooterHtml { get; set; } = null!;
    }
}