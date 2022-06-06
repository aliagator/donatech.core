using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Donatech.Core.Model
{
    /// <summary>
    /// Clase DTO de la tabla Comuna de la base de datos
    /// </summary>
    [Serializable]
    public class ComunaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}