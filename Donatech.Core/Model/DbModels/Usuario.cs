using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Usuario
    {
        public Usuario()
        {
            MensajeIdEmisorNavigations = new HashSet<Mensaje>();
            MensajeIdReceptorNavigations = new HashSet<Mensaje>();
            ProductoIdDemandanteNavigations = new HashSet<Producto>();
            ProductoIdOferenteNavigations = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Run { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public int IdComuna { get; set; }
        public string Password { get; set; } = null!;
        public int IdRol { get; set; }
        public bool Enabled { get; set; }
        public string Celular { get; set; } = null!;

        public virtual Comuna IdComunaNavigation { get; set; } = null!;
        public virtual ICollection<Mensaje> MensajeIdEmisorNavigations { get; set; }
        public virtual ICollection<Mensaje> MensajeIdReceptorNavigations { get; set; }
        public virtual ICollection<Producto> ProductoIdDemandanteNavigations { get; set; }
        public virtual ICollection<Producto> ProductoIdOferenteNavigations { get; set; }
    }
}
