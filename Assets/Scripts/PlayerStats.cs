using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Network-synced player stats. 
/// Must be attached to the Player Armature prefab.
/// </summary>
[DisallowMultipleComponent]
public class PlayerStats : NetworkBehaviour
{
    // Backing field for sword skill.
    // NOTE: "readonly" prevents reassignment of the NetworkVariable,
    // but the internal Value can still be changed across the network.
    private readonly NetworkVariable<int> _swordSkill =
        new NetworkVariable<int>(
            0,  // Default value
            NetworkVariableReadPermission.Everyone,  // Any client can read
            NetworkVariableWritePermission.Owner     // Only the owner can write
        );

    /// <summary>
    /// Encapsulated access to sword skill.
    /// Other classes can read this, but only this class can change it.
    /// </summary>
    public int SwordSkill
    {
        get => _swordSkill.Value;
        private set
        {
            if (!IsOwner) return;

            // Validation example: clamp values between 0–999
            int clamped = Mathf.Clamp(value, 0, 999);
            _swordSkill.Value = clamped;
        }
    }

    /// <summary>
    /// Safely increase sword skill by a given amount.
    /// Called by gameplay events like sword hits.
    /// </summary>
    public void IncreaseSwordSkill(int amount = 1)
    {
        if (!IsOwner) return;

        SwordSkill += amount;
        Debug.Log($"[{OwnerClientId}] Sword skill increased → {SwordSkill}");
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
            _swordSkill.OnValueChanged += HandleSwordSkillChanged;
    }

    public override void OnDestroy()
    {
        if (IsClient)
            _swordSkill.OnValueChanged -= HandleSwordSkillChanged;
    }

    /// <summary>
    /// Called whenever sword skill changes across the network.
    /// Extend this for HUD updates, audio, etc.
    /// </summary>
    private void HandleSwordSkillChanged(int oldValue, int newValue)
    {
        Debug.Log($"Sword skill changed from {oldValue} → {newValue}");
    }
}
