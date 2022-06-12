using System;
using System.Collections.Generic;

namespace Donatech.Core.Model.DbModels
{
    public partial class LogRequest
    {
        public long Id { get; set; }
        public string Url { get; set; } = null!;
        public DateTime FchRequest { get; set; }
        public string? Username { get; set; }
    }
}
