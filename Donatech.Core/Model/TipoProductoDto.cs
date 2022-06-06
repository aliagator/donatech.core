using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Donatech.Core.Model
{
    /// <summary>
    /// Clase DTO de la tabla TipoProducto de la base de datos
    /// </summary>
    [Serializable]
    public class TipoProductoDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }
}