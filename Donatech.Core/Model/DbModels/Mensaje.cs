using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Mensaje
    {
        public long Id { get; set; }
        public int IdEmisor { get; set; }
        public int IdReceptor { get; set; }
        public DateTime FchEnvio { get; set; }
        public string Mensaje1 { get; set; } = null!;
        public bool Enabled { get; set; }
        public int IdProducto { get; set; }

        public virtual Usuario IdEmisorNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
        public virtual Usuario IdReceptorNavigation { get; set; } = null!;
    }
}
