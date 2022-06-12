using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Donatech.Core.Model
{
    /// <summary>
    /// Clase DTO de la tabla Mensaje de la base de datos
    /// </summary>
    public class MensajeDto
    {
        public long Id { get; set; }
        public int IdEmisor { get; set; }
        public UsuarioDto? DatosEmisor { get; set; } = null!;
        public int IdReceptor { get; set; }
        public UsuarioDto? DatosReceptor { get; set; } = null!;
        public System.DateTime FchEnvio { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public int IdProducto { get; set; }
        public bool Enabled { get; set; }
        public bool SesionEmisor { get; set; }
    }

    public class FilterMensajeDto
    {
        public int? UsuarioSession { get; set; }
        public int? IdProducto { get; set; }
    }
}