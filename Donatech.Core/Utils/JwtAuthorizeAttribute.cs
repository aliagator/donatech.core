using System;
using Donatech.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Donatech.Core.Utils
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class JwtAuthorizeAttribute: Attribute, IAuthorizationFilter
	{
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = context?.HttpContext?.Session.Get<UsuarioDto>(Constants.UserSessionContextId);
            Console.WriteLine($"Nombre Usuario: {user?.NombreCompleto}");

            if (user == null)
            {
                context!.Result = new RedirectResult("/Account/Login?c=401");
            }                
        }
    }
}

