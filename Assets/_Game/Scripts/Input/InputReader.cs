using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    public static InputReader Instance;
    private Controls _controls;

    #region Public Values
    public Vector2 MovementValue {  get; private set; }
    public float LookValueX {  get; private set; }
    public float LookValueY {  get; private set; }
    public bool IsSprint {  get; private set; }
    public bool IsFire {  get; private set; }
    public bool IsAuto;
    #endregion

    #region Events
    public event Action JumpEvent;
    public event Action FireEvent;
    #endregion

    private void Awake()
    {
        //If there is an instance that isnt me, delete me
        if(Instance != null &&  Instance != this)
            Destroy(this);

        //Sets me as the instance
        Instance = this;
    }

    private void Start()
    {
        //Cursor (Disabled for now)
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        //Create controls and set its callback to this
        _controls = new Controls();
        _controls.Player.SetCallbacks(this);
        //Enable player controls
        ToggleControls(true);
    }

    public void ToggleControls(bool toggle)
    {
        if (toggle)
            _controls.Player.Enable();
        else
            _controls.Player.Disable();
    }

    public void ToggleMouse(bool toggle)
    {
        if(toggle)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        } else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnMovement(InputAction.CallbackContext context) => MovementValue = context.ReadValue<Vector2>(); 

    public void OnLookX(InputAction.CallbackContext context) => LookValueX = context.ReadValue<float>();

    public void OnLookY(InputAction.CallbackContext context) => LookValueY = context.ReadValue<float>();

    public void OnSprint(InputAction.CallbackContext context) => IsSprint = context.performed;

    public void OnJump(InputAction.CallbackContext context) => JumpEvent?.Invoke();

    public void OnFire(InputAction.CallbackContext context)
    {
        if (IsAuto) IsFire = context.performed;
        else if (!IsAuto && context.started) FireEvent?.Invoke();
    }
}