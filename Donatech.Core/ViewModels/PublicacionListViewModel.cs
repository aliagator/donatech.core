using System;
using System.ComponentModel.DataAnnotations;
using Donatech.Core.Model;

namespace Donatech.Core.ViewModels
{
	public class PublicacionListViewModel
	{
		[Required(ErrorMessage = "Debe ingresar un texto para buscar publicaciones")]
		public string TextSearch { get; set; } = string.Empty;
		public List<ProductoDto>? PublicacionList { get; set; } = null;
	}
}

