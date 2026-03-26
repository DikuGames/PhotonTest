using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService, IInitializable
    {
        private const string PlayerConfigPath = "Configs/Player/PlayerConfig";
        
        public PlayerConfig PlayerConfig { get; private set; }
        
        public void Initialize()
        {
            LoadPlayerConfig();
        }

        private void LoadPlayerConfig()
        {
            PlayerConfig = Resources.Load<PlayerConfig>(PlayerConfigPath);
        }
    }
}