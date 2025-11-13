namespace Core.LibrariesApplication;

public interface ITraceContextAccessor
{
    void SetTrace(TraceContext context);
    TraceContext? GetTrace();
}
