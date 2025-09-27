using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    // Backing field for sword skill.
    // NOTE: "readonly" here means we cannot reassign _swordSkill to a new object,
    // but we CAN still change its internal Value. This is a common pattern in NGO.
    private readonly NetworkVariable<int> _swordSkill =
        new NetworkVariable<int>(
            0, // Default value
            NetworkVariableReadPermission.Everyone,  // Anyone can read this value
            NetworkVariableWritePermission.Owner     // Only the owning client can change it
        );

    // Public property for controlled access.
    // External scripts can read this stat, but only this class can set it.
    public int SwordSkill
    {
        get => _swordSkill.Value;
        private set
        {
            if (!IsOwner) return; // Enforce ownership: only the local player can modify their own stats.

            // Example validation: clamp to valid range
            int clamped = Mathf.Clamp(value, 0, 999);
            _swordSkill.Value = clamped;
        }
    }

    // Safely increase the stat by a given amount.
    public void IncreaseSwordSkill(int amount = 1)
    {
        if (!IsOwner) return; // Prevent other clients from messing with your stats
        SwordSkill += amount;
        Debug.Log($"Sword skill increased. New value: {SwordSkill}");
    }

    // Subscribe to changes when this object spawns over the network.
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            _swordSkill.OnValueChanged += HandleSwordSkillChanged;
        }
    }

    // Unsubscribe when destroyed (prevents memory leaks if player leaves).
    public override void OnDestroy()
    {
        if (IsClient)
        {
            _swordSkill.OnValueChanged -= HandleSwordSkillChanged;
        }
    }

    // Named callback for when sword skill changes across the network.
    private void HandleSwordSkillChanged(int oldValue, int newValue)
    {
        Debug.Log($"Sword skill changed from {oldValue} → {newValue}");

        // Future: Update HUD, trigger animations, play sounds, etc.
    }
}
