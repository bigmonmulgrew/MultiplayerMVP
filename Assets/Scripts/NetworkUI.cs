using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

/// <summary>
/// Simple UI for starting/stopping a Netcode session.
/// Must be placed on a Canvas in the scene with Host, Join, and Quit buttons assigned.
/// </summary>
[DisallowMultipleComponent]
public class NetworkUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        if (hostButton != null) hostButton.onClick.AddListener(OnHostClicked);
        if (joinButton != null) joinButton.onClick.AddListener(OnJoinClicked);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnDestroy()
    {
        if (hostButton != null) hostButton.onClick.RemoveListener(OnHostClicked);
        if (joinButton != null) joinButton.onClick.RemoveListener(OnJoinClicked);
        if (quitButton != null) quitButton.onClick.RemoveListener(OnQuitClicked);
    }

    /// <summary>
    /// Starts hosting the game. This will spawn the local player as host.
    /// </summary>
    private void OnHostClicked()
    {
        NetworkManager.Singleton.StartHost();
        Debug.Log("Started as Host");
    }

    /// <summary>
    /// Starts a client and attempts to join an existing host.
    /// </summary>
    private void OnJoinClicked()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Started as Client");
    }

    /// <summary>
    /// Exits the application. Works only in a build.
    /// In the editor, we log a message instead.
    /// </summary>
    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        Debug.Log("Quit button clicked (only works in build).");
#else
        Application.Quit();
#endif
    }
}
