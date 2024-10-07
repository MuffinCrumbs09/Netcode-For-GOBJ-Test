using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// Changes the crosshair based on who it is looking at
public class Crosshair : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RawImage crosshair;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        // Shoots an INF raycast. If hit player, turn red, else white
        if (Physics.Raycast(GetMouseRay(), out var hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.CompareTag("Player")) crosshair.color = Color.red;
            else crosshair.color = Color.white;
        }
    }

    // Shoots a ray based on mouse position through the camera
    private Ray GetMouseRay()
    {
        return _cam!.ScreenPointToRay(Input.mousePosition);
    }
}
