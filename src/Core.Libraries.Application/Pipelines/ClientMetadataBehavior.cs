using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Libraries.Application.Commands;

namespace Core.Libraries.Application.Pipelines;

public class ClientMetadataBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClientMetadataBehavior(IHttpContextAccessor httpContextAccessor)
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
            var context = _httpContextAccessor.HttpContext;

            var ip = context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = context?.Request.Headers["User-Agent"].FirstOrDefault();
            var origin = context?.Request.Headers["Origin"].FirstOrDefault();
            var deviceId = context?.Request.Headers["X-Device-Id"].FirstOrDefault();
            var sessionId = context?.Request.Cookies["sid"];

            var metadata = new ClientMetadata(
                ip,
                userAgent,
                origin,
                deviceId,
                sessionId
            );

            baseCommand.SetClient(metadata);
        }

        return await next();
    }
}
