using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class WallRun : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] Transform orientation;

    [Header("Detection")]
    [SerializeField] private float wallDistance = .6f;
    [SerializeField] private float minJumpHeight = 1.5f;

    [Header("Wall Running")]
    [SerializeField] private float wallRunGravity = 1f;
    [SerializeField] private float wallRunJumpForce = 125f;

    [Header("Camera")]
    [SerializeField] private float fov = 90;
    [SerializeField] private float wallRunFov = 110;
    [SerializeField] private float wallRunFovTime = 20;
    [SerializeField] private float camTilt = 20f;
    [SerializeField] private float camTiltTime = 20f;

    public float tilt { get; private set; }

    private Camera _cam;

    private bool _wallLeft = false;
    private bool _wallRight = false;
    private bool _wallRunning = false;

    private Rigidbody _rb;

    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cam = FindAnyObjectByType<Camera>();

        InputReader.Instance.JumpEvent += Jump;
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if(_wallLeft)
            {
                StartWallRun();
            }
            else if (_wallRight)
            {
                StartWallRun();
            }
            else
                StopWallRun();
        } else
            StopWallRun();
    }

    private bool CanWallRun() { return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight); }

    private void CheckWall()
    {
        _wallLeft = Physics.Raycast(transform.position, -orientation.right, out _leftWallHit, wallDistance);
        _wallRight = Physics.Raycast(transform.position, orientation.right, out _rightWallHit, wallDistance);
    }

    private void StartWallRun()
    {
        _wallRunning = true;

        _rb.useGravity = false;
        _rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);
        if(_wallLeft) tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if (_wallRight) tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
    }

    private void StopWallRun()
    {
        _wallRunning = false;

        _rb.useGravity = true;

        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    private void Jump()
    {
        if (!_wallRunning) return;

        if (_wallLeft)
        {
            Vector3 wallRunJumpDir = transform.up + _leftWallHit.normal;
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(wallRunJumpDir * wallRunJumpForce, ForceMode.Force);
        } 
        else if (_wallRight)
        {
            Vector3 wallRunJumpDir = transform.up + _rightWallHit.normal;
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            _rb.AddForce(wallRunJumpDir * wallRunJumpForce, ForceMode.Force);
        }
    }

    private void OnEnable()
    {
        if (InputReader.Instance == null) return;
        InputReader.Instance.JumpEvent += Jump;
    }

    private void OnDisable()
    {
        InputReader.Instance.JumpEvent -= Jump;
    }
}
