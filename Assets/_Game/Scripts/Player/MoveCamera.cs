using UnityEngine;

// Sets camera position
public class MoveCamera : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraPosition;

    private void Update()
    {
        if(cameraPosition == null) return;

        transform.position = cameraPosition.position;   
    }
}
