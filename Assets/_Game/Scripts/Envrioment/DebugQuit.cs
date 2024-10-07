using UnityEngine;

public class DebugQuit : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) Application.Quit();
    }
}
