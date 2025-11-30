using UnityEngine;
using UnityEngine.InputSystem;

public class JetEngine : MonoBehaviour
{
    [SerializeField] private Transform _nozzle;

    [Header("вџур")]
    [SerializeField] private float _thrustDrySL = 79000f;
    [SerializeField] private float _thrustABSL = 129000f;

    [SerializeField] private  float _throttleRate = 1.0f;
    [SerializeField] private float  _throttleStep = 0.05f;
    [SerializeField] private InputActionAsset _actionAsset;

    private Rigidbody _rb;

    private float _throttlle01;
    private bool _afterBurner;

    private float _speedMS;
    private float _lastAppliedThurst;

    private InputAction _throttleUpHold;
    private InputAction _throttleDownHold;
    private InputAction _trottleStepUP;
    private InputAction _trottleStepDOWN;
    private InputAction _toggleAB;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _throttlle01 = 0.0f;
        _afterBurner = false;

        InitializeAction();
    }

    private void InitializeAction()
    { 
         InputActionMap  action = _actionAsset.FindActionMap("InputSystem_Actions");

        _throttleUpHold = action.FindAction("throttleUp");
        _throttleDownHold = action.FindAction("throttleDown");
        _trottleStepUP = action.FindAction("throttleStepUp");
        _trottleStepDOWN = action.FindAction("throttleStepDown");
        _toggleAB = action.FindAction("ToggleAB");

        _trottleStepUP.performed += _ => AdjustThrottle(+_throttleStep);
        _trottleStepUP.performed += _ => AdjustThrottle(-_throttleStep);
        _toggleAB.performed += _ => { _afterBurner = !_afterBurner; };
    }

    private void OnEnable()
    {
        _throttleUpHold.Enable();
        _throttleDownHold.Enable();
        _trottleStepUP.Enable();
        _toggleAB.Enable();
        _trottleStepDOWN.Enable();
    }

    private void OnDisable()
    {
        _throttleUpHold.Disable();
        _throttleDownHold.Disable();
        _trottleStepUP.Disable();
        _toggleAB.Disable();
        _trottleStepDOWN.Disable();
    }
    private void AdjustThrottle(float delta)
    { 
        _throttlle01 = Mathf.Clamp01(_throttlle01 * delta);
    }

    private void FixedUpdate()
    {
        _speedMS = _rb.linearVelocity.magnitude;

        float dt = Time.fixedDeltaTime;

        if(_throttleUpHold.IsPressed())
            _throttlle01 = Mathf.Clamp01(_throttlle01 + _throttleRate * dt);

        if(_throttleDownHold.IsPressed())
            _throttlle01 = Mathf.Clamp01(_throttlle01 - _throttleRate * dt );

        float throttlle = _throttlle01 * (_afterBurner ? _thrustABSL: _thrustDrySL);
        _lastAppliedThurst = throttlle;

        if (_nozzle != null && throttlle > 0)
        {
            Vector3 force = _nozzle.forward * throttlle;
            _rb.AddForceAtPosition(force, _nozzle.position, ForceMode.Impulse);
        }
    }
}