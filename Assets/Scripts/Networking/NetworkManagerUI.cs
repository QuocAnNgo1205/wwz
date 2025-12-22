using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

namespace CoopZombieShooter.Networking
{
    /// <summary>
    /// Handles UI for starting/joining networked games.
    /// Manages host and client connection logic.
    /// </summary>
    public class NetworkManagerUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button serverButton;
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private TextMeshProUGUI statusText;

        [Header("Network Settings")]
        [SerializeField] private string defaultIP = "127.0.0.1";
        [SerializeField] private ushort defaultPort = 7777;

        private NetworkManager networkManager;

        private void Awake()
        {
            networkManager = GetComponent<NetworkManager>();
            
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager component not found! Please attach this script to the NetworkManager GameObject.");
                enabled = false;
                return;
            }

            SetupButtonListeners();
            UpdateStatusText("Ready to connect");
        }

        private void SetupButtonListeners()
        {
            if (hostButton != null)
                hostButton.onClick.AddListener(StartHost);
            
            if (clientButton != null)
                clientButton.onClick.AddListener(StartClient);
            
            if (serverButton != null)
                serverButton.onClick.AddListener(StartServer);
        }

        /// <summary>
        /// Starts the game as Host (Server + Client combined).
        /// Best for co-op games where one player hosts.
        /// </summary>
        private void StartHost()
        {
            if (networkManager.StartHost())
            {
                Debug.Log("Host started successfully");
                UpdateStatusText("Hosting game...");
                HideMenu();
                SubscribeToNetworkEvents();
            }
            else
            {
                Debug.LogError("Failed to start host");
                UpdateStatusText("Failed to start host!");
            }
        }

        /// <summary>
        /// Starts the game as Client (connects to existing host).
        /// </summary>
        private void StartClient()
        {
            // Configure connection data
            var transport = networkManager.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            if (transport != null)
            {
                transport.ConnectionData.Address = defaultIP;
                transport.ConnectionData.Port = defaultPort;
            }

            if (networkManager.StartClient())
            {
                Debug.Log("Client started successfully");
                UpdateStatusText("Connecting to host...");
                HideMenu();
                SubscribeToNetworkEvents();
            }
            else
            {
                Debug.LogError("Failed to start client");
                UpdateStatusText("Failed to connect!");
            }
        }

        /// <summary>
        /// Starts dedicated server (no local player).
        /// Optional for testing; typically use Host for co-op.
        /// </summary>
        private void StartServer()
        {
            if (networkManager.StartServer())
            {
                Debug.Log("Server started successfully");
                UpdateStatusText("Server running...");
                HideMenu();
                SubscribeToNetworkEvents();
            }
            else
            {
                Debug.LogError("Failed to start server");
                UpdateStatusText("Failed to start server!");
            }
        }

        private void SubscribeToNetworkEvents()
        {
            networkManager.OnClientConnectedCallback += OnClientConnected;
            networkManager.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"Client connected: {clientId}");
            
            if (NetworkManager.Singleton.IsServer)
            {
                UpdateStatusText($"Players connected: {NetworkManager.Singleton.ConnectedClients.Count}/4");
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"Client disconnected: {clientId}");
            
            if (NetworkManager.Singleton.IsServer)
            {
                UpdateStatusText($"Players connected: {NetworkManager.Singleton.ConnectedClients.Count}/4");
            }
            else if (clientId == NetworkManager.Singleton.LocalClientId)
            {
                // Local client was disconnected
                UpdateStatusText("Disconnected from host");
                ShowMenu();
            }
        }

        private void UpdateStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[Network Status] {message}");
        }

        private void HideMenu()
        {
            if (menuPanel != null)
                menuPanel.SetActive(false);
        }

        private void ShowMenu()
        {
            if (menuPanel != null)
                menuPanel.SetActive(true);
        }

        private void OnDestroy()
        {
            // Cleanup event subscriptions
            if (networkManager != null)
            {
                networkManager.OnClientConnectedCallback -= OnClientConnected;
                networkManager.OnClientDisconnectCallback -= OnClientDisconnected;
            }

            // Cleanup button listeners
            if (hostButton != null)
                hostButton.onClick.RemoveAllListeners();
            
            if (clientButton != null)
                clientButton.onClick.RemoveAllListeners();
            
            if (serverButton != null)
                serverButton.onClick.RemoveAllListeners();
        }

        // Editor helper methods
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (networkManager == null)
                networkManager = GetComponent<NetworkManager>();
        }
        #endif
    }
}