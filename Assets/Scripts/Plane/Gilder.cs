using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Gilder : MonoBehaviour
{
    [SerializeField]private Transform _wingCP;
    [Header("Air Density")]
    [SerializeField] private float _airDensity = 1.25f;

    [Header("Aerodynamic characteristics of the wing ")]
    [SerializeField] private float _wingArea = 1.5f; //s
    [SerializeField] private float _wingAspect = 8.0f; // AR=b^2/S

    [SerializeField] private float _wingCDO = 0.02f;
    [SerializeField] private float _wingCLalpha = 5.5f;

    private Rigidbody _rigidbody;

    private Vector3 _worldVelocity;
    private Vector3 _vPoint;

    private float _speedMS;
    private float _alphaRad;

    private float _cd, _cl, _qDyn, _lMag, _dMag, _glideK;  

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        _vPoint = _rigidbody.GetPointVelocity(_wingCP.position);
        _speedMS = _vPoint.magnitude;

        Vector3 flowDir = (-_vPoint).normalized;
        Vector3 xChord = _wingCP.forward;
        Vector3 zUP = _wingCP.up;
        Vector3 ySpan = _wingCP.right;

        float flowX = Vector3.Dot(lhs: flowDir, rhs: xChord);
        float flowZ = Vector3.Dot(lhs: flowDir, rhs: zUP);

        _alphaRad = Mathf.Atan2(y: flowZ, flowX);

        _cl = _wingCLalpha * _alphaRad;
        _cd = _wingCDO + _cl * _cl / (Mathf.PI * _wingAspect * 0.85f);

        _qDyn = 0.5f * _airDensity * _speedMS * _speedMS;
        _lMag = _qDyn * _wingArea * _cl;
        _dMag = _qDyn * _wingArea * _cd;

        Vector3 Ddir = -flowDir;

        Vector3 liftDir = Vector3.Cross(flowDir, ySpan);
        liftDir.Normalize();

        Vector3 L = _lMag * liftDir;
        Vector3 D = _dMag * Ddir;

        _rigidbody.AddForceAtPosition(L + D, _wingCP.position, ForceMode.Force);
    }

    private void StepOne()
    {
        Vector3 xGhord = _wingCP.forward; 
        Vector3 zUP = _wingCP.up; 

        Vector3 flowDir = _speedMS > 0 ? -_worldVelocity.normalized : _wingCP.forward;

        float flowX = Vector3.Dot(lhs: flowDir, rhs: xGhord);
        float flowZ = Vector3.Dot(lhs: flowDir, rhs: zUP);

        _alphaRad = Mathf.Atan2(y: flowZ, flowX);
    }

    private void OnGUI()
    {
        GUI.color = Color.black;
        GUILayout.Label(text: $"��������:{_speedMS:0,0} m/s*");
        GUILayout.Label(text: $"���� ����:{_alphaRad * Mathf.Deg2Rad:0,0} *");
    }
}