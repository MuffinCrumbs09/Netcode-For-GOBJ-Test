using Unity.Netcode;
using UnityEngine;

// Handles dealing damage to players
public class WeaponDamage : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damage = 100f;
    [SerializeField] private float bulletRange = Mathf.Infinity;
    [SerializeField] private GameObject bulletTrail;

    private Transform _cam;

    private void Start()
    {
        _cam = FindAnyObjectByType<Camera>().transform;
    }

    public void Shoot()
    {
        // If you dont own this, return
        if (!IsOwner) return;

        // Sets the gun ray from our cam, shooting forward
        Ray gunRay = new Ray(_cam.position, _cam.forward);

        // If we hit something
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, bulletRange))
        {
            //Checks if we hit something
            if (hitInfo.collider != null)
            {
                // Create a local lineRender for bulletTrail and sets their position
                GameObject lr = Instantiate(bulletTrail, transform.position, bulletTrail.transform.rotation);
                lr.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                lr.GetComponent<LineRenderer>().SetPosition(1, hitInfo.point);

                ShootRpc(hitInfo.transform.name, transform.position, hitInfo.point, 100);
            }
        }
        // Locally plays gun shot sound
        SoundManager.PlaySound(SoundType.SHOOT, 0.8f);
    }

    [Rpc(SendTo.NotMe)] // Displays line renderer and checks if local was hit
    public void ShootRpc(string hit, Vector3 startPos, Vector3 endPos, float damage = 0)
    {
        Debug.Log(hit);

        // Locally plays sound for everyone else
        SoundManager.PlaySound(SoundType.SHOOT, 0.8f);

        // Create a local lineRender for bulletTrail and sets their position for everyone else
        GameObject lr = Instantiate(bulletTrail, startPos, bulletTrail.transform.rotation);
        lr.GetComponent<LineRenderer>().SetPosition(0, startPos);
        lr.GetComponent<LineRenderer>().SetPosition(1, endPos);

        // Get the hit object, return if null
        GameObject _hit = GameObject.Find(hit);
        if (_hit == null) return;

        // If it is a player that is the local client, deal damage
        if (_hit.TryGetComponent<NetworkBehaviour>(out NetworkBehaviour _net) && _net.IsLocalPlayer)
        {
            _hit.GetComponent<PlayerHealth>().Damage(damage);
        }
    }
}
