using UnityEngine;
using UnityEngine.Events;

// Handles shooting the gun
public class WeaponManager : MonoBehaviour
{
    public UnityEvent OnGunShoot;

    public float fireCooldown;
    public bool automatic;

    private float _curCooldown;

    private void OnEnable()
    {
        InputReader.Instance.FireEvent += Fire;
    }
    private void OnDisable()
    {
        InputReader.Instance.FireEvent -= Fire;
    }

    private void Start()
    {
        _curCooldown = fireCooldown;
    }

    private void Update()
    {
        // Keeps InputReader and this in sync
        InputReader.Instance.IsAuto = automatic;

        // If you click, fire
        if(InputReader.Instance.IsFire)
            Fire();

        _curCooldown -= Time.deltaTime;
    }

    // Now were shooting, kinda
    private void Fire()
    {
        // If its too soon, return
        if(_curCooldown > 0f) return;

        // Try running OnGunShoot, and reset cooldown
        OnGunShoot?.Invoke();
        _curCooldown = fireCooldown;
    }
}
