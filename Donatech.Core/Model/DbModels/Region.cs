using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Region
    {
        public Region()
        {
            Provincia = new HashSet<Provincia>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public virtual ICollection<Provincia> Provincia { get; set; }
    }
}
