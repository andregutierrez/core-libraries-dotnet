namespace Core.Libraries.Application.Commands;

public interface IHasTraceContext
{
    TraceContext? GetTrace();
}
