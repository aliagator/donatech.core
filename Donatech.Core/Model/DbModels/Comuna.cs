using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Comuna
    {
        public Comuna()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdProvincia { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
