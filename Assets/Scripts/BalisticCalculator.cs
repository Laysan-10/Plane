using UnityEngine;
using UnityEngine.InputSystem;

namespace Group_9
{
    [RequireComponent(typeof(TraectoryRenderer))]
    public class BalisticCalculator:MonoBehaviour
    {
        [Header("Header params")]
        [SerializeField]  private float _mass = 1f;
        [SerializeField] private float _radius = 0.1f;
        [SerializeField] private float _dragCoefficient = 0.47f;
        [SerializeField] private float _airDensity = 1.225f;
        [SerializeField]  private Vector3 _wind = Vector3.zero;
        [SerializeField] private Transform _zapustikPoint;
        [SerializeField] private Transform _shootRound;
        [SerializeField] private float  _muzzleVelocity = 20;
        [SerializeField, Range(0, 85)] private float _muzleAngle = 20;

        private TraectoryRenderer _traectoryRenderer;

        private void Start()
        {
            _traectoryRenderer = GetComponent<TraectoryRenderer>();
        }

        public void Update()
        {

            if (_zapustikPoint == null) return;
            Vector3 v0 = CalculateVelocityVector(_muzleAngle);
            _traectoryRenderer.DrawVacuum(_zapustikPoint.position, v0);

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Fire(v0);
        }

        private void Fire(Vector3 initialVelocity)
        {
            if (_shootRound == null) return;
            GameObject newShootRound = Instantiate (_shootRound.gameObject, _zapustikPoint.position, Quaternion.identity);

            QuadraticDrag quadraticDrag = newShootRound.GetComponent<QuadraticDrag>();
            quadraticDrag.SetPhysicalParams(_mass, _radius, _dragCoefficient, _airDensity, _wind, initialVelocity);

        }

        private Vector3 CalculateVelocityVector(float angle)
        {
            float vx = _muzleAngle * Mathf.Cos(angle * Mathf.Deg2Rad);
            float vy = _muzleAngle * Mathf.Sin(angle * Mathf.Deg2Rad);

            return _zapustikPoint.forward * vx + _zapustikPoint.up * vy;
        }
    }


}

