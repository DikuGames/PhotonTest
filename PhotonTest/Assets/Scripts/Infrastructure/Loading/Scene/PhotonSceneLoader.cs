using Photon.Pun;
using UnityEngine.SceneManagement;

namespace Infrastructure.Loading.Scene
{
    public class PhotonSceneLoader : ISceneLoader
    {
        public void Load(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LoadLevel(sceneName);
                return;
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}
