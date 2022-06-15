using System;
namespace Donatech.Core.Utils
{
	public class EnumUtils
	{
		public enum RolEnum
        {
			Oferente = 1,
			Demandante = 2,
			Administrador = 3
        };

		public static readonly (int Id, string Descripcion)[] Roles = {
			new (1, "Oferente"),
			new (2, "Demandante"),
			new (3, "Administrador")
		};
	}
}

