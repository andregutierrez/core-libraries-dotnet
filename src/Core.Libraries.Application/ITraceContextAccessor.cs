namespace Core.Libraries.Application;

public interface ITraceContextAccessor
{
    void SetTrace(TraceContext context);
    TraceContext? GetTrace();
}
