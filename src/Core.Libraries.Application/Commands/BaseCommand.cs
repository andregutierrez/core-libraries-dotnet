namespace Core.LibrariesApplication.Commands;

public class BaseCommand : ICommand, IHasPermissionContext, IHasTraceContext, IHasClientMetadata
{
    private PermissionContext? _permission;
    private TraceContext? _trace;
    private ClientMetadata? _client;

    protected BaseCommand() { }

    public PermissionContext? GetPermission() => _permission;
    public TraceContext? GetTrace() => _trace;
    public ClientMetadata? GetClient() => _client;

    public void SetPermission(PermissionContext permission) => _permission = permission;
    public void SetTrace(TraceContext trace) => _trace = trace;
    public void SetClient(ClientMetadata client) => _client = client;
}

public class BaseCommand<TResponse> : ICommand<TResponse>
{
    private PermissionContext? _permission;
    private TraceContext? _trace;
    private ClientMetadata? _client;

    protected BaseCommand() { }

    public PermissionContext? GetPermission() => _permission;
    public TraceContext? GetTrace() => _trace;
    public ClientMetadata? GetClient() => _client;

    public void SetPermission(PermissionContext permission) => _permission = permission;
    public void SetTrace(TraceContext trace) => _trace = trace;
    public void SetClient(ClientMetadata client) => _client = client;
}
