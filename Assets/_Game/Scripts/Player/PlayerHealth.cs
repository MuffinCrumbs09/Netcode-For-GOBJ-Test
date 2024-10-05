using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float maxHealth;
    private float _health;

    private void Start()
    {
        _health = maxHealth;
    }

    public void Damage(float damage)
    {
        SetHealth(-damage);
    }

    private void SetHealth(float amount)
    {
        _health += amount;
        if(_health <= 0)
        {
            DeadRpc();
        }
    }

    [Rpc(SendTo.NotMe)] public void DeadRpc() => gameObject.SetActive(false);

}
