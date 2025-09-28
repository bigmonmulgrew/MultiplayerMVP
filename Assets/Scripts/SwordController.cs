using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Handles sword attack input and animation. 
/// Must be attached to the Player Armature prefab (same object as Animator).
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerStats))]
[DisallowMultipleComponent]
public class SwordController : NetworkBehaviour
{
    private Animator _animator;
    private PlayerStats _stats;

    // Cache Animator parameter IDs for performance
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetMouseButtonDown(0)) // Left mouse click
            TriggerAttack();
    }

    /// <summary>
    /// Sends the attack trigger to the Animator.
    /// </summary>
    private void TriggerAttack()
    {
        _animator.SetTrigger(AttackTrigger);
    }

    /// <summary>
    /// Called by the sword swing animation event.
    /// Increments stats and handles gameplay logic at hit frame.
    /// </summary>
    public void OnSwordHit()
    {
        if (!IsOwner) return;

        _stats.IncreaseSwordSkill();
        Debug.Log($"[{OwnerClientId}] OnSwordHit → Sword skill = {_stats.SwordSkill}");
    }
}
