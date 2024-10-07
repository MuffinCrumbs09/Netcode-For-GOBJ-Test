using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RawImage crossHair;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if(Physics.Raycast(GetMouseRay(), out var hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.CompareTag("Player")) crossHair.color = Color.red;
            else crossHair.color = Color.white;
        }
    }

    private Ray GetMouseRay()
    {
        return _cam!.ScreenPointToRay(Input.mousePosition);
    }
}
