using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public sealed class LobbyView : MonoBehaviour
    {
        public event Action OnConnectClicked;

        [SerializeField] private Button _connectButton;

        private void Awake()
        {
            if (_connectButton != null)
            {
                _connectButton.onClick.AddListener(ConnectClicked);
            }
        }

        private void OnDestroy()
        {
            if (_connectButton != null)
            {
                _connectButton.onClick.RemoveListener(ConnectClicked);
            }
        }

        private void ConnectClicked()
        {
            OnConnectClicked?.Invoke();
        }
    }
}
