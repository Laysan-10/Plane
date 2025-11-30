using Unity.VisualScripting;
using UnityEngine;

public class QuadraticDrag : MonoBehaviour
{
    private float _mass;
    private float _radius;
    private float _dragCoefficient;
    private float _airDensity;
    private Vector3 _wind = Vector3.zero;


    private Rigidbody _rigidbody;
    private float _area;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 vReaL = _rigidbody.linearVelocity - _wind;
        float speed = vReaL.magnitude;

        Vector3 drag = -0.5f * _airDensity * _dragCoefficient * _area * speed * vReaL;
        _rigidbody.AddForce(drag, ForceMode.Force);
    }

    public void SetPhysicalParams(float mass, float radius, float dragCoefficent, float airDensty,Vector3 wind, Vector3 initialVelocity )
    {
        _radius = radius;
        _dragCoefficient = dragCoefficent;
        _airDensity = airDensty;
        _wind = wind;

        _rigidbody.mass = mass;
        _rigidbody.useGravity = true;
        _rigidbody.linearVelocity= initialVelocity;

        _area = _radius * _radius * Mathf.PI;
    }
}
