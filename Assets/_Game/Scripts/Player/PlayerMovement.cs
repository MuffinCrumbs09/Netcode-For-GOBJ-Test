using System.Globalization;
using Unity.Netcode;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float movementMultipler = 10f;
    [SerializeField] private float airMultipler = .4f;
    [SerializeField] private Transform orientation;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float aceleration = 10f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundDistance = .4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;

    [Header("Drag")]
    [SerializeField] private float groundDrag = 6f;
    [SerializeField] private float airDrag = 2f;

    [Header("Network Variables")]
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

    private float _playerHeight = 2f;

    private Vector3 _moveDir;
    private Vector3 _inputVec;
    private Vector3 _slopeMoveDir;

    private Rigidbody _rb;
    private bool _isGrounded;
    private RaycastHit _slopeHit;

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
            transform.position = new Vector3(0, 1, 0);
    }

    [Rpc(SendTo.Server)] private void UpdatePlayerPositionRpc(float x,  float y, float z) => Position.Value = new Vector3(x, y, z);
    [Rpc(SendTo.Server)] private void UpdatePlayerRotationRpc(Quaternion rotation) => Rotation.Value = rotation;

    private void Start()
    {
        Destroy(GameObject.Find("JoinCanvas"));
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        InputReader.Instance.JumpEvent += Jump;
        InputReader.Instance.ToggleMouse(false);
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            transform.rotation = orientation.rotation;

            Input();
            ControlDrag();
            ControlSpeed();

            _slopeMoveDir = Vector3.ProjectOnPlane(_moveDir, _slopeHit.normal);
            UpdatePlayerPositionRpc(transform.position.x, transform.position.y, transform.position.z);
            UpdatePlayerRotationRpc(orientation.rotation);
        }
        else
        {
            transform.position = Position.Value;
            transform.rotation = Rotation.Value;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(_isGrounded && !OnSlope()) _rb.AddForce(_moveDir.normalized * moveSpeed * movementMultipler, ForceMode.Acceleration);
        else if (_isGrounded && OnSlope()) _rb.AddForce(_slopeMoveDir.normalized * moveSpeed * movementMultipler, ForceMode.Acceleration);
        else if (!_isGrounded) _rb.AddForce(_moveDir.normalized * moveSpeed * movementMultipler * airMultipler, ForceMode.Acceleration);
    }

    private void ControlSpeed()
    {
        if (InputReader.Instance.IsSprint && _isGrounded) moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, aceleration * Time.deltaTime);
        else moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, aceleration * Time.deltaTime);
    }

    private void ControlDrag()
    {
        if(_isGrounded) _rb.linearDamping = groundDrag;
        else _rb.linearDamping = airDrag;
    }

    private void Input()
    {
        _inputVec = InputReader.Instance.MovementValue;
        _moveDir = orientation.forward * _inputVec.y + orientation.right * _inputVec.x;
    }

    private void Jump()
    {
        if (!_isGrounded) return;

        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight / 2 + 0.5f))
        {
            if (_slopeHit.normal != Vector3.up) return true;
        } else
            return false;
        return false;
    }

    private void OnEnable()
    {
        if(InputReader.Instance == null) return;
        InputReader.Instance.JumpEvent += Jump;
    }

    private void OnDisable()
    {
        InputReader.Instance.JumpEvent -= Jump;
    }
}
