using Unity.Netcode;
using UnityEngine;

// How the player moves the camera
public class PlayerLook : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private WallRun _wallRun;
    [SerializeField] Transform orientation;

    [Header("Settings - Sens")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    Transform _cam;

    float _mouseX;
    float _mouseY;

    float multipler = 0.01f;

    float xRotation;
    float yRotation;

    // If client doesn't own this, disable me
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
    }

    private void Start()
    {
        _cam = FindAnyObjectByType<Camera>().transform;
        _cam.parent.GetComponent<MoveCamera>().cameraPosition = transform.GetChild(1);
    }

    private void Update()
    {
        MyInput();

        // Rotates the cam (X, Y, Z) and player (Y)
        _cam.rotation = Quaternion.Euler(xRotation, yRotation, _wallRun.tilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    // Reads input from input reader.
    private void MyInput()
    {
        _mouseX = InputReader.Instance.LookValueX;
        _mouseY = InputReader.Instance.LookValueY;

        yRotation += _mouseX * sensX * multipler;
        xRotation -= _mouseY * sensY * multipler;

        xRotation = Mathf.Clamp(xRotation, -90f, 90);
    }
}
