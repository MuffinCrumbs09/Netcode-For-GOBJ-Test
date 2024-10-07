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
        Debug.Log(transform.name + " took damage");
    }

    private void SetHealth(float amount)
    {
        _health += amount;
        if(_health <= 0)
        {
            DeadRpc();
            transform.position = new Vector3(0, 1, 0);
            AliveRpc();
        }
    }

    [Rpc(SendTo.Everyone)] public void DeadRpc() => gameObject.SetActive(false);
    [Rpc(SendTo.Everyone)] public void AliveRpc() => gameObject.SetActive(true);

}
