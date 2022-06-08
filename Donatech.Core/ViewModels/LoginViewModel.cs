using System;
using System.ComponentModel.DataAnnotations;

namespace Donatech.Core.ViewModels
{
	public class LoginViewModel
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar un email")]
		[EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
		public string Email { get; set; } = string.Empty;

		[Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar una contraseña")]		
		public string Password { get; set; } = string.Empty;
	}
}

