using CookieUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugUISampleObject : MonoBehaviour, IDebugDrawer, IPointerDownHandler, IPointerUpHandler
{
    private bool _isHeld;
    private Rigidbody2D _rb;
    [SerializeField] private float deltaWeight = 3f;
    [SerializeField] private float posWeight = 5f;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isHeld) {
            var mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            var force = Mouse.current.delta.ReadValue() * deltaWeight + (Vector2)(mousePos - transform.position) * posWeight;
            _rb.AddForceAtPosition(force, _rb.ClosestPoint(mousePos), ForceMode2D.Force);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHeld = false;
    }

    private void Start()
    {
        CookieDebug.Register(this);
    }

    public void DrawDebugUI(IDebugUIBuilderProvider provider)
    {
        provider.Get(this)
            .Label("This is some cool debug text!", "cool-label")
            .Foldout("Stats", "stats")
            .Foldout("Transform", "transform")
            .Label($"Position is {transform.position.xy()}", "position")
            .Label($"Rotation is {transform.eulerAngles.z:0.0}", "rotation")
            .EndFoldout()
            .Foldout("Rigidbody", "rigidbody")
            .Label($"Velocity is {_rb.linearVelocity}", "velocity")
            .EndFoldout()
            .EndFoldout();
    }
}
