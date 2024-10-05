using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    private void Update()
    {
        if(cameraPosition == null) return;

        transform.position = cameraPosition.position;   
    }
}
