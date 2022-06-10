using System;
using System.ComponentModel.DataAnnotations;

namespace Donatech.Core.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "Debe ingresar un email")]
		[EmailAddress(ErrorMessage = "Debe ingresar un email válido")]
		public string Email { get; set; } = String.Empty;
	}
}

