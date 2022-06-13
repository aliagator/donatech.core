using System;
using Donatech.Core.Model;

namespace Donatech.Core.ViewModels
{
	public class DetallePublicacionViewModel
	{
		public long? ProductoId { get; set; }
		public ProductoDto? DetalleProducto { get; set; }
		public UsuarioDto? DetalleContacto { get; set; }
	}
}

