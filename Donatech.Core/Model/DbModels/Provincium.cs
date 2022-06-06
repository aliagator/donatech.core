using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Provincia
    {
        public Provincia()
        {
            Comunas = new HashSet<Comuna>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdRegion { get; set; }

        public virtual Region? IdRegionNavigation { get; set; }
        public virtual ICollection<Comuna> Comunas { get; set; }
    }
}
