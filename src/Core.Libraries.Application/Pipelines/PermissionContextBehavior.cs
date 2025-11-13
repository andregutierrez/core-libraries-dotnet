using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Core.LibrariesApplication.Commands;

namespace Core.LibrariesApplication.Behavior;

public class PermissionContextBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionContextBehavior(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is BaseCommand baseCommand)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var userId = user.FindFirst("sub")?.Value
                             ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? throw new UnauthorizedAccessException("Usuário sem ID");

                var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

                var permissions = user.FindAll("permission").Select(p => p.Value).ToArray(); // se houver claims customizadas

                var context = new PermissionContext(userId, roles, permissions);

                baseCommand.SetPermission(context);
            }
            else
            {
                throw new UnauthorizedAccessException("Usuário não autenticado");
            }
        }

        return await next();
    }
}
