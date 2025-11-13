namespace Core.Libraries.Application;

public interface IClientMetadataAccessor
{
    void SetClient(ClientMetadata context);
    ClientMetadata? GetClient();
}