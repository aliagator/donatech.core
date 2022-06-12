using System;
using Donatech.Core.Model;

namespace Donatech.Core.Utils
{
	public class JwtSessionUtils
	{
		public static void RemoveJwtTokenAndSession(HttpContext context)
        {
			context.Session.Clear();		
        }

		public static UsuarioDto? GetCurrentUserSession(HttpContext context)
        {
			try
			{
				return context.Session.Get<UsuarioDto>(Constants.UserSessionContextId);				
			}
			catch
			{				
				context.Response.Redirect("/Account/Login?c=401");
				return null;
			}
        }
	}
}

