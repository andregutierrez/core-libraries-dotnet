using Core.Libraries.Application;

namespace Core.Libraries.Application.Commands;

public interface IHasClientMetadata
{
    ClientMetadata? GetClient();
}
