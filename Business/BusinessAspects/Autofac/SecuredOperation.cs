using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using Business.Constants;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;


namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation : MethodInterception
    {
        private readonly string[] _roles;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            var userId = user?.Claims.FirstOrDefault(x => x.Type.EndsWith("userId"))?.Value;

            if (userId == null)
            {
                throw new UnauthorizedAccessException(Message.AuthorizationDenied);
            }

            var roleClaims = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            foreach (var role in _roles)
            {
                if (roleClaims!=null && roleClaims.Contains(role))
                {
                    return;
                }
            }

            throw new UnauthorizedAccessException(Message.AuthorizationDenied);
        }


    }
}
