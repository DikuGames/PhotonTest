namespace Networking.Artifacts
{
    public interface IArtifactNetworkService
    {
        bool ApplyInitialState();
        void RequestCollect(int artifactId);
    }
}
