using Unity.Netcode;
using UnityEngine;

public class PlayerLook : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private WallRun _wallRun;
    [SerializeField] Transform orientation;

    [Header("Settings")]
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    Transform _cam;

    float _mouseX;
    float _mouseY;

    float multipler = 0.01f;

    float xRotation;
    float yRotation;

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

        _cam.rotation = Quaternion.Euler(xRotation, yRotation, _wallRun.tilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void MyInput()
    {
        _mouseX = InputReader.Instance.LookValueX;
        _mouseY = InputReader.Instance.LookValueY;

        yRotation += _mouseX * sensX * multipler;
        xRotation -= _mouseY * sensY * multipler;

        xRotation = Mathf.Clamp(xRotation, -90f, 90);
    }
}
