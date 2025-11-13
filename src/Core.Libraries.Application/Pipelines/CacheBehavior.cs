using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Core.Libraries.Application.Pipelines;

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheBehavior<TRequest, TResponse>> _logger;
    private const int DefaultCacheSeconds = 300;

    public CacheBehavior(IMemoryCache cache, ILogger<CacheBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheableQuery)
            return await next();

        var cacheKey = GenerateCacheKey(request);

        if (_cache.TryGetValue(cacheKey, out TResponse? cached))
        {
            _logger.LogInformation("Cache hit: {Key}", cacheKey);
            return cached!;
        }

        var response = await next();

        _logger.LogInformation("Cache miss: {Key}. Caching for {Seconds}s", cacheKey, DefaultCacheSeconds);
        _cache.Set(cacheKey, response, TimeSpan.FromSeconds(DefaultCacheSeconds));

        return response;
    }

    private string GenerateCacheKey(TRequest request)
    {
        var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
        var hashString = Convert.ToHexString(hash);

        return $"{typeof(TRequest).FullName}_{hashString}";
    }
}


public interface ICacheableQuery
{

}

