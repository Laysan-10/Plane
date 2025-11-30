using System.Collections.Generic;
using UnityEngine;
namespace Group_9
{

    [RequireComponent(typeof(ForceVisuliizers))]
    public class SimplyPhysicsEngine : MonoBehaviour
    {
        [Header("Физические параметры")]
        [SerializeField] private float _mass;
        [SerializeField] private bool _isGravity;
        [SerializeField] private float _dragCoeficient = 0.1f;
        [SerializeField] private Vector3 _windForce;


        private ForceVisuliizers _forceVisualizers;
        private Vector3 _netForce;
        private Vector3 _velocity = Vector3.zero;



        private void Start()
        {
            _forceVisualizers = GetComponent<ForceVisuliizers>();
        }


        private void FixedUpdate()
        {
            _netForce = Vector3.zero;
            _forceVisualizers.ForceClear();

            if (_isGravity)
            {
                Vector3 grtavity = Physics.gravity * _mass;
                ApplyForce(grtavity, Color.cyan, name: "Gravity");
            }


            ApplyForce(_windForce, Color.blue, name: "WindForce");


            Vector3 acceleration = _netForce / _mass;
            IntegrateMotion(acceleration);


            _forceVisualizers.AddForce(_netForce, Color.red, name: "ForceMAIN");

        }


        private void IntegrateMotion(Vector3 acceleration)
        {
            _velocity += acceleration * Time.fixedDeltaTime;
            transform.position += _velocity * Time.fixedDeltaTime;
        }

        private void ApplyForce(Vector3 force, Color colorForce, string name)
        {
            _netForce += force;
            _forceVisualizers.AddForce(force, colorForce, name);
        }

    }
}