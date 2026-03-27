using Gameplay.Artifacts;
using Gameplay.Player.Factory;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ArtifactRegistry _artifactRegistry;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ArtifactCollectionService>().AsSingle().WithArguments(_artifactRegistry);
            Container.Bind<IPlayerFactory>().To<PhotonPlayerFactory>().AsSingle();
        }
    }
}
