using CookieUtils;
using CookieUtils.Debugging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugUISampleObject : MonoBehaviour, IDebugDrawer, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float deltaWeight = 3f;
    [SerializeField] private float posWeight = 5f;
    private Camera _camera;
    private bool _isHeld;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        CookieDebug.Register(this);
    }

    private void FixedUpdate()
    {
        if (_isHeld) {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 force = Mouse.current.delta.ReadValue() * deltaWeight +
                            (Vector2)(mousePos - transform.position) * posWeight;
            _rb.AddForceAtPosition(force, _rb.ClosestPoint(mousePos), ForceMode2D.Force);
        }
    }

    public void DrawDebugUI(IDebugUIBuilderProvider provider)
    {
        IDebugUIBuilder builder = provider.Get(this)
            .Label("This is some cool debug text!", "cool-label")
            .Foldout("Stats", "stats")
            .Foldout("Transform", "transform")
            .Label($"Position is {transform.position.xy()}", "position")
            .Label($"Rotation is {transform.eulerAngles.z:0.0}", "rotation")
            .EndFoldout();

        if (_rb.linearVelocity.sqrMagnitude > 0.01f || _rb.angularVelocity > 0.1f)
            builder
                .Foldout("Rigidbody", "rigidbody")
                .Label($"Velocity is {_rb.linearVelocity}", "velocity")
                .Label($"Angular velocity is {_rb.angularVelocity:0.0}", "angular_velocity")
                .EndFoldout();

        builder.EndFoldout();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHeld = false;
    }
}