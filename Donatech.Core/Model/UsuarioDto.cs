using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Donatech.Core.Model
{
    /// <summary>
    /// Clase DTO de la tabla Usuario de la base de datos
    /// </summary>
    [Serializable]
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Iniciales { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;        
        public string Run { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public int IdComuna { get; set; }
        public ComunaDto? Comuna { get; set; } = null!;
        public string Password { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool Enabled { get; set; }
        public string Celular { get; set; } = string.Empty;
        public bool? Validated { get; set; }
        public string? AccountToken { get; set; }
        public Lazy<string> NombreCompleto
        {
            get
            {
                return new Lazy<string>($"{this.Nombre} {this.Apellidos}");
            }
        }
    }
}