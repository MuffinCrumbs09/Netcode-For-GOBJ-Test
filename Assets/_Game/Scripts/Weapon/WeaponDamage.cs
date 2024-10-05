using UnityEngine;
using Unity.Netcode;

public class WeaponDamage : NetworkBehaviour
{
    public float damage;
    public float bulletRange;
    public GameObject bulletTrail;
    private Transform _cam;

    private void Start()
    {
        _cam = FindAnyObjectByType<Camera>().transform;
    }

    public void Shoot()
    {
        if(!IsOwner) return;

        Ray gunRay = new Ray(_cam.position, _cam.forward);
        if(Physics.Raycast(gunRay, out RaycastHit hitInfo, bulletRange))
        {
            //Checks if we hit a player
            if(hitInfo.collider != null)
            {
                //Deal damage here
                Debug.Log("Hit " + hitInfo.transform.name);

                GameObject lr = Instantiate(bulletTrail, transform.position, bulletTrail.transform.rotation);
                lr.GetComponent<LineRenderer>().SetPosition(0, transform.position);
                lr.GetComponent<LineRenderer>().SetPosition(1, hitInfo.point);

                ShootRpc(hitInfo.transform.name, transform.position, hitInfo.point);
            }
        }
        SoundManager.PlaySound(SoundType.SHOOT, 0.8f);
    }

    [Rpc(SendTo.NotMe)]
    public void ShootRpc(string hit, Vector3 startPos, Vector3 endPos, float damage = 0)
    {
        Debug.Log(hit);

        GameObject lr = Instantiate(bulletTrail, startPos, bulletTrail.transform.rotation);
        lr.GetComponent<LineRenderer>().SetPosition(0, startPos);
        lr.GetComponent<LineRenderer>().SetPosition(1, endPos);
    }
}
