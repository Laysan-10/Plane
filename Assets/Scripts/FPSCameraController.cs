using UnityEngine;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] private float _yamSensity = 180f;
    [SerializeField] private float _pitchSensity = 180f;
    [SerializeField] private float _maxPitchDrag = 89f;
    private Quaternion _targetRotation;
    [SerializeField] private float _rotationDamping;

    private float _yamDeg;
    private float _pitchDeg;

    private void Awake()

    {
        _yamDeg = transform.eulerAngles.y;
        _pitchDeg = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float dy = Input.GetAxis("Mouse X");
        float dx = Input.GetAxis("Mouse Y");

        _yamDeg -= dy * _yamSensity * Time.deltaTime;
        _pitchDeg -= dx * _pitchSensity * Time.deltaTime; //инверсия

        _pitchDeg = Mathf.Clamp(_pitchDeg, -_maxPitchDrag, _maxPitchDrag);

        Quaternion yamRot = Quaternion.AngleAxis(_yamDeg, Vector3.up);

        Vector3 rifghAxis = yamRot * Vector3.right;
        Quaternion pitchRot = Quaternion.AngleAxis(_pitchDeg, rifghAxis);

        _targetRotation = pitchRot * yamRot;

        float t = 1 - Mathf.Pow(1 - Mathf.Clamp01(_rotationDamping), Time.deltaTime * 60f);

        transform.localRotation = Quaternion.Slerp(transform.rotation, _targetRotation, t);
    }


}
