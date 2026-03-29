using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class LobbyView : MonoBehaviour
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

        public void SetConnectButtonInteractable(bool isInteractable)
        {
            if (_connectButton == null)
            {
                return;
            }

            _connectButton.interactable = isInteractable;
        }

        private void ConnectClicked()
        {
            OnConnectClicked?.Invoke();
        }
    }
}
