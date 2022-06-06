using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Producto
    {
        public Producto()
        {
            Mensajes = new HashSet<Mensaje>();
        }

        public int Id { get; set; }
        public int IdTipo { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public byte[] Imagen { get; set; } = null!;
        public string ImagenMimeType { get; set; } = null!;
        public int IdOferente { get; set; }
        public int? IdDemandante { get; set; }
        public DateTime FchPublicacion { get; set; }
        public DateTime? FchFinalizacion { get; set; }
        public bool Enabled { get; set; }

        public virtual Usuario? IdDemandanteNavigation { get; set; }
        public virtual Usuario IdOferenteNavigation { get; set; } = null!;
        public virtual TipoProducto IdTipoNavigation { get; set; } = null!;
        public virtual ICollection<Mensaje> Mensajes { get; set; }
    }
}
