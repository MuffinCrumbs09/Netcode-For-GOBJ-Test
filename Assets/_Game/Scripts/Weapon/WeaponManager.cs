using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public float fireCooldown;

    public bool automatic;

    private float _curCooldown;

    private void OnEnable()
    {
        InputReader.Instance.FireEvent += FireSingle;
    }

    private void OnDisable()
    {
        InputReader.Instance.FireEvent -= FireSingle;
    }

    private void Start()
    {
        _curCooldown = fireCooldown;
    }

    private void Update()
    {
        InputReader.Instance.IsAuto = automatic;

        if(InputReader.Instance.IsFire)
            FireAuto();

        _curCooldown -= Time.deltaTime;
    }
    private void FireAuto()
    {
        if(_curCooldown > 0f) return;

        OnGunShoot?.Invoke();
        _curCooldown = fireCooldown;
    }

    private void FireSingle()
    {
        if (_curCooldown > 0f) return;

        OnGunShoot?.Invoke();
        _curCooldown = fireCooldown;
    }
}
