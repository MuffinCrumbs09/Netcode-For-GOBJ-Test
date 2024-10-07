using UnityEngine;
using Unity.Netcode;

// Handles players health and sends Death and Alive events to server
public class PlayerHealth : NetworkBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float maxHealth;
    private float _health;

    private void Start()
    {
        _health = maxHealth;
    }

    // Contract via IDamageable
    public void Damage(float damage)
    {
        // Sets health -damage
        SetHealth(-damage);
        Debug.Log(transform.name + " took damage");
    }

    // Used to increase or decrease _health, also checks if dead.
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

    // Dead and Alive functions that get executed by everyone
    [Rpc(SendTo.Everyone)] public void DeadRpc() => gameObject.SetActive(false);
    [Rpc(SendTo.Everyone)] public void AliveRpc() => gameObject.SetActive(true);

}