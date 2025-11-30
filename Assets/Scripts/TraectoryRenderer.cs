using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TraectoryRenderer : MonoBehaviour
{
    //формула ваакумной траектории 
    [Header("Trajectory paramets")]
    [SerializeField] private float _lineWidth = 0.15f;
    [SerializeField] private int _pointCount = 30;
    [SerializeField] private float _timeStep = 0.1f;
    [Space]
    [SerializeField] private QuadraticDrag _shootRound;
    private LineRenderer _lineRenderer;

    private void Awake() => InitializeLineRenderer();

    private void InitializeLineRenderer()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.useWorldSpace = true;

        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void DrawVacuum(Vector3 startPosition, Vector3 startVelocity)
    {
        if (_pointCount < 2) _pointCount = 2;

        _lineRenderer.positionCount = _pointCount;

        for (int i = 0; i < _pointCount; i++)
        {
            float t = i * _timeStep;
            Vector3 newPosition = startPosition + t * startVelocity + Physics.gravity * t * t / 2;
            _lineRenderer.SetPosition(i, newPosition);
        }
    }
}
