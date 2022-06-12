using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class Provincia
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdRegion { get; set; }
    }
}
