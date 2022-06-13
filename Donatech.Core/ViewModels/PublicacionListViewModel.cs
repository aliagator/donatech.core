using System;
using System.ComponentModel.DataAnnotations;
using Donatech.Core.Model;

namespace Donatech.Core.ViewModels
{
	public class PublicacionListViewModel
	{		
		public string? TextSearch { get; set; } = string.Empty;
		public List<ProductoDto>? PublicacionList { get; set; } = null;
	}
}

