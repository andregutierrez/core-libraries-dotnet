namespace Core.LibrariesApplication;

public interface IClientMetadataAccessor
{
    void SetClient(ClientMetadata context);
    ClientMetadata? GetClient();
}