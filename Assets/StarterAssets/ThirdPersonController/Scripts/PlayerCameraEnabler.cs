using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Ensures only the local player's camera is active in multiplayer.
/// Attach this to the Player prefab root.
/// </summary>
public class PlayerCameraEnabler : NetworkBehaviour
{
    [SerializeField] private GameObject playerCamera;

    public override void OnNetworkSpawn()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("PlayerCameraEnabler: No camera assigned!");
            return;
        }

        // Only enable the local owner's camera
        playerCamera.SetActive(IsOwner);
    }
}
