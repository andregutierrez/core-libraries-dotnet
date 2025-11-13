namespace Core.LibrariesApplication.Commands;

public interface IHasTraceContext
{
    TraceContext? GetTrace();
}
