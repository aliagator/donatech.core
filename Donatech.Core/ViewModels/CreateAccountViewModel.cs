using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Donatech.Core.ViewModels
{
	public class CreateAccountViewModel
	{
		[Required(ErrorMessage = "Debe ingresar un Run")]
		[MaxLength(10, ErrorMessage = "El Run no puede superar los {1} caracteres")]
		[Display(Name = "Run")]
		[Remote("CheckRun", "Account", ErrorMessage = "El Run ingresado ya existe en el sistema", HttpMethod = "Get")]
		public string Run { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe ingresar un Nombre")]
		[MaxLength(50, ErrorMessage = "El Nombre no puede superar los {1} caracteres")]
		[Display(Name = "Nombre")]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe ingresar un Apellido")]
		[MaxLength(80, ErrorMessage = "El Apellido no puede superar los {1} caracteres")]
		[Display(Name = "Apellido")]
		public string Apellidos { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe ingresar un Email")]
		[MaxLength(50, ErrorMessage = "El Email no puede superar los {1} caracteres")]
		[EmailAddress(ErrorMessage = "Debe ingresar un Email válido")]
		[Display(Name = "Email")]
		[Remote("CheckEmail", "Account", ErrorMessage = "El Email ingresado ya existe en el sistema", HttpMethod = "Get")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe ingresar una Dirección")]
		[MaxLength(100, ErrorMessage = "La Dirección no puede superar los {1} caracteres")]
		[Display(Name = "Direccion")]
		public string Direccion { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe seleccionar una Comuna")]
		[Display(Name = "Comuna")]
		public int Comuna { get; set; } = 0;

		[Required(ErrorMessage = "Debe ingresar una Contraseña")]
		[MaxLength(10, ErrorMessage = "La Contraseña no puede superar los {1} caracteres")]
		[Display(Name = "Contraseña")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe confirmar la Contraseña")]
		[Compare("Password", ErrorMessage = "Las Contraseñas no coinciden")]
		[Display(Name = "Confirmar Contraseña")]
		public string RePassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe ingresar un Teléfono")]
		[MaxLength(12, ErrorMessage = "El Teléfono no puede superar los {1} caracteres")]
		[Display(Name = "Teléfono")]
		public string Telefono { get; set; } = string.Empty;

		[Required(ErrorMessage = "Debe selecciona un Rol")]
		[Display(Name = "Rol")]
		public int Rol { get; set; } = 0;
	}
}

