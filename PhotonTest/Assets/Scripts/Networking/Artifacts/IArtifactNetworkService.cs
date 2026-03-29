namespace Networking.Artifacts
{
    public interface IArtifactNetworkService
    {
        void ApplyInitialState();
        void RequestCollect(int artifactId);
    }
}
